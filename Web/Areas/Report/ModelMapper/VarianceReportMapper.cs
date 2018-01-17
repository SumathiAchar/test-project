/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Variance Report 
/**  User Story Id  :Figure 45 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using AutoMapper;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;

namespace SSI.ContractManagement.Web.Areas.Report.ModelMapper
{
    public class VarianceReportMapper : Profile
    {
        protected override void Configure()
        {
           
                Mapper.CreateMap<BaseModel, BaseViewModel>()
                    .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                    .Include<VarianceReport, VarianceReportViewModel>();

                Mapper.CreateMap<VarianceReport, VarianceReportViewModel>()
                    .ForMember(cv => cv.ClaimId, m => m.MapFrom(s => s.ClaimId))
                    .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                    .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                    .ForMember(cv => cv.TotalClaimCharges, m => m.MapFrom(s => s.TotalClaimCharges))
                    .ForMember(cv => cv.ActualReimbursement, m => m.MapFrom(s => s.ActualReimbursement))
                    .ForMember(cv => cv.ProjectReimbursement, m => m.MapFrom(s => s.ProjectReimbursement))
                    .ForMember(cv => cv.ActualAdjustment, m => m.MapFrom(s => s.ActualAdjustment))
                    .ForMember(cv => cv.ProjectAdjustment, m => m.MapFrom(s => s.ProjectAdjustment))
                    .ForMember(cv => cv.NetVariancepercentage, m => m.MapFrom(s => s.NetVariancepercentage))
                    .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                    .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                    .ForMember(cv => cv.StartDate, m => m.MapFrom(s => s.StartDate))
                    .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                    .ForMember(cv => cv.EndDate, m => m.MapFrom(s => s.EndDate))
                    .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                    .ForMember(cv => cv.IsTotalRow, m => m.MapFrom(s => s.IsTotalRow))
                    .ForMember(cv => cv.IsTopPayerData, m => m.MapFrom(s => s.IsTopPayerData))
                    .ForMember(cv => cv.ClaimSearchCriteria, m => m.MapFrom(s => s.ClaimSearchCriteria))
                    .ForMember(cv => cv.NetVarianceDollar, m => m.MapFrom(s => s.NetVarianceDollar))
                    .ForMember(cv => cv.PatientAccountNumber, m => m.MapFrom(s => s.PatientAccountNumber))
                    .ForMember(cv => cv.FacilityName, m => m.MapFrom(s => s.FacilityName))
                    .ForMember(cv => cv.ReportLevel, m => m.MapFrom(s => s.ReportLevel))

                    .ForMember(cv => cv.RevCode, m => m.MapFrom(s => s.RevCode))
                    .ForMember(cv => cv.Hcpcs, m => m.MapFrom(s => s.Hcpcs))
                    .ForMember(cv => cv.ServDate, m => m.MapFrom(s => s.ServDate))
                    .ForMember(cv => cv.ServUnits, m => m.MapFrom(s => s.ServUnits))
                    .ForMember(cv => cv.TotalCharges, m => m.MapFrom(s => s.TotalCharges))
                    .ForMember(cv => cv.ExpPayment, m => m.MapFrom(s => s.ExpPayment))
                    .ForMember(cv => cv.ExpContAdj, m => m.MapFrom(s => s.ExpContAdj))
                    .ForMember(cv => cv.ActualPmt, m => m.MapFrom(s => s.ActualPayment))
                    .ForMember(cv => cv.ActualContAdj, m => m.MapFrom(s => s.ActualContAdj))
                    .ForMember(cv => cv.Line, m => m.MapFrom(s => s.Line))
                    .ForMember(cv => cv.CalAllowedLine, m => m.MapFrom(s => s.CalAllowedLine))
                    .ForMember(cv => cv.PlaceOfService, m => m.MapFrom(s => s.PlaceOfService))
                    .ForMember(cv => cv.IsPlaceOfServiceEnable, m => m.MapFrom(s => s.IsPlaceOfServiceEnable))
                    .ForMember(cv => cv.ClaimType, m => m.MapFrom(s => s.ClaimType))

                    .ForMember(cv => cv.BillType, m => m.MapFrom(s => s.BillType))
                    .ForMember(cv => cv.Drg, m => m.MapFrom(s => s.Drg))
                    .ForMember(cv => cv.Los, m => m.MapFrom(s => s.Los))
                    .ForMember(cv => cv.StatFrom, m => m.MapFrom(s => s.StatFrom))
                    .ForMember(cv => cv.StatThrough, m => m.MapFrom(s => s.StatThrough))
                    .ForMember(cv => cv.ClaimToatal, m => m.MapFrom(s => s.ClaimToatal))
                    .ForMember(cv => cv.CalAllowed, m => m.MapFrom(s => s.CalAllowed))
                    .ForMember(cv => cv.CalAdj, m => m.MapFrom(s => s.CalAdj))
                    .ForMember(cv => cv.CalPmt, m => m.MapFrom(s => s.CalPmt))
                    .ForMember(cv => cv.ClaimCount, m => m.MapFrom(s => s.ClaimCount))
                    .ForMember(cv => cv.TotalClaims, m => m.MapFrom(s => s.TotalClaims))
                    .ForMember(cv => cv.ClaimsAdjudicated, m => m.MapFrom(s => s.ClaimsAdjudicated))
                    .ForMember(cv => cv.PaymentsLinked, m => m.MapFrom(s => s.PaymentsLinked))
                    .ForMember(cv => cv.ClaimCharges, m => m.MapFrom(s => s.ClaimCharges))
                    .ForMember(cv => cv.ClaimVariances, m => m.MapFrom(s => s.ClaimVariances))
                    .ForMember(cv => cv.VarianceRanges, m => m.MapFrom(s => s.VarianceRanges));

                Mapper.CreateMap<BaseModel, BaseViewModel>()
                    .ForMember(a => a.FacilityId, b => b.MapFrom(c => c.FacilityId))
                    .Include<VarianceReport, VarianceReportViewModel>();

                Mapper.CreateMap<VarianceReportViewModel, VarianceReport>()
                    .ForMember(cv => cv.ClaimId, m => m.MapFrom(s => s.ClaimId))
                    .ForMember(cv => cv.ContractId, m => m.MapFrom(s => s.ContractId))
                    .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                    .ForMember(cv => cv.TotalClaimCharges, m => m.MapFrom(s => s.TotalClaimCharges))
                    .ForMember(cv => cv.ActualReimbursement, m => m.MapFrom(s => s.ActualReimbursement))
                    .ForMember(cv => cv.ProjectReimbursement, m => m.MapFrom(s => s.ProjectReimbursement))
                    .ForMember(cv => cv.ActualAdjustment, m => m.MapFrom(s => s.ActualAdjustment))
                    .ForMember(cv => cv.ProjectAdjustment, m => m.MapFrom(s => s.ProjectAdjustment))
                    .ForMember(cv => cv.ContractName, m => m.MapFrom(s => s.ContractName))
                    .ForMember(cv => cv.NetVariancepercentage, m => m.MapFrom(s => s.NetVariancepercentage))
                    .ForMember(cv => cv.NodeId, m => m.MapFrom(s => s.NodeId))
                    .ForMember(cv => cv.DateType, m => m.MapFrom(s => s.DateType))
                    .ForMember(cv => cv.StartDate, m => m.MapFrom(s => s.StartDate))
                    .ForMember(cv => cv.EndDate, m => m.MapFrom(s => s.EndDate))
                    .ForMember(cv => cv.PayerName, m => m.MapFrom(s => s.PayerName))
                    .ForMember(cv => cv.IsTotalRow, m => m.MapFrom(s => s.IsTotalRow))
                    .ForMember(cv => cv.IsTopPayerData, m => m.MapFrom(s => s.IsTopPayerData))
                    .ForMember(cv => cv.ClaimSearchCriteria, m => m.MapFrom(s => s.ClaimSearchCriteria))
                    .ForMember(cv => cv.NetVarianceDollar, m => m.MapFrom(s => s.NetVarianceDollar))
                    .ForMember(cv => cv.PatientAccountNumber, m => m.MapFrom(s => s.PatientAccountNumber))
                    .ForMember(cv => cv.FacilityName, m => m.MapFrom(s => s.FacilityName))
                    .ForMember(cv => cv.ReportLevel, m => m.MapFrom(s => s.ReportLevel))

                    .ForMember(cv => cv.RevCode, m => m.MapFrom(s => s.RevCode))
                    .ForMember(cv => cv.Hcpcs, m => m.MapFrom(s => s.Hcpcs))
                    .ForMember(cv => cv.ServDate, m => m.MapFrom(s => s.ServDate))
                    .ForMember(cv => cv.ServUnits, m => m.MapFrom(s => s.ServUnits))
                    .ForMember(cv => cv.TotalCharges, m => m.MapFrom(s => s.TotalCharges))
                    .ForMember(cv => cv.ExpPayment, m => m.MapFrom(s => s.ExpPayment))
                    .ForMember(cv => cv.ExpContAdj, m => m.MapFrom(s => s.ExpContAdj))
                    .ForMember(cv => cv.ActualPayment, m => m.MapFrom(s => s.ActualPmt))
                    .ForMember(cv => cv.ActualContAdj, m => m.MapFrom(s => s.ActualContAdj))
                    .ForMember(cv => cv.Line, m => m.MapFrom(s => s.Line))
                    .ForMember(cv => cv.CalAllowedLine, m => m.MapFrom(s => s.CalAllowedLine))

                    .ForMember(cv => cv.BillType, m => m.MapFrom(s => s.BillType))
                    .ForMember(cv => cv.Drg, m => m.MapFrom(s => s.Drg))
                    .ForMember(cv => cv.Los, m => m.MapFrom(s => s.Los))
                    .ForMember(cv => cv.StatFrom, m => m.MapFrom(s => s.StatFrom))
                    .ForMember(cv => cv.StatThrough, m => m.MapFrom(s => s.StatThrough))
                    .ForMember(cv => cv.ClaimToatal, m => m.MapFrom(s => s.ClaimToatal))
                    .ForMember(cv => cv.CalAllowed, m => m.MapFrom(s => s.CalAllowed))
                    .ForMember(cv => cv.CalAdj, m => m.MapFrom(s => s.CalAdj))
                    .ForMember(cv => cv.CalPmt, m => m.MapFrom(s => s.CalPmt))
                    .ForMember(cv => cv.ClaimCount, m => m.MapFrom(s => s.ClaimCount));
           
        }
    }
}