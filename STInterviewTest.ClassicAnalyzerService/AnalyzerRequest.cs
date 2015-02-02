using System.IO;
using System.ServiceModel;

namespace STInterviewTest.ClassicAnalyzerService
{
    [MessageContract]
    public class AnalyzerRequest
    {
        [MessageHeader(MustUnderstand = true)] public string FileName;

        [MessageBodyMember(Order = 1)] public Stream FileContent;
    }
}
