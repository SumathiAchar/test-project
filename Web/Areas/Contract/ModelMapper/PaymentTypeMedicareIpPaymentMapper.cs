using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Class PaymentTypeMedicareIPPaymentMapper.
    /// </summary>
    public class PaymentTypeMedicareIpPaymentMapper:Profile
    {
        protected override void Configure()
        {

            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypeMedicareIp, PaymentTypeMedicareIpPaymentViewModel>();


            Mapper.CreateMap<PaymentTypeMedicareIpPaymentViewModel, PaymentTypeMedicareIp>()
                .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
                .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.InPatient, m => m.MapFrom(s => s.InPatient))
                .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
                .ForMember(cv => cv.MedicareIpAcuteOptions, m => m.MapFrom(s => s.MedicareIpAcuteOptions))
                .ForMember(cv => cv.Formula, m => m.MapFrom(s => s.Formula));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<PaymentTypeMedicareIp, PaymentTypeMedicareIpPaymentViewModel>();

            Mapper.CreateMap<MedicareIpAcuteOption, MedicareIpAcuteOptionViewModel>()
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId))
                .ForMember(cv => cv.MedicareIpAcuteOptionCode, m => m.MapFrom(s => s.MedicareIpAcuteOptionCode))
                .ForMember(cv => cv.MedicareIpAcuteOptionName, m => m.MapFrom(s => s.MedicareIpAcuteOptionName))
                .ForMember(cv => cv.MedicareIpAcuteOptionChilds, m => m.MapFrom(s => s.MedicareIpAcuteOptionChilds));

            Mapper.CreateMap<MedicareIpAcuteOptionChildViewModel, MedicareIpAcuteOptionChild>()
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId))
                .ForMember(cv => cv.MedicareIpAcuteOptionChildCode, m => m.MapFrom(s => s.MedicareIpAcuteOptionChildCode))
                .ForMember(cv => cv.MedicareIpAcuteOptionChildName, m => m.MapFrom(s => s.MedicareIpAcuteOptionChildName))
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId));

            Mapper.CreateMap<MedicareIpAcuteOptionViewModel, MedicareIpAcuteOption>()
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId))
                .ForMember(cv => cv.MedicareIpAcuteOptionCode, m => m.MapFrom(s => s.MedicareIpAcuteOptionCode))
                .ForMember(cv => cv.MedicareIpAcuteOptionName, m => m.MapFrom(s => s.MedicareIpAcuteOptionName))
                .ForMember(cv => cv.MedicareIpAcuteOptionChilds, m => m.MapFrom(s => s.MedicareIpAcuteOptionChilds));

            Mapper.CreateMap<MedicareIpAcuteOptionChild, MedicareIpAcuteOptionChildViewModel>()
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId))
                .ForMember(cv => cv.MedicareIpAcuteOptionChildCode, m => m.MapFrom(s => s.MedicareIpAcuteOptionChildCode))
                .ForMember(cv => cv.MedicareIpAcuteOptionChildName, m => m.MapFrom(s => s.MedicareIpAcuteOptionChildName))
                .ForMember(cv => cv.MedicareIpAcuteOptionId, m => m.MapFrom(s => s.MedicareIpAcuteOptionId));

            Mapper.CreateMap<PaymentTypeMedicareIp, PaymentTypeMedicareIpPaymentViewModel>()
              .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
              .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
              .ForMember(cv => cv.InPatient, m => m.MapFrom(s => s.InPatient))
              .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
              .ForMember(cv => cv.MedicareIpAcuteOptions, m => m.MapFrom(s => s.MedicareIpAcuteOptions))
              .ForMember(cv => cv.Formula, m => m.MapFrom(s => s.Formula));
        }
    }
}