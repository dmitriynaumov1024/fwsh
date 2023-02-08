namespace Fwsh.MockData;

using System;

public class PasswordFactory : Factory<string>
{
    Random random = new Random();

    static string passwordChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789!/;:()*&^%$#@?_-+=,.'[]";
    
    public override string Next()
    {
        char[] result = new char[random.Next(8, 17)];
        for (int i=0; i<result.Length; i++) {
            result[i] = random.Choice(passwordChars);
        }
        return new String(result);
    }
}
