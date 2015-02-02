using System;
using System.IO;
using System.Linq;
using NLog;

namespace STInterviewTest.Infrastructure
{
    public static class OpinionLexicon
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        public static readonly string[] PositiveWords;
        public static readonly string[] NegativeWords;

        private const string PositiveWordsFile = @"Resources\positive-words.txt";
        private const string NegativeWordsFile = @"Resources\negative-words.txt";

        static OpinionLexicon()
        {
            Predicate<string> isValidLine = line =>
                string.IsNullOrEmpty(line) == false &&
                line.StartsWith(";") == false;

            PositiveWords = File.ReadAllLines(PositiveWordsFile).Where(line => isValidLine(line)).ToArray();

            Logger.Info("Finished generating the positive words lexicon ({0} words)", PositiveWords.Count());

            NegativeWords = File.ReadAllLines(NegativeWordsFile).Where(line => isValidLine(line)).ToArray();

            Logger.Info("Finished generating the negative words lexicon ({0} words)", NegativeWords.Count());
        }
    }
}
