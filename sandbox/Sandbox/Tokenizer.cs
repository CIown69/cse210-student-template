using System;
using System.Collections.Generic;

namespace MyNlpLibrary
{
    public class Tokenizer
    {
        public List<string> Tokenize(string text)
        {
            char[] delimiters = new char[] { ' ', '\t', '\n', '.', ',', ';', ':', '!', '?', '(', ')', '[', ']', '{', '}', '"' };
            var tokens = new List<string>(text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries));
            return tokens;
        }
    }
}
