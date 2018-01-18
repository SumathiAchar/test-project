using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.ModelMapper
{
    public class ClaimDataMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
             .Include<VarianceReport, VarianceReportViewModel>();

            Mapper.CreateMap<EvaluateableClaim, ClaimDataViewModel>()
                .ForMember(a => a.PatientAccountNumber, b => b.MapFrom(c => c.PatientAccountNumber))
                .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
                .ForMember(a => a.PriPayerName, b => b.MapFrom(c => c.PriPayerName))
                .ForMember(a => a.ClaimId, b => b.MapFrom(c => c.ClaimId))
                .ForMember(a => a.BillType, b => b.MapFrom(c => c.BillType))
                .ForMember(a => a.Drg, b => b.MapFrom(c => c.Drg))
                .ForMember(a => a.Los, b => b.MapFrom(c => c.Los))
                .ForMember(a => a.StatementFrom, b => b.MapFrom(c => c.StatementFrom))
                .ForMember(a => a.StatementThru, b => b.MapFrom(c => c.StatementThru))
                .ForMember(a => a.ClaimTotal, b => b.MapFrom(c => c.ClaimTotal))
                .ForMember(a => a.CalculatedAdjustment, b => b.MapFrom(c => c.CalculatedAdjustment))
                .ForMember(a => a.CalculatedAllowed, b => b.MapFrom(c => c.CalculatedAllowed))
                .ForMember(a => a.ActualPayment, b => b.MapFrom(c => c.ActualPayment));

            Mapper.CreateMap<ClaimDataViewModel, EvaluateableClaim>()
                .ForMember(a => a.PatientAccountNumber, b => b.MapFrom(c => c.PatientAccountNumber))
                .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
                .ForMember(a => a.PriPayerName, b => b.MapFrom(c => c.PriPayerName))
                .ForMember(a => a.ClaimId, b => b.MapFrom(c => c.ClaimId))
                .ForMember(a => a.BillType, b => b.MapFrom(c => c.BillType))
                .ForMember(a => a.Drg, b => b.MapFrom(c => c.Drg))
                .ForMember(a => a.Los, b => b.MapFrom(c => c.Los))
                .ForMember(a => a.StatementFrom, b => b.MapFrom(c => c.StatementFrom))
                .ForMember(a => a.StatementThru, b => b.MapFrom(c => c.StatementThru))
                .ForMember(a => a.ClaimTotal, b => b.MapFrom(c => c.ClaimTotal))
                .ForMember(a => a.CalculatedAdjustment, b => b.MapFrom(c => c.CalculatedAdjustment))
                .ForMember(a => a.CalculatedAllowed, b => b.MapFrom(c => c.CalculatedAllowed))
                .ForMember(a => a.ActualPayment, b => b.MapFrom(c => c.ActualPayment));
        }
    }
}