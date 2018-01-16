using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractNoteController : CommonController
    {
        public ActionResult ContractNote()
        {
            return View();
        }

        /// <summary>
        /// Save Contract Notes
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveContractNotes(ContractNotesViewModel info)
        {
            ContractNote note = AutoMapper.Mapper.Map<ContractNotesViewModel, ContractNote>(info);

            //Get the Name of User logged in
            note.UserName = GetCurrentUserName();
            ContractNote responseNote = PostApiResponse<ContractNote>("ContractNote", "AddEditContractNote", note);
            if (responseNote.ContractNoteId > 0)
            {

                return
                    Json(
                        new
                        {
                            sucess = true,
                            Id = responseNote.ContractNoteId,
                            userName = note.UserName,
                            insertedTime = Convert.ToDateTime(responseNote.InsertDate).ToString(Constants.DateTimeFormatWithSecond)
                        });
            }
            return Json(new { sucess = false });
        }

        /// <summary>
        /// Deletes the contract notes.
        /// </summary>
        /// <param name="id">The unique identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteContractNotes(int id)
        {
            ContractNote contractNotes = new ContractNote {ContractNoteId = id, UserName = GetCurrentUserName()};
            //Get the UserName logged in
            bool isSuccess = PostApiResponse<bool>("ContractNote", "DeleteContractNoteById", contractNotes);
            return Json(new { sucess = isSuccess });
        }
    }
}
