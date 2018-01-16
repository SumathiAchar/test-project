/************************************************************************************************************/
/**  Author         : Vishesh Bhawsar
/**  Created        : 23-Aug-2013
/**  Summary        : Handles Contract Filters info
/**  User Story Id  : 5.User Story Add a new contract Figure 15
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class ContractFiltersMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
             .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
             .Include<ContractFilter, ContractFiltersViewModel>();

            Mapper.CreateMap<ContractFilter, ContractFiltersViewModel>()
                  .ForMember(cv => cv.FilterName, m => m.MapFrom(s => s.FilterName))
                  .ForMember(cv => cv.FilterValues, m => m.MapFrom(s => s.FilterValues))
                  .ForMember(cv => cv.ServiceLineTypeId, m => m.MapFrom(s => s.ServiceLineTypeId))
                  .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                  .ForMember(cv => cv.IsServiceTypeFilter, m => m.MapFrom(s => s.IsServiceTypeFilter));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<ContractFilter, ContractFiltersViewModel>();

            Mapper.CreateMap<ContractFiltersViewModel, ContractFilter>()
                  .ForMember(cv => cv.FilterName, m => m.MapFrom(s => s.FilterName))
                  .ForMember(cv => cv.FilterValues, m => m.MapFrom(s => s.FilterValues))
                  .ForMember(cv => cv.ServiceLineTypeId, m => m.MapFrom(s => s.ServiceLineTypeId))
                  .ForMember(cv => cv.PaymentTypeId, m => m.MapFrom(s => s.PaymentTypeId))
                  .ForMember(cv => cv.IsServiceTypeFilter, m => m.MapFrom(s => s.IsServiceTypeFilter));
        }
    }
}