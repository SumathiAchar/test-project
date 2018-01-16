using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeStopLossMapper.
    /// </summary>
    public class PaymentTypeStopLossMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypeStopLoss, PaymentTypeStopLossViewModel>();

            Mapper.CreateMap<PaymentTypeStopLossViewModel, PaymentTypeStopLoss>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.Expression, m => m.MapFrom(s => s.Expression))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
                .ForMember(cv => cv.Days, m => m.MapFrom(s => s.Days))
                .ForMember(cv => cv.RevCode, m => m.MapFrom(s => s.RevCode))
                .ForMember(cv => cv.HcpcsCode, m => m.MapFrom(s => s.CptCode))
                .ForMember(cv => cv.StopLossConditionId, m => m.MapFrom(s => s.StopLossConditionId))
                .ForMember(cv => cv.IsExcessCharge, m => m.MapFrom(s => s.IsExcessCharge));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypeStopLoss, PaymentTypeStopLossViewModel>();

            Mapper.CreateMap<PaymentTypeStopLoss, PaymentTypeStopLossViewModel>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.Threshold, m => m.MapFrom(s => s.Threshold))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
                .ForMember(cv => cv.Days, m => m.MapFrom(s => s.Days))
                .ForMember(cv => cv.RevCode, m => m.MapFrom(s => s.RevCode))
                .ForMember(cv => cv.CptCode, m => m.MapFrom(s => s.HcpcsCode))
                .ForMember(cv => cv.StopLossConditionId, m => m.MapFrom(s => s.StopLossConditionId))
                .ForMember(cv => cv.IsExcessCharge, m => m.MapFrom(s => s.IsExcessCharge));
        }
    }
}