using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class PaymentTypePerDiemMapper:Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypePerDiem, PaymentTypePerDiemViewModel>();

            Mapper.CreateMap<PaymentTypePerDiemViewModel, PaymentTypePerDiem>()
               .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
               //.ForMember(cv => cv., m => m.MapFrom(s => s.Rate))
               //.ForMember(cv => cv.DaysFrom, m => m.MapFrom(s => s.DaysFrom))
              // .ForMember(cv => cv.DaysTo, m => m.MapFrom(s => s.DaysTo))
               .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
               .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              // .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
               .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId))
                .ForMember(d => d.PerDiemSelections, opt => opt.MapFrom(s => s.PerDiemSelections));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<PaymentTypePerDiem, PaymentTypePerDiemViewModel>();

            Mapper.CreateMap<PaymentTypePerDiem, PaymentTypePerDiemViewModel>()
               .ForMember(cv => cv.PaymentTypeDetailId, m => m.MapFrom(s => s.PaymentTypeDetailId))
               //.ForMember(cv => cv.Rate, m => m.MapFrom(s => s.Rate))
               //.ForMember(cv => cv.DaysFrom, m => m.MapFrom(s => s.DaysFrom))
               //.ForMember(cv => cv.DaysTo, m => m.MapFrom(s => s.DaysTo))
               .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
               .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
              // .ForMember(cv => cv.Percentage, m => m.MapFrom(s => s.Percentage))
               .ForMember(cv => cv.PerDiemSelections, m => m.MapFrom(s => s.PerDiemSelections))
               .ForMember(cv => cv.ServiceTypeId, m => m.MapFrom(s => s.ServiceTypeId));


        }

    }
}