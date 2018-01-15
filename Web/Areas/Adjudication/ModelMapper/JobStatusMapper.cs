using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.ModelMapper
{
    public class JobStatusMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName))
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<TrackTask, JobStatusViewModel>();
            Mapper.CreateMap<TrackTask, JobStatusViewModel>()
                 .ForMember(a => a.RequestName, b => b.MapFrom(c => c.RequestName))
                .ForMember(a => a.TaskId, b => b.MapFrom(c => c.TaskId))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
                .ForMember(a => a.Progresss, b => b.MapFrom(c => c.Progresss))
                .ForMember(a => a.NoOfClaimsSelected, b => b.MapFrom(c => c.ClaimsSelectionCount))
                .ForMember(a => a.NoOfClaimsAdjudicated, b => b.MapFrom(c => c.AdjudicatedClaimsCount))
                .ForMember(a => a.Completion, b => b.MapFrom(c => c.Completion))
                .ForMember(a => a.NoofDaysToDismissCompletedJobs, b => b.MapFrom(c => c.ThresholdDaysToExpireJobs))
                .ForMember(a => a.ElapsedTime, b => b.MapFrom(c => c.ElapsedTime))
                .ForMember(a => a.InitiatedUserName, b => b.MapFrom(c => c.InitiatedUserName))
                .ForMember(a => a.IsVerified, b => b.MapFrom(c => c.IsVerified))
                .ForMember(a => a.ModelId, b => b.MapFrom(c => c.ModelId))
                .ForMember(a => a.ModelName, b => b.MapFrom(c => c.ModelName));


            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName))
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<TrackTask, JobStatusViewModel>();

            Mapper.CreateMap<JobStatusViewModel, TrackTask>()
                .ForMember(a => a.RequestName, b => b.MapFrom(c => c.RequestName))
               .ForMember(a => a.TaskId, b => b.MapFrom(c => c.TaskId))
               .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
               .ForMember(a => a.Progresss, b => b.MapFrom(c => c.Progresss))
               .ForMember(a => a.ClaimsSelectionCount, b => b.MapFrom(c => c.NoOfClaimsSelected))
               .ForMember(a => a.AdjudicatedClaimsCount, b => b.MapFrom(c => c.NoOfClaimsAdjudicated))
               .ForMember(a => a.Completion, b => b.MapFrom(c => c.Completion))
                               .ForMember(a => a.ThresholdDaysToExpireJobs, b => b.MapFrom(c => c.NoofDaysToDismissCompletedJobs))
               .ForMember(a => a.ElapsedTime, b => b.MapFrom(c => c.ElapsedTime))
               .ForMember(a => a.InitiatedUserName, b => b.MapFrom(c => c.InitiatedUserName))
               .ForMember(a => a.IsVerified, b => b.MapFrom(c => c.IsVerified))
               .ForMember(a => a.ModelId, b => b.MapFrom(c => c.ModelId))
               .ForMember(a => a.ModelName, b => b.MapFrom(c => c.ModelName));
                

        }
    }
}