namespace Fwsh.Common;

using System;

public class Dimensions
{
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public string ToConventionalString()
    {
        return String.Format("{0}x{1}x{2}", this.Length, this.Width, this.Height);
    }

    public static bool TryParse (string source, out Dimensions outResult)
    {
        string[] dims = source.Split('x');
        
        int length = 0, 
            width = 0, 
            height = 0;
        
        bool success = false;

        if (dims.Length > 0) success |= int.TryParse(dims[0], out length);
        
        if (dims.Length > 1) success &= int.TryParse(dims[1], out width);
        
        if (dims.Length > 2) success &= int.TryParse(dims[2], out height);
        
        if (success) outResult = new Dimensions { 
            Length = length,
            Width = width,
            Height = height
        };
        else outResult = null;

        return success;
    }

    public static Dimensions Parse (string source)
    {
        var result = new Dimensions();
        
        if (Dimensions.TryParse(source, out result)) {
            return result;
        }
        else {
            throw new ArgumentException("expected 1 to 3 integers separated by 'x'");
        }
    }
}
