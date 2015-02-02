using System;
using System.Collections.Generic;
using System.Linq;
using NLog;

namespace STInterviewTest.Infrastructure.Analyzers
{
    public class OpinionAnalyzer : IProcessPipelineTask
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region IPipelineTask Members

        /// <summary>
        /// Parses the text and returns an opinion by counting the positive words, the negative words and comparing them 
        /// using a very basic algorithm.
        /// The method takes and returns the same object (with additional data) for easy method chaining.
        /// </summary>
        /// <param name="task">The pipeline task witht the input data</param>
        /// <returns>The pipeline task with the input and the output data</returns>
        public PipelineTask DoWork(PipelineTask task)
        {
            if (task == null)
            {
                Logger.Error("Null task received");
                throw new ArgumentNullException("task");
            }

            var score = GetOpinionOfText(task.ExtractedText);

            switch (score)
            {
                case OpinionType.Negative:
                    task.AnalyzerResult = "Negative Opinion";
                    break;
                case OpinionType.Neutral:
                    task.AnalyzerResult = "Neutral Opinion";
                    break;
                case OpinionType.Unknown:
                    task.AnalyzerResult = "Unknown Opinion";
                    break;
                case OpinionType.Positive:
                    task.AnalyzerResult = "Positive Opinion";
                    break;
            }

            Logger.Info("OpinionAnalyzer.DoWork finshed, result: {0}", task.AnalyzerResult);

            return task;
        }

        #endregion IPipelineTask Members

        /// <summary>
        /// Parses the text and returns an opinion by counting the positive words, the negative words and comparing them 
        /// using a very basic algorithm.
        /// </summary>
        /// <param name="text">The text to analyze</param>
        /// <returns>An OpinionType enum representing the opinion (Positive, Negative, Neutral or Unknown) of the text</returns>
        private static OpinionType GetOpinionOfText(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                Logger.Error("Empty text received");
                throw new ArgumentNullException("text");
            }

            var wordsAndFrequencies = new Dictionary<string, int>();

            var query = from word in text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
                        where word.All(Char.IsLetter)
                        select word.ToLowerInvariant();

            foreach (var key in query)
            {
                if (wordsAndFrequencies.ContainsKey(key))
                    wordsAndFrequencies[key]++;
                else
                    wordsAndFrequencies.Add(key, 1);
            }

            Logger.Info("Text has {0} words", wordsAndFrequencies.Keys.Count);

            var positiveWordsCount = OpinionLexicon.PositiveWords
                .Where(wordsAndFrequencies.ContainsKey)
                .Sum(positiveWord => wordsAndFrequencies[positiveWord]);

            Logger.Info("Text has {0} positive words", positiveWordsCount);

            var negativeWordsCount = OpinionLexicon.NegativeWords
                .Where(wordsAndFrequencies.ContainsKey)
                .Sum(negativeWord => wordsAndFrequencies[negativeWord]);

            Logger.Info("Text has {0} negative words", negativeWordsCount);

            var score = (decimal) Math.Abs(positiveWordsCount - negativeWordsCount) / (positiveWordsCount + negativeWordsCount);

            Logger.Info("Score is: {0}", score);

            if (score < 0.01M)
                return OpinionType.Unknown;
            
            if (score < 0.1M)
                return OpinionType.Neutral;

            return (positiveWordsCount > negativeWordsCount) ? OpinionType.Positive : OpinionType.Negative;
        }
    }

    public enum OpinionType
    {
        Unknown,
        Positive,
        Neutral,
        Negative,
    }
}
