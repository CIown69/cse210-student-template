using System;
using MyNlpLibrary;

namespace NlpTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var nlpProcessor = new NlpProcessor();
            Console.WriteLine("Type a sentence:");
            string sentence = Console.ReadLine();
            nlpProcessor.Process(sentence);
        }
    }
}
