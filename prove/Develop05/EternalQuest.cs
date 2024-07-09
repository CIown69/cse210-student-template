using System;
using System.Collections.Generic;
using System.IO;

abstract class Goal
{
    public string Name { get; private set; }
    public int Points { get; protected set; }
    public bool IsCompleted { get; protected set; }

    protected Goal(string name, int points, bool isCompleted = false)
    {
        Name = name;
        Points = points;
        IsCompleted = isCompleted;
    }

    public abstract void RecordEvent();
    public abstract string GetStatus();
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points, bool isCompleted = false) : base(name, points, isCompleted)
    {
    }

    public override void RecordEvent()
    {
        if (!IsCompleted)
        {
            IsCompleted = true;
        }
    }

    public override string GetStatus()
    {
        return IsCompleted ? "[X]" : "[ ]";
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int pointsPerEvent) : base(name, pointsPerEvent)
    {
    }

    public override void RecordEvent()
    {
        // Eternal goals are never marked as completed
    }

    public override string GetStatus()
    {
        return "[ ]";
    }
}

class ChecklistGoal : Goal
{
    public int RequiredCount { get; private set; }
    public int CurrentCount { get; private set; }
    public int BonusPoints { get; private set; }

    public ChecklistGoal(string name, int pointsPerEvent, int requiredCount, int bonusPoints, int currentCount = 0, bool isCompleted = false)
        : base(name, pointsPerEvent, isCompleted)
    {
        RequiredCount = requiredCount;
        CurrentCount = currentCount;
        BonusPoints = bonusPoints;
    }

    public override void RecordEvent()
    {
        if (!IsCompleted)
        {
            CurrentCount++;
            if (CurrentCount >= RequiredCount)
            {
                IsCompleted = true;
            }
        }
    }

    public override string GetStatus()
    {
        return IsCompleted ? "[X]" : $"[ ] Completed {CurrentCount}/{RequiredCount} times";
    }
}

class Program
{
    private static List<Goal> goals = new List<Goal>();
    private static int userScore = 0;

    static void Main(string[] args)
    {
        LoadData();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Eternal Quest");
            Console.WriteLine("1. Create New Goal");
            Console.WriteLine("2. Record Event");
            Console.WriteLine("3. Show Goals");
            Console.WriteLine("4. Show Score");
            Console.WriteLine("5. Save and Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateNewGoal();
                    break;
                case "2":
                    RecordEvent();
                    break;
                case "3":
                    ShowGoalsWithPause();
                    break;
                case "4":
                    ShowScore();
                    break;
                case "5":
                    SaveData();
                    return;
                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private static void CreateNewGoal()
    {
        Console.Clear();
        Console.WriteLine("Create New Goal");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        Console.Write("Choose goal type: ");
        string type = Console.ReadLine();

        Console.Write("Enter goal name: ");
        string name = Console.ReadLine();

        switch (type)
        {
            case "1":
                Console.Write("Enter points: ");
                int simplePoints = int.Parse(Console.ReadLine());
                goals.Add(new SimpleGoal(name, simplePoints));
                break;
            case "2":
                Console.Write("Enter points per event: ");
                int eternalPoints = int.Parse(Console.ReadLine());
                goals.Add(new EternalGoal(name, eternalPoints));
                break;
            case "3":
                Console.Write("Enter points per event: ");
                int checklistPoints = int.Parse(Console.ReadLine());
                Console.Write("Enter required count: ");
                int requiredCount = int.Parse(Console.ReadLine());
                Console.Write("Enter bonus points: ");
                int bonusPoints = int.Parse(Console.ReadLine());
                goals.Add(new ChecklistGoal(name, checklistPoints, requiredCount, bonusPoints));
                break;
            default:
                Console.WriteLine("Invalid goal type. Try again.");
                break;
        }
    }

    private static void RecordEvent()
    {
        Console.Clear();
        Console.WriteLine("Record Event");
        ShowGoals();

        Console.Write("Enter goal number: ");
        int goalNumber = int.Parse(Console.ReadLine());

        if (goalNumber < 1 || goalNumber > goals.Count)
        {
            Console.WriteLine("Invalid goal number. Try again.");
            return;
        }

        Goal goal = goals[goalNumber - 1];
        goal.RecordEvent();
        userScore += goal.Points;

        if (goal is ChecklistGoal checklistGoal && checklistGoal.IsCompleted)
        {
            userScore += checklistGoal.BonusPoints;
        }

        Console.WriteLine("Event recorded. Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void ShowGoals()
    {
        Console.Clear();
        Console.WriteLine("Goals:");
        for (int i = 0; i < goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {goals[i].GetStatus()} {goals[i].Name}");
        }
        
    }

     private static void ShowGoalsWithPause()
    {
        ShowGoals();
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void ShowScore()
    {
        Console.Clear();
        Console.WriteLine($"Your score: {userScore}");
        Console.WriteLine("Press any key to return to the menu...");
        Console.ReadKey();
    }

    private static void SaveData()
    {
        using (StreamWriter writer = new StreamWriter("data.txt"))
        {
            writer.WriteLine(userScore);
            foreach (Goal goal in goals)
            {
                if (goal is ChecklistGoal checklistGoal)
                {
                    writer.WriteLine($"{goal.GetType().Name}|{goal.Name}|{goal.Points}|{goal.IsCompleted}|{checklistGoal.CurrentCount}|{checklistGoal.RequiredCount}|{checklistGoal.BonusPoints}");
                }
                else
                {
                    writer.WriteLine($"{goal.GetType().Name}|{goal.Name}|{goal.Points}|{goal.IsCompleted}");
                }
            }
        }
    }

    private static void LoadData()
    {
        if (File.Exists("data.txt"))
        {
            string[] lines = File.ReadAllLines("data.txt");
            userScore = int.Parse(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split('|');
                string type = parts[0];
                string name = parts[1];
                int points = int.Parse(parts[2]);
                bool isCompleted = bool.Parse(parts[3]);

                switch (type)
                {
                    case "SimpleGoal":
                        goals.Add(new SimpleGoal(name, points, isCompleted ));
                        break;
                    case "EternalGoal":
                        goals.Add(new EternalGoal(name, points));
                        break;
                    case "ChecklistGoal":
                        int currentCount = int.Parse(parts[4]);
                        int requiredCount = int.Parse(parts[5]);
                        int bonusPoints = int.Parse(parts[6]);
                        goals.Add(new ChecklistGoal(name, points, requiredCount, bonusPoints, currentCount, isCompleted));
                        break;
                }
            }
        }
    }
}
