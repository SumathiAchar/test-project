using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    public class ContractMapper : Profile
    {
        /// <summary>
        /// Mapper Class for Contract
        /// </summary>
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel,BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<Shared.Models.Contract,ContractBasicInfoViewModel >();
            Mapper.CreateMap<Shared.Models.Contract, ContractBasicInfoViewModel>()
                  .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                  .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
                  .ForMember(a => a.UpdateDate, b => b.MapFrom(c => c.UpdateDate))
                  .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
                  .ForMember(a => a.EffectiveStartDate, b => b.MapFrom(c => c.StartDate))
                  .ForMember(a => a.EffectiveEndDate, b => b.MapFrom(c => c.EndDate))
                  .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                  .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
                  .ForMember(a => a.NodeId, b => b.MapFrom(c => c.NodeId))
                  .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))
                   .ForMember(a => a.IsClaimStartDate, b => b.MapFrom(c => c.IsClaimStartDate))
                  .ForMember(a => a.IsInstitutional, b => b.MapFrom(c => c.IsInstitutional))
                  .ForMember(a => a.IsProfessional, b => b.MapFrom(c => c.IsProfessional))
                  .ForMember(a => a.IsContractServiceTypeFound, b => b.MapFrom(c => c.IsContractServiceTypeFound))
                  .ForMember(a => a.IsModified, b => b.MapFrom(c => c.IsModified));
            Mapper.CreateMap<BaseViewModel, BaseModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<ContractBasicInfoViewModel, Shared.Models.Contract>();
            Mapper.CreateMap<ContractBasicInfoViewModel, Shared.Models.Contract>()
                .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
                .ForMember(a => a.UpdateDate, b => b.MapFrom(c => c.UpdateDate))
                .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
                .ForMember(a => a.StartDate, b => b.MapFrom(c => c.EffectiveStartDate))
                .ForMember(a => a.EndDate, b => b.MapFrom(c => c.EffectiveEndDate))
                .ForMember(a => a.Status, b => b.MapFrom(c => c.Status))
                .ForMember(a => a.NodeId, b => b.MapFrom(c => c.NodeId))
                .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))
                  .ForMember(a => a.IsClaimStartDate, b => b.MapFrom(c => c.IsClaimStartDate))
                  .ForMember(a => a.IsInstitutional, b => b.MapFrom(c => c.IsInstitutional))
                  .ForMember(a => a.IsProfessional, b => b.MapFrom(c => c.IsProfessional))
                  .ForMember(a => a.IsContractServiceTypeFound, b => b.MapFrom(c => c.IsContractServiceTypeFound))
                .ForMember(a => a.IsModified, b => b.MapFrom(c => c.IsModified));

            Mapper.CreateMap<ContractModifiedReasonViewModel, ContractModifiedReason>()
              .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
              .ForMember(a => a.Notes, b => b.MapFrom(c => c.Notes))
              .ForMember(a => a.ReasonCode, b => b.MapFrom(c => c.ReasonCode))
              .ForMember(a => a.NodeId, b => b.MapFrom(c => c.NodeId));
            Mapper.CreateMap<ContractViewModel, Shared.Models.Contract>();


        }
    }
}