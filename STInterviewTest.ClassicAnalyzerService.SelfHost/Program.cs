using System;
using System.ServiceModel;
using NLog;

namespace STInterviewTest.ClassicAnalyzerService.SelfHost
{
    public class Program
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            Logger.Info("ClassicAnalyzerService.SelfHost started");

            ServiceHost serviceHost = null;

            try
            {
                serviceHost  = new ServiceHost(typeof(ClassicAnalyzerService));
                serviceHost.Open();

                Console.WriteLine("ClassicAnalyzerService started, everything fine so far!");
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

            Logger.Info("ClassicAnalyzerService.SelfHost finished");
        }
    }
}
