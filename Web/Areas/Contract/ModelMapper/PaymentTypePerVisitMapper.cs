using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypePerVisitMapper.
    /// </summary>
    public class PaymentTypePerVisitMapper:Profile
    {
        /// <summary>
        /// Configures this instance.
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypePerVisit, PaymentTypePerVisitViewModel>();

            Mapper.CreateMap<PaymentTypePerVisitViewModel, PaymentTypePerVisit>()
              .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
              .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
              .ForMember(cv => cv.Rate, m => m.MapFrom(s => s.Rate))
              .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
            
            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypePerVisit, PaymentTypePerVisitViewModel>();

            Mapper.CreateMap<PaymentTypePerVisit, PaymentTypePerVisitViewModel>()
             .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
             .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
             .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
             .ForMember(cv => cv.Rate, m => m.MapFrom(s => s.Rate))
             .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));




        }
    }
}