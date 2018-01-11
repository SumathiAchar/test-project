/************************************************************************************************************/
/**  Author         : Ragini Bhandari
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract  notes , it add/modify contract information
/**  User Story Id  : 5.User Story Add a new contract Figure 12
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/
using System;
using System.Data;
using System.Data.Common;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ContractNoteRepository : IContractNoteRepository
    {
        // Variables
        private Database _db;
        DbCommand _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNoteRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractNoteRepository(string connectionString)
        {
            _db = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Add & Edit Contract Notes Info data
        /// </summary>
        /// <param name="contractNotes"></param>
        /// <returns>contractNoteId</returns>
        public ContractNote AddEditContractNote(ContractNote contractNotes)
        {
            ContractNote note = new ContractNote();
            //Checks if input request is not null
            if (contractNotes != null)
            {
                _cmd = _db.GetStoredProcCommand("AddEditContractNotes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _db.AddInParameter(_cmd, "@ContractNoteID ", DbType.Int64, contractNotes.ContractNoteId);
                _db.AddInParameter(_cmd, "@ContractID ", DbType.Int64, contractNotes.ContractId);
                _db.AddInParameter(_cmd, "@Status", DbType.Int32, contractNotes.Status);
                _db.AddInParameter(_cmd, "@NoteText", DbType.String, contractNotes.NoteText.ToTrim());
                _db.AddInParameter(_cmd, "@UserName", DbType.String, contractNotes.UserName.ToTrim());
                // Retrieve the results of the Stored Procedure in Datatable
                DataSet contractNotesDataSet = _db.ExecuteDataSet(_cmd);

                if (contractNotesDataSet.IsTableDataPopulated(0))
                {
                    string localDateTime = Utilities.GetLocalTimeString(contractNotes.CurrentDateTime,
                        Convert.ToDateTime(contractNotesDataSet.Tables[0].Rows[0]["InsertDate"].ToString()));
                    note.ContractNoteId = long.Parse(contractNotesDataSet.Tables[0].Rows[0]["InsertedId"].ToString());
                    note.InsertDate = Convert.ToDateTime(localDateTime);
                }
            }
            //returns response to Business layer
            return note;
        }

        /// <summary>
        ///  Delete Contract Note By Id 
        /// </summary>
        /// <param name="contractNotes"></param>
        /// <returns>returnvalue</returns>
        public bool DeleteContractNote(ContractNote contractNotes)
        {
            //holds the response data
            bool returnvalue = false;

            // Initialize the Stored Procedure
            _cmd = _db.GetStoredProcCommand("DeleteContractNotesByID");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _db.AddInParameter(_cmd, "@ContractNoteId ", DbType.Int64, contractNotes.ContractNoteId);
            _db.AddInParameter(_cmd, "@UserName ", DbType.String, contractNotes.UserName);
            // Retrieve the results of the Stored Procedure in Datatable
            int updatedRow = _db.ExecuteNonQuery(_cmd);
            //returns response to Business layer
            if (updatedRow > 0)
                returnvalue = true;
            //returns false if any exception occurs
            return returnvalue;
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
