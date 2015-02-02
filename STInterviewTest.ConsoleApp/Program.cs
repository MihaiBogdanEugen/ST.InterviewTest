using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Xml.Linq;
using Humanizer;
using STInterviewTest.ConsoleApp.ClassicAnalyzerService;

namespace STInterviewTest.ConsoleApp
{
    public class Program
    {
        private const string RestfulAnalyzerServiceBaseUrl = @"http://localhost:1224/service";
        private const string RestfulAnalyzeServiceUrlFormat = @"http://localhost:1224/service/analyze/{0}";
        private const string RestfulAnalyzeAsyncServiceUrlFormat = @"http://localhost:1224/service/analyzeasync/{0}";

        public static void Main(string[] args)
        {
            Console.WriteLine("START");

            var client = new ClassicAnalyzerServiceClient();

            DoClassicTest(client, @"Files\sample1.pdf", "Positive Opinion"); 
            DoClassicTest(client, @"Files\sample2.pdf", "Positive Opinion");  
            DoClassicTest(client, @"Files\sample3.pdf", "Positive Opinion");  
            DoClassicTest(client, @"Files\sample4.pdf", "Neutral Opinion");  
            DoClassicTest(client, @"Files\sample5.pdf", "Negative Opinion"); 
            DoClassicTest(client, @"Files\sample1.txt", "Positive Opinion"); 
            DoClassicTest(client, @"Files\sample2.txt", "Positive Opinion");  
            DoClassicTest(client, @"Files\sample3.txt", "Positive Opinion");  
            DoClassicTest(client, @"Files\sample4.txt", "Neutral Opinion");  
            DoClassicTest(client, @"Files\sample5.txt", "Negative Opinion");  

            DoRestfulTest(@"Files\sample1.pdf", "Positive Opinion"); 
            DoRestfulTest(@"Files\sample2.pdf", "Positive Opinion");  
            DoRestfulTest(@"Files\sample3.pdf", "Positive Opinion");  
            DoRestfulTest(@"Files\sample4.pdf", "Neutral Opinion");  
            DoRestfulTest(@"Files\sample5.pdf", "Negative Opinion"); 
            DoRestfulTest(@"Files\sample1.txt", "Positive Opinion"); 
            DoRestfulTest(@"Files\sample2.txt", "Positive Opinion");  
            DoRestfulTest(@"Files\sample3.txt", "Positive Opinion");  
            DoRestfulTest(@"Files\sample4.txt", "Neutral Opinion");  
            DoRestfulTest(@"Files\sample5.txt", "Negative Opinion");  

            Console.WriteLine("END");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void DoClassicTest(ClassicAnalyzerServiceClient client, string filePath, string expectedResult)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();      

            if (client == null)
                throw new ArgumentNullException("client");      

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            if (string.IsNullOrEmpty(expectedResult))
                throw new ArgumentNullException("expectedResult");

            var fileName = Path.GetFileName(filePath);
            bool isError;
            string errorMessage;
            string status;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                status = client.Analyze(fileName, fileStream, out errorMessage, out isError);
            }

            stopwatch.Stop();

            Console.WriteLine("\r\nClassic test => Finished analyzing file {0} of {1}, in {2}", 
                fileName, 
                new FileInfo(filePath).Length.Bytes().Humanize("0.00"),
                new TimeSpan(stopwatch.ElapsedTicks).Humanize());

            if (isError)
            {
                Console.WriteLine("Server side error: {0}\r\n", errorMessage);            
            }
            else
            {
                if (string.Compare(expectedResult, status, StringComparison.OrdinalIgnoreCase) == 0)
                    Console.WriteLine("Result is {0}, same as expected => SUCCESS!\r\n", status);
                else
                    Console.WriteLine("Result is {0}, differs from expected ({1}) => FAILURE!\r\n", status, expectedResult);                
            }
        }

        private static void DoRestfulTest(string filePath, string expectedResult)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            if (string.IsNullOrEmpty(expectedResult))
                throw new ArgumentNullException("expectedResult");

