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

using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.Unity;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.BusinessLogic
{
    public class ContractNoteLogic
    {

        /// <summary>
        /// The _contract notes repository
        /// </summary>
        private readonly IContractNoteRepository _contractNotesRepository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNoteLogic"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ContractNoteLogic(string connectionString)
        {
            _contractNotesRepository = Factory.CreateInstance<IContractNoteRepository>(connectionString, true);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ContractNoteLogic"/> class.
        /// </summary>
        /// <param name="contractRepository">The contract repository.</param>
        public ContractNoteLogic(IContractNoteRepository contractRepository)
        {
            if (contractRepository != null)
            {
                _contractNotesRepository = contractRepository;
            }
        }


        /// <summary>
        /// Adds the edit contract note.
        /// </summary>
        /// <param name="contractNotes">The contract notes.</param>
        /// <returns></returns>
        public ContractNote AddEditContractNote(ContractNote contractNotes)
        {
            return _contractNotesRepository.AddEditContractNote(contractNotes);
        }


        /// <summary>
        /// Deletes the contract note by unique identifier.
        /// </summary>
        /// <param name="contractNotes">The contract note unique identifier.</param>
        /// <returns></returns>
        public bool DeleteContractNote(ContractNote contractNotes)
        {
            return _contractNotesRepository.DeleteContractNote(contractNotes);
        }
    }
}
