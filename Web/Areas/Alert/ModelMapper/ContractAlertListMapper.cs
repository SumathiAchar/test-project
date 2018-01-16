using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Alert.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Alert.ModelMapper
{
    // ReSharper disable once UnusedMember.Global
    public class ContractAlertMapper : Profile
    {
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractAlert, ContractAlertViewModel>();

            Mapper.CreateMap<ContractAlertViewModel, ContractAlert>()
                .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                .ForMember(cv => cv.DateOfExpiry, m => m.MapFrom(s => s.DateOfExpiry))
                .ForMember(cv => cv.IsVerified, m => m.MapFrom(s => s.IsVerified))
                .ForMember(cv => cv.ContractAlertId, m => m.MapFrom(s => s.ContractAlertId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId));


            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<ClaimFieldDoc, ClaimFieldDocsViewModel>();

            Mapper.CreateMap<ContractAlert, ContractAlertViewModel>()
               .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                .ForMember(cv => cv.DateOfExpiry, m => m.MapFrom(s => s.DateOfExpiry))
                .ForMember(cv => cv.IsVerified, m => m.MapFrom(s => s.IsVerified))
                .ForMember(cv => cv.ContractAlertId, m => m.MapFrom(s => s.ContractAlertId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId));

        }
    }
}