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
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Shared.Data.Repository
{
    public interface IContractNoteRepository : IDisposable
    {

        /// <summary>
        /// Adds the edit contract note.
        /// </summary>
        /// <param name="contractNotes">The contract notes.</param>
        /// <returns></returns>
        ContractNote AddEditContractNote(ContractNote contractNotes);

        /// <summary>
        /// Deletes the contract note by unique identifier.
        /// </summary>
        /// <param name="contractNotes">The contract note unique identifier.</param>
        /// <returns></returns>
        bool DeleteContractNote(ContractNote contractNotes);
    }
}
