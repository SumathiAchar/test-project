using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Helpers.DataAccess;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ServiceLineCodeRepository : IServiceLineCodeRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLineCodeRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ServiceLineCodeRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Add & Edit the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        public long AddEditServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            //Checks if input request is not null
            if (contractServiceLine != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditServiceLinesInfo");
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceLineId", DbType.Int64, contractServiceLine.ContractServiceLineId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@IncludedCode", DbType.String, contractServiceLine.IncludedCode.ToTrim());
                _databaseObj.AddInParameter(_databaseCommandObj, "@ExcludedCode", DbType.String, contractServiceLine.ExcludedCode.ToTrim());
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractId", DbType.Int64, contractServiceLine.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64, contractServiceLine.ServiceLineId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeId", DbType.Int64, contractServiceLine.ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, contractServiceLine.UserName);

                // Retrieve the results of the Stored Procedure in Datatable
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

                
            }
          
            return 0;
        }

        /// <summary>
        /// Get the service line code details.
        /// </summary>
        /// <param name="contractServiceLine">The contract service line list.</param>
        public ContractServiceLine GetServiceLineCodeDetails(ContractServiceLine contractServiceLine)
        {
            if (contractServiceLine != null)
            {

                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLinesandPaymentTypes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@PaymentTypeID ", DbType.Int64, 0);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractID", DbType.Int64,
                    contractServiceLine.ContractId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ContractServiceTypeID", DbType.Int64,
                    contractServiceLine.ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeId", DbType.Int64,
                    contractServiceLine.ServiceLineId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String,
                    contractServiceLine.UserName);
                DataSet contractServiceLineDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (contractServiceLineDataSet.IsTableDataPopulated(0))
                {
                    //populating ContractBasicInfo data
                    ContractServiceLine serviceLine = new ContractServiceLine
                    {
                        IncludedCode = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[0]["IncludedCode"]),
                        ExcludedCode = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[0]["ExcludedCode"]),
                        ContractServiceLineId = Convert.ToInt64(contractServiceLineDataSet.Tables[0].Rows[0]["ContractServiceLineID"])
                    };
                    return serviceLine;
                }

            }
            //returns response to Business layer
            return contractServiceLine;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommandObj.Dispose();
        }

        /// <summary>
        /// Gets all service line code details by contract id.
        /// </summary>
        /// <param name="contractId">The contract id.</param>
        /// <returns></returns>
        public List<ContractServiceLine> GetAllServiceLineCodeDetailsByContractId(long contractId)
        {
            List<ContractServiceLine> serviceLineList = new List<ContractServiceLine>();
            if (contractId != 0)
            {

                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAllServiceLinesByContractId");
                DataSet contractServiceLineDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (contractServiceLineDataSet.IsTableDataPopulated(0))
                {
                    serviceLineList = new List<ContractServiceLine>();
                    for (int i = 0; i < contractServiceLineDataSet.Tables[0].Rows.Count; i++)
                    {
                        ContractServiceLine contractServiceLine = new ContractServiceLine
                        {
                            ContractServiceLineId =
                                long.Parse(contractServiceLineDataSet.Tables[0].Rows[i]["ContractServiceLineID"].ToString()),
                            InsertDate =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["InsertDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(contractServiceLineDataSet.Tables[0].Rows[0]["InsertDate"]),
                            UpdateDate =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["UpdateDate"]
                                    ? (DateTime?)null
                                    : Convert.ToDateTime(contractServiceLineDataSet.Tables[0].Rows[0]["UpdateDate"]),
                            // UserId = DBNull.Value == dataset.Tables[0].Rows[0]["UserID"] ? (int?)null : Convert.ToInt32(dataset.Tables[0].Rows[0]["UserID"]),
                            ServiceLineId =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["ServiceLineID"]
                                    ? (long?)null
                                    : Convert.ToInt64(contractServiceLineDataSet.Tables[0].Rows[0]["ServiceLineID"]),
                            ContractServiceTypeId =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["ContractServiceTypeID"]
                                    ? (long?)null
                                    : Convert.ToInt64(contractServiceLineDataSet.Tables[0].Rows[0]["ContractServiceTypeID"]),
                            ContractId =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["ContractID"]
                                    ? (long?)null
                                    : Convert.ToInt64(contractServiceLineDataSet.Tables[0].Rows[0]["ContractID"]),
                            FacilityId =
                                Convert.ToInt32(contractServiceLineDataSet.Tables[0].Rows[0]["FacilityID"]),
                            IncludedCode = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[i]["IncludedCode"]),
                            Description = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[i]["Description"]),
                            ExcludedCode = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[i]["ExcludedCode"]),
                            Status =
                                DBNull.Value == contractServiceLineDataSet.Tables[0].Rows[0]["Status"]
                                    ? (int?)null
                                    : int.Parse(contractServiceLineDataSet.Tables[0].Rows[0]["Status"].ToString())
                        };
                        serviceLineList.Add(contractServiceLine);
                    }
                }
            }
            return serviceLineList;
        }

        /// <summary>
        /// Gets all serviceLine Codes.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public List<ServiceLineCode> GetAllServiceLineCodes(long serviceLineTypeId, int pageSize, int pageIndex)
        {
            List<ServiceLineCode> serviceLineCodeslist = new List<ServiceLineCode>();
            if (serviceLineTypeId != 0)
            {

                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetServiceLineCodes");
                _databaseObj.AddInParameter(_databaseCommandObj, "@ServiceLineTypeID", DbType.Int64,
                    serviceLineTypeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PageSize", DbType.Int32,
                  pageSize);
                _databaseObj.AddInParameter(_databaseCommandObj, "@PageIndex", DbType.Int32,
                  pageIndex);
                DataSet contractServiceLineDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (contractServiceLineDataSet.IsTableDataPopulated(0))
                {
                    if (contractServiceLineDataSet.Tables[0].Rows != null && contractServiceLineDataSet.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < contractServiceLineDataSet.Tables[0].Rows.Count; i++)
                        {
                            ServiceLineCode serviceLineCodes = new ServiceLineCode
                            {
                                CodeString = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[i]["Code"]),
                                Description = Convert.ToString(contractServiceLineDataSet.Tables[0].Rows[i]["Description"])
                            };
                            serviceLineCodeslist.Add(serviceLineCodes);
                        }
                    }
                    if (contractServiceLineDataSet.IsTableDataPopulated(1))
                    {
                        serviceLineCodeslist[0].TotalRecs = Convert.ToInt32(contractServiceLineDataSet.Tables[1].Rows[0]["TotalRecords"]);
                    }
                }
            }
            return serviceLineCodeslist;
        }
    }
}
