// See https://aka.ms/new-console-template for more information

// Assignment 1


using System;

public enum Weekdays{
    Sunday = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6
}

public enum DayType{
    Workday,
    Workend
}

public class Program{

    public static DayType WhatDay(Weekdays day){
        swithch(day)
        {

        }
    }

    public static Weekdays NextDay(Weekdays currentDay){
        return (Weekdays)(((int)currentDay + 1) % 7);
    }

public static void Main(string[] args){
    Weekdays today = Weekdays.Friday;
    Weekdays tomorrow = NextDay(today);

    Console.WriteLine($"Today is {today}");
    Console.WriteLine($"Today is {tomorrow}");
}
}


// Assignment 2

