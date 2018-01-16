using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    // ReSharper disable once UnusedMember.Global
    public class ClaimFieldDocsMapper : Profile
    {
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
              
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ClaimFieldDoc, ClaimFieldDocsViewModel>();

            Mapper.CreateMap<ClaimFieldDocsViewModel, ClaimFieldDoc>()
                .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
                .ForMember(cv => cv.FileName, m => m.MapFrom(s => s.FileName))
                
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.TableName, m => m.MapFrom(s => s.TableName))
                .ForMember(cv => cv.ColumnHeaderFirst, m => m.MapFrom(s => s.ColumnHeaderFirst))
                .ForMember(cv => cv.ColumnHeaderSecond, m => m.MapFrom(s => s.ColumnHeaderSecond))
                .ForMember(cv => cv.ClaimFieldId, m => m.MapFrom(s => s.ClaimFieldId));

            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseViewModel, BaseModel>()
              
                .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ClaimFieldDocsViewModel, ClaimFieldDoc>();

            Mapper.CreateMap<ClaimFieldDoc, ClaimFieldDocsViewModel>()
                .ForMember(cv => cv.ClaimFieldDocId, m => m.MapFrom(s => s.ClaimFieldDocId))
                .ForMember(cv => cv.FileName, m => m.MapFrom(s => s.FileName))
                
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.TableName, m => m.MapFrom(s => s.TableName))
                .ForMember(cv => cv.ColumnHeaderFirst, m => m.MapFrom(s => s.ColumnHeaderFirst))
                .ForMember(cv => cv.ColumnHeaderSecond, m => m.MapFrom(s => s.ColumnHeaderSecond))
                .ForMember(cv => cv.ClaimFieldId, m => m.MapFrom(s => s.ClaimFieldId));

        }
    }
}