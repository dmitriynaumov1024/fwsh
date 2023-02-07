namespace Fwsh.MockData;

using System;

public class PasswordFactory : Factory<string>
{
    Random random = new Random();
    
    public override string Next()
    {
        string passwordChars = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789!/;:()*&^%$#@?_-+=,.'[]";
        char[] result = new char[random.Next(8, 19)];
        for (int i=0; i<result.Length; i++) {
            result[i] = passwordChars[random.Next(0, passwordChars.Length)];
        }
        return new String(result);
    }
}
