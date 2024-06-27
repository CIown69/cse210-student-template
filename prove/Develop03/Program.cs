using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Scripture
{
    private readonly List<Word> words = new List<Word>();
    private readonly ScriptureReference reference;

    public Scripture(string reference, string text)
    {
        this.reference = new ScriptureReference(reference);

        string[] wordArray = text.Split(new[] { ' ', ',', '.', ':', ';', '!', '?' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (var word in wordArray)
        {
            words.Add(new Word(word));
        }
    }

    public void Display()
    {
        Console.Clear();
        Console.WriteLine($"{reference}");

        foreach (var word in words)
        {
            Console.Write(word + " ");
        }

        Console.WriteLine("\n\nPress Enter to hide words or type 'quit' to exit:");
        string input = Console.ReadLine().Trim().ToLower();

        if (input == "quit")
        {
            Environment.Exit(0);
        }
        else if (input == "")
        {
            HideRandomWord();
        }
        else
        {
            Display();
        }
    }

    private void HideRandomWord()
    {
        var visibleWords = words.Where(w => !w.IsHidden).ToList();
        if (visibleWords.Count > 0)
        {
            Random random = new Random();
            int index = random.Next(visibleWords.Count);
            visibleWords[index].Hide();
            Display();
        }
        else
        {
            Console.WriteLine("\nAll words are hidden. Press Enter to exit.");
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}

public class Word
{
    private readonly string text;
    private bool isHidden;
    public string Text => text;
    public bool IsHidden => isHidden;

    public Word(string text)
    {
        this.text = text;
        isHidden = false;
    }

    public void Hide()
    {
        isHidden = true;
    }

    public override string ToString()
    {
        return isHidden ? "______" : text;
    }
}

public class ScriptureReference
{
    private string _Book;
    private int _Chapter;
    private int _StartVerse;
    private int _EndVerse;

    public string Book => _Book;
    public int Chapter => _Chapter;
    public int StartVerse => _StartVerse;
    public int EndVerse => _EndVerse;

    public ScriptureReference(string reference)
    {
        var match = Regex.Match(reference, @"^(?<book>[\w\s]+)\s+(?<chapter>\d+):(?<startVerse>\d+)(?:-(?<endVerse>\d+))?$");
        if (!match.Success)
        {
            throw new ArgumentException("Invalid Scripture reference format.");
        }

        _Book = match.Groups["book"].Value;
        _Chapter = int.Parse(match.Groups["chapter"].Value);
        _StartVerse = int.Parse(match.Groups["startVerse"].Value);
        _EndVerse = match.Groups["endVerse"].Success ? int.Parse(match.Groups["endVerse"].Value) : _StartVerse;
    }

    public override string ToString()
    {
        if (_StartVerse == _EndVerse)
            return $"{_Book} {_Chapter}:{_StartVerse}";
        else
            return $"{_Book} {_Chapter}:{_StartVerse}-{_EndVerse}";
    }
}

class Program
{
    static void Main()
    {
        var scriptures = new Dictionary<string, string>
        {
            { "Proverbs 3:5-6", "Trust in the LORD with all your heart and lean not on your own understanding" },
            { "John 3:16", "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life" }
        };

        Console.WriteLine("Select a scripture to display:");
        int index = 1;
        foreach (var reference in scriptures.Keys)
        {
            Console.WriteLine($"{index}. {reference}");
            index++;
        }

        Console.WriteLine("Enter the number of your choice:");
        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= scriptures.Count)
        {
            var selectedReference = scriptures.Keys.ElementAt(choice - 1);
            var text = scriptures[selectedReference];

            Scripture scripture = new Scripture(selectedReference, text);
            scripture.Display();
        }
        else
        {
            Console.WriteLine("Invalid choice. Exiting.");
        }
    }
}