/************************************************************************************************************/
/**  Author         : Mahesh Achina
/**  Created        : 08/Aug/2013
/**  Summary        : Handles Contract Payer Info
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractPayerInfo:BaseModel
    {
        /// <summary>
        /// Set or Get Contract Payer Info Id.
        /// </summary>
        ///  <value>
        /// The Contract Payer Info Id.
        /// </value>
        public long ContractPayerInfoId { get; set; }

        /// <summary>
        /// Set or Get Contract Id
        /// </summary>
        /// <value>
        /// The Contract Id.
        /// </value>
        public long? ContractId { get; set; }

        /// <summary>
        /// Set or Get Payer Id
        /// </summary>
        /// <value>
        /// The Payer Id.
        /// </value>
        public long? PayerId { get; set; }
        
       
        /// <summary>
        /// Set or Get ContractInfo PayerName.
        /// </summary>
        /// <value>
        /// The ContractInfo PayerName.
        /// </value>
        public string ContractInfoPayerName { get; set; }

        /// <summary>
        /// Set or Get MailAddress1.
        /// </summary>
        /// <value>
        /// The MailAddress1.
        /// </value>
        public string MailAddress1 { get; set; }

        /// <summary>
        /// Set or Get MailAddress2.
        /// </summary>
        /// <value>
        /// The MailAddress2.
        /// </value>
        public string MailAddress2 { get; set; }

        /// <summary>
        /// Set or Get City.
        /// </summary>
        /// <value>
        /// The City.
        /// </value>
        public string City { get; set; }

        /// <summary>
        /// Set or Get State.
        /// </summary>
        /// <value>
        /// The State.
        /// </value>
        public string State { get; set; }

        /// <summary>
        /// Set or Get Zip.
        /// </summary>
        /// <value>
        /// The Zip.
        /// </value>
        public string Zip { get; set; }

        /// <summary>
        /// Set or Get Phone1.
        /// </summary>
        /// <value>
        /// The Phone1.
        /// </value>
        public string Phone1 { get; set; }

        /// <summary>
        /// Set or Get Phone2.
        /// </summary>
        /// <value>
        /// The Phone2.
        /// </value>
        public string Phone2 { get; set; }

        /// <summary>
        /// Set or Get Fax
        /// </summary>
        /// <value>
        /// The Fax.
        /// </value>
        public string Fax { get; set; }

        /// <summary>
        /// Set or Get Email
        /// </summary>
        /// <value>
        /// The Email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Set or Get Website
        /// </summary>
        /// <value>
        /// The Website.
        /// </value>
        public string Website { get; set; }

        /// <summary>
        /// Set or Get Tax Id
        /// </summary>
        /// <value>
        /// The Tax Id.
        /// </value>
        public string TaxId { get; set; }

        /// <summary>
        /// Set or Get NPI.
        /// </summary>
        /// <value>
        /// The NPI.
        /// </value>
        public string Npi { get; set; }

        /// <summary>
        /// Set or Get Memo.
        /// </summary>
        /// <value>
        /// The Memo.
        /// </value>
        public string Memo { get; set; }

        /// <summary>
        /// Set or Get Provider Id.
        /// </summary>
        /// <value>
        /// The Provider Id.
        /// </value>
        public string ProviderId { get; set; }

        /// <summary>
        /// Set or Get Plan Id.
        /// </summary>
        /// <value>
        /// The Plan Id.
        /// </value>
        public string PlanId { get; set; }

        /// <summary>
        /// Set or Get Operator.
        /// </summary>
        /// <value>
        /// The Operator.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string Operator { get; set; }

    }
}
