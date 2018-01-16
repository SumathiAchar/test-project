using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypePerCaseMapper.
    /// </summary>
    public class PaymentTypePerCaseMapper:Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<BaseModel, BaseViewModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<PaymentTypePerCase, PaymentTypePerCaseViewModel>();

            Mapper.CreateMap<PaymentTypePerCaseViewModel, PaymentTypePerCase>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.Rate, m => m.MapFrom(s => s.Rate))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<PaymentTypePerCase, PaymentTypePerCaseViewModel>();

            Mapper.CreateMap<PaymentTypePerCase, PaymentTypePerCaseViewModel>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.Rate, m => m.MapFrom(s => s.Rate))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));


        }
    }
}