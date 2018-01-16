using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class For Contract Service Line Table Selection 
    /// </summary>
    public class ContractServiceLineTableSelectionMapper : Profile
    {      
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<ContractServiceLineTableSelection, ContractServiceLineTableSelectionViewModel>();

            Mapper.CreateMap<ContractServiceLineTableSelection, ContractServiceLineTableSelectionViewModel>()
                .ForMember(cv => cv.ContractServiceLineTableSelectionId,
                           m => m.MapFrom(s => s.ContractServiceLineTableSelectionId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.TableName, m => m.MapFrom(s => s.TableName))
                .ForMember(cv => cv.Field, m => m.MapFrom(s => s.Field))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status))
                .ForMember(cv => cv.TableType, m => m.MapFrom(s => s.TableType))
                .ForMember(cv => cv.UserText, m => m.MapFrom(s => s.UserText));

            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .Include<ContractServiceLineTableSelection, ContractServiceLineTableSelectionViewModel>();

            Mapper.CreateMap<ContractServiceLineTableSelectionViewModel,ContractServiceLineTableSelection >()
                 .ForMember(cv => cv.ContractServiceLineTableSelectionId,
                 m => m.MapFrom(s => s.ContractServiceLineTableSelectionId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.FacilityId, m => m.MapFrom(s => s.FacilityId))
                .ForMember(cv => cv.TableName, m => m.MapFrom(s => s.TableName))
                .ForMember(cv => cv.Field, m => m.MapFrom(s => s.Field))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status))
                .ForMember(cv => cv.TableType, m => m.MapFrom(s => s.TableType))
                .ForMember(cv => cv.UserText, m => m.MapFrom(s => s.UserText));
                
            }
        }

    }
