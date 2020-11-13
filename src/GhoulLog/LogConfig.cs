using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;

namespace GhoulLog
{
    /// <summary>
    /// conf of log
    /// </summary>
    public class LogConfig
    {
        public string Path { get; set; }
        public string FileFormat { get; set; }
        public bool SplitLevel { get; set; }
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }


        private static LogConfig m_config = null;
        public static LogConfig Config
        {
            get
            {
                if (m_config == null)
                {
                    lock (typeof(LogConfig))
                    {
                        if (m_config == null)
                            m_config = LogHelper.LoadConfig();
                    }
                }

                return m_config;
            }
        }
    }
}
