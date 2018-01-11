using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.BusinessLogic
{
    public interface IJobStatusLogic
    {
        // ReSharper disable once UnusedMemberInSuper.Global
        // ReSharper disable UnusedMethodReturnValue.Global
        bool IsManualAdjudicationRunning();
        // ReSharper restore UnusedMethodReturnValue.Global

        bool CleanupCancelledTasks();

        // ReSharper disable once UnusedMemberInSuper.Global
        int UpdateJobStatus(TrackTask job);
    }
}