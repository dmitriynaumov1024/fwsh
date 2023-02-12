namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

public class ExternalIdFactory : Factory<string>
{
    Random random = new Random();
    HashSet<string> Ids = new HashSet<string>();

    string firstLetters = "ABCDEFGHJKL";

    public override string Next()
    {
        string result = null;
        do {
            result = String.Format (
                "{0}-{1}-{2:D04}", 
                random.Choice(firstLetters), 
                random.Next(100, 1000), 
                random.Next(1000, 9999)
            );
        } 
        while (this.Ids.Contains(result));
        
        this.Ids.Add(result);
        return result;
    }

}
