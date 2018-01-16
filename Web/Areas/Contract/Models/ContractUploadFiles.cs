using System;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class ContractUploadFiles:BaseViewModel
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        /// <value>
        /// The Id.
        /// </value>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the attached documents.
        /// </summary>
        /// <value>
        /// The attached documents.
        /// </value>
        public string AttachedDocuments { get; set; }
        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public string FileName { get; set; }
        /// <summary>
        /// Gets or sets the contract Id.
        /// </summary>
        /// <value>
        /// The contract Id.
        /// </value>
        public Int64? ContractId { get; set; }
        /// <summary>
        /// Gets or sets the content of the contract.
        /// </summary>
        /// <value>
        /// The content of the contract.
        /// </value>
        public byte[] ContractContent { get; set; }
        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public int? Status { get; set; }
        /// <summary>
        /// Gets or sets the document identifier.
        /// </summary>
        /// <value>
        /// The document identifier.
        /// </value>
        // ReSharper disable once UnusedMember.Global
        public Guid? DocumentId { get; set; }
       
    }
}