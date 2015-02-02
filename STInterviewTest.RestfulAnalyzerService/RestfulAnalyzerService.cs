using System;
using System.IO;
using System.ServiceModel.Web;
using System.Threading;
using NLog;
using STInterviewTest.Infrastructure;

namespace STInterviewTest.RestfulAnalyzerService
{
    /// <remarks>In case this service will be hosted inside IIS, see:
    /// http://weblogs.asp.net/jclarknet/wcf-streaming-issue-under-iis
    /// </remarks>
    public class RestfulAnalyzerService : IRestfulAnalyzerService
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string AssemblyLocation;

        static RestfulAnalyzerService()
        {
            AssemblyLocation = Path.GetDirectoryName(typeof(RestfulAnalyzerService).Assembly.Location);
        }

        #region IAnalyzerService Members

        public AnalyzerResult Analyze(string fileName, Stream fileContent)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            if (fileContent == null)
                throw new ArgumentNullException("fileContent");

            var localFilePath = WriteToLocalFile(fileName, fileContent);
            var pipeline = Pipeline.Setup();

            var extension = Path.GetExtension(fileName).ToLowerInvariant();

            string result;

            try
            {
                result = (extension == ".txt") ? pipeline.ProcessTxtFile(localFilePath) : 
                    (extension == ".pdf") ? pipeline.ProcessPdfFile(localFilePath) : "unknown file type";
            }
            catch (Exception exception)
            {
                Logger.Error("Unexpected error occured!", exception);
                return new AnalyzerResult
                {
                    Result = string.Empty,
                    IsError = true,
                    ErrorMessage = exception.Message
                };                
            }

            return new AnalyzerResult
            {
                Result = result,
                IsError = false,
                ErrorMessage = string.Empty,
            };
        }

        /// <summary>
        /// Uploads the file specified by name and content and processes it.
        /// Asynchronous operation.
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded and processed</param>
        /// <param name="fileContent">The content of the file to be uploaded and processed</param>
        /// <param name="callback">Callback for the async pattern</param>
        /// <param name="asyncState">User state for the async pattern</param>
        /// <returns></returns>
        public IAsyncResult BeginAnalyzeAsync(string fileName, Stream fileContent, AsyncCallback callback, object asyncState)
        {
            return new CompletedAsyncResult<FileInfo>(new FileInfo
            {
                Name = fileName,
                Content = fileContent
            });
        }

        /// <summary>
        /// Ends the asynchonous operation initiated by the call to <see cref="BeginAnalyzeAsync"/>
        /// </summary>
        public AnalyzerResult EndAnalyzeAsync(IAsyncResult result)
        {
            var fileInfo = ((CompletedAsyncResult<FileInfo>)result).Payload;
           
            var localFilePath = WriteToLocalFile(fileInfo.Name, fileInfo.Content);
            var pipeline = Pipeline.Setup();

            var extension = Path.GetExtension(fileInfo.Name);
            if (string.IsNullOrEmpty(extension))
                return new AnalyzerResult { ErrorMessage = "Unknown file type!", IsError = false, Result = "Unknown file type!"};

            extension = extension.ToLowerInvariant();
            
            string pipelineResult;

            try
            {
                pipelineResult = (extension == ".txt") ? pipeline.ProcessTxtFile(localFilePath) : 
                    (extension == ".pdf") ? pipeline.ProcessPdfFile(localFilePath) : "unknown file type";
            }
            catch (Exception exception)
            {
                Logger.Error("Unexpected error occured!", exception);
                return new AnalyzerResult
                {
                    Result = string.Empty,
                    IsError = true,
                    ErrorMessage = exception.Message
                };                
            }

            return new AnalyzerResult
            {
                Result = pipelineResult,
                IsError = false,
                ErrorMessage = string.Empty,
            };
        }

        /// <summary>
        /// Returns a simple string (DateTime.UtcNow) for debugging purposes only.
        /// </summary>
        public string Test()
        {
            Logger.Info("Test called");

            var reply = DateTime.UtcNow + " => everything working fine...";

            var context = WebOperationContext.Current;
            if (context != null)
            {
                context.OutgoingResponse.ContentType = "text/plain";
                context.OutgoingResponse.ContentLength = reply.Length;
            }

            Logger.Info("Test replied: {0}", reply);

            return reply;
        }

        #endregion IAnalyzerService Members

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

    internal class CompletedAsyncResult<T> : IAsyncResult
    {
        public CompletedAsyncResult(T payload)
        {
            this.Payload = payload;
        }

        public T Payload { get; private set; }

        #region IAsyncResult Members

        public object AsyncState
        {
            get
            {
                return (object)this.Payload;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return true;
            } 
        }

        public bool IsCompleted
        {
            get
            {
                return true;
            }
        }

        #endregion IAsyncResult Members
    }

    internal class FileInfo
    {
        public string Name { get; set; }

        public Stream Content { get; set; }
    }
}
