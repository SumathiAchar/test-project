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
    public class ClaimAdjudicationReportMapper : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Shared.Models.Contract, ContractBasicInfoViewModel>()
            .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
             .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
              .ForMember(a => a.EffectiveStartDate, b => b.MapFrom(c => c.StartDate))
                 .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))
                 .ForMember(a => a.ClaimType, b => b.MapFrom(c => c.ClaimType))

            .Include<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>();

            Mapper.CreateMap<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>()
                .ForMember(cv => cv.Reimbursement, m => m.MapFrom(s => s.Reimbursement))
                .ForMember(cv => cv.ClaimId, m => m.MapFrom(s => s.ClaimId))
                .ForMember(cv => cv.AdjudicationStatus, m => m.MapFrom(s => s.AdjudicationStatus))
                .ForMember(cv => cv.Payment, m => m.MapFrom(s => s.Payment))
                .ForMember(cv => cv.CodeSelection, m => m.MapFrom(s => s.CodeSelection))
                .ForMember(cv => cv.ClaimIdentity, m => m.MapFrom(s => s.ClaimIdentity))
                .ForMember(cv => cv.ServiceType, m => m.MapFrom(s => s.ServiceType))
                .ForMember(cv => cv.FacilityName, m => m.MapFrom(s => s.FacilityName))
                .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                .ForMember(cv => cv.ModelId, m => m.MapFrom(s => s.ModelId))
                .ForMember(cv => cv.PatientAccountNumber, m => m.MapFrom(s => s.PatientAccountNumber))
                .ForMember(cv => cv.ClaimSearchCriteria, m => m.MapFrom(s => s.ClaimSearchCriteria))
                .ForMember(cv => cv.TotalRecords, m => m.MapFrom(s => s.TotalRecords))
                .ForMember(cv => cv.PaymentsLinked, m => m.MapFrom(s => s.PaymentsLinked))
                .ForMember(cv => cv.ClaimsAdjudicated, m => m.MapFrom(s => s.ClaimsAdjudicated))
                .ForMember(cv => cv.ClaimCharges, m => m.MapFrom(s => s.ClaimCharges))
                .ForMember(cv => cv.ClaimVariances, m => m.MapFrom(s => s.ClaimVariances))
                .ForMember(cv => cv.VarianceRanges, m => m.MapFrom(s => s.VarianceRanges))
                .ForMember(cv => cv.MedicareSequesterPayment, m => m.MapFrom(s => s.MedicareSequesterPayment))
                .ForMember(cv => cv.PatientResponsibility, m => m.MapFrom(s => s.PatientResponsibility))
                .ForMember(cv => cv.MedicareSequesterAmount, m => m.MapFrom(s => s.MedicareSequesterAmount))
                .ForMember(cv => cv.ContractServiceTypeId, m => m.MapFrom(s => s.ContractServiceTypeId))
                .ForMember(cv => cv.HcpcsModifier, m => m.MapFrom(s => s.HcpcsModifier))
                .ForMember(cv => cv.PlaceOfService, m => m.MapFrom(s => s.PlaceOfService))
                .ForMember(cv => cv.ClaimType, m => m.MapFrom(s => s.ClaimType))
                .ForMember(cv => cv.Contracts, m => m.MapFrom(s => s.Contracts));

            Mapper.CreateMap<Shared.Models.Contract, ContractBasicInfoViewModel>()
           .ForMember(a => a.ContractId, b => b.MapFrom(c => c.ContractId))
            .ForMember(a => a.ContractName, b => b.MapFrom(c => c.ContractName))
             .ForMember(a => a.EffectiveStartDate, b => b.MapFrom(c => c.StartDate))
                .ForMember(a => a.ParentId, b => b.MapFrom(c => c.ParentId))

                .Include<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>();

            Mapper.CreateMap<SummaryReport, SummaryReportViewModel>()
                .ForMember(a => a.Scenario, b => b.MapFrom(c => c.Scenario))
                .ForMember(a => a.ClaimCount, b => b.MapFrom(c => c.ClaimCount))
                .ForMember(a => a.Percentage, b => b.MapFrom(c => c.Percentage))
                .ForMember(a => a.StartValue, b => b.MapFrom(c => c.StartValue))
                .ForMember(a => a.EndValue, b => b.MapFrom(c => c.EndValue))
                .ForMember(a => a.Amount, b => b.MapFrom(c => c.Amount));
           
            Mapper.CreateMap<ClaimAdjudicationReportViewModel, ClaimAdjudicationReport>()
                .ForMember(cv => cv.Reimbursement, m => m.MapFrom(s => s.Reimbursement))
                .ForMember(cv => cv.ClaimId, m => m.MapFrom(s => s.ClaimId))
                .ForMember(cv => cv.AdjudicationStatus, m => m.MapFrom(s => s.AdjudicationStatus))
                .ForMember(cv => cv.Payment, m => m.MapFrom(s => s.Payment))
                .ForMember(cv => cv.CodeSelection, m => m.MapFrom(s => s.CodeSelection))
                .ForMember(cv => cv.ClaimIdentity, m => m.MapFrom(s => s.ClaimIdentity))
                .ForMember(cv => cv.ServiceType, m => m.MapFrom(s => s.ServiceType))
                .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                .ForMember(cv => cv.ModelId, m => m.MapFrom(s => s.ModelId))
                .ForMember(cv => cv.FacilityName, m => m.MapFrom(s => s.FacilityName))
                .ForMember(cv => cv.PatientAccountNumber, m => m.MapFrom(s => s.PatientAccountNumber))
                .ForMember(cv => cv.ClaimSearchCriteria, m => m.MapFrom(s => s.ClaimSearchCriteria))
                .ForMember(cv => cv.PatientResponsibility, m => m.MapFrom(s => s.PatientResponsibility));
        }
    }
}