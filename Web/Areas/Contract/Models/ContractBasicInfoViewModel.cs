using System;
using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractBasicInfoViewModel:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public int ContractId { get; set; }
        /// <summary>
        /// Gets or sets the effective start date.
        /// </summary>
        /// <value>
        /// The effective start date.
        /// </value>
        public DateTime? EffectiveStartDate { get; set; }
        /// <summary>
        /// Gets or sets the effective end date.
        /// </summary>
        /// <value>
        /// The effective end date.
        /// </value>
        public DateTime? EffectiveEndDate { get; set; }
        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }
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
        /// Gets or sets a value indicating whether [status].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [status]; otherwise, <c>false</c>.
        /// </value>
        public bool Status { get; set; }  //TODO:have to change it to int
        /// <summary>
        /// Gets or sets a value indicating whether [is professional].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is professional]; otherwise, <c>false</c>.
        /// </value>
        public bool IsProfessional { get; set; }  //TODO:have to change it to int
        /// <summary>
        /// Gets or sets a value indicating whether [is institutional].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is institutional]; otherwise, <c>false</c>.
        /// </value>
        public bool IsInstitutional { get; set; }  //TODO:have to change it to int
        /// <summary>
        /// Gets or sets a value indicating whether [is claim start date].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is claim start date]; otherwise, <c>false</c>.
        /// </value>
        public bool IsClaimStartDate { get; set; }  //TODO:have to change it to int
        /// <summary>
        /// Gets or sets the node unique identifier.
        /// </summary>
        /// <value>
        /// The node unique identifier.
        /// </value>
        public long? NodeId { get; set; }
        /// <summary>
        /// Gets or sets the parent unique identifier.
        /// </summary>
        /// <value>
        /// The parent unique identifier.
        /// </value>
        public long? ParentId { get; set; }
        /// <summary>
        /// Gets or sets the is modified.
        /// </summary>
        /// <value>
        /// The is modified.
        /// </value>
        public int? IsModified { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [is contract service type found].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is contract service type found]; otherwise, <c>false</c>.
        /// </value>
        public bool IsContractServiceTypeFound { get; set; }
        /// <summary>
        /// Gets or sets the available payer list.
        /// </summary>
        /// <value>
        /// The available payer list.
        /// </value>
        public List<ContractPayerViewModel> AvailablePayerList { get; set; }
        /// <summary>
        /// Gets or sets the selected payer list.
        /// </summary>
        /// <value>
        /// The selected payer list.
        /// </value>
        public List<ContractPayerViewModel> SelectedPayerList { get; set; }

        // ReSharper disable once UnusedMember.Global
        public List<ContractServiceTypeViewModel> ContractServiceTypesList { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string PayersList { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string ContractEffectiveDate { get; set; }
        public string ModelName { get; set; }
        // ReSharper disable once UnusedMember.Global
        public bool IsContractSpecific { get; set; }
        // ReSharper disable once UnusedMember.Global
        public string PaymentTools { get; set; }
        public string ClaimTools { get; set; }
        public long ClaimCount { get; set; }
        public double? TotalClaimCharges { get; set; }
        public double? CalculatedAllowed { get; set; }
        public double? ActualPayment { get; set; }
        // ReSharper disable once UnusedMember.Global
        public double? Variance { get; set; }
        public int? ThresholdDaysToExpireAlters { get; set; }
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

        public double? PatientResponsibility { get; set; }
        public double? CalculatedAdjustment { get; set; }
        public double? ActualAdjustment { get; set; }

        // ReSharper disable once UnusedMember.Global
        public string PayerCode { get; set; }
        // ReSharper disable once UnusedMember.Global
        public int? CustomField { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }
    }
}