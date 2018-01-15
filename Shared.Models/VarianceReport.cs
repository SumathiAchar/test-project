/************************************************************************************************************/
/**  Author         :Girija
/**  Created        :14-Sept-2013
/**  Summary        :Handles Variance Report
/**  User Story Id  : Fig - 44 CMS Projection Variance Report
/** Modification History ************************************************************************************
 *  User Story Id   :Sprint6 US2 - HIPAA Logging 
 *  Date Modified   :4/8/2014
 *  Author          :Sheshagiri
 *  Summary         :Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
/************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class VarianceReport:BaseModel
    {
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
        /// Gets or sets the under variance.
        /// </summary>
        /// <value>
        /// The under variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? UnderVariance { get; set; }
        /// <summary>
        /// Gets or sets the over variance.
        /// </summary>
        /// <value>
        /// The over variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? OverVariance { get; set; }
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
        public bool IsTopPayerData{ get; set; }
        /// <summary>
        /// Gets or sets the claim search criteria.
        /// </summary>
        /// <value>
        /// The claim search criteria.
        /// </value>
        public string ClaimSearchCriteria { get; set; }

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
        /// Gets or sets the report level.
        /// </summary>
        /// <value>
        /// The report level.
        /// </value>
        public int ReportLevel { get; set; }
        /// <summary>
        /// Gets or sets the FacilityName.
        /// </summary>
        /// <value>
        /// The Facility Name.
        /// </value>
        public string FacilityName { get; set; }

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public int? PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the patient account number.
        /// </summary>
        /// <value>
        /// The patient account number.
        /// </value>
        public string PatientAccountNumber { set; get; }

        /// <summary>
        /// Gets or sets the report date.
        /// </summary>
        /// <value>The report date.</value>
        public string ReportDate { get; set; }
        /// <summary>
        /// Gets or sets the line.
        /// </summary>
        /// <value>The line.</value>
        public int? Line { get; set; }
        /// <summary>
        /// Gets or sets the rev code.
        /// </summary>
        /// <value>The rev code.</value>
        public string RevCode { get; set; }
        /// <summary>
        /// Gets or sets the HCPCS.
        /// </summary>
        /// <value>The HCPCS.</value>
        public string Hcpcs { get; set; }
        /// <summary>
        /// Gets or sets the serv date.
        /// </summary>
        /// <value>The serv date.</value>
        public DateTime ServDate { get; set; }
        /// <summary>
        /// Gets or sets the serv units.
        /// </summary>
        /// <value>The serv units.</value>
        public int? ServUnits { get; set; }
        /// <summary>
        /// Gets or sets the total charges.
        /// </summary>
        /// <value>The total charges.</value>
        public double? TotalCharges { get; set; }
        /// <summary>
        /// Gets or sets the exp payment.
        /// </summary>
        /// <value>The exp payment.</value>
        public double? ExpPayment { get; set; }
        /// <summary>
        /// Gets or sets the exp cont adj.
        /// </summary>
        /// <value>The exp cont adj.</value>
        public double? ExpContAdj { get; set; }
        /// <summary>
        /// Gets or sets the actual payment.
        /// </summary>
        /// <value>The actual payment.</value>
        public double? ActualPayment { get; set; }
        /// <summary>
        /// Gets or sets the actual cont adj.
        /// </summary>
        /// <value>The actual cont adj.</value>
        public double? ActualContAdj { get; set; }
        /// <summary>
        /// Gets or sets the variance.
        /// </summary>
        /// <value>The variance.</value>
        // ReSharper disable once UnusedMember.Global
        public double? Variance { get; set; }
        /// <summary>
        /// Gets or sets the cal allowed line.
        /// </summary>
        /// <value>The cal allowed line.</value>
        public double? CalAllowedLine { get; set; }
        /// <summary>
        /// Gets or sets the type of the bill.
        /// </summary>
        /// <value>The type of the bill.</value>
        public string BillType { get; set; }
        /// <summary>
        /// Gets or sets the DRG.
        /// </summary>
        /// <value>The DRG.</value>
        public string Drg { get; set; }
        /// <summary>
        /// Gets or sets the los.
        /// </summary>
        /// <value>The los.</value>
        public int Los { get; set; }
        /// <summary>
        /// Gets or sets the stat from.
        /// </summary>
        /// <value>The stat from.</value>
        public DateTime StatFrom { get; set; }
        /// <summary>
        /// Gets or sets the stat through.
        /// </summary>
        /// <value>The stat through.</value>
        public DateTime StatThrough { get; set; }
        /// <summary>
        /// Gets or sets the claim toatal.
        /// </summary>
        /// <value>The claim toatal.</value>
        public double? ClaimToatal { get; set; }
        /// <summary>
        /// Gets or sets the cal allowed.
        /// </summary>
        /// <value>The cal allowed.</value>
        public double? CalAllowed { get; set; }
        /// <summary>
        /// Gets or sets the cal adj.
        /// </summary>
        /// <value>The cal adj.</value>
        public double? CalAdj { get; set; }
        /// <summary>
        /// Gets or sets the cal PMT.
        /// </summary>
        /// <value>The cal PMT.</value>
        public double? CalPmt { get; set; }
        /// <summary>
        /// Gets or sets the claim count.
        /// </summary>
        /// <value>The claim count.</value>
        public double? ClaimCount { get; set; }
        /// <summary>
        /// Gets or sets the logged in user.
        /// </summary>
        /// <value>The logged in user.</value>
        // ReSharper disable once UnusedMember.Global
        public string LoggedInUser { get; set; }
        /// <summary>
        /// Gets or sets the claim data.
        /// </summary>
        /// <value>The claim data.</value>
        public List<EvaluateableClaim> ClaimData { get; set; }
        /// <summary>
        /// Gets or sets the contracts.
        /// </summary>
        /// <value>The contracts.</value>
        public List<Contract> Contracts { get; set; }

        /// <summary>
        /// Gets or sets the RequestedUserID.
        /// </summary>
        /// <value>
        /// The Requested User ID.
        /// </value>
        public string RequestedUserId { get; set; }
       
        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>The type of the file.</value>
        public int? FileType { get; set; }
        /// <summary>
        /// Gets or sets the count threshold.
        /// </summary>
        /// <value>The count threshold.</value>
        public int CountThreshold { get; set; }

        /// <summary>
        /// Gets or sets the command timeout for variance report.
        /// </summary>
        /// <value>The command timeout for variance report.</value>
        public int CommandTimeoutForVarianceReport { get; set; }
        /// <summary>
        /// Gets or sets the maximum lines for PDF report.
        /// </summary>
        /// <value>The maximum lines for PDF report.</value>
        public int MaxLinesForPdfReport { get; set; }
        
        /// <summary>
        /// Gets or sets the HCPCS code.
        /// </summary>
        /// <value>The HCPCS code.</value>
        public string HcpcsCode { set; get; }
        /// <summary>
        /// Gets or sets the service from date.
        /// </summary>
        /// <value>The service from date.</value>
        public DateTime ServiceFromDate { set; get; }
        /// <summary>
        /// Gets or sets the units.
        /// </summary>
        /// <value>The units.</value>
        public int? Units { set; get; }
        /// <summary>
        /// Gets or sets the calculated allowed charge level.
        /// </summary>
        /// <value>The calculated allowed charge level.</value>
        public double? CalculatedAllowedChargeLevel { get; set; }
        /// <summary>
        /// Gets or sets the calculated allowed claim level.
        /// </summary>
        /// <value>The calculated allowed claim level.</value>
        public double? CalculatedAllowedClaimLevel { get; set; }
        /// <summary>
        /// Gets or sets the expected contractual adjustment.
        /// </summary>
        /// <value>The expected contractual adjustment.</value>
        // ReSharper disable once UnusedMember.Global
        public double? ExpectedContractualAdjustment { get; set; }
        /// <summary>
        /// Gets or sets the actual contractual adjustment.
        /// </summary>
        /// <value>The actual contractual adjustment.</value>
        // ReSharper disable once UnusedMember.Global
        public double? ActualContractualAdjustment { get; set; }
        /// <summary>
        /// Gets or sets the actual payment charge level.
        /// </summary>
        /// <value>The actual payment charge level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? ActualPaymentChargeLevel { get; set; }
        /// <summary>
        /// Gets or sets the actual payment claim level.
        /// </summary>
        /// <value>The actual payment claim level.</value>
        public double? ActualPaymentClaimLevel { get; set; }
        /// <summary>
        /// Gets or sets the variance charge level.
        /// </summary>
        /// <value>The variance charge level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? VarianceChargeLevel { get; set; }
        /// <summary>
        /// Gets or sets the variance claim level.
        /// </summary>
        /// <value>The variance claim level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? VarianceClaimLevel { get; set; }
        /// <summary>
        /// Gets or sets the name of the pri payer.
        /// </summary>
        /// <value>The name of the pri payer.</value>
        public string PriPayerName { get; set; }
        /// <summary>
        /// Gets or sets the statement from.
        /// </summary>
        /// <value>The statement from.</value>
        public DateTime StatementFrom { get; set; }
        /// <summary>
        /// Gets or sets the statement thru.
        /// </summary>
        /// <value>The statement thru.</value>
        public DateTime StatementThru { get; set; }
        /// <summary>
        /// Gets or sets the claim total.
        /// </summary>
        /// <value>The claim total.</value>
        public double? ClaimTotal { get; set; }
        /// <summary>
        /// Gets or sets the calculated adjustment.
        /// </summary>
        /// <value>The calculated adjustment.</value>
        public double? CalculatedAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the variance reports.
        /// </summary>
        /// <value>The variance reports.</value>
        public List<VarianceReport> VarianceReports { get; set; }

        /// <summary>
        /// Gets or sets the contractual variance.
        /// </summary>
        /// <value>
        /// The contractual variance.
        /// </value>
        public double? ContractualVariance { get; set; }

        /// <summary>
        /// Gets or sets the payment variance.
        /// </summary>
        /// <value>
        /// The payment variance.
        /// </value>
        public double? PaymentVariance { get; set; }

        /// <summary>
        /// Gets or sets the actual variance.
        /// </summary>
        /// <value>
        /// The actual variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? ActualVariance { get; set; }
        /// <summary>
        /// Gets or sets the patient responsibility.
        /// </summary>
        /// <value>
        /// The patient responsibility.
        /// </value>
        public double? PatientResponsibility { get; set; }

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
        public List<SummaryReport> ClaimsAdjudicated { get; set; }

        /// <summary>
        /// Gets or sets the payments linked.
        /// </summary>
        /// <value>
        /// The payments linked.
        /// </value>
        public List<SummaryReport> PaymentsLinked { get; set; }

        /// <summary>
        /// Gets or sets the claim charges.
        /// </summary>
        /// <value>
        /// The claim charges.
        /// </value>
        public List<SummaryReport> ClaimCharges { get; set; }

        /// <summary>
        /// Gets or sets the claim variances.
        /// </summary>
        /// <value>
        /// The claim variances.
        /// </value>
        public List<SummaryReport> ClaimVariances { get; set; }

        /// <summary>
        /// Gets or sets the variance ranges.
        /// </summary>
        /// <value>
        /// The variance ranges.
        /// </value>
        public List<SummaryReport> VarianceRanges { get; set; }

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
