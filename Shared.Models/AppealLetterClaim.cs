using System;

namespace SSI.ContractManagement.Shared.Models
{    
    public class AppealLetterClaim 
    {
        /// <summary>
        /// Gets or sets the bill date.
        /// </summary>
        /// <value>
        /// The bill date.
        /// </value>
        public DateTime? BillDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the bill.
        /// </summary>
        /// <value>
        /// The type of the bill.
        /// </value>
        public string BillType { get; set; }

        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>
        /// The claim identifier.
        /// </value>
        public long ClaimId { set; get; }

        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// </summary>
        /// <value>
        /// The DRG.
        /// </value>
        public string Drg { get; set; }

        /// <summary>
        /// Gets or sets the expected allowed.
        /// </summary>
        /// <value>
        /// The expected allowed.
        /// </value>
        public double? ExpectedAllowed { get; set; }

        /// <summary>
        /// </summary>
        /// <value>
        /// The FTN.
        /// </value>
        public string Ftn { get; set; }

        /// <summary>
        /// Gets or sets the primary group number.
        /// </summary>
        /// <value>
        /// The primary group number.
        /// </value>
        public string PrimaryGroupNumber { get; set; }

        /// <summary>
        /// Gets or sets the secondary group number.
        /// </summary>
        /// <value>
        /// The secondary group number.
        /// </value>
        public string SecondaryGroupNumber { get; set; }

        /// <summary>
        /// Gets or sets the tertiary group number.
        /// </summary>
        /// <value>
        /// The tertiary group number.
        /// </value>
        public string TertiaryGroupNumber { get; set; }

        /// <summary>
        /// Gets or sets the primary member identifier.
        /// </summary>
        /// <value>
        /// The primary member identifier.
        /// </value>
        public string PrimaryMemberId { get; set; }

        /// <summary>
        /// Gets or sets the secondary member identifier.
        /// </summary>
        /// <value>
        /// The secondary member identifier.
        /// </value>
        public string SecondaryMemberId { get; set; }

        /// <summary>
        /// Gets or sets the tertiary member identifier.
        /// </summary>
        /// <value>
        /// The tertiary member identifier.
        /// </value>
        public string TertiaryMemberId { get; set; }

        /// <summary>
        /// Gets or sets the med record number.
        /// </summary>
        /// <value>
        /// The med record number.
        /// </value>
        public string MedRecNumber { get; set; }

        /// <summary>
        /// Gets or sets the npi.
        /// </summary>
        /// <value>
        /// The npi.
        /// </value>
        public string Npi { get; set; }

        /// <summary>
        /// Gets or sets the patient account number.
        /// </summary>
        /// <value>
        /// The patient account number.
        /// </value>
        public string PatientAccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the patient dob.
        /// </summary>
        /// <value>
        /// The patient dob.
        /// </value>
        public DateTime? PatientDob { get; set; }

        /// <summary>
        /// Gets or sets the first name of the patient.
        /// </summary>
        /// <value>
        /// The first name of the patient.
        /// </value>
        public string PatientFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the patient.
        /// </summary>
        /// <value>
        /// The last name of the patient.
        /// </value>
        public string PatientLastName { get; set; }

        /// <summary>
        /// Gets or sets the name of the patient middle.
        /// </summary>
        /// <value>
        /// The name of the patient middle.
        /// </value>
        public string PatientMiddleName { get; set; }

        /// <summary>
        /// Gets or sets the patient responsibility.
        /// </summary>
        /// <value>
        /// The patient responsibility.
        /// </value>
        public double? PatientResponsibility { get; set; }

        /// <summary>
        /// Gets or sets the name of the payer.
        /// </summary>
        /// <value>
        /// The name of the payer.
        /// </value>
        public string PayerName { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the remit check date.
        /// </summary>
        /// <value>
        /// The remit check date.
        /// </value>
        public DateTime? RemitCheckDate { get; set; }

        /// <summary>
        /// Gets or sets the remit payment.
        /// </summary>
        /// <value>
        /// The remit payment.
        /// </value>
        public double? RemitPayment { get; set; }

        /// <summary>
        /// Gets or sets the statement from.
        /// </summary>
        /// <value>
        /// The statement from.
        /// </value>
        public DateTime? StatementFrom { set; get; }

        /// <summary>
        /// Gets or sets the statement thru.
        /// </summary>
        /// <value>
        /// The statement thru.
        /// </value>
        public DateTime? StatementThru { set; get; }

        /// <summary>
        /// Gets or sets the claim total.
        /// </summary>
        /// <value>
        /// The claim total.
        /// </value>
        public double? ClaimTotal { get; set; }

        /// <summary>
        /// Gets or sets the icn.
        /// </summary>
        /// <value>
        /// The icn.
        /// </value>
        public string Icn { get; set; }

        /// <summary>
        /// Gets or sets the los.
        /// </summary>
        /// <value>
        /// The los.
        /// </value>
        public int Los { get; set; }


        /// <summary>
        /// Gets or sets the Age.
        /// </summary>
        /// <value>
        /// The Age.
        /// </value>
        public byte Age { get; set; }
    }
}
