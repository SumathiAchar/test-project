using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class PaymentTypeMedicareSequesterMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PaymentTypeMedicareSequesterViewModel, PaymentTypeMedicareSequester>().ReverseMap();
        }
    }
}