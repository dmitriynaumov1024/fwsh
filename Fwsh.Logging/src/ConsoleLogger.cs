namespace Fwsh.Logging;

using System;

public class ConsoleLogger : Logger
{
    public override void Log (string message, params object[] args)
    {
        Console.Write("[{0:HH:mm:ss}][i]: ", DateTime.Now);
        if(args.Length > 0) Console.WriteLine(message, args);
        else Console.WriteLine("{0}", message);
    }

    public override void Warn (string message, params object[] args)
    {
        Console.Write("\u001b[00;33m[{0:HH:mm:ss}][!]: ", DateTime.Now);
        if(args.Length > 0) Console.Write(message, args);
        else Console.Write("{0}", message);
        Console.WriteLine("\u001b[00m");
    }

    public override void Error (string message, params object[] args)
    {
        Console.Write("\u001b[00;31m[{0:HH:mm:ss}][x]: ", DateTime.Now);
        if(args.Length > 0) Console.WriteLine(message, args);
        else Console.Write("{0}", message);
        Console.WriteLine("\u001b[00m");
    }
}
