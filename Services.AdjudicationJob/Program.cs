using System.ServiceProcess;

namespace SSI.ContractManagement.Windows.Services.BackgroundAdjudicationService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] servicesToRun =
            { 
                new ComputeAdjudicationJob() 
            };
            ServiceBase.Run(servicesToRun);
        }
    }
}
