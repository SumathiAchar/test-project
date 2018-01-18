using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.ModelMapper
{
    public class UserMapper : Profile
    {
        /// <summary>
        /// Mapper Class for User
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.UserId, b => b.MapFrom(c => c.UserId))
               .ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName))
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId));

            Mapper.CreateMap<User, UserViewModel>();
            Mapper.CreateMap<UserViewModel,User>();
            Mapper.CreateMap<Facility, FacilityViewModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId));

            Mapper.CreateMap<FacilityViewModel, Facility>();

            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .Include<UserType, UserTypeViewModel>();

            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .Include<UserFacilityMapping, UserFacilityMappingViewModel>();
        }
    }
}