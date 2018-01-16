using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.ModelMapper
{
    /// <summary>
    /// Mapper Class for Contract Payer Info
    /// </summary>
    public class ContractPayerInfoMapper : Profile
    {
        protected override void Configure()
        {
            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseModel, BaseViewModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                .Include<ContractPayerInfo, ContractInfoViewModel>();
            
            Mapper.CreateMap<ContractInfoViewModel, ContractPayerInfo>()
                 .ForMember(a => a.ContractPayerInfoId, b => b.MapFrom(c => c.ContractPayerInfoId))
                 .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
                 .ForMember(a => a.UpdateDate, b => b.MapFrom(c => c.UpdateDate))
                 .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                 .ForMember(a => a.PayerId, b => b.MapFrom(c => c.PayerId))
                 .ForMember(a => a.ContractInfoPayerName, b => b.MapFrom(c => c.ContractInfoPayerName))
                 .ForMember(a => a.MailAddress1, b => b.MapFrom(c => c.MailAddress1))
                 .ForMember(a => a.MailAddress2, b => b.MapFrom(c => c.MailAddress2))
                 .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                 .ForMember(a => a.State, b => b.MapFrom(c => c.State))
                 .ForMember(a => a.Zip, b => b.MapFrom(c => c.Zip))
                 .ForMember(a => a.Phone1, b => b.MapFrom(c => c.Phone1))
                 .ForMember(a => a.Phone2, b => b.MapFrom(c => c.Phone2))
                 .ForMember(a => a.Fax, b => b.MapFrom(c => c.Fax))
                 .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                 .ForMember(a => a.Website, b => b.MapFrom(c => c.Website))
                 .ForMember(a => a.TaxId, b => b.MapFrom(c => c.TaxId))
                 .ForMember(a => a.Npi, b => b.MapFrom(c => c.Npi))
                 .ForMember(a => a.Memo, b => b.MapFrom(c => c.Memo))
                 .ForMember(a => a.ProviderId, b => b.MapFrom(c => c.ProviderId))
                 .ForMember(a => a.PlanId, b => b.MapFrom(c => c.PlanId));

            //Creating mappings for Base class inherited
            Mapper.CreateMap<BaseViewModel, BaseModel>()
               .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
               .Include<ContractInfoViewModel, ContractPayerInfo>();
            Mapper.CreateMap<ContractPayerInfo, ContractInfoViewModel>()
                .ForMember(a => a.ContractPayerInfoId, b => b.MapFrom(c => c.ContractPayerInfoId))
                .ForMember(a => a.InsertDate, b => b.MapFrom(c => c.InsertDate))
                .ForMember(a => a.UpdateDate, b => b.MapFrom(c => c.UpdateDate))
                .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
                .ForMember(a => a.PayerId, b => b.MapFrom(c => c.PayerId))
                .ForMember(a => a.ContractInfoPayerName, b => b.MapFrom(c => c.ContractInfoPayerName))
                .ForMember(a => a.MailAddress1, b => b.MapFrom(c => c.MailAddress1))
                .ForMember(a => a.MailAddress2, b => b.MapFrom(c => c.MailAddress2))
                .ForMember(a => a.City, b => b.MapFrom(c => c.City))
                .ForMember(a => a.State, b => b.MapFrom(c => c.State))
                .ForMember(a => a.Zip, b => b.MapFrom(c => c.Zip))
                .ForMember(a => a.Phone1, b => b.MapFrom(c => c.Phone1))
                .ForMember(a => a.Phone2, b => b.MapFrom(c => c.Phone2))
                .ForMember(a => a.Fax, b => b.MapFrom(c => c.Fax))
                .ForMember(a => a.Email, b => b.MapFrom(c => c.Email))
                .ForMember(a => a.Website, b => b.MapFrom(c => c.Website))
                .ForMember(a => a.TaxId, b => b.MapFrom(c => c.TaxId))
                .ForMember(a => a.Npi, b => b.MapFrom(c => c.Npi))
                .ForMember(a => a.Memo, b => b.MapFrom(c => c.Memo))
                .ForMember(a => a.ProviderId, b => b.MapFrom(c => c.ProviderId))
                .ForMember(a => a.PlanId, b => b.MapFrom(c => c.PlanId));
        }
    }
}