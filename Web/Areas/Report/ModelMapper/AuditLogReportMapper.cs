using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class AuditLogReportMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<AuditLogReport, AuditLogReportViewModel>();
            Mapper.CreateMap<AuditLogReportViewModel, AuditLogReport>();
        }
    }
}
