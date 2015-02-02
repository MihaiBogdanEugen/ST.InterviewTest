using System;
using System.IO;
using NLog;

namespace STInterviewTest.Infrastructure.TextExtractors
{
    public class TxtFileTextExtractor : BaseTextExtractor
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region ITextExtractor Members

        /// <summary>
        /// Extracts the text content of a txt file.
        /// </summary>
        /// <param name="filePath">The file path of the file</param>
        /// <returns>The text content of the file</returns>
        public override string ExtractText(string filePath)
        {
            var fileType = filePath.GetFileType();

            if (fileType != FileTypes.TxtFile)
            {
                Logger.Error("File [{0}] cannot be processed by a TxtFileTextExtractor", filePath);
                throw new NotSupportedException("File [" + filePath + "] cannot be processed by a TxtFileTextExtractor");
            }

            return File.ReadAllText(filePath).RemoveNewLineAndLineFeed();;
        }

        #endregion ITextExtractor Members
    }
}
