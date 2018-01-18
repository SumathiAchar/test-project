using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.ModelMapper
{
    public class FacilityMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<FacilityViewModel, Facility>().ReverseMap();
            Mapper.CreateMap<FeatureControlViewModel, FeatureControl>().ReverseMap();
        }
    }
}