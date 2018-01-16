using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class ContractServiceLineCodeMapper : Profile
    {
        /// <summary>
        /// Mapper Class For Contract ServiceLine Code
        /// </summary>
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<ContractServiceLine, ContractServiceLineViewModel>();

            //Creating mappings for Model & ViewModel 
            Mapper.CreateMap<ContractServiceLine, ContractServiceLineViewModel>()
                .ForMember(cv => cv.ContractServiceLineId, m => m.MapFrom(s => s.ContractServiceLineId))
                .ForMember(cv => cv.ServiceLineTypeId, m => m.MapFrom(s => s.ServiceLineId))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.IncludedCode, m => m.MapFrom(s => s.IncludedCode))
                .ForMember(cv => cv.Description, m => m.MapFrom(s => s.Description))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status))
                .ForMember(cv => cv.ExcludedCode, m => m.MapFrom(s => s.ExcludedCode));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
           .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
           .Include<ContractServiceLine, ContractServiceLineViewModel>();
            Mapper.CreateMap<ContractServiceLineViewModel,ContractServiceLine >()
                .ForMember(cv => cv.ContractServiceLineId, m => m.MapFrom(s => s.ContractServiceLineId))
                .ForMember(cv => cv.ServiceLineId, m => m.MapFrom(s => s.ServiceLineTypeId))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.IncludedCode, m => m.MapFrom(s => s.IncludedCode))
                .ForMember(cv => cv.Description, m => m.MapFrom(s => s.Description))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status))
                .ForMember(cv => cv.ExcludedCode, m => m.MapFrom(s => s.ExcludedCode));
            }
        
    }
}