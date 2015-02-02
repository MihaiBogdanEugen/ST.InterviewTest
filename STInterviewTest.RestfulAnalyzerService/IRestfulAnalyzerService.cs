using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace STInterviewTest.RestfulAnalyzerService
{
    [ServiceContract]
    public interface IRestfulAnalyzerService
    {
        /// <summary>
        /// Uploads the file specified by name and content and processes it.
        /// Synchronous operation.
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded and processed</param>
        /// <param name="fileContent">The content of the file to be uploaded and processed</param>
        [OperationContract]
        [WebInvoke(UriTemplate="analyze/{fileName}")]
        AnalyzerResult Analyze(string fileName, Stream fileContent);

        /// <summary>
        /// Uploads the file specified by name and content and processes it.
        /// Asynchronous operation.
        /// </summary>
        /// <param name="fileName">The name of the file to be uploaded and processed</param>
        /// <param name="fileContent">The content of the file to be uploaded and processed</param>
        /// <param name="callback">Callback for the async pattern</param>
        /// <param name="asyncState">User state for the async pattern</param>
        /// <returns></returns>
        [OperationContract(AsyncPattern = true)]
        [WebInvoke(UriTemplate="analyzeasync/{fileName}")]
        IAsyncResult BeginAnalyzeAsync(string fileName, Stream fileContent, AsyncCallback callback, object asyncState);

        /// <summary>
        /// Ends the asynchonous operation initiated by the call to BeginAnalyzeAsync/>
        /// </summary>
        AnalyzerResult EndAnalyzeAsync(IAsyncResult result);

        /// <summary>
        /// Returns a simple string (DateTime.UtcNow) for debugging purposes only.
        /// </summary>
        [OperationContract]
        [WebGet(UriTemplate="")]
        string Test();
    }
}
