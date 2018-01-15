using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.ModelMapper
{
    public class SelectClaimsMapper : Profile
    {
        protected override void Configure()
        {
                Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<ClaimSelector, SelectClaimsViewModel>();

            Mapper.CreateMap<SelectClaimsViewModel, ClaimSelector>()
                .ForMember(cv => cv.RequestName, m => m.MapFrom(s => s.RequestName))
                .ForMember(cv => cv.ClaimFieldList, m => m.MapFrom(s => s.ClaimFieldList))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.ModelId, m => m.MapFrom(s => s.ModelId))
                .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                .ForMember(cv => cv.DateFrom, m => m.MapFrom(s => s.DateFrom))
                .ForMember(cv => cv.IsUserDefined, m => m.MapFrom(s => s.IsUserDefined))
                .ForMember(cv => cv.RunningStatus, m => m.MapFrom(s => s.RunningStatus))
                .ForMember(cv => cv.Priority, m => m.MapFrom(s => s.Priority))
                .ForMember(cv => cv.DateTo, m => m.MapFrom(s => s.DateTo));

                Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<ClaimSelector, SelectClaimsViewModel>();

            Mapper.CreateMap<ClaimSelector, SelectClaimsViewModel>()
               .ForMember(cv => cv.RequestName, m => m.MapFrom(s => s.RequestName))
                .ForMember(cv => cv.ClaimFieldList, m => m.MapFrom(s => s.ClaimFieldList))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.ModelId, m => m.MapFrom(s => s.ModelId))
                .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                .ForMember(cv => cv.DateFrom, m => m.MapFrom(s => s.DateFrom))
                .ForMember(cv => cv.IsUserDefined, m => m.MapFrom(s => s.IsUserDefined))
                .ForMember(cv => cv.RunningStatus, m => m.MapFrom(s => s.RunningStatus))
                .ForMember(cv => cv.Priority, m => m.MapFrom(s => s.Priority))
                .ForMember(cv => cv.DateTo, m => m.MapFrom(s => s.DateTo));
        }
    }
}