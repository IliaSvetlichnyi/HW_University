// See https://aka.ms/new-console-template for more information
using System;


class Program{

    static void Main(){

        Console.WriteLine("Введите свою дату рождения:");
        string date = Console.ReadLine();


        DateTime.TryParse(date, out DateTime birthDate);

        int age = (DateTime.Now.Date - birthDate.Date).Days / 365;
        Console.WriteLine($"Ваш возраст: {age}");

        DateTime nextBirthDate = new DateTime(DateTime.Now.Year, birthDate.Month, birthDate.Day);

        if (nextBirthDate < DateTime.Now)
        {
            nextBirthDate = nextBirthDate.AddYears(1);
        }

        int DaysUntillNextBirthDay = (nextBirthDate - DateTime.Now.Date).Days;   
        Console.WriteLine($"До следующего день рождения осталось: {DaysUntillNextBirthDay}");
    }
}
