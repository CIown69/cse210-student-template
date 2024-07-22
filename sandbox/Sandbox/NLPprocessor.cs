using System;
using System.Collections.Generic;

namespace MyNlpLibrary
{
    public class NlpProcessor
    {
        private readonly Tokenizer _tokenizer = new Tokenizer();
        private readonly PosTagger _posTagger = new PosTagger();
        private readonly DependencyParser _dependencyParser = new DependencyParser();
        private readonly NamedEntityRecognizer _namedEntityRecognizer = new NamedEntityRecognizer();

        public void Process(string text)
        {
            var tokens = _tokenizer.Tokenize(text);
            var posTags = _posTagger.Tag(tokens);
            _dependencyParser.Parse(tokens, posTags);
            var entities = _namedEntityRecognizer.Recognize(tokens);

            Console.WriteLine("Tokens:");
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            Console.WriteLine("\nPart-of-Speech Tags:");
            foreach (var tag in posTags)
            {
                Console.WriteLine(tag);
            }

            Console.WriteLine("\nNamed Entities:");
            foreach (var entity in entities)
            {
                Console.WriteLine(entity);
            }
        }
    }
}
