namespace Fwsh.MockData;

using System;
using System.Collections.Generic;

public class PhoneNumberFactory : Factory<string>
{
    Random random = new Random();
    HashSet<string> PhoneNumbers = new HashSet<string>();

    public override string Next()
    {
        string result = null;
        do {
            result = String.Format (
                "+38{0:D03}{1:D03}{2:D04}", 
                random.Next(63, 99),
                random.Next(1, 999),
                random.Next(1, 9999)
            );
        } 
        while (this.PhoneNumbers.Contains(result));
        this.PhoneNumbers.Add(result);
        return result;
    }
}
