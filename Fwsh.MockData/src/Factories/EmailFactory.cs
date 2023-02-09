namespace Fwsh.MockData;

using System;
using System.Collections.Generic;
using System.Linq;

public class EmailFactory : Factory<string>
{
    Random random = new Random();
    HashSet<string> Emails = new HashSet<string>();

    public override string Next()
    {
        string alphabetChars = "qwertyuiopasdfghjklzxcvbnm";
        string result = null;
        do {
            result = String.Format (
                "{0}{1:D04}@email.com", 
                String.Join ( "",
                    Enumerable.Range(0, random.Next(4, 8))
                        .Select(_ => random.Choice(alphabetChars))
                ),
                random.Next (1, 9999)
            );
        } 
        while (this.Emails.Contains(result));
        this.Emails.Add(result);
        return result;
    }
}
