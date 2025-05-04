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
            _logger.Information("{Msg}", msg);
        }

        public void LogWarning(string msg)
        {
            _logger.Warning("{Msg}", msg);
        }

        public void LogTrace(string msg)
        {
            _logger.Information("{Msg}", msg);
        }

        public void LogDebug(string msg)
        {
            _logger.Debug("{Msg}", msg);
        }

        public void LogError(object request, string errorMsg)
        {
            if (request != null)
            {
                string requestType = request.GetType().ToString();
                string requestClass = requestType.Substring(requestType.LastIndexOf('.') + 1);
                _logger.Error("{RequestClass} handled with the error: {ErrorMsg}", requestClass, errorMsg);
            }
            else
            {
                _logger.Error(errorMsg);
            }
        }
    }
}