            var fileName = Path.GetFileName(filePath);
            var url = string.Format(CultureInfo.InvariantCulture, RestfulAnalyzeServiceUrlFormat, fileName);

            var result = DoTest(url, filePath);

            stopwatch.Stop();

            Console.WriteLine("\r\nRestful test => Finished analyzing file {0} of {1}, in {2}", 
                fileName, 
                new FileInfo(filePath).Length.Bytes().Humanize("0.00"),
                new TimeSpan(stopwatch.ElapsedTicks).Humanize());

            if (result.IsError)
            {
                Console.WriteLine("Server side error: {0}\r\n", result.ErrorMessage);            
            }
            else
            {
                if (string.Compare(expectedResult, result.Status, StringComparison.OrdinalIgnoreCase) == 0)
                    Console.WriteLine("Result is {0}, same as expected => SUCCESS!\r\n", result.Status);
                else
                    Console.WriteLine("Result is {0}, differs from expected ({1}) => FAILURE!\r\n", result.Status, expectedResult);                
            }
        }

        private static void DoRestfulAsyncTest(string filePath, string expectedResult)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("filePath");

            if (string.IsNullOrEmpty(expectedResult))
                throw new ArgumentNullException("expectedResult");

            var fileName = Path.GetFileName(filePath);
            var url = string.Format(CultureInfo.InvariantCulture, RestfulAnalyzeAsyncServiceUrlFormat, fileName);

            var result = DoTest(url, filePath);

            stopwatch.Stop();

            Console.WriteLine("\r\nRestful test => Finished analyzing file {0} of {1}, in {2}", 
                fileName, 
                new FileInfo(filePath).Length.Bytes().Humanize("0.00"),
                new TimeSpan(stopwatch.ElapsedTicks).Humanize());

            if (result.IsError)
            {
                Console.WriteLine("Server side error: {0}\r\n", result.ErrorMessage);            
            }
            else
            {
                if (string.Compare(expectedResult, result.Status, StringComparison.OrdinalIgnoreCase) == 0)
                    Console.WriteLine("Result is {0}, same as expected => SUCCESS!\r\n", result.Status);
                else
                    Console.WriteLine("Result is {0}, differs from expected ({1}) => FAILURE!\r\n", result.Status, expectedResult);                
            }
        }

        private static AnalyzerResult DoTest(string url, string filePath)
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.AllowWriteStreamBuffering = false;
            request.SendChunked = true;
            request.Method = "POST";
            request.ContentType = "application/octet-stream";

            var buffer = new byte[0x20000]; //128 KB

            using(var requestStream = request.GetRequestStream())
            {
                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    var noOfBytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    while (noOfBytesRead > 0)
                    {
                        requestStream.Write(buffer, 0, noOfBytesRead);
                        noOfBytesRead = fileStream.Read(buffer, 0, buffer.Length);
                    }
                }
            }

            var actualResult = string.Empty;
            var isError = false;
            var errorMessage = string.Empty;
            XElement xElement;

            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    xElement = XElement.Load(responseStream);
                }
            }

            foreach(var element in xElement.Elements())
            {
                switch (element.Name.LocalName)
                {
                    case "Result":
                    {
                        actualResult = element.Value;
                        break;
                    }
                    case "IsError":
                    {
                        bool.TryParse(element.Value, out isError);
                        break;
                    }
                    case "ErrorMessage":
                    {
                        errorMessage = element.Value;
                        break;
                    }
                }
            }

            return new AnalyzerResult
            {
                ErrorMessage = errorMessage,
                IsError = isError,
                Status = actualResult
            };
        }

        private static void SmokeTest()
        {
            var request = (HttpWebRequest)WebRequest.Create(new Uri(RestfulAnalyzerServiceBaseUrl));
            request.Method = "GET";

            var response = request.GetResponse();

            var result = string.Empty;
            using (var stream = response.GetResponseStream())
            {
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }

            response.Close();

            Console.WriteLine("Service replied: {0}", result);        
        }
    }

    internal class AnalyzerResult
    {
        public string Status { get; set; }

        public bool IsError { get; set; }

        public string ErrorMessage { get; set; }
    }
}
