using System;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractInfoViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contract payer information Id.
        /// </summary>
        /// <value>
        /// The contract payer information Id.
        /// </value>
        public Int64? ContractPayerInfoId { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public Int64? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the payer Id.
        /// </summary>
        /// <value>
        /// The payer Id.
        /// </value>
        public Int64? PayerId { get; set; } //TODO change it to ContractPayerId
        /// <summary>
        /// Gets or sets the name of the contract information payer.
        /// </summary>
        /// <value>
        /// The name of the contract information payer.
        /// </value>
        public string ContractInfoPayerName { get; set; }
        /// <summary>
        /// Gets or sets the mail address1.
        /// </summary>
        /// <value>
        /// The mail address1.
        /// </value>
        public string MailAddress1 { get; set; }
        /// <summary>
        /// Gets or sets the mail address2.
        /// </summary>
        /// <value>
        /// The mail address2.
        /// </value>
        public string MailAddress2 { get; set; }
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        public string City { get; set; }
        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public string State { get; set; }
        /// <summary>
        /// Gets or sets the zip.
        /// </summary>
        /// <value>
        /// The zip.
        /// </value>
        public string Zip { get; set; }
        /// <summary>
        /// Gets or sets the phone1.
        /// </summary>
        /// <value>
        /// The phone1.
        /// </value>
        public string Phone1 { get; set; }
        /// <summary>
        /// Gets or sets the phone2.
        /// </summary>
        /// <value>
        /// The phone2.
        /// </value>
        public string Phone2 { get; set; }
        /// <summary>
        /// Gets or sets the fax.
        /// </summary>
        /// <value>
        /// The fax.
        /// </value>
        public string Fax { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
        /// <summary>
        /// Gets or sets the website.
        /// </summary>
        /// <value>
        /// The website.
        /// </value>
        public string Website { get; set; }
        /// <summary>
        /// Gets or sets the tax Id.
        /// </summary>
        /// <value>
        /// The tax Id.
        /// </value>
        public string TaxId { get; set; }
        /// <summary>
        /// Gets or sets the npi.
        /// </summary>
        /// <value>
        /// The npi.
        /// </value>
        public string Npi { get; set; }
        /// <summary>
        /// Gets or sets the memo.
        /// </summary>
        /// <value>
        /// The memo.
        /// </value>
        public string Memo { get; set; }
        /// <summary>
        /// Gets or sets the provider Id.
        /// </summary>
        /// <value>
        /// The provider Id.
        /// </value>
        public string ProviderId { get; set; }
        /// <summary>
        /// Gets or sets the plan Id
        /// </summary>
        /// <value>
        /// The plan Id.
        /// </value>
        public string PlanId { get; set; }
    }
}