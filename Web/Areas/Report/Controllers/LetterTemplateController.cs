using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AutoMapper;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;

namespace SSI.ContractManagement.Web.Areas.Report.Controllers
{
    public class LetterTemplateController : CommonController
    {

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Letters the template.
        /// </summary>
        /// <param name="letterTemplateId">The letter template identifier.</param>
        /// <returns></returns>
        public ActionResult LetterTemplate(int letterTemplateId)
        {
            LetterTemplateViewModel letterTemplateViewModel = new LetterTemplateViewModel();

            if (letterTemplateId > 0)
            {
                LetterTemplate letterTemplate = new LetterTemplate { LetterTemplateId = letterTemplateId, UserName = GetCurrentUserName() };
                letterTemplate = PostApiResponse<LetterTemplate>(Constants.LetterTemplate, Constants.Get,
                            letterTemplate);


                if (letterTemplate != null && letterTemplate.TemplateText != null)
                {
                    letterTemplate = LetterReportUtil.UpdateImageUrl(letterTemplate, Request.ApplicationPath);

                    letterTemplateViewModel = Mapper.Map<LetterTemplate, LetterTemplateViewModel>(letterTemplate);
                }
            }
            return View(letterTemplateViewModel);
        }

        /// <summary>
        /// Creates the letter template.
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Saves the letter template.
        /// </summary>
        /// <param name="letterTemplateViewModel">The letter template view model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Save(LetterTemplateViewModel letterTemplateViewModel)
        {
            JsonResult jsonResult;
            if (letterTemplateViewModel == null)
            {
                jsonResult = Json(new { isSuccess = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                LetterTemplate letterTemplate =
                    Mapper.Map<LetterTemplateViewModel, LetterTemplate>(letterTemplateViewModel);
                letterTemplate.UserName = GetCurrentUserName();
                letterTemplate.FacilityId = GetCurrentFacilityId();
                bool isLetterTemplateNameExists = false;

                //Check duplicate unique name
                if (letterTemplateViewModel.LetterTemplateId == 0)
                {
                    //Check letter is already exist in DB
                    isLetterTemplateNameExists = PostApiResponse<bool>(Constants.LetterTemplate, Constants.IsLetterNameExists,
                        letterTemplate);
                }
                if (!isLetterTemplateNameExists)
                {
                    long letterTemplateTypeId = PostApiResponse<long>(Constants.LetterTemplate, Constants.Save,
                        letterTemplate);
                    jsonResult = Json(new { isSuccess = true, letterTemplateTypeId });
                }
                else
                {
                    jsonResult = Json(new { isSuccess = false, isExists = true });
                }
            }
            return jsonResult;
        }

        /// <summary>
        /// Gets all letter templates.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <returns></returns>
        public ContentResult GetAll(int take, int skip)
        {
            PageSetting pageSetting = new PageSetting { Skip = skip, Take = take };

            LetterTemplateContainer templateInfo = new LetterTemplateContainer
            {
                PageSetting = pageSetting,
                FacilityId = GetCurrentFacilityId()
            };
            // Getting current selected facility Id by the logged in user
            LetterTemplateContainer templateContainer = PostApiResponse<LetterTemplateContainer>(Constants.LetterTemplate, Constants.GetAll, templateInfo);

            DataSourceResult letterTemplatesResult = new DataSourceResult();

            if (templateContainer != null && templateContainer.LetterTemplates != null && templateContainer.LetterTemplates.Any())
            {
                letterTemplatesResult.Data = templateContainer.LetterTemplates.Select(Mapper.Map<LetterTemplate, LetterTemplateViewModel>).ToList();
                letterTemplatesResult.Total = templateContainer.TotalRecords;
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            return new ContentResult
            {
                Content = serializer.Serialize(letterTemplatesResult),
                ContentType = Constants.ContentTypeApplication,
            };
        }

        /// <summary>
        /// Previews the letter template.
        /// </summary>
        /// <param name="letterTemplateViewModel">The letter template view model.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Preview(LetterTemplateViewModel letterTemplateViewModel)
        {
            
            if (letterTemplateViewModel.LetterTemplateId > 0 && letterTemplateViewModel.TemplateText == null)
            {
                LetterTemplate letterTemplate = new LetterTemplate
                {
                    UserName = GetCurrentUserName(),
                    LetterTemplateId = letterTemplateViewModel.LetterTemplateId
                };
                letterTemplate = PostApiResponse<LetterTemplate>(Constants.LetterTemplate, Constants.Get,
                    letterTemplate);
                if (letterTemplate != null)
                {
                    letterTemplateViewModel = Mapper.Map<LetterTemplate, LetterTemplateViewModel>(letterTemplate);
                }
            }
            else
            {
                letterTemplateViewModel.UserName = GetCurrentUserName();
                PostApiResponse<bool>(Constants.LetterTemplate, Constants.InsertAuditLog,
                    letterTemplateViewModel);
            }
            var serializer = new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue };
            
            string[] fileName = LetterReportUtil.RtfPreview(letterTemplateViewModel,
                GlobalConfigVariable.ReportsFilePath);
            var result = new ContentResult
            {
                Content = serializer.Serialize(new { fileNameData = fileName }),
                ContentType = Constants.ContentTypeApplication
            };
            return result;
        }

        /// <summary>
        /// Gets the letter variables.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetLetterVariables()
        {
            SelectList selectList = new SelectList(
                Constants.LetterVariables.Select(x => new { value = x.Value, text = x.Key }),
                Constants.DropDownListValueField,
                Constants.DropDownListTextField
                );
            return Json(new { letterVariables = selectList }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Deletes the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public JsonResult Delete(long id)
        {
            LetterTemplate letterTemplate = new LetterTemplate
            {
                LetterTemplateId = id,
                UserName = GetCurrentUserName()
            };

            bool isSuccess = PostApiResponse<bool>(Constants.LetterTemplate, Constants.DeleteLetter, letterTemplate);
            return Json(new { sucess = isSuccess });
        }
    }
}
