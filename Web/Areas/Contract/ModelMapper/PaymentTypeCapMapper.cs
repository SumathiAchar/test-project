using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeCapMapper.
    /// </summary>
    public class PaymentTypeCapMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<PaymentTypeCap, PaymentTypeCapViewModel>();

            Mapper.CreateMap<PaymentTypeCapViewModel, PaymentTypeCap>()
            .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
            .ForMember(cv => cv.Threshold, m => m.MapFrom(s => s.Threshold))
            .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
            .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
            .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
            .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
            
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<PaymentTypeCap, PaymentTypeCapViewModel>();

            Mapper.CreateMap<PaymentTypeCap, PaymentTypeCapViewModel>()
           .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
           .ForMember(cv => cv.Threshold, m => m.MapFrom(s => s.Threshold))
           .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
           .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
           .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
           .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));


        }
    }
}