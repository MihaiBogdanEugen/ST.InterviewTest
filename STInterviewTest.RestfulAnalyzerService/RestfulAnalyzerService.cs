using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STInterviewTest.RestfulAnalyzerService
{
    public class RestfulAnalyzerService : IRestfulAnalyzerService
    {
        #region IRestfulAnalyzerService Members

        public AnalyzerResult Analyze(string fileName, System.IO.Stream fileContent)
        {
            throw new NotImplementedException();
        }

        public IAsyncResult BeginAnalyzeAsync(string fileName, System.IO.Stream fileContent, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException();
        }

        public AnalyzerResult EndAnalyzeAsync(IAsyncResult result)
        {
            throw new NotImplementedException();
        }

        public string Test()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
