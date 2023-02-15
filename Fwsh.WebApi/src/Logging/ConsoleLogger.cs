namespace Fwsh.WebApi.Logging;

using System;

public class ConsoleLogger : Logger
{
    public override void Log (string message, params object[] args)
    {
        Console.Write("[{0:HH:mm:ss}]: ", DateTime.Now);
        Console.WriteLine(message, args);
    }

    public override void Warn (string message, params object[] args)
    {
        Console.Write("\u001b[00;33m[{0:HH:mm:ss}]: Warning: ", DateTime.Now);
        Console.Write(message, args);
        Console.WriteLine("\u001b[00m");
    }

    public override void Error (string message, params object[] args)
    {
        Console.Write("\u001b[00;31m[{0:HH:mm:ss}]: Error: ", DateTime.Now);
        Console.Write(message, args);
        Console.WriteLine("\u001b[00m");
    }
}
