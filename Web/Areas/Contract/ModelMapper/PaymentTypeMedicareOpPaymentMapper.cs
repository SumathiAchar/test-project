using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class PaymentTypeMedicareOpPaymentMapper:Profile
    {
        protected override void Configure()
        {
                Mapper.CreateMap<BaseModel, BaseViewModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<PaymentTypeMedicareOp, PaymentTypeMedicareOpPaymentViewModel>();

             Mapper.CreateMap<PaymentTypeMedicareOpPaymentViewModel, PaymentTypeMedicareOp>()
                .ForMember(cv => cv.PaymentTypeDetailId,m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.OutPatient, m => m.MapFrom(s => s.OutPatient))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));
           
            
                Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<PaymentTypeMedicareOp, PaymentTypeMedicareOpPaymentViewModel>();

            Mapper.CreateMap<PaymentTypeMedicareOp, PaymentTypeMedicareOpPaymentViewModel>()
               .ForMember(cv => cv.PaymentTypeDetailId,m => m.MapFrom(s => s.PaymentTypeDetailId))
               .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
               .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
               .ForMember(cv => cv.OutPatient, m => m.MapFrom(s => s.OutPatient))
               .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));



        }
    }
}