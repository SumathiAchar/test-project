using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypePercentageDiscountMapper.
    /// </summary>
    public class PaymentTypePercentageChargeMapper:Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<PaymentTypePercentageCharge, PaymentTypePercentageChargeViewModel>();

            Mapper.CreateMap<PaymentTypePercentageChargeViewModel, PaymentTypePercentageCharge>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<PaymentTypePercentageCharge, PaymentTypePercentageChargeViewModel>();

            Mapper.CreateMap<PaymentTypePercentageCharge, PaymentTypePercentageChargeViewModel>()
               .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
               .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
               .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
               .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
               .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

        }
    }
}