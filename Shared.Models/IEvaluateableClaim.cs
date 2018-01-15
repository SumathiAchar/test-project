using System;
using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    /// <summary>
    /// Interface for the Evaluateable item
    /// </summary>
    public interface IEvaluateableClaim
    {
        /// <summary>
        /// Gets or sets RowId.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        int RowId { set; get; }

        /// <summary>
        /// Gets or sets ClaimId.
        /// </summary>
        long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>
        /// The contract identifier.
        /// </value>
        long? ContractId { set; get; }
        /// <summary>
        /// Gets or sets the ClaimLink.
        /// </summary>
        long? ClaimLink { set; get; }

        /// <summary>
        /// Gets or sets the Ssinumber.
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        int? Ssinumber { set; get; }

        /// <summary>
        /// Gets or sets the ClaimType
        /// </summary>
        string ClaimType { set; get; }

        /// <summary>
        /// Gets or sets Claim ClaimState.
        /// </summary>
        string ClaimState { set; get; }

        /// <summary>
        /// Gets or sets the Payer Sequence.
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        int? PayerSequence { set; get; }

        /// <summary>
        /// Gets or sets the PatAcctNum.
        /// </summary>
        string PatAcctNum { set; get; }

        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        /// <value>
        /// The Age.
        /// </value>
        byte Age { get; set; }

        /// <summary>
        /// Gets or sets the Claim Total.
        /// </summary>
        double? ClaimTotal { set; get; }
        
        /// <summary>
        /// Gets or sets the Statement From
        /// </summary> 
        DateTime? StatementFrom { set; get; }

        /// <summary>
        /// Gets or sets the StatementThru.
        /// </summary> 
        DateTime? StatementThru { set; get; }

        /// <summary>
        /// Gets or sets the Claim Date.
        /// </summary> 
        DateTime? ClaimDate { set; get; }

        /// <summary>
        /// Gets or sets the Bill Date.
        /// </summary> 
        DateTime? BillDate { set; get; }

        /// <summary>
        /// Gets or sets the Last Filed Date.
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        DateTime? LastFiledDate { set; get; }

        /// <summary>
        /// Gets or sets the Bill Type
        /// </summary>
        string BillType { set; get; }

        /// <summary>
        /// Gets or sets DRG
        /// </summary>
        string Drg { set; get; }

        /// <summary>
        /// Gets or sets the PriPayerName
        /// </summary>
        string PriPayerName { set; get; }

        /// <summary>
        /// Gets or sets the SecPayerName
        /// </summary> 
        // ReSharper disable once UnusedMemberInSuper.Global
        string SecPayerName { set; get; }

        /// <summary>
        /// Gets or sets the TerPayerName
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        string TerPayerName { set; get; }

        /// <summary>
        /// Gets or sets the FTN
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        string Ftn { set; get; }

        /// <summary>
        /// Gets or sets the NPI
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        string Npi { set; get; }

        /// <summary>
        /// Gets or sets the RenderingPHY
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        string RenderingPhy { set; get; }

        /// <summary>
        /// Gets or sets the RefPHY
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        string RefPhy { set; get; }

        /// <summary>
        /// Gets or sets the AttendingPHY
        /// </summary>  
        // ReSharper disable once UnusedMemberInSuper.Global
        string AttendingPhy { set; get; }

        /// <summary>
        /// Set or Get ModelId.
        /// </summary>
        /// <value>
        /// The ModelId.
        /// </value>
        // ReSharper disable once UnusedMemberInSuper.Global
        long? ModelId { get; set; }

        /// <summary>
        /// Gets or sets the PriICDDCode 
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        string PriIcddCode { set; get; }

        /// <summary>
        /// Gets or sets the PriICDPCode
        /// </summary>
        // ReSharper disable once UnusedMemberInSuper.Global
        string PriIcdpCode { set; get; }

        /// <summary>
        /// Set or Get Claim Charge Data List.
        /// </summary>
        /// <value>
        /// The Claim Charge Data List.
        /// </value>
        List<ClaimCharge> ClaimCharges { get; set; }

        /// <summary>
        /// Set or Get Diagnosis Code List.
        /// </summary>
        /// <value>
        /// The Diagnosis Code List.
        /// </value>
        List<DiagnosisCode> DiagnosisCodes { get; set; }

        /// <summary>
        /// Set or Get Procedure Code List.
        /// </summary>
        /// <value>
        /// The Procedure Code List.
        /// </value>
        List<ProcedureCode> ProcedureCodes { get; set; }


        /// <summary>
        /// Gets or sets the claim physician data.
        /// </summary>
        /// <value>
        /// The claim physician data.
        /// </value>
        List<Physician> Physicians { get; set; }

        /// <summary>
        /// Gets or sets the insured data.
        /// </summary>
        /// <value>
        /// The insured data.
        /// </value>
        List<InsuredData> InsuredCodes { get; set; }

        /// <summary>
        /// Gets or sets the value code data.
        /// </summary>
        /// <value>
        /// The value code data.
        /// </value>
        List<ValueCode> ValueCodes { get; set; }

        /// <summary>
        /// Gets or sets the occurrence code data.
        /// </summary>
        /// <value>
        /// The occurrence code data.
        /// </value>
        List<OccurrenceCode> OccurrenceCodes { get; set; }

        /// <summary>
        /// Gets or sets the condition code data.
        /// </summary>
        /// <value>
        /// The condition code data.
        /// </value>
        List<ConditionCode> ConditionCodes { get; set; }
        
        /// <summary>
        /// Gets or sets the medicare lab fee schedule list.
        /// </summary>
        /// <value>
        /// The medicare lab fee schedule list.
        /// </value>
        List<MedicareLabFeeSchedule> MedicareLabFeeSchedules { get; set; }

        /// <summary>
        /// Gets or sets the medicare in patient.
        /// </summary>
        /// <value>
        /// The medicare in patient.
        /// </value>
        MedicareInPatient MedicareInPatient { get; set; }

        /// <summary>
        /// Gets or sets the microdyn apc edit input.
        /// </summary>
        /// <value>
        /// The microdyn apc edit input.
        /// </value>
        MicrodynApcEditInput MicrodynApcEditInput { get; set; }

        /// <summary>
        /// Gets or sets smartbox values. Smartbox is used in Stop loss threshold and custom payment formula
        /// </summary>
        SmartBox SmartBox { get; set; }

        /// <summary>
        /// Gets or sets the custom field1.
        /// </summary>
        /// <value>
        /// The custom field1.
        /// </value>
        string CustomField1 { get; set; }

        /// <summary>
        /// Gets or sets the custom field2.
        /// </summary>
        /// <value>
        /// The custom field2.
        /// </value>
        string CustomField2 { get; set; }

        /// <summary>
        /// Gets or sets the custom field3.
        /// </summary>
        /// <value>
        /// The custom field3.
        /// </value>
        string CustomField3 { get; set; }

        /// <summary>
        /// Gets or sets the custom field4.
        /// </summary>
        /// <value>
        /// The custom field4.
        /// </value>
        string CustomField4 { get; set; }

        /// <summary>
        /// Gets or sets the custom field5.
        /// </summary>
        /// <value>
        /// The custom field5.
        /// </value>
        string CustomField5 { get; set; }

        /// <summary>
        /// Gets or sets the custom field6.
        /// </summary>
        /// <value>
        /// The custom field6.
        /// </value>
        string CustomField6 { get; set; }

        /// <summary>
        /// Gets or sets the icn.
        /// </summary>
        /// <value>
        /// The icn.
        /// </value>
        string Icn { get; set; }
        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        string Mrn { get; set; }

        // ReSharper disable once UnusedMemberInSuper.Global
        string DischargeStatus { set; get; }

        /// <summary>
        /// Gets or sets the Los.
        /// </summary>
        /// <value>
        /// The Los.
        /// </value>
        int Los { get; set; }

        /// <summary>
        /// Gets or sets the patient responsibility.
        /// </summary>
        /// <value>
        /// The patient responsibility.
        /// </value>
        double? PatientResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the check date.
        /// </summary>
        /// <value>
        /// The check date.
        /// </value>
        string CheckDate { get; set; }

        /// <summary>
        /// Gets or sets the check number.
        /// </summary>
        /// <value>
        /// The check number.
        /// </value>
        string CheckNumber { get; set; }

        /// <summary>
        /// Gets or sets the Adjudicated Contract Name.
        /// </summary>
        /// <value>
        /// The Adjudicated Contract Name.
        /// </value>
        string AdjudicatedContractName { get; set; }

        /// <summary>
        /// Gets or sets the Insured's Group Number.
        /// </summary>
        /// <value>
        /// the Insured's Group Number.
        /// </value>
        string InsuredsGroupNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is claim adjudicated.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is claim adjudicated; otherwise, <c>false</c>.
        /// </value>
        bool IsClaimAdjudicated { get; set; }

        /// <summary>
        /// Gets or sets the last adjudicated contract identifier.
        /// </summary>
        /// <value>
        /// The last adjudicated contract identifier.
        /// </value>
        long LastAdjudicatedContractId { get; set; }

        /// <summary>
        /// Gets or sets the background contract identifier.
        /// </summary>
        /// <value>
        /// The background contract identifier.
        /// </value>
        long BackgroundContractId { get; set; }
    }
}
