using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class MedicareLabFeeScheduleRepository :BaseRepository, IMedicareLabFeeScheduleRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareLabFeeScheduleRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MedicareLabFeeScheduleRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the name of the Medicare Lab Fee Schedule Table.
        /// </summary>
        /// <returns></returns>
        public List<MedicareLabFeeSchedule> GetMedicareLabFeeScheduleTableNames(MedicareLabFeeSchedule mCareFeeSchedule)
        {
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetMedicareLabFeeScheduleTableNames");
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserText", DbType.String, mCareFeeSchedule.UserText);
            DataSet medicareLabFeeDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (medicareLabFeeDataSet.IsTableDataPopulated(0))
            {
                List<MedicareLabFeeSchedule> mCareLabFeeSchedules = (from DataRow row in medicareLabFeeDataSet.Tables[0].Rows
                    select new MedicareLabFeeSchedule
                    {
                        Date = GetValue<int>(row["Date"].ToString(), typeof(int)),
                        Name = GetStringValue(row["TableName"])
                    }).ToList();
                return mCareLabFeeSchedules;
            }
            return null;
        }

        /// <summary>
        /// Gets the Medicare Lab Fee Schedule table data.
        /// </summary>
        /// <param name="mCareLabFeeSchedule">The Medicare Lab Fee Schedule table.</param>
        /// <returns></returns>
        public MedicareLabFeeScheduleResult GetMedicareLabFeeSchedule(MedicareLabFeeSchedule mCareLabFeeSchedule)
        {
            if (mCareLabFeeSchedule != null)
            {
                //holds the response
                string finalStrXml = string.Empty;
                //Checks for Payers, if payers exists stores it in DB
                if (mCareLabFeeSchedule.PageSetting != null &&
                    mCareLabFeeSchedule.PageSetting.SearchCriteriaList != null &&
                    mCareLabFeeSchedule.PageSetting.SearchCriteriaList.Any())
                {
                    //Serializes list of payers to store it in DB
                    XmlSerializer serializer =
                        new XmlSerializer(mCareLabFeeSchedule.PageSetting.SearchCriteriaList.GetType());
                    StringWriter stringWriter = new StringWriter();
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                    XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                    XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                    serializer.Serialize(xmlWriter, mCareLabFeeSchedule.PageSetting.SearchCriteriaList, emptyNs);
                    //holds the final payer response
                    finalStrXml = stringWriter.ToString();
                }


                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetMedicareLabFeeSchedule");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@Date", DbType.Int32, mCareLabFeeSchedule.Date);
                // ReSharper disable once PossibleNullReferenceException
                _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.Int32, mCareLabFeeSchedule.PageSetting.Take);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.Int32, mCareLabFeeSchedule.PageSetting.Skip);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortField", DbType.String, mCareLabFeeSchedule.PageSetting.SortField);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SortDirection", DbType.String, mCareLabFeeSchedule.PageSetting.SortDirection);
                _databaseObj.AddInParameter(_databaseCommandObj, "@XmlSearchCriteria", DbType.Xml, finalStrXml);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, mCareLabFeeSchedule.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.Int32, mCareLabFeeSchedule.FacilityId);

                // Retrieve the results of the Stored Procedure in Dataset
                DataSet medicareLabFeeDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (medicareLabFeeDataSet.IsTableDataPopulated() && medicareLabFeeDataSet.Tables.Count > 1)
                {
                    MedicareLabFeeScheduleResult medicareLabFeeScheduleResult = new MedicareLabFeeScheduleResult
                    {
                        Total = GetValue<int>(medicareLabFeeDataSet.Tables[0].Rows[0][0], typeof(int)),
                        MedicareLabFeeScheduleList = new List<MedicareLabFeeSchedule>()
                    };

                    // Bind Medicare Lab Fee Schedule Data
                    medicareLabFeeScheduleResult.MedicareLabFeeScheduleList =
                            (from DataRow row in medicareLabFeeDataSet.Tables[1].Rows
                             select new MedicareLabFeeSchedule
                             {
                                 Hcpcs = GetStringValue(row["HCPCS"]),
                                 Carrier = GetStringValue(row["Carrier"]),
                                 Amount = GetValue<double>(row["Amount"], typeof(double))
                             }).ToList();
                        return medicareLabFeeScheduleResult;
                    }
                }
            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommandObj.Dispose();
            _databaseObj = null;
        }
    }
}
