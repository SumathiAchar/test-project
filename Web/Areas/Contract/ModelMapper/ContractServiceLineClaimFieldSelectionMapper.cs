using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class For Contract Service Line Claim Field Selection
    /// </summary>
    public class ContractServiceLineClaimFieldSelectionMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
              .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<ContractServiceLineClaimFieldSelection, ContractServiceLineClaimFieldSelectionViewModel>();

            Mapper.CreateMap<ContractServiceLineClaimFieldSelection, ContractServiceLineClaimFieldSelectionViewModel>()
               .ForMember(cv => cv.ContractServiceLineClaimFieldId, m => m.MapFrom(s => s.ContractServiceLineClaimFieldId))
               .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
               .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
               .ForMember(cv => cv.ClaimFieldId, m => m.MapFrom(s => s.ClaimFieldId))
               .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
               .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
               .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
               .ForMember(cv => cv.ClaimField, m => m.MapFrom(s => s.ClaimField))
               .ForMember(cv => cv.Operator, m => m.MapFrom(s => s.Operator))
               .ForMember(cv => cv.Values, m => m.MapFrom(s => s.Values))
               .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status));

            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
              .Include<ContractServiceLineClaimFieldSelection, ContractServiceLineClaimFieldSelectionViewModel>();

            Mapper.CreateMap<ContractServiceLineClaimFieldSelectionViewModel, ContractServiceLineClaimFieldSelection>()
                .ForMember(cv => cv.ContractServiceLineClaimFieldId, m => m.MapFrom(s => s.ContractServiceLineClaimFieldId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.ClaimFieldId, m => m.MapFrom(s => s.ClaimFieldId))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.ClaimField, m => m.MapFrom(s => s.ClaimField))
                .ForMember(cv => cv.Operator, m => m.MapFrom(s => s.Operator))
                .ForMember(cv => cv.Values, m => m.MapFrom(s => s.Values))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status));
        }
    }
}
