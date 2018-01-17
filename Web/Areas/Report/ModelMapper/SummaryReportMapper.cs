using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class SummaryReportMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<SummaryReport, SummaryReportViewModel>()
               .ForMember(a => a.Scenario, b => b.MapFrom(c => c.Scenario))
               .ForMember(a => a.ClaimCount, b => b.MapFrom(c => c.ClaimCount))
               .ForMember(a => a.Percentage, b => b.MapFrom(c => c.Percentage))
               .ForMember(a => a.StartValue, b => b.MapFrom(c => c.StartValue))
               .ForMember(a => a.EndValue, b => b.MapFrom(c => c.EndValue))
               .ForMember(a => a.Amount, b => b.MapFrom(c => c.Amount));
        }
    }
}