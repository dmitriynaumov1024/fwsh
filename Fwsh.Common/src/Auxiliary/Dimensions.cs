namespace Fwsh.Common;

using System;

public class Dimensions
{
    public int Width { get; set; }
    public int Length { get; set; }
    public int Height { get; set; }

    public string MeasureUnit { get; set; }

    public static Dimensions ParseWLH (string dimString) 
    {
        if (dimString == null || dimString.Length < 2) return null;

        string[] wlh = dimString.Split(';');
        
        return new Dimensions {
            Width  = wlh.Length > 0 ? int.Parse(wlh[0]) : 0,
            Length = wlh.Length > 1 ? int.Parse(wlh[1]) : 0,
            Height = wlh.Length > 2 ? int.Parse(wlh[2]) : 0,
            MeasureUnit = wlh.Length > 3 && MeasureUnits.Contains(wlh[3]) ? wlh[3] : null
        };
    }

    public static string StringifyWLH (Dimensions dim) 
    {
        return String.Format ("{0};{1};{2};{3}", dim.Width, dim.Length, dim.Height, dim.MeasureUnit);
    }
}
