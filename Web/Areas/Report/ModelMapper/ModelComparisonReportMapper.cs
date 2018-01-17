using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class ModelComparisonReportMapper : Profile
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<ModelComparisonReport, ModelComparisonReportViewModel>()
                  .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                  .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                  .ForMember(cv => cv.ModelName, m => m.MapFrom(s => s.ModelName))
                  .ForMember(cv => cv.IsCheckedDetailLevel, m => m.MapFrom(s => s.IsCheckedDetailLevel));
        }
    }
}