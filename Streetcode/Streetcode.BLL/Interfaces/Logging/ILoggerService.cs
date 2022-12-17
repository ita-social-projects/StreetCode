using Microsoft.Extensions.Logging;

namespace Streetcode.BLL.Interfaces.Logging
{
    public interface ILoggerService<out T>
    {
        void LogInformation(string msg);
        void LogWarning(string msg);
        void LogTrace(string msg);
        void LogDebug(string msg);
        void LogError(string msg);
    }
}
