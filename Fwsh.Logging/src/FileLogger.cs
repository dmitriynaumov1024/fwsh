namespace Fwsh.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

public class FileLogger : Logger
{
    private StreamWriter writer;

    public override void Log (string message, params object[] args)
    {
        writer.Write("[{0:HH:mm:ss}][i]: ", DateTime.Now);
        if (args.Length > 0) writer.WriteLine(message, args);
        else writer.WriteLine(message);
        writer.Flush();
    }

    public override void Warn (string message, params object[] args)
    {
        writer.Write("[{0:HH:mm:ss}][!]: ", DateTime.Now);
        if (args.Length > 0) writer.WriteLine(message, args);
        else writer.WriteLine(message);
        writer.Flush();
    }

    public override void Error (string message, params object[] args)
    {
        writer.Write("[{0:HH:mm:ss}][x]: ", DateTime.Now);
        if (args.Length > 0) writer.WriteLine(message, args);
        else writer.WriteLine(message);
        writer.Flush();
    }

    static Dictionary<string, FileLogger> existingLoggers = new Dictionary<string, FileLogger>();

    public static FileLogger To (string path) 
    {
        path = Path.GetFullPath(path);

        if (!existingLoggers.ContainsKey(path)) {
            existingLoggers[path] = new FileLogger { 
                writer = new StreamWriter(path, true, Encoding.UTF8)
            };
        }
        
        return existingLoggers[path];
    }

}
