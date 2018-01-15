using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSI.ContractManagement.Shared.Models
{
    public class ClaimFieldDoc : BaseModel
    {
        /// <summary>
        /// Gets or sets the ClaimFieldDocId.
        /// </summary>
        /// <value>
        ///  ClaimFieldDocId.
        /// </value>
        public long ClaimFieldDocId { set; get; }

        /// <summary>
        /// Gets or sets the FileName.
        /// </summary>
        /// <value>
        ///  FileName.
        /// </value>
        public string FileName { set; get; }
        
        /// <summary>
        /// Gets or sets the TableName.
        /// </summary>
        /// <value>
        ///  TableName.
        /// </value>
        public string TableName { set; get; }

        /// <summary>
        /// Gets or sets the ColumnHeaderFirst.
        /// </summary>
        /// <value>
        ///  ColumnHeaderFirst.
        /// </value>
        public string ColumnHeaderFirst { set; get; }

        /// <summary>
        /// Gets or sets the ColumnHeaderSecond.
        /// </summary>
        /// <value>
        ///  ColumnHeaderSecond.
        /// </value>
        public string ColumnHeaderSecond { set; get; }


        /// <summary>
        /// Gets or sets the ClaimFieldID.
        /// </summary>
        /// <value>
        ///  ClaimFieldID.
        /// </value>
        public long? ClaimFieldId { set; get; }

        /// <summary>
        /// Gets or sets the ContractId.
        /// </summary>
        /// <value>
        ///  ContractId.
        /// </value>
        public long? ContractId { set; get; }

        /// <summary>
        /// Gets or sets the ClaimFieldValues.
        /// </summary>
        /// <value>
        ///  ClaimFieldValues.
        /// </value>
        public List<ClaimFieldValue> ClaimFieldValues { get; set; }

        public long NodeId { get; set; }
        
        /// <summary>
        /// Gets or sets the page setting.
        /// </summary>
        /// <value>
        /// The page setting.
        /// </value>
        public PageSetting PageSetting { get; set; }

        public string XmlSerialize()
        {
            XmlSerializer serializer = new XmlSerializer(PageSetting.SearchCriteriaList.GetType());
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(xmlWriter, PageSetting.SearchCriteriaList, emptyNs);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Gets or sets the session time out.
        /// </summary>
        /// <value>
        /// The session time out.
        /// </value>
        public int SessionTimeOut { get; set; }
    }
}
