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

using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class ClaimAdjudicationReportViewModel : ContractBasicInfoViewModel
    {
        /// <summary>
        /// Gets or sets the  reimbursement.
        /// </summary>
        /// <value>
        /// The  reimbursement.
        /// </value>
        public double? Reimbursement { get; set; }
        /// <summary>
        /// Gets or sets the claim unique identifier.
        /// </summary>
        /// <value>
        /// The claim unique identifier.
        /// </value>
        public string ClaimId { get; set; }
        /// <summary>
        /// Gets or sets the adjudication status.
        /// </summary>
        /// <value>
        /// The adjudication status.
        /// </value>
        public string AdjudicationStatus { get; set; }

        /// <summary>
        /// Gets or sets the claim identity.
        /// </summary>
        /// <value>
        /// The claim identity.
        /// </value>
        public long ClaimIdentity { get; set; }

        /// <summary>
        /// Gets or sets the payment.
        /// </summary>
        /// <value>
        /// The payment.
        /// </value>
        public string Payment { get; set; }
        /// <summary>
        /// Gets or sets the code selection.
        /// </summary>
        /// <value>
        /// The code selection.
        /// </value>
        public string CodeSelection { get; set; }
        /// <summary>
        /// Gets or sets the type of the service.
        /// </summary>
        /// <value>
        /// The type of the service.
        /// </value>
        public string ServiceType { get; set; }
        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        /// <value>
        /// The name of the payer.
        /// </value>
        public string PayerName { get; set; }
        /// <summary>
        /// Gets or sets the claim search criteria.
        /// </summary>
        /// <value>
        /// The claim search criteria.
        /// </value>
        public string ClaimSearchCriteria { get; set; }
        /// <summary>
        /// Gets or sets the model unique identifier.
        /// </summary>
        /// <value>
        /// The model unique identifier.
        /// </value>
        public long ModelId { get; set; }
        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int DateType { get; set; }

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
        // ReSharper disable once UnusedMember.Global
        public string BillType { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string Drg { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string PriPayerName { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string ClaimContractName { get; set; }
        // ReSharper disable once UnusedMember.Global
        public int Los { get; set; }
        // ReSharper disable once UnusedMember.Global
        public DateTime StatementFrom { get; set; }
        /// <summary>
        /// Gets or sets the service from date.
        /// </summary>
        /// <value>
        /// The service from date.
        /// </value>
        public DateTime ServiceFromDate { get; set; }
        // ReSharper disable once UnusedMember.Global
        public DateTime StatementThrough { get; set; }
        //public double? ClaimToatal { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string RevCode { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string HcpcsCode { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string ServUnits { get; set; }
        // ReSharper disable once UnusedMember.Global
        public DateTime ServiceThruDate { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string ServiceLine { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string AdjudicationAmount { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedChargeLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedClaimLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? TotalCharges { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ActualPaymentClaimLevel { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? ClaimTotal { get; set; }
        // ReSharper disable once UnusedMember.Global
        public bool IsClaimChargeData { get; set; }
        public string LoggedInUser { get; set; }
        public int ReportThreshold { get; set; }

        public List<ClaimAdjudicationReportViewModel> ClaimAdjudicationReports { get; set; }
        /// <summary>
        /// Gets or sets the claim data.
        /// </summary>
        /// <value>The claim data.</value>
        // ReSharper disable once CollectionNeverUpdated.Global
        // ClaimDataViewModel is getting used in Report.
        public List<ClaimDataViewModel> ClaimData { get; set; }

        // ReSharper disable once UnusedMember.Global
        public bool IsFormulaError { get; set; }

        /// <summary>
        /// Gets or sets the current date time.
        /// </summary>
        /// <value>
        /// The current date time.
        /// </value>
        public new string CurrentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Gets or sets the payments linked.
        /// </summary>
        /// <value>
        /// The payments linked.
        /// </value>
        public List<SummaryReportViewModel> PaymentsLinked { get; set; }

        /// <summary>
        /// Gets or sets the claims adjudicated.
        /// </summary>
        /// <value>
        /// The claims adjudicated.
        /// </value>
        public List<SummaryReportViewModel> ClaimsAdjudicated { get; set; }

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
        /// Gets or sets the ColumnNames List.
        /// </summary>
        /// <value>
        /// The ColumnNamesList.
        /// </value>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// Gets or sets the medicare sequester payment.
        /// </summary>
        /// <value>
        /// The medicare sequester payment.
        /// </value>
        public string MedicareSequesterPayment { get; set; }

        /// <summary>
        /// Gets or sets the medicare sequester amount.
        /// </summary>
        /// <value>
        /// The medicare sequester amount.
        /// </value>
        public double? MedicareSequesterAmount { get; set; }

        /// <summary>
        /// Gets or sets the contract service type identifier.
        /// </summary>
        /// <value>
        /// The contract service type identifier.
        /// </value>
        public long? ContractServiceTypeId { get; set; }

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
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the contracts.
        /// </summary>
        /// <value>
        /// The contracts.
        /// </value>
        public List<ContractBasicInfoViewModel> Contracts {get; set; }  
    }
}