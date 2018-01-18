using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.UserManagement.ModelMapper
{
    public class AccountMapper : Profile
    {
        protected override void Configure()
        {           
            Mapper.CreateMap<SecurityQuestion, SecurityQuestionViewModel>().ReverseMap();
            Mapper.CreateMap<User, UserViewModel>().ReverseMap();           
        }
    }
}
