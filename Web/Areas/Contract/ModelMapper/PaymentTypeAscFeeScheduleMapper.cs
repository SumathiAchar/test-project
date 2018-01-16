using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeASCFeeScheduleMapper.
    /// </summary>
    public class PaymentTypeAscFeeScheduleMapper : Profile
    {
        protected override void Configure()
        {
         Mapper.CreateMap<BaseModel, BaseViewModel>()
        .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
        .Include<PaymentTypeAscFeeSchedule, PaymentTypeAscFeeScheduleViewModel>();

            Mapper.CreateMap<PaymentTypeAscFeeScheduleViewModel, PaymentTypeAscFeeSchedule>()
             .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
             .ForMember(cv => cv.Primary, m => m.MapFrom(s => s.Primary))
             .ForMember(cv => cv.Secondary, m => m.MapFrom(s => s.Secondary))
             .ForMember(cv => cv.Tertiary, m => m.MapFrom(s => s.Tertiary))
             .ForMember(cv => cv.Quaternary, m => m.MapFrom(s => s.Quaternary))
             .ForMember(cv => cv.Others, m => m.MapFrom(s => s.Others))
             .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
             .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
             .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
             .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
             .ForMember(cv => cv.OptionSelection, m => m.MapFrom(s => s.OptionSelection));

             Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypeAscFeeSchedule, PaymentTypeAscFeeScheduleViewModel>();

            Mapper.CreateMap<PaymentTypeAscFeeSchedule, PaymentTypeAscFeeScheduleViewModel>()
             .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
             .ForMember(cv => cv.Primary, m => m.MapFrom(s => s.Primary))
             .ForMember(cv => cv.Secondary, m => m.MapFrom(s => s.Secondary))
             .ForMember(cv => cv.Tertiary, m => m.MapFrom(s => s.Tertiary))
             .ForMember(cv => cv.Quaternary, m => m.MapFrom(s => s.Quaternary))
             .ForMember(cv => cv.Others, m => m.MapFrom(s => s.Others))
             .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
             .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
             .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
             .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

            Mapper.CreateMap<AscFeeScheduleOption, AscFeeScheduleOptionViewModel>()
                .ForMember(cv => cv.AscFeeScheduleOptionId, m => m.MapFrom(s => s.AscFeeScheduleOptionId))
                .ForMember(cv => cv.AscFeeScheduleOptionName, m => m.MapFrom(s => s.AscFeeScheduleOptionName));

        }
    }
}