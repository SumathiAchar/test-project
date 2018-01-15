using System;

namespace SSI.ContractManagement.Shared.Models
{

    /// <summary>
    /// Class PaymentResult.
    /// </summary>
    public class PaymentResult 
    {

        /// <summary>
        /// Gets or sets the claim service line identifier.
        /// </summary>
        /// <value>The claim service line identifier.</value>
        public int? Line { set; get; }
        /// <summary>
        /// Gets or sets the claim identifier.
        /// </summary>
        /// <value>The claim identifier.</value>
        public long ClaimId { set; get; }
        /// <summary>
        /// Gets or sets the contract identifier.
        /// </summary>
        /// <value>The contract identifier.</value>
        public long? ContractId { set; get; }
        /// <summary>
        /// Gets or sets the service type identifier.
        /// </summary>
        /// <value>The service type identifier.</value>
        public long? ServiceTypeId { set; get; }
        /// <summary>
        /// Gets or sets the claim status.
        /// </summary>
        /// <value>The claim status.</value>
        public int ClaimStatus { set; get; }
        /// <summary>
        /// Gets or sets the adjudicated value.
        /// </summary>
        /// <value>The adjudicated value.</value>
        public double? AdjudicatedValue { set; get; }
        /// <summary>
        /// Gets or sets the claim total charges.
        /// </summary>
        /// <value>The claim total charges.</value>
        public double? ClaimTotalCharges { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is claim charge data.
        /// </summary>
        /// <value><c>true</c> if this instance is claim charge data; otherwise, <c>false</c>.</value>
        public bool IsClaimChargeData { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is initial entry.
        /// </summary>
        /// <value><c>true</c> if this instance is initial entry; otherwise, <c>false</c>.</value>
        public bool IsInitialEntry { get; set; }
        
        /// <summary>
        /// Gets or sets the payment type detail identifier.
        /// </summary>
        /// <value>
        /// The payment type detail identifier.
        /// </value>
        public long? PaymentTypeDetailId { set; get; }

        /// <summary>
        /// Gets or sets the payment type identifier.
        /// </summary>
        /// <value>
        /// The payment type identifier.
        /// </value>
        public int? PaymentTypeId { set; get; }

        /// <summary>
        /// Calculated allowed amount(CAA)/Adjudicated value before adjudication hits to stop loss payment type. 
        /// </summary>
        public double? IntermediateAdjudicatedValue { get; set; }

        /// <summary>
        /// Evaluated double value from the threshold expression user entered from stop loss UI
        /// </summary>
        public double? ExpressionResult { get; set; }

        public string Expression { get; set; }
        public string ExpandedExpression { get; set; }

        //TO-DO - this below to be refactored
        public double? CustomExpressionResult { get; set; }
        public string CustomExpression { get; set; }
        public string CustomExpandedExpression { get; set; }

        public bool IsFormulaError { get; set; }

        public string MicrodynEditErrorCodes { get; set; }
        public int? MicrodynPricerErrorCodes { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>PaymentResult.</returns>
        public PaymentResult Clone()
        {
            return (PaymentResult)MemberwiseClone();
        }

        /// <summary>
        /// Gets or sets the microdyn edit return remarks.
        /// </summary>
        /// <value>
        /// The microdyn edit return remarks.
        /// </value>
        public string MicrodynEditReturnRemarks { get; set; }

        /// <summary>
        /// Gets or sets the medicare sequester amount.
        /// </summary>
        /// <value>
        /// The medicare sequester amount.
        /// </value>
        public double? MedicareSequesterAmount { get; set; }

        /// <summary>
        /// Gets or sets the multiplier expression result.
        /// </summary>
        /// <value>
        /// The multiplier expression result.
        /// </value>
        public double? MultiplierExpressionResult { get; set; }
      
        /// <summary>
        /// Gets or sets a value indicating whether this instance is multiplier formula error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is multiplier formula error; otherwise, <c>false</c>.
        /// </value>
        public bool IsMultiplierFormulaError { get; set; }

        /// <summary>
        /// Gets or sets the multiplier.
        /// </summary>
        /// <value>
        /// The multiplier.
        /// </value>
        public int Multiplier { get; set; }

        /// <summary>
        /// Gets or sets the multiplier expanded expression.
        /// </summary>
        /// <value>
        /// The multiplier expanded expression.
        /// </value>
        public string MultiplierExpandedExpression { get; set; }
       

        /// <summary>
        /// Gets or sets the limit expression result.
        /// </summary>
        /// <value>
        /// The limit expression result.
        /// </value>
        public double? LimitExpressionResult { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is others multiplier error.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is others multiplier error; otherwise, <c>false</c>.
        /// </value>
        public bool IsLimitError { get; set; }

       /// <summary>
        /// Gets or sets the limit expanded expression.
        /// </summary>
        /// <value>
        /// The limit expanded expression.
        /// </value>
        public string LimitExpandedExpression { get; set; }
      

        /// <summary>
        /// Gets or sets the service line code.
        /// </summary>
        /// <value>
        /// The service line code.
        /// </value>
        public string ServiceLineCode { get; set; }

        /// <summary>
        /// Gets or sets the service line date.
        /// </summary>
        /// <value>
        /// The service line date.
        /// </value>
        public DateTime ServiceLineDate { get; set; }
      }

    
}
