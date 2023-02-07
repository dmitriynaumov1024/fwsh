namespace Fwsh.MockData;

using System;
using System.Collections.Generic;

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
                "{0}{1}{2}{3}{4}{5:D04}@email.com", 
                alphabetChars[random.Next(0, alphabetChars.Length)],
                alphabetChars[random.Next(0, alphabetChars.Length)],
                alphabetChars[random.Next(0, alphabetChars.Length)],
                alphabetChars[random.Next(0, alphabetChars.Length)],
                alphabetChars[random.Next(0, alphabetChars.Length)],
                random.Next(1, 9999)
            );
        } 
        while (this.Emails.Contains(result));
        this.Emails.Add(result);
        return result;
    }
}
