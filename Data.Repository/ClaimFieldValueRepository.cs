using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    public class ClaimFieldValueRepository : IClaimFieldValueRepository
    {
        private readonly Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimFieldValueRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimFieldValueRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        //TODO: This method is referring to an SP "AddEditCAPPayment" not present, so I assume this method is not in use. Please verify.
        /// <summary>
        /// Adds the claim field values.
        /// </summary>
        /// <param name="claimFieldValues">The claim field values.</param>
        /// <returns></returns>
        public long AddClaimFieldValues(ClaimFieldValue claimFieldValues)
        {
            //Checks if input request is not null
            if (claimFieldValues != null)
            {
                 // Initialize the Stored Procedure
                    _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditClaimFieldvalues");
                    // Pass parameters to Stored Procedure(i.e., @ParamName), add values for

                    // Retrieve the results of the Stored Procedure in Dataset
                long claimFieldValueId;
                long returnValue = long.TryParse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString(), out claimFieldValueId) ? claimFieldValueId : 0;
              
                //returns response to Business layer
                return returnValue;
            }
            return 0;
        }
    }
 }

