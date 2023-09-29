using System;

namespace lab1
{
    class Lab2_Part_1
    {
        static void Main()
        {
            Console.WriteLine("Choose an exercise to run:");
            Console.WriteLine("1. Sum_from_1_to_n");
            Console.WriteLine("2. Sum_of_arr");
            Console.WriteLine("3. All values to zero");
            Console.WriteLine("4. Count unsigned Types");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    SumFrom1ToN();
                    break;
                case "2":
                    SumOfArray();
                    break;
                case "3":
                    AllValuesToZero();
                    break;
                case "4":
                    CountUnsignedTypes();
                    break;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }
        }

        static void SumFrom1ToN()
        {
            Console.WriteLine("Insert an n value:");
            string userInput = Console.ReadLine();

            if (int.TryParse(userInput, out int number))
            {
                int sum = 0;
                for (int i = 0; i <= number; i++)
                {
                    sum += i;
                }
                Console.WriteLine($"Resulted sum is = {sum}");
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        static void SumOfArray()
        {
            Console.WriteLine("Enter numbers separated by commas:");
            string userInput = Console.ReadLine();

            string[] stringNumbers = userInput.Split(',');

            int[] numbers = new int[stringNumbers.Length];
            for (int i = 0; i < stringNumbers.Length; i++)
            {
                if (!int.TryParse(stringNumbers[i].Trim(), out numbers[i]))
                {
                    Console.WriteLine($"Invalid number at position {i + 1}");
                    return;
                }
            }

            int sum = 0;
            for (int i = 0; i < numbers.Length; i++)
            {
                sum += numbers[i];
            }
            Console.WriteLine($"Sum of numbers: {sum}");
        }

        static void AllValuesToZero()
        {
            Console.WriteLine("Enter numbers separated by commas:");
            string userInput = Console.ReadLine();

            string[] numberStrings = userInput.Split(',');

            int[] ints = new int[numberStrings.Length];
            for (int i = 0; i < numberStrings.Length; i++)
            {
                if (!int.TryParse(numberStrings[i].Trim(), out ints[i]))
                {
                    Console.WriteLine($"Invalid number at position {i + 1}");
                    return;
                }
            }

            Set0(ints);
            Console.WriteLine("All values have been set to zero.");
        }

        static void SetStep(int[] arr, int index, int value)
        {
            if (index >= 0 && index < arr.Length)
            {
                arr[index] = value;
            }
        }

        static void Set0(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                SetStep(arr, i, 0);
            }
        }

        static void CountUnsignedTypes()
        {
            int ushortCount = 0;
            int uintCount = 0;
            int ulongCount = 0;

            Console.WriteLine("Enter values (empty line to stop):");

            while (true)
            {
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                if (ushort.TryParse(input, out ushort _))
                {
                    ushortCount++;
                }
                else if (uint.TryParse(input, out uint _))
                {
                    uintCount++;
                }
                else if (ulong.TryParse(input, out ulong _))
                {
                    ulongCount++;
                }
            }

            Console.WriteLine($"Number of ushort values: {ushortCount}");
            Console.WriteLine($"Number of uint values: {uintCount}");
            Console.WriteLine($"Number of ulong values: {ulongCount}");
        }

    }
}
