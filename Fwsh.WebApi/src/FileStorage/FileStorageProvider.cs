namespace Fwsh.WebApi.FileStorage;

using System;
using System.IO;

public abstract class FileStorageProvider
{
    public abstract bool Exists (string targetPath);
    public abstract bool TrySave (Stream stream, string targetPath);
    public abstract bool TrySaveOrReplace (Stream stream, string targetPath);
    public abstract bool TryDelete (string targetPath);
}
