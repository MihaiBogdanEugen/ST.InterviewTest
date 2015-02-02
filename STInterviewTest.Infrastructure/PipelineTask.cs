using System;

namespace STInterviewTest.Infrastructure
{
    public sealed class PipelineTask
    {
        public PipelineTask()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Unique Id, used for debugging purposes only
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The path of the original input file
        /// </summary>
        public string OriginalFilePath { get; set; }

        /// <summary>
        /// The path of the pre-processed file. If the original file is a zip archive, this is the path of the unarchived file.
        /// If not, this is the same as OriginalFilePath
        /// </summary>
        public string ActualFilePath { get; set; }

        /// <summary>
        /// The text content of the file
        /// </summary>
        public string ExtractedText { get; set; }

        /// <summary>
        /// The opinion analyzer result
        /// </summary>
        public string AnalyzerResult { get; set; }
    }
}
