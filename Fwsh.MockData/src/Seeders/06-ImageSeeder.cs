namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

using Fwsh.Utils;
using Fwsh.Database;

public class ImageSeeder : Seeder
{
    public override void Seed (FwshDataContext context) 
    {
        string srcPath = env.get("IMAGES_DIR");
        string destPath = env.get("UPLOAD_DIR");
        Directory.CreateDirectory(destPath);
        foreach (var filePath in Directory.EnumerateFiles(srcPath)) {
            File.Copy(filePath, Path.Join(destPath, Path.GetFileName(filePath)));
        }
    }
}
