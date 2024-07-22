using System;
using System.Collections.Generic;

namespace MyNlpLibrary
{
    public enum PosTag
    {
        Noun,
        Verb,
        Adjective,
        Adverb,
        Pronoun,
        Preposition,
        Conjunction,
        Determiner,
        Interjection,
        Unknown
    }

    public class PosTagger
    {
        private readonly Dictionary<string, List<PosTag>> _wordTags = new Dictionary<string, List<PosTag>>
        {
            { "I", new List<PosTag> { PosTag.Pronoun } },
            { "a", new List<PosTag> { PosTag.Determiner } },
            { "about", new List<PosTag> { PosTag.Preposition } },
            { "after", new List<PosTag> { PosTag.Preposition } },
            { "all", new List<PosTag> { PosTag.Determiner } },
            { "also", new List<PosTag> { PosTag.Adverb } },
            { "an", new List<PosTag> { PosTag.Determiner } },
            { "and", new List<PosTag> { PosTag.Conjunction } },
            { "any", new List<PosTag> { PosTag.Determiner } },
            { "as", new List<PosTag> { PosTag.Conjunction } },
            { "at", new List<PosTag> { PosTag.Preposition } },
            { "back", new List<PosTag> { PosTag.Noun, PosTag.Adverb } },
            { "be", new List<PosTag> { PosTag.Verb } },
            { "because", new List<PosTag> { PosTag.Conjunction } },
            { "before", new List<PosTag> { PosTag.Preposition } },
            { "big", new List<PosTag> { PosTag.Adjective } },
            { "but", new List<PosTag> { PosTag.Conjunction } },
            { "by", new List<PosTag> { PosTag.Preposition } },
            { "can", new List<PosTag> { PosTag.Verb } },
            { "come", new List<PosTag> { PosTag.Verb } },
            { "could", new List<PosTag> { PosTag.Verb } },
            { "day", new List<PosTag> { PosTag.Noun } },
            { "do", new List<PosTag> { PosTag.Verb } },
            { "down", new List<PosTag> { PosTag.Preposition } },
            { "even", new List<PosTag> { PosTag.Adverb } },
            { "first", new List<PosTag> { PosTag.Adjective } },
            { "for", new List<PosTag> { PosTag.Preposition } },
            { "from", new List<PosTag> { PosTag.Preposition } },
            { "get", new List<PosTag> { PosTag.Verb } },
            { "give", new List<PosTag> { PosTag.Verb } },
            { "go", new List<PosTag> { PosTag.Verb } },
            { "good", new List<PosTag> { PosTag.Adjective } },
            { "have", new List<PosTag> { PosTag.Verb } },
            { "he", new List<PosTag> { PosTag.Pronoun } },
            { "her", new List<PosTag> { PosTag.Pronoun } },
            { "him", new List<PosTag> { PosTag.Pronoun } },
            { "his", new List<PosTag> { PosTag.Pronoun } },
            { "how", new List<PosTag> { PosTag.Adverb } },
            { "I'm", new List<PosTag> { PosTag.Pronoun } },
            { "if", new List<PosTag> { PosTag.Conjunction } },
            { "in", new List<PosTag> { PosTag.Preposition } },
            { "into", new List<PosTag> { PosTag.Preposition } },
            { "it's", new List<PosTag> { PosTag.Pronoun } },
            { "just", new List<PosTag> { PosTag.Adverb } },
            { "know", new List<PosTag> { PosTag.Verb } },
            { "like", new List<PosTag> { PosTag.Preposition } },
            { "look", new List<PosTag> { PosTag.Verb } },
            { "make", new List<PosTag> { PosTag.Verb } },
            { "me", new List<PosTag> { PosTag.Pronoun } },
            { "most", new List<PosTag> { PosTag.Determiner } },
            { "my", new List<PosTag> { PosTag.Pronoun } },
            { "new", new List<PosTag> { PosTag.Adjective } },
            { "no", new List<PosTag> { PosTag.Determiner } },
            { "not", new List<PosTag> { PosTag.Adverb } },
            { "now", new List<PosTag> { PosTag.Adverb } },
            { "of", new List<PosTag> { PosTag.Preposition } },
            { "on", new List<PosTag> { PosTag.Preposition } },
            { "only", new List<PosTag> { PosTag.Adjective } },
            { "or", new List<PosTag> { PosTag.Conjunction } },
            { "other", new List<PosTag> { PosTag.Adjective } },
            { "our", new List<PosTag> { PosTag.Pronoun } },
            { "out", new List<PosTag> { PosTag.Adverb } },
            { "over", new List<PosTag> { PosTag.Preposition } },
            { "people", new List<PosTag> { PosTag.Noun } },
            { "say", new List<PosTag> { PosTag.Verb } },
            { "see", new List<PosTag> { PosTag.Verb } },
            { "she", new List<PosTag> { PosTag.Pronoun } },
            { "so", new List<PosTag> { PosTag.Conjunction } },
            { "some", new List<PosTag> { PosTag.Determiner } },
            { "take", new List<PosTag> { PosTag.Verb } },
            { "than", new List<PosTag> { PosTag.Conjunction } },
            { "that", new List<PosTag> { PosTag.Pronoun } },
            { "the", new List<PosTag> { PosTag.Determiner } },
            { "their", new List<PosTag> { PosTag.Pronoun } },
            { "them", new List<PosTag> { PosTag.Pronoun } },
            { "then", new List<PosTag> { PosTag.Adverb } },
            { "there", new List<PosTag> { PosTag.Adverb } },
            { "they", new List<PosTag> { PosTag.Pronoun } },
            { "think", new List<PosTag> { PosTag.Verb } },
            { "this", new List<PosTag> { PosTag.Determiner } },
            { "time", new List<PosTag> { PosTag.Noun } },
            { "to", new List<PosTag> { PosTag.Preposition } },
            { "up", new List<PosTag> { PosTag.Adverb } },
            { "us", new List<PosTag> { PosTag.Pronoun } },
            { "use", new List<PosTag> { PosTag.Verb } },
            { "want", new List<PosTag> { PosTag.Verb } },
            { "way", new List<PosTag> { PosTag.Noun } },
            { "we", new List<PosTag> { PosTag.Pronoun } },
            { "well", new List<PosTag> { PosTag.Adverb } },
            { "what", new List<PosTag> { PosTag.Pronoun } },
            { "when", new List<PosTag> { PosTag.Adverb } },
            { "which", new List<PosTag> { PosTag.Pronoun } },
            { "who", new List<PosTag> { PosTag.Pronoun } },
            { "will", new List<PosTag> { PosTag.Verb } },
            { "with", new List<PosTag> { PosTag.Preposition } },
            { "work", new List<PosTag> { PosTag.Verb } },
            { "would", new List<PosTag> { PosTag.Verb } },
            { "year", new List<PosTag> { PosTag.Noun } },
            { "you", new List<PosTag> { PosTag.Pronoun } },
            { "your", new List<PosTag> { PosTag.Pronoun } },
            // Add more words as necessary
        };

        public List<PosTag> Tag(List<string> tokens)
        {
            var tags = new List<PosTag>();

            foreach (var token in tokens)
            {
                if (_wordTags.ContainsKey(token))
                {
                    tags.AddRange(_wordTags[token]);
                }
                else
                {
                    tags.Add(PosTag.Unknown);
                }
            }

            return tags;
        }
    }
}