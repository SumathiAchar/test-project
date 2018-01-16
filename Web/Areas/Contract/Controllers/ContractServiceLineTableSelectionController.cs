using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractServiceLineTableSelectionController : CommonController
    {
        //
        // GET: /ServiceLineTableSelection/

        public ActionResult ContractServiceLineTableSelection(long? contractId, long? serviceTypeId,
            long serviceLineTypeId, bool isEdit)
        {
            ContractServiceLineTableSelectionViewModel contractServiceLineTable =
                new ContractServiceLineTableSelectionViewModel();

            if (isEdit)
            {
                ContractServiceLineTableSelection contractServiceLineTableSelection =
                    new ContractServiceLineTableSelection
                    {
                        ContractServiceTypeId = serviceTypeId,
                        ContractId = contractId,
                        ServiceLineTypeId = serviceLineTypeId,
                        UserName = GetCurrentUserName()
                    };

                //Get the Name of User logged in
                List<ContractServiceLineTableSelection> serviceLineTableList =
                    PostApiResponse<List<ContractServiceLineTableSelection>>("ContractServiceLineTableSelection",
                        "GetServiceLineTableSelection",
                        contractServiceLineTableSelection);

                List<ContractServiceLineTableSelectionViewModel> tableList =
                    AutoMapper.Mapper
                        .Map<List<ContractServiceLineTableSelection>, List<ContractServiceLineTableSelectionViewModel>>(
                            serviceLineTableList);
                contractServiceLineTable.TableselectionList = tableList;
                contractServiceLineTable.ContractServiceLineId = tableList[0].ContractServiceLineId;
            }
            contractServiceLineTable.ModuleId =
                Convert.ToByte(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.ClaimToolTableSelection).FieldIdentityNumber);
            contractServiceLineTable.ContractId = contractId;
            contractServiceLineTable.ContractServiceTypeId = serviceTypeId;
            contractServiceLineTable.ServiceLineTypeId = serviceLineTypeId;
            contractServiceLineTable.IsEdit = isEdit;
            return View(contractServiceLineTable);
        }

        /// <summary>
        /// Gets the claim field and tables.
        /// </summary>
        /// <param name="contractId">The contract identifier.</param>
        /// <param name="serviceTypeId">The service type identifier.</param>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public JsonResult GetClaimFieldAndTables(long? contractId, long? serviceTypeId, int? moduleId)
        {
            ContractServiceLineTableSelectionViewModel contractServiceLineTableSelectionViewModel =
                new ContractServiceLineTableSelectionViewModel { ContractId = contractId, ContractServiceTypeId = serviceTypeId };

            // Get Claim field from DB based on module id
            List<ClaimField> contractClaimFeild = GetApiResponse<List<ClaimField>>("ClaimField", "GetClaimFieldsByModule", moduleId);

            List<EnumHelper> fieldInfoFromEnum = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.TableSelectionClaimType>();

            List<ClaimField> claimFields = (contractClaimFeild.Where(
                element => fieldInfoFromEnum.Select(x => x.FieldIdentityNumber).Contains(element.ClaimFieldId))).ToList();

            List<SelectListItem> contractServiceClaims = new List<SelectListItem>();
            if (claimFields.Count > 0)
            {
                contractServiceClaims.AddRange(claimFields.Select(item => new SelectListItem { Text = item.Text, Value = item.ClaimFieldId.ToString(CultureInfo.InvariantCulture) }));
            }

            //Removing ClaimID,Adjudication Request Name as user should not ab able to select ClaimID,Adjudication Request Name for adjudication
            contractServiceClaims.RemoveAll(a => a.Text == "Adjudication Request Name" || a.Text == "ClaimID");

            //Get the Name of User logged in
            contractServiceLineTableSelectionViewModel.UserName = GetCurrentUserName();
            return Json(new { claimFeilds = contractServiceClaims}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the edit service line claim and tables.
        /// </summary>
        /// <param name="listofServiceLineClaimandTable">The list of service line claim and table.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEditServiceLineClaimAndTables(List<ContractServiceLineTableSelectionViewModel> listofServiceLineClaimandTable)
        {
            long noOfRecord = 0;
            bool isTableValid = false;
            if (listofServiceLineClaimandTable.All(f => f.ClaimFieldDocId != null))
            {
                isTableValid = true;
                List<ContractServiceLineTableSelection> serviceLineModel = new List<ContractServiceLineTableSelection>();
                if (listofServiceLineClaimandTable != null && listofServiceLineClaimandTable.Count > 0)
                {
                    serviceLineModel.AddRange(
                        listofServiceLineClaimandTable.Select(
                            AutoMapper.Mapper
                                .Map<ContractServiceLineTableSelectionViewModel, ContractServiceLineTableSelection>));
                }
                //Get the Name of User logged in
                foreach (var contractServiceLineTableSelection in serviceLineModel)
                {
                    contractServiceLineTableSelection.UserName = GetCurrentUserName();
                }
                noOfRecord = PostApiResponse<long>("ContractServiceLineTableSelection",
                    "AddEditServiceLineClaimAndTables", serviceLineModel);
            }
            return noOfRecord > 0 ? Json(new { success = true, Id = noOfRecord }) : Json(new { success = false, Id = noOfRecord, IsTableValid = isTableValid });
        }
        /// <summary>
        /// Gets the table selection claim field operators.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTableSelectionClaimFieldOperators()
        {
            List<ClaimFieldOperator> tableSelectionOperators = GetApiResponse<List<ClaimFieldOperator>>("ContractServiceLineTableSelection", "GetTableSelectionClaimFieldOperators");
            List<SelectListItem> tableSelectionOperatorList = new List<SelectListItem>();
            if (tableSelectionOperators != null && tableSelectionOperators.Any())
            {
                tableSelectionOperatorList.AddRange(
                    tableSelectionOperators.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.OperatorType,
                                Value = item.OperatorId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(new { claimFeildOperatorList = tableSelectionOperatorList }, JsonRequestBehavior.AllowGet);

           
        }

        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetTables(long? contractId, long? serviceTypeId, int? tableType, string userText)
        {
            ContractServiceLineTableSelectionViewModel contractServiceLineTableSelectionViewModel =
               new ContractServiceLineTableSelectionViewModel { ContractId = contractId, ContractServiceTypeId = serviceTypeId, TableType = tableType, UserText = userText };
            List<ClaimField> contractClaimFeilds = PostApiResponse<List<ClaimField>>(
                "ContractServiceLineTableSelection", "GetClaimFieldAndTables", contractServiceLineTableSelectionViewModel);
            List<SelectListItem> contractServiceTables = new List<SelectListItem>();
            if (contractClaimFeilds != null && contractClaimFeilds.Count > 0)
            {
                contractServiceTables.AddRange(
                    contractClaimFeilds.Select(
                        item =>
                            new SelectListItem
                            {
                                Text = item.TableName,
                                Value =
                                    item.ClaimFieldDocId.ToString(CultureInfo.InvariantCulture) + "-" +
                                    item.ClaimFieldId.ToString(CultureInfo.InvariantCulture)
                            }));
            }
            return Json(contractServiceTables, JsonRequestBehavior.AllowGet);
        }
    }
}
