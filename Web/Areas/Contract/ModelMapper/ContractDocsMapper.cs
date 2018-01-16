using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class for Contract Docs
    /// </summary>
    public class ContractDocsMapper :Profile
    {
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractDoc, ContractUploadFiles>();
            Mapper.CreateMap<ContractDoc, ContractUploadFiles>()
                   .ForMember(cv => cv.Id, m => m.MapFrom(s => s.ContractDocId))
                   .ForMember(cv => cv.AttachedDocuments, m => m.MapFrom(s => s.FileName))
                   .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                    .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                    .ForMember(cv => cv.UserName, m => m.MapFrom(s => s.UserName))
                    .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                   .ForMember(cv => cv.ContractContent, m => m.MapFrom(s => s.ContractContent));

            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseViewModel, BaseModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractUploadFiles,ContractDoc>();
            Mapper.CreateMap<ContractUploadFiles, ContractDoc>()
                .ForMember(cv => cv.ContractDocId, m => m.MapFrom(s => s.Id))
                .ForMember(cv => cv.FileName, m => m.MapFrom(s => s.AttachedDocuments))
                .ForMember(cv => cv.InsertDate, m => m.MapFrom(s => s.InsertDate))
                 .ForMember(cv => cv.UpdateDate, m => m.MapFrom(s => s.UpdateDate))
                 .ForMember(cv => cv.UserName, m => m.MapFrom(s => s.UserName))
                 .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.ContractContent, m => m.MapFrom(s => s.ContractContent));

        }
    }
}