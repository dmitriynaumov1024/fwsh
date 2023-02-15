namespace Fwsh.WebApi.Logging;

public abstract class Logger
{
    public abstract void Log (string message, params object[] args);
    public abstract void Warn (string message, params object[] args);
    public abstract void Error (string message, params object[] args);
}
