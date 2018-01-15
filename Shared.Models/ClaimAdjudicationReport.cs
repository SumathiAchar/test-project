/**  Summary        :Handles Claim Adjudication Report
/**  User Story Id  :Figure 45  
/** Modification History ************************************************************************************
 *  User Story Id   :Sprint6 US2 - HIPAA Logging 
 *  Date Modified   :4/8/2014
 *  Author          :Sheshagiri
 *  Summary         :Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
/************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimAdjudicationReport : Contract
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
        /// Gets or sets the claim identity.
        /// </summary>
        /// <value>
        /// The claim identity.
        /// </value>
        public long ClaimIdentity { get; set; }
        /// <summary>
        /// Gets or sets the adjudication status.
        /// </summary>
        /// <value>
        /// The adjudication status.
        /// </value>
        public string AdjudicationStatus { get; set; }
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
        public new long? ModelId { get; set; }
        /// <summary>
        /// Gets or sets the type of the date.
        /// </summary>
        /// <value>
        /// The type of the date.
        /// </value>
        public int DateType { get; set; }


        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int? PageIndex { get; set; }
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
        /// <summary>
        /// Gets or sets the RequestedUserID.
        /// </summary>
        /// <value>
        /// The Requested User ID.
        /// </value>
        public string RequestedUserId { get; set; }
       

        /// <summary>
        /// Gets or sets the command timeout for claim adjudication.
        /// </summary>
        /// <value>The command timeout for claim adjudication.</value>
        public int CommandTimeoutForClaimAdjudication { get; set; }

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
        /// Gets or sets the name of the pri payer.
        /// </summary>
        /// <value>The name of the pri payer.</value>
        // ReSharper disable once UnusedMember.Global
        public string PriPayerName { get; set; }
        /// <summary>
        /// Gets or sets the name of the claim contract.
        /// </summary>
        /// <value>The name of the claim contract.</value>
        public string ClaimContractName { get; set; }
        /// <summary>
        /// Gets or sets the los.
        /// </summary>
        /// <value>The los.</value>
        public int Los { get; set; }
        /// <summary>
        /// Gets or sets the statement from.
        /// </summary>
        /// <value>The statement from.</value>
        public DateTime StatementFrom { get; set; }
        /// <summary>
        /// Gets or sets the statement through.
        /// </summary>
        /// <value>The statement through.</value>
        public DateTime StatementThrough { get; set; }
        /// <summary>
        /// Gets or sets the service from date.
        /// </summary>
        /// <value>
        /// The service from date.
        /// </value>
        public DateTime ServiceFromDate { get; set; }
        /// <summary>
        /// Gets or sets the service thru date.
        /// </summary>
        /// <value>The service thru date.</value>
        public DateTime ServiceThruDate { get; set; }
        //public double? ClaimToatal { get; set; }
        /// <summary>
        /// Gets or sets the rev code.
        /// </summary>
        /// <value>The rev code.</value>
        public string RevCode { get; set; }
        /// <summary>
        /// Gets or sets the HCPCS code.
        /// </summary>
        /// <value>The HCPCS code.</value>
        public string HcpcsCode { get; set; }
        /// <summary>
        /// Gets or sets the serv units.
        /// </summary>
        /// <value>The serv units.</value>
        public int? ServUnits { get; set; }
        /// <summary>
        /// Gets or sets the service line.
        /// </summary>
        /// <value>The service line.</value>
        public string ServiceLine { get; set; }
        /// <summary>
        /// Gets or sets the adjudication amount.
        /// </summary>
        /// <value>The adjudication amount.</value>
        // ReSharper disable once UnusedMember.Global
        public string AdjudicationAmount { get; set; }
        /// <summary>
        /// Gets or sets the calculated allowed charge level.
        /// </summary>
        /// <value>The calculated allowed charge level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedChargeLevel { get; set; }
        /// <summary>
        /// Gets or sets the calculated allowed claim level.
        /// </summary>
        /// <value>The calculated allowed claim level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? CalculatedAllowedClaimLevel { get; set; }
        /// <summary>
        /// Gets or sets the total charges.
        /// </summary>
        /// <value>The total charges.</value>
        // ReSharper disable once UnusedMember.Global
        public double? TotalCharges { get; set; }
        /// <summary>
        /// Gets or sets the actual payment claim level.
        /// </summary>
        /// <value>The actual payment claim level.</value>
        // ReSharper disable once UnusedMember.Global
        public double? ActualPaymentClaimLevel { get; set; }
        ///// <summary>
        ///// Gets or sets the actual payment.
        ///// </summary>
        ///// <value>The actual payment.</value>
        //public double? ActualPayment { get; set; }
        /// <summary>
        /// Gets or sets the claim total.
        /// </summary>
        /// <value>The claim total.</value>
        public double? ClaimTotal { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is claim charge data.
        /// </summary>
        /// <value><c>true</c> if this instance is claim charge data; otherwise, <c>false</c>.</value>
        public bool IsClaimChargeData { get; set; }
        /// <summary>
        /// Gets or sets the report threshold.
        /// </summary>
        /// <value>The report threshold.</value>
        public int ReportThreshold { get; set; }
        /// <summary>
        /// Gets or sets the maximum lines for PDF report.
        /// </summary>
        /// <value>The maximum lines for PDF report.</value>
        public int MaxLinesForPdfReport { get; set; }
        // ReSharper disable once CollectionNeverQueried.Global
        public List<ClaimAdjudicationReport> ClaimAdjudicationReports { get; set; }

        public int Take { get; set; }
        /// <summary>
        /// Gets or sets the skip.
        /// </summary>
        /// <value>
        /// The skip.
        /// </value>
        public int Skip { get; set; }
        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public int TotalRecords { get; set; }
        /// <summary>
        /// Gets or sets the claim data.
        /// </summary>
        /// <value>The claim data.</value>
        public List<EvaluateableClaim> ClaimData { get; set; }
        /// <summary>
        /// Gets or sets the contract variance.
        /// </summary>
        /// <value>
        /// The contract variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? ContractVariance { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is formula error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is formula error; otherwise, <c>false</c>.
        /// </value>
        public bool IsFormulaError { get; set; }

        /// <summary>
        /// Gets or sets the npi.
        /// </summary>
        /// <value>
        /// The npi.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Npi { get; set; }

        /// <summary>
        /// Gets or sets the time zone information.
        /// </summary>
        /// <value>
        /// The time zone information.
        /// </value>
        public new string CurrentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the payments linked.
        /// </summary>
        /// <value>
        /// The payments linked.
        /// </value>
        public List<SummaryReport> PaymentsLinked { get; set; }

        /// <summary>
        /// Gets or sets the claims adjudicated.
        /// </summary>
        /// <value>
        /// The claims adjudicated.
        /// </value>
        public List<SummaryReport> ClaimsAdjudicated { get; set; }

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
        /// Gets or sets a value indicating whether this instance is select claims.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is select claims; otherwise, <c>false</c>.
        /// </value>

        public bool IsSelectClaims { get; set; }

        /// <summary>
        /// Gets or sets the column names.
        /// </summary>
        /// <value>
        /// The ColumnNames
        /// </value>
        public List<string> ColumnNames { get; set; }

        /// <summary>
        /// Gets or sets the PageSetting.
        /// </summary>
        /// <summary>
        /// The PageSetting
        /// </summary>
        public PageSetting PageSetting { get; set; }

        /// <summary>
        /// The XmlSerialize
        /// </summary>
        /// <returns></returns>
        public string XmlSerialize()
        {
            XmlSerializer serializer = new XmlSerializer(PageSetting.SearchCriteriaList.GetType());
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(xmlWriter, PageSetting.SearchCriteriaList, emptyNs);
            return stringWriter.ToString();
        }


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
        public List<Contract> Contracts { get; set; } 
    }
}
