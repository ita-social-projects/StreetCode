using Streetcode.BLL.Interfaces.Logging;
using Serilog;

namespace Streetcode.BLL.Services.Logging
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger _logger;

        public LoggerService(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string msg)
        {
            _logger.Information($"{msg}");
        }

        public void LogWarning(string msg)
        {
            _logger.Warning($"{msg}");
        }

        public void LogTrace(string msg)
        {
            _logger.Information($"{msg}");
        }

        public void LogDebug(string msg)
        {
            _logger.Debug($"{msg}");
        }

        public void LogError(object request, string erroMsg)
        {
            string requestType = request.GetType().ToString();
            string requestClass = requestType.Substring(requestType.LastIndexOf('.') + 1);
            _logger.Error($"{requestClass} handled with the error: {erroMsg}");
        }
    }
}
