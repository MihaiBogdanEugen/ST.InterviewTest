using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using NLog;

namespace STInterviewTest.Infrastructure.TextExtractors
{
    public sealed class PdfFileTextExtractor : BaseTextExtractor
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region ITextExtractor Members

        /// <summary>
        /// Extracts the text of a PDF file using a SimpleTextExtractionStrategy object and iTextSharp
        /// </summary>
        /// <param name="filePath">The file path of the file</param>
        /// <returns>The text content of the file</returns>
        public override string ExtractText(string filePath)
        {
            var fileType = filePath.GetFileType();

            if (fileType != FileTypes.PdfFile)
            {
                Logger.Error("File [{0}] cannot be processed by a PdfFileTextExtractor", filePath);
                throw new NotSupportedException("File [" + filePath + "] cannot be processed by a PdfFileTextExtractor");
            }

            var result = string.Empty;

            try
            {
                var pdfReader = new PdfReader(filePath); 
                var writer = new StringWriter();
                var noOfPages = pdfReader.NumberOfPages;

                for (var pageIndex = 1; pageIndex <= noOfPages; pageIndex++)
                    writer.WriteLine(PdfTextExtractor.GetTextFromPage(pdfReader, pageIndex, new SimpleTextExtractionStrategy()));

                result = writer.ToString().RemoveNewLineAndLineFeed();
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Cannot parse file", exception);
            }

            return result;
        }

        #endregion ITextExtractor Members
    }
}
