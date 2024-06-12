using System;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Enter the secret number (between 1 and 100): ");
        string secretInput = Console.ReadLine();
        int Number;

        while (!int.TryParse(secretInput, out Number) || Number < 1 || Number > 100)
        {
            Console.WriteLine("Please enter a valid number between 1 and 100.");
            secretInput = Console.ReadLine();
        }

        int guess = -1;

        while (guess != Number)
        {
            Console.Write("What is your guess? ");
            string guessInput = Console.ReadLine();

            // Validate the guess input
            if (!int.TryParse(guessInput, out guess))
            {
                Console.WriteLine("Please enter a valid number.");
                continue;
            }

            if (Number > guess)
            {
                Console.WriteLine("Higher");
            }
            else if (Number < guess)
            {
                Console.WriteLine("Lower");
            }
            else
            {
                Console.WriteLine("You guessed it!");
            }
        }
    }
}