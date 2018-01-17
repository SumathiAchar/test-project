/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Variance Report
/**  User Story Id  : Fig - 44 CMS Projection Variance Report
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    // Below are the resharper disabled properties of VarianceReportViewModel which are used in Reports
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class VarianceReportViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the node Id.
        /// </summary>
        /// <value>
        /// The node Id.
        /// </value>
        public long? NodeId { get; set; }
        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int? DateType { get; set; }
        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the claim Id.
        /// </summary>
        /// <value>
        /// The claim Id
        /// </value>
        public string ClaimId { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public Int64? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the total claim charges.
        /// </summary>
        /// <value>
        /// The total claim charges.
        /// </value>
        public double? TotalClaimCharges { get; set; }
        /// <summary>
        /// Gets or sets the actual reimbursement.
        /// </summary>
        /// <value>
        /// The actual reimbursement.
        /// </value>
        public double? ActualReimbursement { get; set; }
        /// <summary>
        /// Gets or sets the project reimbursement.
        /// </summary>
        /// <value>
        /// The project reimbursement.
        /// </value>
        public double? ProjectReimbursement { get; set; }
        /// <summary>
        /// Gets or sets the actual adjustment.
        /// </summary>
        /// <value>
        /// The actual adjustment.
        /// </value>
        public double? ActualAdjustment { get; set; }
        /// <summary>
        /// Gets or sets the project adjustment.
        /// </summary>
        /// <value>
        /// The project adjustment.
        /// </value>
        public double? ProjectAdjustment { get; set; }
        /// <summary>
        /// Gets or sets the net variance.
        /// </summary>
        /// <value>
        /// The net variance.
        /// </value>
        public double? NetVariancepercentage { get; set; }
        /// <summary>
        /// Gets or sets the net variance dollar.
        /// </summary>
        /// <value>
        /// The net variance dollar.
        /// </value>
        public double? NetVarianceDollar { get; set; }
        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        /// <value>
        /// The name of the payer.
        /// </value>
        public string PayerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is top payer data].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is top payer data]; otherwise, <c>false</c>.
        /// </value>
        public bool IsTopPayerData { get; set; }
        /// <summary>
        /// Gets or sets the claim search criteria.
        /// </summary>
        /// <value>
        /// The claim search criteria.
        /// </value>
        public string ClaimSearchCriteria { get; set; }

        // public List<VarianceReportViewModel> VarianceModelList { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is total row].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is total row]; otherwise, <c>false</c>.
        /// </value>
        public bool IsTotalRow { get; set; }
        /// <summary>
        /// Gets or sets the FacilityName.
        /// </summary>
        /// <value>
        /// The Facility Name.
        /// </value>
        public string FacilityName { get; set; }

        /// <summary>
        /// Gets or sets the patient account number.
        /// </summary>
        /// <value>
        /// The patient account number.
        /// </value>
        public string PatientAccountNumber { set; get; }

        public int ReportLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string ReportDate { get; set; }
        public int? Line { get; set; }
        public string RevCode { get; set; }
        public string Hcpcs { get; set; }
        public DateTime ServDate { get; set; }
        public int? ServUnits { get; set; }
        public double? TotalCharges { get; set; }

        public double? ExpPayment { get; set; }
        public double? ExpContAdj { get; set; }
        public double? ActualPmt { get; set; }
        public double? ActualContAdj { get; set; }
        public double? CalAllowedLine { get; set; }


        public string BillType { get; set; }
        public string Drg { get; set; }
        public int Los { get; set; }
        public DateTime StatFrom { get; set; }
        public DateTime StatThrough { get; set; }
        public double? ClaimToatal { get; set; }
        public double? CalAllowed { get; set; }
        public double? CalAdj { get; set; }
        public double? CalPmt { get; set; }
        public double? ClaimCount { get; set; }
        // ReSharper disable once CollectionNeverUpdated.Global
        // ClaimDataViewModel is getting used in Report.
        public List<ClaimDataViewModel> ClaimData { get; set; }
        public List<ContractBasicInfoViewModel> Contracts { get; set; }
        public string LoggedInUser { get; set; }
        public int CountThreshold { get; set; }

        // ReSharper disable once UnusedMember.Global
        public string HcpcsCode { set; get; }
        // ReSharper disable once UnusedMember.Global
        public DateTime ServiceFromDate { set; get; }
        // ReSharper disable once UnusedMember.Global
        public int? Units { set; get; }
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedChargeLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedClaimLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ExpectedContractualAdjustment { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ActualContractualAdjustment { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ActualPaymentChargeLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ActualPaymentClaimLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? VarianceChargeLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? VarianceClaimLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string PriPayerName { get; set; }
        // ReSharper disable once UnusedMember.Global
        public DateTime StatementFrom { get; set; }
        // ReSharper disable once UnusedMember.Global
        public DateTime StatementThru { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ClaimTotal { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAdjustment { get; set; }

        public List<VarianceReportViewModel> VarianceReports { get; set; }
        // ReSharper disable once UnusedMember.Global
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the contractual variance.
        /// </summary>
        /// <value>
        /// The contractual variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? ContractualVariance { get; set; }

        /// <summary>
        /// Gets or sets the payment variance.
        /// </summary>
        /// <value>
        /// The payment variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? PaymentVariance { get; set; }

        /// <summary>
        /// Gets or sets the actual variance.
        /// </summary>
        /// <value>
        /// The actual variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? ActualVariance { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? PatientResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the current date time.
        /// </summary>
        /// <value>
        /// The current date time.
        /// </value>
        public new string CurrentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the total claims.
        /// </summary>
        /// <value>
        /// The total claims.
        /// </value>
        public int TotalClaims { get; set; }

        /// <summary>
        /// Gets or sets the claims adjudicated.
        /// </summary>
        /// <value>
        /// The claims adjudicated.
        /// </value>
        public List<SummaryReportViewModel> ClaimsAdjudicated { get; set; }

        /// <summary>
        /// Gets or sets the payments linked.
        /// </summary>
        /// <value>
        /// The payments linked.
        /// </value>
        public List<SummaryReportViewModel> PaymentsLinked { get; set; }

        /// <summary>
        /// Gets or sets the claim charges.
        /// </summary>
        /// <value>
        /// The claim charges.
        /// </value>
        public List<SummaryReportViewModel> ClaimCharges { get; set; }

        /// <summary>
        /// Gets or sets the claim variances.
        /// </summary>
        /// <value>
        /// The claim variances.
        /// </value>
        public List<SummaryReportViewModel> ClaimVariances { get; set; }

        /// <summary>
        /// Gets or sets the variance ranges.
        /// </summary>
        /// <value>
        /// The variance ranges.
        /// </value>
        public List<SummaryReportViewModel> VarianceRanges { get; set; }

        /// <summary>
        /// Gets or sets the HCPCS modifier.
        /// </summary>
        /// <value>
        /// The HCPCS modifier.
        /// </value>
        public string HcpcsModifier { get; set; }

        /// <summary>
        /// Gets or sets the place of service.
        /// </summary>
        /// <value>
        /// The place of service.
        /// </value>
        public string PlaceOfService { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is place of service enable.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is place of service enable; otherwise, <c>false</c>.
        /// </value>
        public bool IsPlaceOfServiceEnable { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }
    }
}