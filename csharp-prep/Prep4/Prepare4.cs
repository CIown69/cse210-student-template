using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        List<int> numbers = new List<int>();
    
        int userNumber = -1;

        do
        {
            Console.Write("Enter a number (0 to quit): ");

            string userResponse = Console.ReadLine();
            if (!int.TryParse(userResponse, out userNumber))
            {
                Console.WriteLine("Invalid input. Please enter a valid integer.");
                continue;
            }

            if (userNumber != 0)
            {
                numbers.Add(userNumber);
            }
        } while (userNumber != 0);

        if (numbers.Count == 0)
        {
            Console.WriteLine("No numbers entered. Exiting.");
            return;
        }

        int sum = 0;
        foreach (int number in numbers)
        {
            sum += number;
        }

        Console.WriteLine($"The sum is: {sum}");

        float average = ((float)sum) / numbers.Count;
        Console.WriteLine($"The average is: {average}");

        
        int max = numbers[0];

        foreach (int number in numbers)
        {
            if (number > max)
            {
                max = number;
            }
        }

        Console.WriteLine($"The max is: {max}");

        int smallestPositive = int.MaxValue;
        foreach (int number in numbers)
        {
            if (number > 0 && number < smallestPositive)
            {
                smallestPositive = number;
            }
        }
        if (smallestPositive == int.MaxValue)
        {
            Console.WriteLine("No positive numbers entered.");
        }
        else
        {
            Console.WriteLine($"The smallest positive number closest to zero is: {smallestPositive}");
        }

        numbers.Sort();
        Console.WriteLine("Sorted numbers:");
        foreach (int number in numbers)
        {
            Console.Write(number + " ");
        }
        Console.WriteLine();
    }
}