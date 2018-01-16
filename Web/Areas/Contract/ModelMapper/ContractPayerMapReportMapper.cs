using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class ContractPayerMapReportMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<ContractPayerMapReport, ContractPayerMapReportViewModel>()
                .ForMember(cv => cv.StatementThrough, m => m.MapFrom(s => s.StatementThrough))
                .ForMember(cv => cv.BilledDate, m => m.MapFrom(s => s.BilledDate))
                .ForMember(cv => cv.ReportType, m => m.MapFrom(s => s.ReportType))
                .ForMember(cv => cv.ContractPayerMapReportViewModels, m => m.MapFrom(s => s.ContractPayerMapReports));

            Mapper.CreateMap<ContractPayerMapReportViewModel, ContractPayerMapReport>()
                .ForMember(cv => cv.StatementThrough, m => m.MapFrom(s => s.StatementThrough))
                .ForMember(cv => cv.BilledDate, m => m.MapFrom(s => s.BilledDate))
                .ForMember(cv => cv.ReportType, m => m.MapFrom(s => s.ReportType))
                .ForMember(cv => cv.ContractPayerMapReports, m => m.MapFrom(s => s.ContractPayerMapReportViewModels));
        }
    }
}