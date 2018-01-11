using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class MedicareIpAcuteOptionRepository : IMedicareIpAcuteOptionRepository
    {
        // Variables
        private Database _database;
        private DbCommand _command;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedicareIpAcuteOptionRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public MedicareIpAcuteOptionRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _database = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the payment type Medicare ip Options.
        /// </summary>
        /// <returns></returns>
        public List<MedicareIpAcuteOption> GetMedicareIpAcuteOptions()
        {
            List<MedicareIpAcuteOption> medicareIpAcuteOptions = new List<MedicareIpAcuteOption>();

            // Initialize the Stored Procedure
            _command = _database.GetStoredProcCommand("GetMedicareIpAcuteOptions");

            // Retrieve the results of the Stored Procedure in Data set
            DataSet medicareIpDataSet = _database.ExecuteDataSet(_command);

            if (medicareIpDataSet.IsTableDataPopulated(0))
            {
                DataTable medicareIpData = medicareIpDataSet.Tables[0].DefaultView.ToTable(true,
                    Constants.MedicareIpAcuteOptionId, Constants.MedicareIpAcuteOptionCode,
                    Constants.MedicareIpAcuteOptionName);

                for (int count = 0; count < medicareIpData.Rows.Count; count++)
                {
                    // Getting the Parent items
                    MedicareIpAcuteOption medicareIpAcuteOption = new MedicareIpAcuteOption
                    {
                        MedicareIpAcuteOptionId = Convert.ToInt32(medicareIpData.Rows[count][Constants.MedicareIpAcuteOptionId]),
                        MedicareIpAcuteOptionCode = Convert.ToString(medicareIpData.Rows[count][Constants.MedicareIpAcuteOptionCode]),
                        MedicareIpAcuteOptionName = Convert.ToString(medicareIpData.Rows[count][Constants.MedicareIpAcuteOptionName])
                    };

                    DataRow[] medicareIpDataRow =
                        medicareIpDataSet.Tables[0].Select(string.Format("{0}={1}"
                            , Constants.MedicareIpAcuteOptionChildParentId
                            , medicareIpAcuteOption.MedicareIpAcuteOptionId));

                    if (medicareIpDataRow.Any())
                    {
                        medicareIpAcuteOption.MedicareIpAcuteOptionChilds = new List<MedicareIpAcuteOptionChild>();
                        // Getting the child items 
                        foreach (MedicareIpAcuteOptionChild medicareIpAcuteOptionChild in medicareIpDataRow.Select(medicareIpRow => new MedicareIpAcuteOptionChild
                        {
                            MedicareIpAcuteOptionChildId = Convert.ToInt32(medicareIpRow[Constants.MedicareIpAcuteOptionChildId]),
                            MedicareIpAcuteOptionChildCode = Convert.ToString(medicareIpRow[Constants.MedicareIpAcuteOptionChildCode]),
                            MedicareIpAcuteOptionChildName = Convert.ToString(medicareIpRow[Constants.MedicareIpAcuteOptionChildName]),
                            MedicareIpAcuteOptionId = medicareIpAcuteOption.MedicareIpAcuteOptionId
                        }))
                        {
                            medicareIpAcuteOption.MedicareIpAcuteOptionChilds.Add(medicareIpAcuteOptionChild);
                        }
                    }
                    medicareIpAcuteOptions.Add(medicareIpAcuteOption);
                }
            }

            //returns response to Business layer
            return medicareIpAcuteOptions;
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _command.Dispose();
            _database = null;
        }
    }
}
