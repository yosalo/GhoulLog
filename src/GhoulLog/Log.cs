using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace GhoulLog
{
    public class Log
    {
        private static string ConcatMessage(string message, object data)
        {
            if (data != null)
            {
                message += Environment.NewLine;
                try
                {
                    message += JsonConvert.SerializeObject(data);
                }
                catch
                {
                    message += $"Serialization failed => {data.ToString()}";
                }
            }
            return message;
        }

        public static ILogger GetLogger<T>(LogLevel level, string fileName = null)
        {
            var factory = LogHelper.GetFactory(level, fileName);
            return factory.CreateLogger<T>();
        }
        public static ILogger GetLogger(string categoryName, LogLevel level, string fileName = null)
        {
            var factory = LogHelper.GetFactory(level, fileName);
            return factory.CreateLogger(categoryName);
        }

        public static void Debug<T>(string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger<T>(LogLevel.Debug, fileName).LogDebug(message);
        }
        public static void Debug(string categoryName, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger(categoryName, LogLevel.Debug, fileName).LogDebug(message);
        }

        public static void Info<T>(string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            var logger = GetLogger<T>(LogLevel.Information, fileName);
            logger.LogInformation(message);
        }
        public static void Info(string categoryName, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger(categoryName, LogLevel.Information, fileName).LogInformation(message);
        }

        public static void Warning<T>(string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger<T>(LogLevel.Warning, fileName).LogWarning(message);
        }
        public static void Warning(string categoryName, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger(categoryName, LogLevel.Warning, fileName).LogWarning(message);
        }

        public static void Error<T>(string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger<T>(LogLevel.Error, fileName).LogError(message);
        }
        public static void Error(string categoryName, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger(categoryName, LogLevel.Error, fileName).LogError(message);
        }

        public static void Exception<T>(Exception ex, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger<T>(LogLevel.Error, fileName).LogError(ex, message);
        }
        public static void Exception(Exception ex, string categoryName, string message, object data = null, string fileName = null)
        {
            message = ConcatMessage(message, data);
            GetLogger(categoryName, LogLevel.Error, fileName).LogError(ex, message);
        }
    }
}
