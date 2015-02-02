
namespace STInterviewTest.Infrastructure.TextExtractors
{
    public interface ITextExtractor
    {
        /// <summary>
        /// Extracts the text of a given file
        /// </summary>
        /// <param name="filePath">The file path of the file</param>
        /// <returns>The text content of the file</returns>
        string ExtractText(string filePath);
    }
}
