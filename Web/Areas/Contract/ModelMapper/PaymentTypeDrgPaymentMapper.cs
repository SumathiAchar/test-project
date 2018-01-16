using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeDrgPaymentMapper.
    /// </summary>
    public class PaymentTypeDrgPaymentMapper:Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
             .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
             .Include<PaymentTypeDrg, PaymentTypeDrgPaymentViewModel>();

            Mapper.CreateMap<PaymentTypeDrgPaymentViewModel, PaymentTypeDrg>()
           .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
           .ForMember(cv => cv.BaseRate, m => m.MapFrom(s => s.BaseRate))
           .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
           .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
           .ForMember(cv => cv.RelativeWeightId, m => m.MapFrom(s => s.RelativeWeightId))
           .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
           .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
            
            Mapper.CreateMap<BaseModel, BaseViewModel>()
             .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
             .Include<PaymentTypeDrg, PaymentTypeDrgPaymentViewModel>();

           Mapper.CreateMap<PaymentTypeDrg, PaymentTypeDrgPaymentViewModel>()
          .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
          .ForMember(cv => cv.BaseRate, m => m.MapFrom(s => s.BaseRate))
          .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
          .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
          .ForMember(cv => cv.RelativeWeightId, m => m.MapFrom(s => s.RelativeWeightId))
          .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
          .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));


        }

    }
}