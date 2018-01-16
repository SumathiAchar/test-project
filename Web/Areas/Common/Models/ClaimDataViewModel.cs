using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
  
    public class ClaimDataViewModel
    {
        // The below properties of ClaimDataViewModel is used in Reports.
        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the name of the request.
        /// </summary>
        /// <value>
        /// The name of the request.
        /// </value>
        public string RequestName{ get; set; }

        /// <summary>
        /// Gets or sets the patient account number.
        /// </summary>
        /// <value>
        /// The patient account number.
        /// </value>
        public string PatientAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        /// <value>
        /// The Age.
        /// </value>
        public string Age { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the los.
        /// </summary>
        /// <value>
        /// The los.
        /// </value>
        public string Los { get; set; }

        /// <summary>
        /// Gets or sets the calculated allowed.
        /// </summary>
        /// <value>
        /// The calculated allowed.
        /// </value>
        public double? CalculatedAllowed { get; set; }

        /// <summary>
        /// Gets or sets the calculated adjustment.
        /// </summary>
        /// <value>
        /// The calculated adjustment.
        /// </value>
        public double? CalculatedAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the variance.
        /// </summary>
        /// <value>
        /// The variance.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? Variance { get; set; }

        /// <summary>
        /// Gets or sets the actual payment.
        /// </summary>
        /// <value>
        /// The actual payment.
        /// </value>
        public double? ActualPayment { get; set; }

        /// <summary>
        /// Gets or sets the name of the pri payer.
        /// </summary>
        /// <value>
        /// The name of the pri payer.
        /// </value>
        public string PriPayerName { set; get; }

        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the type of the bill.
        /// </summary>
        /// <value>
        /// The type of the bill.
        /// </value>
        public string BillType { set; get; }

        /// <summary>
        /// Gets or sets the DRG.
        /// </summary>
        /// <value>
        /// The DRG.
        /// </value>
        public string Drg { set; get; }

        /// <summary>
        /// Gets or sets the statement from.
        /// </summary>
        /// <value>
        /// The statement from.
        /// </value>
        public DateTime? StatementFrom { set; get; }

        /// <summary>
        /// Gets or sets the statement from value.
        /// </summary>
        /// <value>
        /// The statement from value.
        /// </value>
        public string StatementFromValue { get; set; }

        /// <summary>
        /// Gets or sets the statement thru.
        /// </summary>
        /// <value>
        /// The statement thru.
        /// </value>
        public DateTime? StatementThru { set; get; }

        /// <summary>
        /// Gets or sets the statement thru value.
        /// </summary>
        /// <value>
        /// The statement thru value.
        /// </value>
        public string StatementThruValue { get; set; }

        /// <summary>
        /// Gets or sets the claim total.
        /// </summary>
        /// <value>
        /// The claim total.
        /// </value>
        public double? ClaimTotal { set; get; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the claim charge data list.
        /// </summary>
        /// <value>
        /// The claim charge data list.
        /// </value>
        public List<ClaimChargeDataViewModel> ClaimChargeDataList { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the adjudicated date.
        /// </summary>
        /// <value>
        /// The adjudicated date.
        /// </value>
        public DateTime? AdjudicatedDate { get; set; }

        /// <summary>
        /// Gets or sets the adjudicated date value.
        /// </summary>
        /// <value>
        /// The adjudicated date value.
        /// </value>
        public string AdjudicatedDateValue { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the state of the claim.
        /// </summary>
        /// <value>
        /// The state of the claim.
        /// </value>
        public string ClaimState { get; set; }

        /// <summary>
        /// Gets or sets the payer sequence.
        /// </summary>
        /// <value>
        /// The payer sequence.
        /// </value>
        public string PayerSequence { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the claim date.
        /// </summary>
        /// <value>
        /// The claim date.
        /// </value>
        public DateTime? ClaimDate { get; set; }

        /// <summary>
        /// Gets or sets the claim date value.
        /// </summary>
        /// <value>
        /// The claim date value.
        /// </value>
        public string ClaimDateValue { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the bill date.
        /// </summary>
        /// <value>
        /// The bill date.
        /// </value>
        public DateTime? BillDate { get; set; }

        /// <summary>
        /// Gets or sets the bill date value.
        /// </summary>
        /// <value>
        /// The bill date value.
        /// </value>
        public string BillDateValue { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the last filed date.
        /// </summary>
        /// <value>
        /// The last filed date.
        /// </value>
        public DateTime? LastFiledDate { get; set; }

        /// <summary>
        /// Gets or sets the last filed date value.
        /// </summary>
        /// <value>
        /// The last filed date value.
        /// </value>
        public string LastFiledDateValue { get; set; }

        /// <summary>
        /// Gets or sets the pri icdd code.
        /// </summary>
        /// <value>
        /// The pri icdd code.
        /// </value>
        public string PriIcddCode { get; set; }

        /// <summary>
        /// Gets or sets the pri icdp code.
        /// </summary>
        /// <value>
        /// The pri icdp code.
        /// </value>
        public string PriIcdpCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the sec payer.
        /// </summary>
        /// <value>
        /// The name of the sec payer.
        /// </value>
        public string SecPayerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the ter payer.
        /// </summary>
        /// <value>
        /// The name of the ter payer.
        /// </value>
        public string TerPayerName { get; set; }

        /// <summary>
        /// Gets or sets the is remit linked.
        /// </summary>
        /// <value>
        /// The is remit linked.
        /// </value>
        public string IsRemitLinked { get; set; }

        /// <summary>
        /// Gets or sets the claim stat.
        /// </summary>
        /// <value>
        /// The claim stat.
        /// </value>
        public string ClaimStat { get; set; }

        /// <summary>
        /// Gets or sets the adjudicated value.
        /// </summary>
        /// <value>
        /// The adjudicated value.
        /// </value>
        public double? AdjudicatedValue { get; set; }

        /// <summary>
        /// Gets or sets the patient responsibility.
        /// </summary>
        /// <value>
        /// The patient responsibility.
        /// </value>
        public double? PatientResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the remit allowed amt.
        /// </summary>
        /// <value>
        /// The remit allowed amt.
        /// </value>
        public double? RemitAllowedAmt { get; set; }

        /// <summary>
        /// Gets or sets the remit non covered.
        /// </summary>
        /// <value>
        /// The remit non covered.
        /// </value>
        public double? RemitNonCovered { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the remit contr adj.
        /// </summary>
        /// <value>
        /// The remit contr adj.
        /// </value>
        public double? RemitContrAdj { get; set; }

        /// <summary>
        /// Gets or sets the claim link.
        /// </summary>
        /// <value>
        /// The claim link.
        /// </value>
        public string ClaimLink { get; set; }

        /// <summary>
        /// Gets or sets the linked remit identifier.
        /// </summary>
        /// <value>
        /// The linked remit identifier.
        /// </value>
        public string LinkedRemitId { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the exp cont adj.
        /// </summary>
        /// <value>
        /// The exp cont adj.
        /// </value>
        public double? ExpContAdj { get; set; }

        /// <summary>
        /// Gets or sets the ssi number.
        /// </summary>
        /// <value>
        /// The ssi number.
        /// </value>
        public int? SsiNumber { get; set; }

        /// <summary>
        /// Gets or sets the actual adjustment.
        /// </summary>
        /// <value>
        /// The actual adjustment.
        /// </value>
        public double? ActualAdjustment { get; set; }

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
        /// Gets or sets the custom field1.
        /// </summary>
        /// <value>
        /// The custom field1.
        /// </value>
        public string CustomField1 { set; get; }

        /// <summary>
        /// Gets or sets the custom field2.
        /// </summary>
        /// <value>
        /// The custom field2.
        /// </value>
        public string CustomField2 { set; get; }

        /// <summary>
        /// Gets or sets the custom field3.
        /// </summary>
        /// <value>
        /// The custom field3.
        /// </value>
        public string CustomField3 { set; get; }

        /// <summary>
        /// Gets or sets the custom field4.
        /// </summary>
        /// <value>
        /// The custom field4.
        /// </value>
        public string CustomField4 { set; get; }

        /// <summary>
        /// Gets or sets the custom field5.
        /// </summary>
        /// <value>
        /// The custom field5.
        /// </value>
        public string CustomField5 { set; get; }

        /// <summary>
        /// Gets or sets the custom field6.
        /// </summary>
        /// <value>
        /// The custom field6.
        /// </value>
        public string CustomField6 { set; get; }

        /// <summary>
        /// Gets or sets the npi.
        /// </summary>
        /// <value>
        /// The npi.
        /// </value>
        public string Npi { get; set; }

        /// <summary>
        /// Gets or sets the discharge status.
        /// </summary>
        /// <value>
        /// The discharge status.
        /// </value>
        public string DischargeStatus { get; set; }

        /// <summary>
        /// Gets or sets the member identifier.
        /// </summary>
        /// <value>
        /// The member identifier.
        /// </value>
        public string MemberId { get; set; }

        /// <summary>
        /// Gets or sets the icn.
        /// </summary>
        /// <value>
        /// The icn.
        /// </value>
        public string Icn { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public string Mrn { get; set; }

        /// <summary>
        /// Gets or sets the ClaimIdValue.
        /// </summary>
        /// <value>
        /// The ClaimIdValue.
        /// </value>
        public string ClaimIdValue { set; get; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is reviewed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is reviewed; otherwise, <c>false</c>.
        /// </value>
        public bool IsReviewed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is all reviewed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is all reviewed; otherwise, <c>false</c>.
        /// </value>
        public bool IsAllReviewed { get; set; }

        /// <summary>
        /// Gets or sets the check date.
        /// </summary>
        /// <value>
        /// The check date.
        /// </value>
        public string CheckDate { get; set; }

        /// <summary>
        /// Gets or sets the check number.
        /// </summary>
        /// <value>
        /// The check number.
        /// </value>
        public string CheckNumber { get; set; }

        /// <summary>
        /// Gets or sets the Adjudicated Contract Name.
        /// </summary>
        /// <value>
        /// The Adjudicated Contract Name.
        /// </value>
        public string AdjudicatedContractName { get; set; }

        /// <summary>
        /// Gets or sets the Insured's Group Number.
        /// </summary>
        /// <value>
        /// the Insured's Group Number.
        /// </value>
        public string InsuredsGroupNumber { get; set; }
    }
}