using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    public class ClaimFieldRepository : IClaimFieldRepository
    {
        private readonly Database _databaseObj;
        DbCommand _databaseCommandObj;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        public List<ClaimField> GetClaimFieldsByModule(int moduleId)
        {
            List<ClaimField> claimFieldList = new List<ClaimField>();
            
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetClaimFieldsByModuleId");
                _databaseObj.AddInParameter(_databaseCommandObj, "@ModuleId", DbType.Int64, moduleId);
                DataSet claimFieldDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

                if (claimFieldDataSet.IsTableDataPopulated(0))
                {
                    for (int i = 0; i < claimFieldDataSet.Tables[0].Rows.Count; i++)
                    {
                        ClaimField claimField = new ClaimField
                        {
                            ClaimFieldId = long.Parse(claimFieldDataSet.Tables[0].Rows[i]["ClaimFieldId"].ToString()),
                            Text = Convert.ToString(claimFieldDataSet.Tables[0].Rows[i]["Text"])
                        };
                        claimFieldList.Add(claimField);
                    }
                }
            return claimFieldList;
        }
    }
}