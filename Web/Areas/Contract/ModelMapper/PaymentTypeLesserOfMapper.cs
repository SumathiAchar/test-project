using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class PaymentTypeLesserOfMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<PaymentTypeLesserOf, PaymentTypeLesserOfViewModel>();

            Mapper.CreateMap<PaymentTypeLesserOfViewModel, PaymentTypeLesserOf>()
                .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<PaymentTypeLesserOf, PaymentTypeLesserOfViewModel>();

            Mapper.CreateMap<PaymentTypeLesserOf, PaymentTypeLesserOfViewModel>()
           .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
           .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
           .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId));
        }
    }
}