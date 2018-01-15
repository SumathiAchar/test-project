/*******************************************************************************************************/
/**  Author         : Prasad & Manjunath
/**  Created        : 12-Sep-2013
/**  Summary        : Handles Claim Data
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Class for the evaluateable claim
    /// </summary>
    public class EvaluateableClaim : IEvaluateableClaim
    {
        /// <summary>
        /// Gets or sets RowId.
        /// </summary>
        public int RowId { set; get; }

        /// <summary>
        /// Gets or sets ClaimId.
        /// </summary>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the ClaimLink.
        /// </summary>
        public long? ClaimLink { set; get; }

        /// <summary>
        /// Gets or sets the Ssinumber.
        /// </summary>
        public int? Ssinumber { set; get; }

        /// <summary>
        /// Gets or sets the ClaimType
        /// </summary>
        public string ClaimType { set; get; }

        /// <summary>
        /// Gets or sets Claim ClaimState.
        /// </summary>
        public string ClaimState { set; get; }

        /// <summary>
        /// Gets or sets the Payer Sequence.
        /// </summary>
        public int? PayerSequence { set; get; }

        /// <summary>
        /// Gets or sets the PatAcctNum.
        /// </summary>
        public string PatAcctNum { set; get; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        /// <value>
        /// The Age.
        /// </value>
        public byte Age { get; set; }

        /// <summary>
        /// Gets or sets the Claim Total.
        /// </summary>
        public double? ClaimTotal { set; get; }

        /// <summary>
        /// Gets or sets the Statement From
        /// </summary> 
        public DateTime? StatementFrom { set; get; }

        /// <summary>
        /// Gets or sets the StatementThru.
        /// </summary> 
        public DateTime? StatementThru { set; get; }

        /// <summary>
        /// Gets or sets the Claim Date.
        /// </summary> 
        public DateTime? ClaimDate { set; get; }

        /// <summary>
        /// Gets or sets the Bill Date.
        /// </summary> 
        public DateTime? BillDate { set; get; }

        /// <summary>
        /// Gets or sets the Last Filed Date.
        /// </summary>  
        public DateTime? LastFiledDate { set; get; }

        /// <summary>
        /// Gets or sets the Bill Type
        /// </summary>
        public string BillType { set; get; }

        /// <summary>
        /// Gets or sets DRG
        /// </summary>
        public string Drg { set; get; }

        /// <summary>
        /// Gets or sets the PriPayerName
        /// </summary>
        public string PriPayerName { set; get; }

        /// <summary>
        /// Gets or sets the SecPayerName
        /// </summary> 
        public string SecPayerName { set; get; }

        /// <summary>
        /// Gets or sets the TerPayerName
        /// </summary>
        public string TerPayerName { set; get; }

        /// <summary>
        /// Gets or sets the FTN
        /// </summary>  
        public string Ftn { set; get; }

        /// <summary>
        /// Gets or sets the NPI
        /// </summary>  
        public string Npi { set; get; }

        /// <summary>
        /// Gets or sets the RenderingPHY
        /// </summary>  
        public string RenderingPhy { set; get; }

        /// <summary>
        /// Gets or sets the RefPHY
        /// </summary>  
        public string RefPhy { set; get; }

        /// <summary>
        /// Gets or sets the AttendingPHY
        /// </summary>  
        public string AttendingPhy { set; get; }

        /// <summary>
        /// Gets or sets the provider zip.
        /// </summary>
        /// <value>
        /// The provider zip.
        /// </value>
        public string ProviderZip { set; get; }

        /// <summary>
        /// Set or Get ModelId.
        /// </summary>
        /// <value>
        /// The ModelId.
        /// </value>
        public long? ModelId { get; set; }

        /// <summary>
        /// Gets or sets the PriICDDCode
        /// </summary>
        public string PriIcddCode { set; get; }

        /// <summary>
        /// Gets or sets the PriICDPCode
        /// </summary>
        public string PriIcdpCode { set; get; }


        /// <summary>
        /// Set or Get Claim Charge Data List.
        /// </summary>
        /// <value>
        /// The Claim Charge Data List.
        /// </value>
        public List<ClaimCharge> ClaimCharges { get; set; }

        /// <summary>
        /// Set or Get Diagnosis Code List.
        /// </summary>
        /// <value>
        /// The Diagnosis Code List.
        /// </value>
        public List<DiagnosisCode> DiagnosisCodes { get; set; }

        /// <summary>
        /// Set or Get Procedure Code List.
        /// </summary>
        /// <value>
        /// The Procedure Code List.
        /// </value>
        public List<ProcedureCode> ProcedureCodes { get; set; }


        /// <summary>
        /// Gets or sets the claim physician data.
        /// </summary>
        /// <value>
        /// The claim physician data.
        /// </value>
        public List<Physician> Physicians { get; set; }

        /// <summary>
        /// Gets or sets the insured data.
        /// </summary>
        /// <value>
        /// The insured data.
        /// </value>
        public List<InsuredData> InsuredCodes { get; set; }

        /// <summary>
        /// Gets or sets the value code data.
        /// </summary>
        /// <value>
        /// The value code data.
        /// </value>
        public List<ValueCode> ValueCodes { get; set; }

        /// <summary>
        /// Gets or sets the occurrence code data.
        /// </summary>
        /// <value>
        /// The occurrence code data.
        /// </value>
        public List<OccurrenceCode> OccurrenceCodes { get; set; }

        /// <summary>
        /// Gets or sets the condition code data.
        /// </summary>
        /// <value>
        /// The condition code data.
        /// </value>
        public List<ConditionCode> ConditionCodes { get; set; }

        /// <summary>
        /// Gets or sets the medicare lab fee schedule list.
        /// </summary>
        /// <value>
        /// The medicare lab fee schedule list.
        /// </value>
        public List<MedicareLabFeeSchedule> MedicareLabFeeSchedules { get; set; }

        /// <summary>
        /// Gets or sets the medicare in patient.
        /// </summary>
        /// <value>
        /// The medicare in patient.
        /// </value>
        public MedicareInPatient MedicareInPatient { get; set; }

        /// <summary>
        /// Gets or sets the microdyn apc edit input.
        /// </summary>
        /// <value>
        /// The microdyn apc edit input.
        /// </value>
        public MicrodynApcEditInput MicrodynApcEditInput { get; set; }

        /// <summary>
        /// Gets or sets the patient data.
        /// </summary>
        /// <value>
        /// The patient data.
        /// </value>
        public PatientData PatientData { get; set; }

        /// <summary>
        /// Gets or sets the patient account number.
        /// </summary>
        /// <value>
        /// The patient account number.
        /// </value>
        public string PatientAccountNumber { get; set; }

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
        public int Los { get; set; }

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
        /// Gets or sets the actual payment.
        /// </summary>
        /// <value>
        /// The actual payment.
        /// </value>
        public double? ActualPayment { get; set; }

        /// <summary>
        /// Gets or sets the adjudicated date.
        /// </summary>
        /// <value>
        /// The adjudicated date.
        /// </value>
        public DateTime? AdjudicatedDate { get; set; }

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

        /// <summary>
        /// Gets or sets the remit contr adj.
        /// </summary>
        /// <value>
        /// The remit contr adj.
        /// </value>
        public double? RemitContrAdj { get; set; }

        /// <summary>
        /// Gets or sets the linked remit identifier.
        /// </summary>
        /// <value>
        /// The linked remit identifier.
        /// </value>
        public string LinkedRemitId { get; set; }

        /// <summary>
        /// Gets or sets the exp cont adj.
        /// </summary>
        /// <value>
        /// The exp cont adj.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        // Used in Variance report
        public double? ExpContAdj { get; set; }

        /// <summary>
        /// Gets or sets the bill date value.
        /// </summary>
        /// <value>
        /// The bill date value.
        /// </value>
        public string BillDateValue { get; set; }

        /// <summary>
        /// Gets or sets the adjudicated date value.
        /// </summary>
        /// <value>
        /// The adjudicated date value.
        /// </value>
        public string AdjudicatedDateValue { get; set; }

        /// <summary>
        /// Gets or sets the statement from value.
        /// </summary>
        /// <value>
        /// The statement from value.
        /// </value>
        public string StatementFromValue { get; set; }

        /// <summary>
        /// Gets or sets the statement thru value.
        /// </summary>
        /// <value>
        /// The statement thru value.
        /// </value>
        public string StatementThruValue { get; set; }

        /// <summary>
        /// Gets or sets the claim date value.
        /// </summary>
        /// <value>
        /// The claim date value.
        /// </value>
        public string ClaimDateValue { get; set; }

        /// <summary>
        /// Gets or sets the last filed date value.
        /// </summary>
        /// <value>
        /// The last filed date value.
        /// </value>
        public string LastFiledDateValue { get; set; }

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
        /// Gets or sets the user defined field1.
        /// </summary>
        /// <value>
        /// The user defined field1.
        /// </value>
        public string CustomField1 { set; get; }

        /// <summary>
        /// Gets or sets the user defined field2.
        /// </summary>
        /// <value>
        /// The user defined field2.
        /// </value>
        public string CustomField2 { set; get; }

        /// <summary>
        /// Gets or sets the user defined field3.
        /// </summary>
        /// <value>
        /// The user defined field3.
        /// </value>
        public string CustomField3 { set; get; }

        /// <summary>
        /// Gets or sets the user defined field4.
        /// </summary>
        /// <value>
        /// The user defined field4.
        /// </value>
        public string CustomField4 { set; get; }

        /// <summary>
        /// Gets or sets the user defined field5.
        /// </summary>
        /// <value>
        /// The user defined field5.
        /// </value>
        public string CustomField5 { set; get; }

        /// <summary>
        /// Gets or sets the user defined field6.
        /// </summary>
        /// <value>
        /// The user defined field6.
        /// </value>
        public string CustomField6 { set; get; }

        /// <summary>
        /// Gets or sets the detailed selection.
        /// </summary>
        /// <value>
        /// The detailed selection.
        /// </value>
        public string DetailedSelection { set; get; }

        /// <summary>
        /// Gets or sets smartbox values. Smartbox is used in Stop loss threshold and custom payment formula
        /// </summary>
        public SmartBox SmartBox { get; set; }

        /// <summary>
        /// Gets or sets the node text.
        /// </summary>
        /// <value>
        /// The node text.
        /// </value>
        public string NodeText { get; set; }

        /// <summary>
        /// Gets or sets the discharge status.
        /// </summary>
        /// <value>
        /// The discharge status.
        /// </value>
        public string DischargeStatus { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// Gets or sets the payer code.
        /// </summary>
        /// <value>
        /// The payer code.
        /// </value>
        public string PayerCode { get; set; }

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
        /// Gets or sets a value indicating whether this instance is retained.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is retained; otherwise, <c>false</c>.
        /// </value>
        public bool IsRetained { get; set; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        public long? ContractId { get; set; }

        /// <summary>
        /// Gets or sets the is selected.
        /// </summary>
        /// <value>
        /// The is selected.
        /// </value>
        public bool? IsSelected { get; set; }

        //FIXED-2016-R3-S3 Add CheckDate,CheckNumber,AdjudicatedContractName properties into IEvaluateableClaim
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
        /// Gets or sets the claim ids.
        /// </summary>
        /// <value>
        /// The claim ids.
        /// </value>
        public string ClaimIds { get; set; }

        /// <summary>
        /// Gets or sets the Insured's Group Number.
        /// </summary>
        /// <value>
        /// the Insured's Group Number.
        /// </value>
        public string InsuredsGroupNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is claim adjudicated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is claim adjudicated; otherwise, <c>false</c>.
        /// </value>
        public bool IsClaimAdjudicated { get; set; }

        /// <summary>
        /// Gets or sets the last adjudicated contract identifier.
        /// </summary>
        /// <value>
        /// The last adjudicated contract identifier.
        /// </value>
        public long LastAdjudicatedContractId { get; set; }

        /// <summary>
        /// Gets or sets the background contract identifier.
        /// </summary>
        /// <value>
        /// The background contract identifier.
        /// </value>
        public long BackgroundContractId { get; set; }

    }
}
