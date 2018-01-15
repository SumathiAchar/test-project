/************************************************************************************************************/
/**  Author         : Manjunath
/**  Created        : 19-Sep-2013
/**  Summary        : Handles insertion of contractLogs infromation
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
namespace SSI.ContractManagement.Shared.Models
{
    public class ContractLog : BaseModel
    {
        /// <summary>
        /// The model name
        /// </summary>
        // It is used in ClaimFieldDocRepository 
        // ReSharper disable once NotAccessedField.Global
        public string ModelName;

        /// <summary>
        /// Gets or sets the ContractId.
        /// </summary>
        /// <value> 
        /// ContractId.
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the ClaimId.
        /// </summary>
        /// <value> 
        /// ClaimId.
        /// </value>
        public long? ClaimId { get; set; }
        /// <summary>
        /// Gets or sets the LogContent.
        /// </summary>
        /// <value> 
        /// LogContent.
        /// </value>
        public string LogContent { get; set; }
        
        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public string StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the contract.
        /// </summary>
        /// <value>
        /// The name of the contract.
        /// </value>
        public string ContractName { get; set; }

        /// <summary>
        /// Gets or sets the name of the service type.
        /// </summary>
        /// <value>
        /// The name of the service type.
        /// </value>
        public string ServiceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the service line.
        /// </summary>
        /// <value>
        /// The service line.
        /// </value>
        public long? ServiceLine { get; set; }

        /// <summary>
        /// Gets or sets the type of the payment.
        /// </summary>
        /// <value>
        /// The type of the payment.
        /// </value>
        public string PaymentType { get; set; }

        
    }

}
