// See https://aka.ms/new-console-template for more information
using System;

class Week1{
    enum Days {Sat, Sun, Mon, Tue, Wed, Thu, Fri};

    struct Person{
        public string fName, lName;
        public Person(string fName, string lName){
            this.fName = fName;
            this.lName = lName;
        }


    }

    static void changeNumber(int n){
        n = 100;
    }
        
    static void Main(){
        Console.WriteLine("Hello, World!");
        Console.WriteLine(Days.Mon);
        Person p = new Person("John", "Smith");
        Person q = p;
        p.fName = "Will";

        // string s1 = "First";
        // string s2 = s1;
        // s1 = "Second";
        //Console.WriteLine(s2);
        int n = 0;
        changeNumber(n);
        Console.WriteLine(n);
        Console.WriteLine(q.fName);

    }
}