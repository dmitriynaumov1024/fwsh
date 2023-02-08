namespace Fwsh.MockData;

using System;
using System.Collections.Generic;

public class FullNameFactory : Factory<Tuple<string, string, string>>
{
    Random random = new Random();

    static string[] Surnames = new[]{
        "Іваненко", "Петренко", "Юхименко", "Коваль", 
        "Мельник", "Сидоренко", "Терненко", "Євтушенко",
        "Науменко", "Мельниченко", "Тимченко", "Сергієнко", 
        "Васильчук", "Іванчук", "Парасюк", "Тютюнник"
    };

    static string[] MaleNames = new[]{
        "Сергій", "Іван", "Петро", "Максим", 
        "Кирило", "Владислав", "Вадим", "Богдан", 
        "Дмитро", "Даниїл", "Микола", "Степан"
    };

    static string[] FemaleNames = new[]{
        "Катерина", "Анна", "Світлана", "Олена", 
        "Ольга", "Тетяна", "Єлизавета", "Оксана"
    };

    static string[] MalePatronyms = new[]{
        "Сергійович", "Іванович", "Петрович", "Юрійович", 
        "Федорович", "Дмитрович", "Андрійович", "Богданович"
    };

    static string[] FemalePatronyms = new[]{
        "Віталіївна", "Петрівна", "Сергіївна", "Андріївна",
        "Євгенівна", "Федорівна", "Вадимівна", "Степанівна"
    };

    public override Tuple<string, string, string> Next()
    {
        bool halfProbability = random.Next(0, 100) >= 50;
        
        if (halfProbability) {
            return new (
                random.Choice(Surnames),
                random.Choice(MaleNames),
                random.Choice(MalePatronyms)
            );
        }
        else {
            return new (
                random.Choice(Surnames),
                random.Choice(FemaleNames),
                random.Choice(FemalePatronyms)
            );
        }
    }
}
