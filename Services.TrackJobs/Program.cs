using System.ServiceProcess;

namespace SSI.ContractManagement.Windows.Services.OnDemandAdjudicationService
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
                new OnDemandAdjudicationService() 
            };
            ServiceBase.Run(servicesToRun);
            
//            #if DEBUG
//            OnDemandAdjudicationService myService = new OnDemandAdjudicationService();
//            myService.OnDemandAdjudication();
//#else
//        ServiceBase[] servicesToRun =
//            { 
//                new OnDemandAdjudicationService() 
//            };
//            ServiceBase.Run(servicesToRun);
//#endif

        }
    }
}
