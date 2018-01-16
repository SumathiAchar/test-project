using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class for Contract Notes
    /// </summary>
    public class ContractNotesMapper : Profile
    {
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractNote, ContractNotesViewModel>();
            Mapper.CreateMap<ContractNote, ContractNotesViewModel>()
                .ForMember(cv => cv.ContractNoteId, m => m.MapFrom(s => s.ContractNoteId))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                .ForMember(cv => cv.UserName, m => m.MapFrom(s => s.UserName))
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.NoteText, m => m.MapFrom(s => s.NoteText))
                .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status));
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseViewModel,BaseModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include< ContractNotesViewModel,ContractNote>();
            Mapper.CreateMap<ContractNotesViewModel, ContractNote>()
            .ForMember(cv => cv.ContractNoteId, m => m.MapFrom(s => s.ContractNoteId))
            .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
            .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
            .ForMember(cv => cv.UserName, m => m.MapFrom(s => s.UserName))
            .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
            .ForMember(cv => cv.NoteText, m => m.MapFrom(s => s.NoteText))
            .ForMember(cv => cv.Status, m => m.MapFrom(s => s.Status));
        }
    }
}