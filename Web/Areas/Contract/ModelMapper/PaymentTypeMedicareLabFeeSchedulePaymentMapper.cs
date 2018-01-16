using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeMedicareLabFeeSchedulePaymentMapper.
    /// </summary>
    public class PaymentTypeMedicareLabFeeSchedulePaymentMapper:Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypeMedicareLabFeeSchedule, PaymentTypeMedicareLabFeeSchedulePaymentViewModel>();


            Mapper.CreateMap<PaymentTypeMedicareLabFeeSchedulePaymentViewModel, PaymentTypeMedicareLabFeeSchedule>()
              .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
              .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
              .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
              .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypeMedicareLabFeeSchedule, PaymentTypeMedicareLabFeeSchedulePaymentViewModel>();

            Mapper.CreateMap<PaymentTypeMedicareLabFeeSchedule, PaymentTypeMedicareLabFeeSchedulePaymentViewModel>()
              .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
              .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
              .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
              .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
        }
    }
}