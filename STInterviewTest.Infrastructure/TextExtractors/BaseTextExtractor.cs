using System;
using NLog;

namespace STInterviewTest.Infrastructure.TextExtractors
{
    public abstract class BaseTextExtractor : ITextExtractor, IProcessPipelineTask
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region IPipelineTask Members

        /// <summary>
        /// Extracts the text of a txt or pdf file.
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

            task.ExtractedText = this.ExtractText(task.ActualFilePath);

            Logger.Info("BaseTextExtractor.DoWork finshed");

            return task;
        }

        #endregion IPipelineTask Members

        public abstract string ExtractText(string filePath);
    }
}
