using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class LetterTemplateMapper :Profile{

        protected override void Configure()
        {
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName))
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
            .Include<LetterTemplate, LetterTemplateViewModel>();

            Mapper.CreateMap<LetterTemplateViewModel, LetterTemplate>()
               .ForMember(cv => cv.LetterTemplateId, m => m.MapFrom(s => s.LetterTemplateId))
               .ForMember(cv => cv.TemplateText, m => m.MapFrom(s => s.TemplateText))
               .ForMember(cv => cv.Name, m => m.MapFrom(s => s.Name));
            
            Mapper.CreateMap<BaseModel, BaseViewModel>()
            .ForMember(a => a.UserName, b => b.MapFrom(c => c.UserName))
            .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
            .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
            .Include<LetterTemplate, LetterTemplateViewModel>();

            Mapper.CreateMap<LetterTemplate, LetterTemplateViewModel>()
               .ForMember(cv => cv.LetterTemplateId, m => m.MapFrom(s => s.LetterTemplateId))
               .ForMember(cv => cv.TemplateText, m => m.MapFrom(s => s.TemplateText))
               .ForMember(cv => cv.Name, m => m.MapFrom(s => s.Name));


        }
    }
}