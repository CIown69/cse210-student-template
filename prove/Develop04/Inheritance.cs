using System;
using System.Collections.Generic;
using System.Buffers;
using System.Threading;
using System.Threading.Tasks;

public abstract class Activity
{
    protected string name;
    protected string description;
    protected int duration;

    protected Activity(string name, string description)
    {
        this.name = name;
        this.description = description;
    }

    public void Run()
    {
        DisplayStartingMessage();
        PrepareForActivity();
        PerformActivity();
        DisplayEndingMessage();
    }

    protected void DisplayStartingMessage()
    {
        Console.WriteLine($"Activity: {name}");
        Console.WriteLine(description);
        Console.WriteLine($"Please set the duration of the activity in seconds: ");
        duration = int.Parse(Console.ReadLine());
        Console.WriteLine($"Get ready to begin in 3 seconds...");
        Thread.Sleep(3000);
    }

    protected void PrepareForActivity()
    {
        // Can implement some animation or countdown here
        Console.WriteLine("Starting activity in:");
        for (int i = 3; i > 0; i--)
        {
            Console.WriteLine(i);
            Thread.Sleep(1000);
        }
    }

    protected abstract void PerformActivity();

    protected void DisplayEndingMessage()
    {
        Console.WriteLine($"Good job! You have completed the {name}.");
        Console.WriteLine($"Time taken: {duration} seconds");
        Console.WriteLine($"Finishing in 3 seconds...");
        Thread.Sleep(3000);
    }
}
public class BreathingActivity : Activity
{
    public BreathingActivity() : base("Breathing Activity", "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
    {
    }

    protected override void PerformActivity()
    {
        int countdown = duration;
        while (countdown > 0)
        {
            Console.WriteLine("Breathe in...");
            Thread.Sleep(2000);
            Console.WriteLine("Breathe out...");
            Thread.Sleep(2000);
            countdown -= 4; // Each breathe in/out cycle takes 4 seconds
        }
    }
}
public class ReflectionActivity : Activity
{
    private string[] prompts = {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private string[] questions = {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base("Reflection Activity", "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
    {
    }

    protected override void PerformActivity()
    {
        Random rand = new Random();
        int countdown = duration;
        while (countdown > 0)
        {
            string prompt = prompts[rand.Next(prompts.Length)];
            Console.WriteLine(prompt);

            foreach (var question in questions)
            {
                Console.WriteLine(question);
                Thread.Sleep(3000); // Pause between questions
            }

            countdown -= (prompts.Length * questions.Length * 3); // Each prompt-question set takes approximately 3 seconds
        }
    }
}

public class ListingActivity : Activity
{
    private string[] prompts = {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() : base("Listing Activity", "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
    {
    }

    protected override void PerformActivity()
    {
        Random rand = new Random();
        int intitialDuraction = duration;
        DateTime startTime = DateTime.Now;
        int countdown = duration;
        string prompt = prompts[rand.Next(prompts.Length)];
        
        Console.WriteLine(prompt);
        Console.WriteLine($"Starting to list items in {prompt} in 5 seconds...");
        Thread.Sleep(5000); 

        List<string> listedItems = new List<string>();

        while (countdown > 0)
        {
            Console.WriteLine($"Time left: {countdown} seconds");

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            Task<string> userInputTask = GetUserInputAsync(token);
            Task delayTask = Task.Delay(countdown * 1000, token);

            Task completedTask = Task.WhenAny(userInputTask, delayTask).Result;
                
            if (completedTask == userInputTask)
            {
                string userInput = userInputTask.Result;    
                
                if (userInput.ToLower() == "done")
                    break;

                listedItems.Add(userInput);
                countdown = intitialDuraction - (int)(DateTime.Now - startTime).TotalSeconds;
            }
            else if (completedTask == delayTask)
            {
                Console.WriteLine("Time is up. Exiting input phase.");
                break;
            }
        }
    }
        private async Task<string> GetUserInputAsync(CancellationToken token)
    {
        try
        {
            Console.WriteLine("Enter an item (or 'done' to finish):");

            // Read user input asynchronously
            return await Task.Run(() =>
            {
                string userInput = Console.ReadLine();
                token.ThrowIfCancellationRequested();
                return userInput;
            }, token);
        }
        catch (OperationCanceledException)
        {
            return ""; // Return empty string in case of cancellation
        }
    }
}
class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Choose an activity:");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    BreathingActivity breathingActivity = new BreathingActivity();
                    breathingActivity.Run();
                    break;
                case 2:
                    ReflectionActivity reflectionActivity = new ReflectionActivity();
                    reflectionActivity.Run();
                    break;
                case 3:
                    ListingActivity listingActivity = new ListingActivity();
                    listingActivity.Run();
                    break;
                case 4:
                    Console.WriteLine("Exiting program...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please choose again.");
                    break;
            }

            Console.WriteLine();
        }
    }
}