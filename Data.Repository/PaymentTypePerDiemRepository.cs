/************************************************************************************************************/
/**  Author         : Girija Mohanty
/**  Created        : 22-Aug-2013
/**  Summary        : Handles Add/Modify PaymentType PerDiem Details DataAccess Layer

/************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class PaymentTypePerDiemRepository : IPaymentTypePerDiemRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentTypePerDiemRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public PaymentTypePerDiemRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type per case.
        /// </summary>
        /// <param name="contractServiceTypeId">The contract service type identifier.</param>
        /// <param name="dtPerDiem">The dt per case table.</param>
        /// <returns></returns>
        public static PaymentTypePerDiem GetPaymentType(long contractServiceTypeId, DataTable dtPerDiem)
        {
            PaymentTypePerDiem paymentTypePerDiem = null;

            if (dtPerDiem != null && dtPerDiem.Rows.Count > 0)
            {
                paymentTypePerDiem = new PaymentTypePerDiem { PerDiemSelections = new List<PerDiemSelection>() };
                foreach (DataRow row in dtPerDiem.Rows)
                {
                    PerDiemSelection perDiemSelection = new PerDiemSelection();
                    if ((Convert.ToInt64(DBNull.Value == row["contractServiceTypeId"] ? (long?)null : Convert.ToInt64(row["contractServiceTypeId"])) == contractServiceTypeId))
                    {
                        perDiemSelection.Rate = DBNull.Value == row["Rate"] ? (double?)null : Convert.ToDouble(row["Rate"]);
                        perDiemSelection.DaysFrom = DBNull.Value == row["DaysFrom"]
                                ? (int?)null
                            : Convert.ToInt32(row["DaysFrom"]);

                        perDiemSelection.DaysTo = DBNull.Value == row["DaysTo"]
                                ? (int?)null
                            : Convert.ToInt32(row["DaysTo"]);
                        paymentTypePerDiem.PaymentTypeId = (byte)Enums.PaymentTypeCodes.PerDiem;
                        paymentTypePerDiem.PaymentTypeDetailId = DBNull.Value == row["PaymentTypeDetailID"]
                            ? 0 : Convert.ToInt64(row["PaymentTypeDetailID"]);
                        paymentTypePerDiem.ContractId = DBNull.Value == row["ContractId"]
                                  ? (long?)null
                            : Convert.ToInt64(row["ContractId"]);
                        paymentTypePerDiem.ServiceTypeId = DBNull.Value == row["ContractServiceTypeID"]
                           ? (long?)null
                            : Convert.ToInt64(row["ContractServiceTypeID"]);
                        paymentTypePerDiem.PerDiemSelections.Add(perDiemSelection);
                    }
                }
            }

            return paymentTypePerDiem;
        }

        /// <summary>
        /// Add Edit PaymentType Per Diem Details
        /// </summary>
        /// <param name="paymentTypePerDiemList">PaymentTypePerDiem List</param>
        /// <returns>Inserted Data Count</returns>
        public long AddEditPaymentTypePerDiemDetails(PaymentTypePerDiem paymentTypePerDiemList)
        {
            //Checks if input request is not null
            XmlSerializer serializer = new XmlSerializer(paymentTypePerDiemList.GetType());
            StringWriter stringWriter = new StringWriter();
            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
            XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            serializer.Serialize(xmlWriter, paymentTypePerDiemList, emptyNs);
            string finalStrXml = stringWriter.ToString();
            _cmd = _db.GetStoredProcCommand("AddEditPaymentTypePerDiem");
            _db.AddInParameter(_cmd, "@XmlPaymentTypePerDiemData", DbType.String,
                                        finalStrXml);
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePerDiemList.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerDiemList.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerDiemList.ServiceTypeId);
            _db.AddInParameter(_cmd, "@PaymentTypeDetailsId", DbType.Int64, paymentTypePerDiemList.PaymentTypeDetailId);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerDiemList.UserName);

            // Retrieve the results of the Stored Procedure in Datatable
            return long.Parse(_db.ExecuteScalar(_cmd).ToString());


            
        }

        /// <summary>
        /// Get PaymentType Per Diem Details
        /// </summary>
        /// <param name="paymentTypePerDiemList">PaymentTypePerDiem List</param>
        /// <returns>Inserted Data Count</returns>
        public PaymentTypePerDiem GetPaymentTypePerDiem(PaymentTypePerDiem paymentTypePerDiemList)
        {

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@PaymentTypeID ", DbType.Int64, paymentTypePerDiemList.PaymentTypeId);
            _db.AddInParameter(_cmd, "@ContractID", DbType.Int64, paymentTypePerDiemList.ContractId);
            _db.AddInParameter(_cmd, "@ContractServiceTypeID", DbType.Int64, paymentTypePerDiemList.ServiceTypeId);
            _db.AddInParameter(_cmd, "@ServiceLineTypeId", DbType.Int64, 0);
            _db.AddInParameter(_cmd, "@UserName", DbType.String, paymentTypePerDiemList.UserName);


            // Retrieve the results of the Stored Procedure in Data set
            DataSet paymentTypePerDiemDataSet = _db.ExecuteDataSet(_cmd);
            List<PerDiemSelection> perDiemSelections = new List<PerDiemSelection>();
            if (paymentTypePerDiemDataSet != null && paymentTypePerDiemDataSet.Tables.Count > 0)
            {
                //populating PerDiem data
                if (paymentTypePerDiemDataSet.Tables[0].Rows != null && paymentTypePerDiemDataSet.Tables[0] != null && paymentTypePerDiemDataSet.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < paymentTypePerDiemDataSet.Tables[0].Rows.Count; i++)
                    {
                        if (paymentTypePerDiemDataSet.Tables[0].Rows[i]["DaysFrom"] != null && paymentTypePerDiemDataSet.Tables[0].Rows[i]["DaysTo"] != null &&
                            paymentTypePerDiemDataSet.Tables[0].Rows[i]["Rate"] != null)
                        {
                            PerDiemSelection perDiemSelection = new PerDiemSelection
                            {
                                DaysFrom = Convert.ToInt32(
                                    paymentTypePerDiemDataSet.Tables[0].Rows[i]["DaysFrom"]),
                                DaysTo = Convert.ToInt32(
                                    paymentTypePerDiemDataSet.Tables[0].Rows[i]["DaysTo"]),
                                Rate = Convert.ToDouble(
                                    paymentTypePerDiemDataSet.Tables[0].Rows[i]["Rate"])
                            };

                            perDiemSelections.Add(perDiemSelection);
                        }
                    }
                    paymentTypePerDiemList.PerDiemSelections = perDiemSelections;
                    paymentTypePerDiemList.PaymentTypeDetailId = Convert.ToInt64(paymentTypePerDiemDataSet.Tables[0].Rows[0]["PaymentTypeDetailID"]);
                    return paymentTypePerDiemList;
                }
            }

            //returns response to Business layer
            return null;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _cmd.Dispose();
            _db = null;
        }
    }
}
