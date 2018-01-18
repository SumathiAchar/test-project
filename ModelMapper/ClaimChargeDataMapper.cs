using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.ModelMapper
{
    public class ClaimChargeDataMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
             .Include<VarianceReport, VarianceReportViewModel>();

            Mapper.CreateMap<ClaimCharge, ClaimChargeDataViewModel>()
                .ForMember(a => a.ClaimId, b => b.MapFrom(c => c.ClaimId))
                .ForMember(a => a.Line, b => b.MapFrom(c => c.Line))
                .ForMember(a => a.RevCode, b => b.MapFrom(c => c.RevCode))
                .ForMember(a => a.HcpcsCode, b => b.MapFrom(c => c.HcpcsCode))
                .ForMember(a => a.ServiceFromDate, b => b.MapFrom(c => c.ServiceFromDate))
                .ForMember(a => a.Units, b => b.MapFrom(c => c.Units))
                .ForMember(a => a.CalculatedAllowed, b => b.MapFrom(c => c.CalculatedAllowed))
                .ForMember(a => a.ExpectedContractualAdjustment, b => b.MapFrom(c => c.ExpectedContractualAdjustment))
                .ForMember(a => a.ActualPayment, b => b.MapFrom(c => c.ActualPayment))
                .ForMember(a => a.ActualContractualAdjustment, b => b.MapFrom(c => c.ActualContractualAdjustment));

            Mapper.CreateMap<ClaimChargeDataViewModel, ClaimCharge>()
                .ForMember(a => a.ClaimId, b => b.MapFrom(c => c.ClaimId))
                .ForMember(a => a.Line, b => b.MapFrom(c => c.Line))
                .ForMember(a => a.RevCode, b => b.MapFrom(c => c.RevCode))
                .ForMember(a => a.HcpcsCode, b => b.MapFrom(c => c.HcpcsCode))
                .ForMember(a => a.ServiceFromDate, b => b.MapFrom(c => c.ServiceFromDate))
                .ForMember(a => a.Units, b => b.MapFrom(c => c.Units))
                .ForMember(a => a.CalculatedAllowed, b => b.MapFrom(c => c.CalculatedAllowed))
                .ForMember(a => a.ExpectedContractualAdjustment, b => b.MapFrom(c => c.ExpectedContractualAdjustment))
                .ForMember(a => a.ActualPayment, b => b.MapFrom(c => c.ActualPayment))
                .ForMember(a => a.ActualContractualAdjustment, b => b.MapFrom(c => c.ActualContractualAdjustment));


        }
    }
}