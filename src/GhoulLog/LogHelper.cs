using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace GhoulLog
{
    public class LogHelper
    {
        const string M_LogFileName = "log";
        private static IDictionary<string, LoggerFactory> m_factorys = new Dictionary<string, LoggerFactory>();

        public static LoggerFactory GetFactory(LogLevel level, string fileName = M_LogFileName)
        {
            if (string.IsNullOrEmpty(fileName))
                fileName = M_LogFileName;
            if (fileName == M_LogFileName && LogConfig.Config.SplitLevel)
                fileName += $".{level.ToString().ToLower()}";

            if (!m_factorys.ContainsKey(fileName))
            {
                lock (m_factorys)
                {
                    if (!m_factorys.ContainsKey(fileName))
                    {
                        var logPath = GetLogPath();

                        var pathFormat = System.IO.Path.Combine(logPath, LogConfig.Config.FileFormat.Replace("@FileName", fileName));

                        var factory = new LoggerFactory();
                        factory.AddDebug();
                        factory.AddConsole(LogConfig.Config.LogLevel, true);
                        factory.AddFile(pathFormat, LogConfig.Config.LogLevel);
                        m_factorys.Add(fileName, factory);
                    }
                }
            }

            return m_factorys[fileName];
        }

        public static string GetLogPath()
        {
            var logPath = LogConfig.Config.Path;
            if (string.IsNullOrEmpty(logPath))
                logPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            return logPath;
        }


        internal static LogConfig LoadConfig()
        {
            var fileName = "logging.json";
            var configStream = GetConfigStreamFromFile(fileName);
            if (configStream == null)
                configStream = GetConfigStreamFromResource(fileName);

            return DeserializeFromStream<LogConfig>(configStream);
        }

        private static string GetCurrentPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static string FoldSplitSymbol
        {
            get
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    return "\\";
                return "/";
            }
        }

        private static Stream GetConfigStreamFromFile(string fileName)
        {
            var path = System.IO.Path.Combine(GetCurrentPath(), FoldSplitSymbol, fileName);
            if (!File.Exists(path)) return null;

            using (var file = File.OpenRead(path))
            {
                var buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
                return new MemoryStream(buffer);
            }
        }

        /// <summary>
        /// read resource
        /// </summary>
        private static Stream GetConfigStreamFromResource(string fileName)
        {
            try
            {
                var asm = Assembly.GetExecutingAssembly();
                var projectName = asm.GetName().Name.ToString();
                return asm.GetManifestResourceStream(projectName + "." + fileName);
            }
            catch
            {
                return null;
            }
        }

        private static T DeserializeFromStream<T>(Stream stream)
        {
            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(jsonTextReader);
            }
        }
    }
}
