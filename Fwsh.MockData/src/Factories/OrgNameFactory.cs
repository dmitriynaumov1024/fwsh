namespace Fwsh.MockData;

using System;

public class OrgNameFactory : Factory<string>
{
    Random random = new Random();

    static string[] orgNames = new[] {
        "ABCF",
        "Дніпро запчастини",
        "ЄвроТекстиль",
        "ТОВ \"Полісся\"",
        "ТОВ \"Мальва\""
    };
    
    public override string Next()
    {
        return random.Choice(orgNames);
    }
}
