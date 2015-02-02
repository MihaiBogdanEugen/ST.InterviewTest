using System.ServiceModel;

namespace STInterviewTest.ClassicAnalyzerService
{
    [ServiceContract]
    public interface IClassicAnalyzerService
    {
        /// <summary>
        /// Returns a simple string (DateTime.UtcNow) for debugging purposes only.
        /// </summary>
        [OperationContract]
        AnalyzerResponse Test();

        /// <summary>
        /// Sends an analyze request (a file name and a file content) and receives the results
        /// </summary>
        /// <param name="request">The request object</param>
        /// <returns>AnalzyzerResponse object</returns>
        [OperationContract]
        AnalyzerResponse Analyze(AnalyzerRequest request);
    }
}
