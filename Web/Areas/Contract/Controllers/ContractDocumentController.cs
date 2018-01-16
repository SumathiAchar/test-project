using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using System;
using System.IO;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractDocumentController : CommonController
    {
        //
        // GET: /ContractDocuments/
        public ActionResult ContractDocument()
        {
            return View();
        }

        /// <summary>
        /// Gets the document.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public FileContentResult GetDocument(long id)
        {
            ContractDoc document = GetApiResponse<ContractDoc>("ContractDoc", "GetContractDocById", id);
            if (document != null)
            {
                if (document.ContractContent == null)
                {
                    string fileNameWithExtension = Path.GetFileName(document.DocumentId.ToString());
                    string extentionOnly = Path.GetExtension(document.FileName);
                    string mimeType = MimeAssistant.GetMimeTypeByFileName(fileNameWithExtension + extentionOnly);
                    return
                        File(
                            System.IO.File.ReadAllBytes(Path.Combine(
                                GlobalConfigVariable.ContractDocumentsPath, fileNameWithExtension + extentionOnly)),
                            mimeType, document.FileName);
                }
                else
                {
                    byte[] fileContent = document.ContractContent;
                    string mimeType = MimeAssistant.GetMimeTypeByFileName(document.FileName);
                    return File(fileContent, mimeType, document.FileName);
                }
            }
            return null;
        }

        /// <summary>
        /// Saves the contract document.
        /// </summary>
        /// <param name="contractId">The contract unique identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveContractDoc(long contractId)
        {
            //Check if file exists
            if (Request.Files != null && Request.Files.Count > 0)
            {
                //Fetches the uploaded file
                var httpPostedFileBase = Request.Files[0];
                if (httpPostedFileBase != null)
                {
                    if (httpPostedFileBase.ContentLength > GlobalConfigVariable.ContractDocumentFileSize)
                        return
                            Json(
                                new
                                {
                                    status = "fail",
                                    fileSize =
                                        string.Format(Constants.FileMaxSize,
                                            GlobalConfigVariable.ContractDocumentFileSize / Convert.ToInt64(Constants.ConvertToMb))
                                }, "text/plain");
                    var fileName = Path.GetFileName(httpPostedFileBase.FileName);

                    //Get the Name of User logged in
                    ContractDoc contractDoc = new ContractDoc
                    {
                        ContractId = contractId,
                        FileName = fileName,
                        UserName = GetCurrentUserName()
                    };
                    ContractDoc contractDocument = PostApiResponse<ContractDoc>("ContractDoc", "AddEditContractDocs", contractDoc);
                    if (fileName != null)
                    {
                        // Checking whether path is exist or not
                        if (!Directory.Exists(GlobalConfigVariable.ContractDocumentsPath))
                        {
                            Directory.CreateDirectory(GlobalConfigVariable.ContractDocumentsPath);
                        }
                        //Save file into local folder
                        var filePath =
                            Path.Combine(GlobalConfigVariable.ContractDocumentsPath,
                                contractDocument.DocumentId + Path.GetExtension(httpPostedFileBase.FileName));
                        httpPostedFileBase.SaveAs(filePath);
                    }
                    return contractDocument.ContractDocId > 0
                        ? Json(new { isSuccess = true, id = contractDocument.ContractDocId }, "text/plain")
                        : Json(new { isSuccess = false, id = 0 });
                }
            }
            return Json(new { isSuccess = false, id = 0 });
        }

        /// <summary>
        /// Delete Contract Documents
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteContractDoc(long id)
        {
            ContractDoc contractDocs = new ContractDoc {ContractDocId = id, UserName = GetCurrentUserName(),FacilityId = GetCurrentFacilityId()};
            //Get the UserName logged in
            bool isSuccess = PostApiResponse<bool>("ContractDoc", "DeleteContractDoc", contractDocs);
            return Json(new { sucess = isSuccess });
        }
    }
}