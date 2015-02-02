using System;
using System.IO;
using System.ServiceModel;
using NLog;
using STInterviewTest.Infrastructure;

namespace STInterviewTest.ClassicAnalyzerService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Single)]
    public class ClassicAnalyzerService : IClassicAnalyzerService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string AssemblyLocation;

        static ClassicAnalyzerService()
        {
            AssemblyLocation = Path.GetDirectoryName(typeof(ClassicAnalyzerService).Assembly.Location);
        }

        #region IClassicAnalyzerService Members

        /// <summary>
        /// Returns a simple string (DateTime.UtcNow) for debugging purposes only.
        /// </summary>
        public AnalyzerResponse Test()
        {
            Logger.Info("Test called");

            var reply = DateTime.UtcNow + " => everything working fine...";

            Logger.Info("Test replied: {0}", reply);

            return new AnalyzerResponse
            {
                Result = reply,
                IsError = false,
                ErrorMessage = string.Empty
            };
        }

        /// <summary>
        /// Sends an analyze request (a file name and a file content) and receives the results
        /// </summary>
        /// <param name="request">The request object</param>
        /// <returns>AnalzyzerResponse object</returns>
        public AnalyzerResponse Analyze(AnalyzerRequest request)
        {
            if (request == null)
            {
                Logger.Error("Null request received");
                throw new ArgumentNullException("request");
            }

            var localFilePath = WriteToLocalFile(request.FileName, request.FileContent);
            var pipeline = Pipeline.Setup();

            var extension = Path.GetExtension(request.FileName);
            if (string.IsNullOrEmpty(extension))
                return new AnalyzerResponse { ErrorMessage = "Unknown file type!", IsError = false, Result = "Unknown file type!"};

            extension = extension.ToLowerInvariant();

            string result;

            try
            {
                result = (extension == ".txt") ? pipeline.ProcessTxtFile(localFilePath) : 
                    (extension == ".pdf") ? pipeline.ProcessPdfFile(localFilePath) : "unknown file type";
            }
            catch (Exception exception)
            {
                Logger.Error("Unexpected error occured!", exception);
                return new AnalyzerResponse
                {
                    Result = string.Empty,
                    IsError = true,
                    ErrorMessage = exception.Message
                };                
            }

            return new AnalyzerResponse
            {
                Result = result,
                IsError = false,
                ErrorMessage = string.Empty,
            };
        }

        #endregion IClassicAnalyzerService Members

        private static string WriteToLocalFile(string fileName, Stream fileContent)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            if (fileContent == null)
                throw new ArgumentNullException("fileContent");

            var uploadLocation = Path.Combine(AssemblyLocation, Guid.NewGuid().ToString());
            Directory.CreateDirectory(uploadLocation);

            var filePath = Path.Combine(uploadLocation, fileName);

            var buffer = new byte[0x20000]; // 128 KB

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                var noOfBytesRead = fileContent.Read(buffer, 0, buffer.Length);

                while (noOfBytesRead > 0)
                {
                    stream.Write(buffer, 0, noOfBytesRead);
                    noOfBytesRead = fileContent.Read(buffer, 0, buffer.Length);
                }
            }

            return filePath;
        }
    }
}
