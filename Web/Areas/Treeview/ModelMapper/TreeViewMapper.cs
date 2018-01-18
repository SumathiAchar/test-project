using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Treeview.Models;

namespace SSI.ContractManagement.Web.Areas.Treeview.ModelMapper
{
    public class TreeViewMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<ContractHierarchy, TreeViewModel>();

            Mapper.CreateMap<TreeViewModel, ContractHierarchy>()
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.AppendString, m => m.MapFrom(s => s.AppendString))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.IsContract, m => m.MapFrom(s => s.IsContract))
                .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                .ForMember(cv => cv.IsPrimaryNode, m => m.MapFrom(s => s.IsPrimaryNode))
                .ForMember(cv => cv.NodeText, m => m.MapFrom(s => s.NodeText))
                .ForMember(cv => cv.IsCarveOut, m => m.MapFrom(s => s.IsCarveOut))
                .ForMember(cv => cv.ParentId, m => m.MapFrom(s => s.ParentId));


            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractHierarchy, TreeViewModel>();

            Mapper.CreateMap<ContractHierarchy, TreeViewModel>()
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.AppendString, m => m.MapFrom(s => s.AppendString))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.IsContract, m => m.MapFrom(s => s.IsContract))
                .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                .ForMember(cv => cv.IsPrimaryNode, m => m.MapFrom(s => s.IsPrimaryNode))
                .ForMember(cv => cv.NodeText, m => m.MapFrom(s => s.NodeText))
                .ForMember(cv => cv.IsCarveOut, m => m.MapFrom(s => s.IsCarveOut))
                .ForMember(cv => cv.ParentId, m => m.MapFrom(s => s.ParentId));
        }
    }
}
