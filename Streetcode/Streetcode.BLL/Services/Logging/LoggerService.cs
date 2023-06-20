﻿using Microsoft.Extensions.Logging;
using Streetcode.BLL.Interfaces.Logging;
using Serilog;

namespace Streetcode.BLL.Services.Logging
{
    public class LoggerService<T> : ILoggerService<T>
    {
        private ILogger<T> _logger;

        public LoggerService(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string msg)
        {
            _logger.Log(LogLevel.Information, $"{msg}");
        }

        public void LogWarning(string msg)
        {
            _logger.Log(LogLevel.Warning, $"{msg}");
        }

        public void LogTrace(string msg)
        {
            _logger.Log(LogLevel.Trace, $"{msg}");
        }

        public void LogDebug(string msg)
        {
            _logger.Log(LogLevel.Debug, $"{msg}");
        }

        public void LogError(string msg)
        {
            _logger.Log(LogLevel.Error, $"{msg}");
        }
    }
}
