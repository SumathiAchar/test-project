using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;

namespace SSI.ContractManagement.Web.Areas.Adjudication.ModelMapper
{
    public class ClaimColumnOptionsMapper : Profile
    {
        /// <summary>
        /// Mapper Class for ClaimColumnOptionsMapper
        /// </summary>
        protected override void Configure()
        {
            Mapper.CreateMap<ClaimColumnOptions, ClaimColumnOptionsViewModel>();

        }
    }
}