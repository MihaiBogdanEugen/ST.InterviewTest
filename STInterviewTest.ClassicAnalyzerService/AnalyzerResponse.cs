using System.ServiceModel;

namespace STInterviewTest.ClassicAnalyzerService
{
    [MessageContract]
    public class AnalyzerResponse
    {
        [MessageHeader] public string Result;

        [MessageBodyMember] public bool IsError;

        [MessageBodyMember] public string ErrorMessage;
    }
}
