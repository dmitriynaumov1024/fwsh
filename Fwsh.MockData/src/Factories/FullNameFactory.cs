namespace Fwsh.MockData;

using System;
using System.Collections.Generic;

public class FullNameFactory : Factory<Tuple<string, string, string>>
{
    Random random = new Random();

    string[] Surnames = new[]{
        "Іваненко", "Петренко", "Юхименко", "Коваль", 
        "Мельник", "Сидоренко", "Терненко", "Євтушенко" 
    };

    string[] MaleNames = new[]{
        "Сергій", "Іван", "Петро", "Максим", "Кирило",
        "Владислав", "Вадим", "Богдан", "Дмитро", "Даниїл"
    };

    string[] FemaleNames = new[]{
        "Катерина", "Анна", "Світлана", "Олена", 
        "Ольга", "Тетяна", "Єлизавета", "Оксана"
    };

    string[] MalePatronyms = new[]{
        "Сергійович", "Іванович", "Петрович", "Юрійович", 
        "Федорович", "Дмитрович", "Андрійович", "Богданович"
    };

    string[] FemalePatronyms = new[]{
        "Віталіївна", "Петрівна", "Сергіївна", "Андріївна",
        "Євгенівна", "Федорівна", "Вадимівна", "Степанівна"
    };

    public override Tuple<string, string, string> Next()
    {
        bool halfProbability = random.Next(0, 100) >= 50;
        
        if (halfProbability) {
            return new (
                Surnames[random.Next(Surnames.Length)],
                MaleNames[random.Next(MaleNames.Length)],
                MalePatronyms[random.Next(MalePatronyms.Length)]
            );
        }
        else {
            return new (
                Surnames[random.Next(Surnames.Length)],
                FemaleNames[random.Next(FemaleNames.Length)],
                FemalePatronyms[random.Next(FemalePatronyms.Length)]
            );
        }
    }
}
