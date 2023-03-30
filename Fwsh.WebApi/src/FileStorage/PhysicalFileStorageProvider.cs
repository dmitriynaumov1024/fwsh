namespace Fwsh.WebApi.FileStorage;

using System;
using System.IO;

public class PhysicalFileStorageProvider : FileStorageProvider 
{
    public string BasePath { get; private set; }

    public PhysicalFileStorageProvider (string basePath) 
    {
        this.BasePath = basePath;
        Directory.CreateDirectory(basePath);
    }

    protected string ResolvePath (string relativePath)
    {
        return Path.Join(this.BasePath, relativePath);
    }

    public override bool Exists (string targetPath)
    {
        return File.Exists(ResolvePath(targetPath));
    }

    public override bool TrySave (Stream stream, string targetPath)
    {
        targetPath = ResolvePath(targetPath);
        
        if (Exists(targetPath)) return false;        

        try {
            using (var target = File.OpenWrite(targetPath)) {
                stream.CopyTo(target);
            }
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    public override bool TrySaveOrReplace (Stream stream, string targetPath)
    {
        targetPath = ResolvePath(targetPath);
        
        if (Exists(targetPath)) File.Delete(targetPath);

        try {
            using (var target = File.OpenWrite(targetPath)) {
                stream.CopyTo(target);
            }
            return true;
        }
        catch (Exception) {
            return false;
        }
    }

    public override bool TryDelete (string targetPath)
    {
        targetPath = ResolvePath(targetPath);

        if (! Exists(targetPath)) return false;

        try {
            File.Delete(targetPath);
            return true;
        }
        catch (Exception) {
            return false;
        }
    }
}
