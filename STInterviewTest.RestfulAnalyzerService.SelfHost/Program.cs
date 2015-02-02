using System;
using System.ServiceModel.Web;
using NLog;

namespace STInterviewTest.RestfulAnalyzerService.SelfHost
{
    public class Program
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("RestfulAnalyzerService.SelfHost started");

            WebServiceHost serviceHost = null;

            try
            {
                serviceHost  = new WebServiceHost(typeof(RestfulAnalyzerService));
                serviceHost.Open();

                Console.WriteLine("RestfulAnalyzerService started, everything fine so far!");
                Console.WriteLine("Press [Enter] to quit...");
                Console.ReadLine();
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Unexpected error occured!", exception);
                throw;
            }
            finally
            {
                if (serviceHost != null)
                    serviceHost.Close();
            }

            Logger.Info("RestfulAnalyzerService.SelfHost finished");
        }
    }
}

