/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract Hierarchical structure
/**  User Story Id  : 	Contract tree structure Figure 9
/** Modification History ************************************************************************************
 *  Date Modified   : 21 Aug 2013
 *  Author          : Vishesh
 *  Summary         : CopyContractByNodeAndParentId,DeleteNodeAndContractByNodeId & GetContractHierarchy Method Updated/Created
/************************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractHierarchyRepository : BaseRepository, IContractHierarchyRepository
    {
        /// <summary>
        /// The _database obj variable
        /// </summary>
        private Database _databaseObj;

        /// <summary>
        /// The _database command variable
        /// </summary>
        private DbCommand _databaseCommand;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractHierarchyRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractHierarchyRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the contract hierarchy.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<ContractHierarchy> GetContractHierarchy(ContractHierarchy data)
        {
            //holds the response data
            List<ContractHierarchy> contractHierarchy = new List<ContractHierarchy>();

            if (data != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("GetContractHierarchy");
                _databaseObj.AddInParameter(_databaseCommand, "@NodeId", DbType.Int64, data.NodeId);
                _databaseObj.AddInParameter(_databaseCommand, "@ParentId", DbType.Int64, data.ParentId);
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, data.FacilityId);
                _databaseCommand.CommandTimeout = data.CommandTimeoutForContractHierarchy;
                // Retrieve the results of the Stored Procedure 
                DataSet dataSetObj = _databaseObj.ExecuteDataSet(_databaseCommand);

                for (int count = 0; count < dataSetObj.Tables.Count; count++)
                {
                    //TODO: As per reviewed I removed first logic, but now from database we are getting 
                    // first two tables which are need to be parse using below condition 
                    // The last table ( 3rd table ) need to be parse in different manner. 
                    // Please suggestion how to handle it?
                    if (count < 2)
                    {
                        //Map datatable to business objects
                        // sending 0 to insure that table at index is having valid data.
                        if (dataSetObj.IsTableDataPopulated(count))
                        {
                            DataTable dataTable = dataSetObj.Tables[count];
                            for (int indexCount = 0; indexCount < dataTable.Rows.Count; indexCount++)
                            {
                                if (count == 0)
                                {
                                    ContractHierarchy facilityDetails = new ContractHierarchy
                                    {
                                        NodeId = GetValue<long>(dataTable.Rows[indexCount]["NodeId"], typeof(long)),
                                        ParentId = GetValue<long>(dataTable.Rows[indexCount]["ParentId"], typeof(long))
                                    };
                                    contractHierarchy.Add(facilityDetails);
                                }
                                else
                                {
                                ContractHierarchy tempData = new ContractHierarchy
                                {
                                        NodeId = GetValue<long>(dataTable.Rows[indexCount]["NodeId"], typeof(long)),
                                        ParentId = GetValue<long>(dataTable.Rows[indexCount]["ParentId"], typeof(long)),
                                        FacilityId = GetValue<int>(dataTable.Rows[indexCount]["FacilityID"], typeof(int)),
                                        NodeText = GetStringValue(dataTable.Rows[indexCount]["NodeText"]),
                                        AppendString = GetStringValue(dataTable.Rows[indexCount]["AppendString"]),
                                    ContractId = DBNull.Value == dataTable.Rows[indexCount]["ContractId"]
                                        ? 0
                                            : GetValue<long>(dataTable.Rows[indexCount]["ContractId"], typeof(long)),
                                    IsContract = DBNull.Value == dataTable.Rows[indexCount]["IsContract"]
                                        ? (bool?)null
                                            : GetValue<bool>(dataTable.Rows[indexCount]["IsContract"], typeof(bool)),
                                    IsPrimaryNode = DBNull.Value != dataTable.Rows[indexCount]["IsPrimaryNode"]
                                                    &&
                                                        GetValue<bool>(dataTable.Rows[indexCount]["IsPrimaryNode"], typeof(bool))
                                };


                                contractHierarchy.Add(tempData);
                            }
                        }
                    }
                    }
                    else
                    {
                        if (dataSetObj.IsTableDataPopulated(count))
                        {
                            DataTable dataTable = dataSetObj.Tables[count];
                            for (int indexCount = 0; indexCount < dataTable.Rows.Count; indexCount++)
                            {
                                ContractHierarchy tempData = new ContractHierarchy
                                {
                                    NodeId = GetValue<long>(dataTable.Rows[indexCount]["NodeId"], typeof(long)),
                                    ContractId = GetValue<long>(dataTable.Rows[indexCount]["ContractId"], typeof(long)),
                                    ContractServiceTypeId = GetValue<long>(dataTable.Rows[indexCount]["ContractServiceTypeId"], typeof(long)),
                                    NodeText = GetStringValue(dataTable.Rows[indexCount]["ContractServiceTypeName"]),
                                    FacilityId = GetValue<int>(dataTable.Rows[indexCount]["FacilityID"], typeof(int)),
                                    IsCarveOut = DBNull.Value != dataTable.Rows[indexCount]["IsCarveOut"]
                                                 &&
                                                 GetValue<bool>(dataTable.Rows[indexCount]["IsCarveOut"], typeof(bool))
                                };
                                contractHierarchy.Add(tempData);
                            }
                        }
                    }
                }
                //returns the response to Business layer
                return contractHierarchy;
            }
            return contractHierarchy;

        }

        /// <summary>
        /// Copy Contract model
        /// </summary>
        /// <param name="moduleToCopy">The module to copy.</param>
        /// <returns>Inserted Model Id</returns>
        /// WID-1922
        public long CopyNode(ContractHierarchy moduleToCopy)
        {

            if (moduleToCopy != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("CopyContractByNodeAndParentId");
                _databaseObj.AddInParameter(_databaseCommand, "@NodeID", DbType.Int64, moduleToCopy.NodeId);
                _databaseObj.AddInParameter(_databaseCommand, "@ParentId", DbType.Int64, moduleToCopy.ParentId);
               _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String,
                    moduleToCopy.UserName.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@NodeText", DbType.String,
                    moduleToCopy.NodeText.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@LoggedInUserName", DbType.String,
                    moduleToCopy.LoggedInUserName.ToTrim());
                _databaseCommand.CommandTimeout = moduleToCopy.CommandTimeoutForContractHierarchy;
                long insertedModelId = GetValue<long>(_databaseObj.ExecuteScalar(_databaseCommand), typeof(long));
                return insertedModelId;
            }
            return 0;
        }


        /// <summary>
        /// Deletes the node and contract by node id.
        /// </summary>
        /// <param name="data">The node id.</param>
        /// <returns></returns>
        public bool DeleteNode(ContractHierarchy data)
        {

            _databaseCommand = _databaseObj.GetStoredProcCommand("DeleteNodeAndContractByNodeId");
            _databaseObj.AddInParameter(_databaseCommand, "@NodeId", DbType.Int64, data.NodeId);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, data.UserName);

            _databaseObj.ExecuteScalar(_databaseCommand);
            return true;
        }

        /// <summary>
        /// Deletes the contract service type by Id.
        /// </summary>
        /// <param name="data">The Id.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool DeleteContractServiceType(ContractHierarchy data)
        {
            if (data != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("DeleteContractServiceTypeById");
                _databaseObj.AddInParameter(_databaseCommand, "@ContractServiceTypeId", DbType.Int64,
                    data.ContractServiceTypeId);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String,
                   data.UserName);
                _databaseObj.ExecuteScalar(_databaseCommand);
            }
            return true;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommand.Dispose();
            _databaseObj = null;
        }


        /// <summary>
        /// Copies the contract by contract id.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>contractid</returns>
        public long CopyContract(ContractHierarchy data)
        {
            if (data != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("CopyContractHierarchyByNodeId");
                _databaseObj.AddInParameter(_databaseCommand, "@NodeId", DbType.Int64, data.NodeId);
                _databaseObj.AddInParameter(_databaseCommand, "@ContractID", DbType.Int64, data.ContractId);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, data.UserName.ToTrim());
               _databaseObj.AddInParameter(_databaseCommand, "@ContractName", DbType.String, data.ContractName);
                _databaseCommand.CommandTimeout = data.CommandTimeoutForContractHierarchy;
                long contractId;
                return long.TryParse(_databaseObj.ExecuteScalar(_databaseCommand).ToString(), out contractId)
                        ? contractId
                        : 0;
            }
             return 0;
        }

        /// <summary>
        /// Checks the model name is unique.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public bool IsModelNameExit(ContractHierarchy data)
        {
            if (data != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("IsModelNameExist");
                _databaseObj.AddInParameter(_databaseCommand, "@NodeText", DbType.String, data.NodeText.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityId", DbType.Int64, data.FacilityId);
                _databaseObj.AddInParameter(_databaseCommand, "@NodeId", DbType.Int64, data.NodeId);
                _databaseCommand.CommandTimeout = data.CommandTimeoutForContractHierarchy;

                return Convert.ToInt32(_databaseObj.ExecuteScalar(_databaseCommand)) == 0;

            }
            return false;

        }
    }
}
