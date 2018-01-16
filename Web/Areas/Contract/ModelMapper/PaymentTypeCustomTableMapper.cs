/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Claim Adjudication Report
/**  User Story Id  :Figure 45 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class PaymentTypeCustomTableMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<PaymentTypeCustomTable, PaymentTypeCustomTableViewModel>()
                .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                .ForMember(a => a.ClaimFieldId, b => b.MapFrom(c => c.ClaimFieldId))
                .ForMember(a => a.DocumentId, b => b.MapFrom(c => c.DocumentId))
                .ForMember(a => a.Expression, b => b.MapFrom(c => c.Expression))
                .ForMember(a => a.PaymentTypeDetailId, b => b.MapFrom(c => c.PaymentTypeDetailId))
                .ForMember(a => a.ServiceTypeId, b => b.MapFrom(c => c.ServiceTypeId));

            Mapper.CreateMap<PaymentTypeCustomTableViewModel, PaymentTypeCustomTable>()
                .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                .ForMember(a => a.ClaimFieldId, b => b.MapFrom(c => c.ClaimFieldId))
                .ForMember(a => a.DocumentId, b => b.MapFrom(c => c.DocumentId))
                .ForMember(a => a.Expression, b => b.MapFrom(c => c.Expression))
                .ForMember(a => a.PaymentTypeDetailId, b => b.MapFrom(c => c.PaymentTypeDetailId))
                .ForMember(a => a.ServiceTypeId, b => b.MapFrom(c => c.ServiceTypeId));

       }
    }
}