using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class For Contract Service Type 
    /// </summary>
    public class ContractServiceTypeMapper : Profile
    {        
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
          .Include<ContractServiceType, ContractServiceTypeViewModel>();

            Mapper.CreateMap<ContractServiceType, ContractServiceTypeViewModel>()
             .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
             .ForMember(cv => cv.IsCarveOut, m => m.MapFrom(s => s.IsCarveOut))
             .ForMember(cv => cv.ContractServiceTypeName, m => m.MapFrom(s => s.ContractServiceTypeName))
             .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
             .ForMember(cv => cv.Notes, m => m.MapFrom(s => s.Notes))
             .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
             .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
          .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
         .Include<ContractServiceType, ContractServiceTypeViewModel>();

            Mapper.CreateMap<ContractServiceTypeViewModel, ContractServiceType>()
             .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
             .ForMember(cv => cv.IsCarveOut, m => m.MapFrom(s => s.IsCarveOut))
             .ForMember(cv => cv.ContractServiceTypeName, m => m.MapFrom(s => s.ContractServiceTypeName))
             .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
             .ForMember(cv => cv.Notes, m => m.MapFrom(s => s.Notes))
             .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
             .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate));
        }
    }
}