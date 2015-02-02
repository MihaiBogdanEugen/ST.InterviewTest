using System.Runtime.Serialization;

namespace STInterviewTest.RestfulAnalyzerService
{
    [DataContract]
    public class AnalyzerResult
    {
        [DataMember]
        public string Result { get; set; }

        [DataMember]
        public bool IsError { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }
}
