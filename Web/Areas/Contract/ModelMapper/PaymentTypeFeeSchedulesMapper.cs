using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeFeeSchedulesMapper.
    /// </summary>
    public class PaymentTypeFeeSchedulesMapper:Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypeFeeSchedule, PaymentTypeFeeSchedulesViewModel>();

            Mapper.CreateMap<PaymentTypeFeeSchedulesViewModel, PaymentTypeFeeSchedule>()
               .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FeeSchedule, m => m.MapFrom(s => s.FeeSchedule))
                .ForMember(cv => cv.NonFeeSchedule, m => m.MapFrom(s => s.NonFeeSchedule))
                .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypeFeeSchedule, PaymentTypeFeeSchedulesViewModel>();

            Mapper.CreateMap<PaymentTypeFeeSchedule, PaymentTypeFeeSchedulesViewModel>()
               .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FeeSchedule, m => m.MapFrom(s => s.FeeSchedule))
                .ForMember(cv => cv.NonFeeSchedule, m => m.MapFrom(s => s.NonFeeSchedule))
                .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
            
        }


    }
}