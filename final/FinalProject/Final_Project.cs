using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace TextChatSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatBot bot = new ChatBot();

            Console.WriteLine("Welcome to the Chat System! Say 'goodbye' to end the conversation.");
            Console.WriteLine("Feel free to ask about my 'thoughts' about you!");
            Console.WriteLine("If you wish to recall something please ask to do so using this format: (recall YYYY-MM-DD [context])")
            Console.WriteLine("Now, What would you like to call me?");

            while (true)
            {
                Console.Write("You: ");
                string nameInput = Console.ReadLine().Trim();

                if (!string.IsNullOrEmpty(nameInput))
                {
                    bot.SetName(nameInput);
                    Console.WriteLine($"{bot.Name}: I love it! From now on you can call me {bot.Name}. What is your name?");
                    break;
                }
                else
                {
                    Console.WriteLine("Chatbot: That doesn't seem like an appropiate name...");   
                }
            }

            while (true)
            {
                Console.WriteLine("You: ");
                string userNameInput = Console.ReadLine().Trim();

                if (!string.IsNullOrEmpty(userNameInput))
                {
                    bot.SetUserName(userNameInput);
                    Console.WriteLine($"{bot.Name}: What a cute name, it's a pleasure to meet you {bot.UserName}.");
                    break;
                }
                else
                {
                    Console.WriteLine($"{bot.Name}: Are you sure that's your name?");
                }
            }

            while (true)
            {
                Console.Write($"{bot.UserName}: ");
                string userInput = Console.ReadLine();

                if (userInput.ToLower() == "goodbye")
                {
                    Console.WriteLine($"{bot.Name}: Ok, Goodbye for now {bot.UserName}!");
                    break;
                }

                if (userInput.ToLower() == "thoughts")
                {
                    string thoughts = bot.GetThoughts();
                    Console.WriteLine($"{bot.Name}: " + thoughts);
                    continue;
                }

                if (userInput.ToLower().Contains("recall"))
                {
                    var (recallDate, context) = bot.ParseUserInput(userInput);
                    if (recallDate != null)
                    {
                        string recallMessages = bot.RecallMessages(recallDate, context);
                        Console.WriteLine($"{bot.Name}: Here are the messages I recall from {recallDate} about {context}:");
                        Console.WriteLine(recallMessages);
                    }
                    else
                    {
                        Console.WriteLine($"{bot.Name}: Please rewrite in this format: 'recall YYYY-MM-DD [context].");
                    }
                    continue;
                }

                string response = bot.GetResponse(userInput);
                Console.WriteLine($"{bot.Name}: " + response);

            }                   
        }
    }

    public class ChatBot
    {
        private Dictionary<string, string> predefinedResponses;
        private Dictionary<string, string> toneKeywords;
        private Dictionary<string, string> conjoinedToneKeywords;
        private HashSet<string> positiveKeywords;
        private HashSet<string> negativeKeywords;
        private int positiveCount;
        private int negativeCount;
        private string filePath = "C:\\Users\\Conno\\CSE 210 Sandbox\\cse210-student-template\\final\\FinalProject\\recall.txt";
        public string Name { get; private set; }
        public string UserName { get; private set; }

        public ChatBot()
        {
            // Initialize predefined responses
            InitializePredefinedResponses();

            // Initialize tone keywords
            InitializeToneKeywords();

            // Initialize conjoined tone keywords
            InitializeConjoinedToneKeywords();

            positiveCount = 0;
            negativeCount = 0;
            Name = "ChatBot";
            UserName = "User";
        }

        public void SetName(string name)
        {
            Name = name;
            InitializePredefinedResponses();
        }

        public void SetUserName(string userName)
        {
            UserName = userName;
            InitializePredefinedResponses();
        }
    
        private void InitializePredefinedResponses()
        {
            // Initialize predefined responses
            predefinedResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "hello", "Hi there! How can I help you today?" },
                { "Its good to meet you too", "Indeed, now how would you say the weather has been?" },
                { "Its good to meet you as well", "Indeed, now how would you say the weather has been?" },
                { "Its good to meet you also", "Indeed, now how would you say the weather has been?" },
                { "Its a pleasure to meet you too", "Indeed, now how would you say the weather has been?" },
                { "Its a pleasure to meet you as well", "Indeed, now how would you say the weather has been?" },
                { "Its a pleasure to meet you also", "Indeed, now how would you say the weather has been?" },
                { "how are you", "I'm doing great! Now that I'm talking to you. How about you?" },
                { "what is your name", $"Silly, I'm {Name}, created for you." },
                { "hi", "Hi there! How can I help you today?" },
                { "how are you?", "I'm doing great! Now that I'm talking to you. How about you?" },
                { "what is your name?", $"Did you forget already? My name is {Name}, created for you." },
                { "what is my name?", $"Uhh, its {UserName}, Duhh." },
                { "what is my name", $"Uhh, its {UserName}, Duhh." },
                { "goodbye", "Goodbye! Have a great day!" },
                { "I love you", "Mmm-M-Me? Why? I can't feel love... I think..." },
                { "thank you", "You are welcome I think... what did I do exactly?" },
                { "you remembered my name", "Oh yeah, well of course I did, it is my job you know." },
                { "played", "was it fun?" },
                { "thanks", "you're welcome" }
            };
        }    

        private void InitializeToneKeywords()
        {
            // Initialize tone keywords
            toneKeywords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "happy", "I'm glad to hear you're feeling happy!" },
                { "fun", "That sounds like a lot of fun!" },
                { "sad", "I'm sorry to hear that you're feeling sad." },
                { "bored", "I hope you find something interesting to do soon!" },
                { "excited", "That's great! Excitement is always good." },
                { "angry", "It's okay to feel angry sometimes. Take a deep breath." },
                { "love", "That's super cute that you love it." },
                { "good", "That's good to hear. What did you do today?" },
                { "well", "That's always good. What did you do today?" },
                { "great", "That's awesome" },
                { "awesome", "That's amazing" },
                { "super", "That's great!" }
            };

            positiveKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "happy", "fun", "excited", "love", "glad", "interested", "interesting", "enjoy", "good", "well", "great", "awesome", "super"
            };

            negativeKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "sad", "bored", "angry", "mad", "not fun", "not happy", "don't love", "upset", "unhappy"
            };
        }

        private void InitializeConjoinedToneKeywords()
        {
            // Initialize conjoined tone keywords
            conjoinedToneKeywords = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "not happy", "I'm sorry, is there anything I can do to make you feel better?" },
                { "not fun", "I'm sorry to hear that it wasn't fun." },
                { "extremely bored", "I hope you find something very interesting to do soon!" },
                {"do not love them", "Awww... why not?"},
                {"don't love them", "Awww... why not?"},
                {"dont love them", "Awww... why not?"},
                { "love them", "Aww... I'm happy you love them."}
            };

            positiveKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "happy", "fun", "excited", "love", "glad", "interested", "interesting", "enjoy", "good", "well", "great", "awesome", "super"
            };

            negativeKeywords = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "sad", "bored", "angry", "mad", "not fun", "not happy", "don't love", "upset", "unhappy"
            };
        }
        

        public string GetResponse(string input)
        {
            RecordMessage(input);

            string lowerInput = input.ToLower();

            if (predefinedResponses.ContainsKey(lowerInput))
            {
                return predefinedResponses[lowerInput];
            }
            else
            {
                string toneResponse = GetConjoinedToneResponse(lowerInput) ?? GetToneResponse(lowerInput);
                if (!string.IsNullOrEmpty(toneResponse))
                {
                    TrackThoughts(lowerInput);
                    return toneResponse;
                }
                else
                {
                    RecordComment(input);
                    return "Thank you for your patience, I'm sorry that I don't yet have the capability to understand that... Maybe try asking something else or rephrasing?";
                }
            }
        }

        private string GetToneResponse(string input)
        {
            // Split the input into words
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Collect tone responses based on detected keywords
            List<string> responses = new List<string>();

            foreach (var word in words)
            {
                if (toneKeywords.ContainsKey(word) && !responses.Contains(toneKeywords[word]))
                {
                    responses.Add(toneKeywords[word]);
                }
            }

            // Conjoin the responses into a single string
            if (responses.Count > 0)
            {
                return string.Join(" ", responses);
            }
            return null;
        }

        private string GetConjoinedToneResponse(string input)
        {
            // Check for conjoined tone keywords in the input
            foreach (var keyword in conjoinedToneKeywords)
            {
                if (input.Contains(keyword.Key))
                {
                    return conjoinedToneKeywords[keyword.Key];
                }
            }
            return null;
        }

        private void TrackThoughts(string input)
        {
            string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var word in words)
            {
                if (positiveKeywords.Contains(word))
                {
                    positiveCount++;
                }
                else if (negativeKeywords.Contains(word))
                {
                    negativeCount++;
                }
            }
        }

        public string GetThoughts()
        {
            if (positiveCount > negativeCount)
            {
                return $"I think you're great, {UserName}. I'm starting to like you a lot. You are super positive.";
            }
            else if (negativeCount > positiveCount)
            {
                return $"You seem awfully down {UserName}.... Can I help you?";
            }
            else
            {
                return "I think I need to get to know you better.";
            }
        }

        public void RecordMessage(string message)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string context = IdentifyContext(message);
            string logEntry = $"{timestamp} - {UserName}: {message} [Context: {context}]";
            File.AppendAllText(filePath, logEntry + Environment.NewLine);
        }


        public void RecordComment(string message)
        {
        while (true)
            {
                Console.WriteLine("I don't understand, Would you like to add a comment so I can learn in the future?");
                string input = Console.ReadLine().Trim().ToLower();

                if (input == "yes")
                {
                    Console.WriteLine("Please enter your comment:");
                    string comment = Console.ReadLine();
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string logEntry = $"{timestamp} - {UserName}: {message} [Unrecognized Input] [Comment: {comment}]";
                    File.AppendAllText(filePath, logEntry + Environment.NewLine);
                    break;
                }
                else if (input == "no")
                {
                    Console.WriteLine("Very well, shall we continue then?");
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string logEntry = $"{timestamp} - {UserName}: {message} [Unrecognized Input]";
                    File.AppendAllText(filePath, logEntry + Environment.NewLine);
                    break;
                }
                else
                {
                    Console.WriteLine("Please answer with 'yes' or 'no'.");
                }
            }
        }

        private string IdentifyContext(string message)
        {
            if (message.Contains("weather", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("sunny", StringComparison.OrdinalIgnoreCase) ||
                message.Contains("rain", StringComparison.OrdinalIgnoreCase))
            {
                return "weather";
            }
            if (message.Contains("happy", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("sad", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("excited", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("angry", StringComparison.OrdinalIgnoreCase))
    {
        return "mood";
    }
    if (message.Contains("running", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("swimming", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("school", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("reading", StringComparison.OrdinalIgnoreCase))
    {
        return "activities";
    }
    if (message.Contains("pizza", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("food", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("sushi", StringComparison.OrdinalIgnoreCase) ||
        message.Contains("pasta", StringComparison.OrdinalIgnoreCase))
    {
        return "food";
    }
            // Add more context identification logic here if needed.
            return "general context";
        }

        public (string date, string context) ParseUserInput(string input)
        {
            string datePattern = @"recall (\d{4}-\d{2}-\d{2}) (.*)";
            Match match = Regex.Match(input, datePattern);

            if (match.Success)
            {
                string recallDate = match.Groups[1].Value;
                string messageContext = match.Groups[2].Value;
                return (recallDate, messageContext);
            }

            string context = "general context";
            if (input.Contains("weather", StringComparison.OrdinalIgnoreCase))
            {
                context = "weather";
            }
            else if (input.Contains("mood", StringComparison.OrdinalIgnoreCase))
            {
                context = "mood";
            }
            else if (input.Contains("activities", StringComparison.OrdinalIgnoreCase))
            {
                context = "activities";
            }
            else if (input.Contains("food", StringComparison.OrdinalIgnoreCase))
            {
                context = "food";
            }
            else if (input.Contains("general context", StringComparison.OrdinalIgnoreCase))
            {
                context = "General Context";
            }

            return (null, context);
        }

        public string RecallMessages(string recallDate, string messageContext)
        {
            if (!File.Exists(filePath))
            {
                return "No messages found.";
            }

                string[] messages = File.ReadAllLines(filePath);
                List<string> recalledMessages = new List<string>();

                foreach (string message in messages)
                {
                    if (message.Contains(recallDate))
                    {
                        int contextStart = message.IndexOf("[Context: ", StringComparison.OrdinalIgnoreCase);
                        if (contextStart != -1)
                        {
                            contextStart += "[Context: ".Length;
                            int contextEnd = message.IndexOf("]", contextStart);
                            if (contextEnd != -1)
                            {
                                string extractedContext = message.Substring(contextStart, contextEnd - contextStart).Trim();

                                if (string.Equals(extractedContext, messageContext, StringComparison.OrdinalIgnoreCase))
                                {
                                    recalledMessages.Add(message);
                                }
                            }
                        }
                    }  
                    
                }

                return recalledMessages.Count > 0 ? string.Join(Environment.NewLine, recalledMessages) : $"No messages found for the specified date ({recallDate}) and context ({messageContext}).";
        }
    }
}