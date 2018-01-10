/************************************************************************************************************/
/**  Author         : 
/**  Created        : 12-Aug-2013
/**  Summary        : Handles Contract  notes , it add/modify contract information
/**  User Story Id  : 5.User Story Add a new contract Figure 12
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Contract
{
    // ReSharper disable once UnusedMember.Global
    public class ContractNoteController : BaseController
    {
        /// <summary>
        /// The contract logic
        /// </summary>
        private readonly ContractNoteLogic _contractLogic;


        /// <summary>
        /// Prevents a default instance of the <see cref="ContractNoteController"/> class from being created.
        /// </summary>
        ContractNoteController()
        {
            int facilityId = Convert.ToInt32(System.Web.HttpContext.Current.Request.Headers[Constants.BubbleDataSource]);
            string bubbleDataSource = GetFacilityConnection(facilityId); 
            _contractLogic = new ContractNoteLogic(bubbleDataSource);
        }

        /// <summary>
        /// This method will add/edit ContractNote in database and return inserted/updated id
        /// </summary>
        /// <param name="contractNotes">ContractNotes object</param>
        /// <returns>inserted/updated id</returns>
        [HttpPost]
        public ContractNote AddEditContractNote(ContractNote contractNotes)
        {
            return _contractLogic.AddEditContractNote(contractNotes);
        }

        /// <summary>
        /// Delete Contract note by Id
        /// </summary>
        /// <param name="contractNotes">contractNoteId</param>
        /// <returns>true/false</returns>
        [HttpPost]
        public bool DeleteContractNoteById(ContractNote contractNotes)
        {
            return _contractLogic.DeleteContractNote(contractNotes);
        }

    }
}
