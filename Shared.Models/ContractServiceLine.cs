/************************************************************************************************************/
/**  Author         : G mohanty
/**  Created        :19/Aug/2013
/**  Summary        : Handles Contract Service Line
/**  User Story Id  :
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractServiceLine : BaseModel
    {

        /// <summary>
        /// Gets or sets the contract service line id.
        /// </summary>
        /// <value>
        /// The contract service line claim field id.
        /// </value>
        public long ContractServiceLineId { get; set; }

        /// <summary>
        /// Gets or sets the service line unique identifier.
        /// </summary>
        /// <value>
        /// The service line unique identifier.
        /// </value>
        public long? ServiceLineId { get; set; }
        /// <summary>
        /// Gets or sets the ServiceLineId..
        /// </summary>
        /// <value>
        /// The ServiceLineId..
        /// </value>
        public long? ContractServiceTypeId { get; set; }
        /// <summary>
        /// Gets or sets the ContractServiceTypeId..
        /// </summary>
        /// <value>
        /// The ContractServiceTypeId..
        /// </value>
        public long? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the FacilityId..
        /// </summary>
        /// <value>
        /// The FacilityId..
        /// </value>
        public string IncludedCode { get; set; }
        /// <summary>
        /// Gets or sets the FacilityId..
        /// </summary>
        /// <value>
        /// The FacilityId..
        /// </value>
        //public List<string> IncludedCodeList { get; set; }
        /// <summary>
        /// Gets or sets the Code..
        /// </summary>
        /// <value>
        /// The Code..
        /// </value>
        public string Description { get; set; }
        /// <summary>
        /// Gets or sets the Description..
        /// </summary>
        /// <value>
        /// The Description..
        /// </value>
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the  Status..
        /// </summary>
        /// <value>
        /// The  Status.
        /// </value>
        public string ExcludedCode { get; set; }
    
        //todo: need to add one field for is modified(isModified)

        /// <summary>
        /// Gets or sets the size of the page.
        /// </summary>
        /// <value>
        /// The size of the page.
        /// </value>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>
        /// The index of the page.
        /// </value>
        public int PageIndex { get; set; }
        /// <summary>
        /// Gets or sets the total records.
        /// </summary>
        /// <value>
        /// The total records.
        /// </value>
        public long? TotalRecords { get; set; }
    }
}
