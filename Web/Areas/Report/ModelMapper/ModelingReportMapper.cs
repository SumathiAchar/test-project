/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Claim Adjudication Report
/**  User Story Id  :Figure 45 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class ModelingReportMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Shared.Models.Contract, ContractBasicInfoViewModel>()
             .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
              .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
               .ForMember(a => a.EffectiveStartDate, b => b.MapFrom(c => c.StartDate))
                 .ForMember(a => a.EffectiveEndDate, b => b.MapFrom(c => c.EndDate))
                  .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))

             .Include<ModelingReport, ModelingReportViewModel>();

            Mapper.CreateMap<ModelingReport, ModelingReportViewModel>()
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                 .ForMember(cv => cv.ContactInfoName, m => m.MapFrom(s => s.ContactInfoName))
                .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                 .ForMember(cv => cv.PageIndex, m => m.MapFrom(s => s.PageIndex))
                  .ForMember(cv => cv.PageSize, m => m.MapFrom(s => s.PageSize))
                    .ForMember(cv => cv.TotalRecords, m => m.MapFrom(s => s.TotalRecords))
                .ForMember(cv => cv.Email, m => m.MapFrom(s => s.Email))
                .ForMember(cv => cv.ServiceType, m => m.MapFrom(s => s.ServiceType))
                .ForMember(cv => cv.ClaimTools, m => m.MapFrom(s => s.ClaimTools))
                .ForMember(cv => cv.PaymentTool, m => m.MapFrom(s => s.PaymentTool))
                .ForMember(cv => cv.PayerId, m => m.MapFrom(s => s.PayerId))
                .ForMember(cv => cv.ContractPhone, m => m.MapFrom(s => s.ContractPhone))
                .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                .ForMember(cv => cv.FacilityName, m => m.MapFrom(s => s.FacilityName))
                .ForMember(cv => cv.PayerCodes, m => m.MapFrom(s => s.PayerCodes));

            Mapper.CreateMap<Shared.Models.Contract, ContractBasicInfoViewModel>()
            .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
             .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
              .ForMember(a => a.EffectiveStartDate, b => b.MapFrom(c => c.StartDate))
                .ForMember(a => a.EffectiveEndDate, b => b.MapFrom(c => c.EndDate))
                 .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))

            .Include<ModelingReport, ModelingReportViewModel>();

            Mapper.CreateMap<ModelingReportViewModel, ModelingReport>()
                .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                 .ForMember(cv => cv.ContactInfoName, m => m.MapFrom(s => s.ContactInfoName))
                  .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                    .ForMember(cv => cv.PageSize, m => m.MapFrom(s => s.PageSize))
                      .ForMember(cv => cv.PageIndex, m => m.MapFrom(s => s.PageIndex))
                        .ForMember(cv => cv.TotalRecords, m => m.MapFrom(s => s.TotalRecords))
                 .ForMember(cv => cv.Email, m => m.MapFrom(s => s.Email))
                .ForMember(cv => cv.ServiceType, m => m.MapFrom(s => s.ServiceType))
                .ForMember(cv => cv.ClaimTools, m => m.MapFrom(s => s.ClaimTools))
                .ForMember(cv => cv.PaymentTool, m => m.MapFrom(s => s.PaymentTool))
                .ForMember(cv => cv.PayerId, m => m.MapFrom(s => s.PayerId))
                .ForMember(cv => cv.ContractPhone, m => m.MapFrom(s => s.ContractPhone))
                .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                .ForMember(cv => cv.PayerCodes, m => m.MapFrom(s => s.PayerCodes));



        }
    }
}