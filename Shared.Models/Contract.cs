/************************************************************************************************************/
/**  Author         :Mahesh Achina
/**  Created        :08/Aug/2013
/**  Summary        :Handles Contracts
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
    public class Contract : BaseModel
    {
        /// <summary>
        /// Set or Get Contract Id
        /// </summary>
        ///  <value>
        /// The Contract Id.
        /// </value>
        public long ContractId { get; set; }

        /// <summary>
        /// Set or Get Contract Name.
        /// </summary>
        /// <value>
        /// The Contract Name.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Set or Get Start Date.
        /// </summary>
        /// <value>
        /// The Start Date.
        /// </value>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Set or Get End Date.
        /// </summary>
        /// <value>
        /// The End Date.
        /// </value>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Set or Get Status.
        /// </summary>
        /// <value>
        /// The Status.
        /// </value>
        public bool Status { get; set; }  //TODO:have to change it to int

        /// <summary>
        /// Set or Get Node Id.
        /// </summary>
        /// <value>
        /// The Node Id.
        /// </value>
        public long? NodeId { get; set; }

        /// <summary>
        /// Set or Get Parent Id.
        /// </summary>
        /// <value>
        /// The Parent Id.
        /// </value>
        public long? ParentId { get; set; }

        /// <summary>
        /// Set or Get Is Modified.
        /// </summary>
        /// <value>
        /// The Is Modified.
        /// </value>
        public int? IsModified { get; set; }

        /// <summary>
        /// Set or Get Is Contract Service Type Found.
        /// </summary>
        /// <value>
        /// The Is Contract Service Type Found.
        /// </value>
        public bool IsContractServiceTypeFound { get; set; }

        /// <summary>
        /// Set or Get Operator.
        /// </summary>
        /// <value>
        /// The Operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Operator { get; set; }

        /// <summary>
        /// Set or Get Is Professional.
        /// </summary>
        /// <value>
        /// The Is Professional.
        /// </value>
        public bool IsProfessional { get; set; }  

        /// <summary>
        /// Set or Get Is Institutional.
        /// </summary>
        /// <value>
        /// The Is Institutional.
        /// </value>
        public bool IsInstitutional { get; set; }  

        /// <summary>
        /// Set or Get Is Claim Start Date.
        /// </summary>
        /// <value>
        /// The Is Claim Start Date.
        /// </value>
        public bool IsClaimStartDate { get; set; } 

        /// <summary>
        /// Set or Get LesserOfPercentage.
        /// </summary>
        /// <value>
        /// isLesserOf.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public double? LesserOfPercentage { get; set; }

        /// <summary>
        /// Set or Get Payers.
        /// </summary>
        /// <value>
        /// The Payers.
        /// </value>
        public ICollection<Payer> Payers { get; set; }

        /// <summary>
        /// Gets or sets the contract service types.
        /// </summary>
        /// <value>The contract service types.</value>
        public ICollection<ContractServiceType> ContractServiceTypes { get; set; }

        /// <summary>
        /// Gets or sets the conditions.
        /// </summary>
        /// <value>
        /// The conditions.
        /// </value>
        public List<ICondition> Conditions { get; set; }

        /// <summary>
        /// Gets or sets the payment types.
        /// </summary>
        /// <value>The payment types.</value>
        public List<PaymentTypeBase> PaymentTypes { get; set; }

        public string PayersList { get; set; }
        public string ModelName { get; set; }
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
        public string PayerCode { get; set; }
        public int? CustomField { get; set; }

        // ReSharper disable once UnusedMember.Global
        public Contract Clone()
        {
            return (Contract)MemberwiseClone();
        }

        /// <summary>
        /// Gets or sets the model identifier.
        /// </summary>
        /// <value>
        /// The model identifier.
        /// </value>
        public long ModelId { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public string Values { get; set; }

        /// <summary>
        /// Gets or sets the type of the claim.
        /// </summary>
        /// <value>
        /// The type of the claim.
        /// </value>
        public string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the contract unique identifier.
        /// </summary>
        /// <value>
        /// The contract unique identifier.
        /// </value>
        public string ContractGuid { get; set; }
    }
}
