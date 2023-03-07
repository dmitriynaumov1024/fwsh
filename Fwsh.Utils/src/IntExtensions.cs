namespace Fwsh.Utils;

public static class IntExtensions
{
    public static bool InRange (this int number, int lowerBound, int upperBound)
    {
        return lowerBound <= number && number <= upperBound;
    }
}
