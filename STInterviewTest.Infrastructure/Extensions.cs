using System;
using System.IO;
using NLog;

namespace STInterviewTest.Infrastructure
{
    public static class Extensions
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        private const string Space = " ";
        private const string DoubleSpace = "  ";

        public static FileTypes GetFileType(this string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                Logger.Error("Null file path received as parameter");
                throw new ArgumentNullException(filePath);
            }

            if (File.Exists(filePath) == false)
            {
                Logger.Error("Unexisting file path received as parameter");
                throw new FileNotFoundException("Cannot find file [" + filePath + "]", filePath);
            }

            var fileExtension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (fileExtension)
            {
                case ".zip":
                    return FileTypes.ZipFile;
                case ".txt":
                    return FileTypes.TxtFile;
                case ".pdf":
                    return FileTypes.PdfFile;
            }

            return FileTypes.Unknown;   
        }

        public static string RemoveNewLineAndLineFeed(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            text = text.Replace(Environment.NewLine, " ");

            text = text.Replace("\n", " ");

            while (text.Contains(DoubleSpace))
                text = text.Replace(DoubleSpace, Space);

            text = text.Replace("- ", "-");

            text = text.Replace(" -", "-");

            return text;
        }
    }

    public enum FileTypes
    {
        Unknown,
        ZipFile,
        TxtFile,
        PdfFile,
    }
}
