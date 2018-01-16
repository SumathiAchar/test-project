using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class ContractLinkController : SessionStoreController
    {
        public ActionResult ContractLink()
        {
            return View();
        }

        /// <summary>
        /// Inserting ContractBasicInfoData into DataBase
        /// </summary>
        /// <param name="basicInfo"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public JsonResult PushContractBasicInfoData(ContractBasicInfoViewModel basicInfo)
        {
            Shared.Models.Contract contract = AutoMapper.Mapper.Map<ContractBasicInfoViewModel, Shared.Models.Contract>(basicInfo);
            if (basicInfo != null)
            {
                if (basicInfo.SelectedPayerList != null && basicInfo.SelectedPayerList.Count > 0)
                {
                    contract.Payers = new List<Payer>();
                    foreach (var payer in basicInfo.SelectedPayerList)
                    {
                        contract.Payers.Add(new Payer { PayerName = payer.Name });
                    }
                }
                LastExpandedNodeId = basicInfo.NodeId;
                LastHighlightedNodeId = basicInfo.NodeId;
            }
            contract.UserName = GetLoggedInUserName();
            Shared.Models.Contract contractinfo = PostApiResponse<Shared.Models.Contract>(Constants.Contract, Constants.AddEditContractBasicInfo, contract);

            bool isSuccess = false;
            if (contractinfo != null)
            {
                LastHighlightedNodeId = contractinfo.NodeId;
                LastExpandedNodeId = contractinfo.NodeId;
                isSuccess = contractinfo.ContractId != 0;
            }
            return Json(new { sucess = isSuccess, addedId = contractinfo != null ? contractinfo.ContractId : 0 });
        }

        /// <summary>
        /// Gets the udf fields.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetUdfFields()
        {
            List<EnumHelper> udfFields = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.ClaimFieldTypes>();

            if (udfFields != null && udfFields.Any())
            {
                udfFields =
                    udfFields.Where(
                        a =>
                            a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField1.ToString())) || a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField2.ToString())) || a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField3.ToString())) || a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField4.ToString())) || a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField5.ToString())) || a.FieldIdentityNumber ==
                             Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldTypes),
                                Enums.ClaimFieldTypes.CustomField6.ToString()))).ToList();
            }
            return Json(new { udfFields }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Checks the contract name is unique.
        /// </summary>
        /// <param name="basicInfo">The basic information.</param>
        /// <returns></returns>
        public JsonResult IsContractNameExist(ContractBasicInfoViewModel basicInfo)
        {
            Shared.Models.Contract contract = AutoMapper.Mapper.Map<ContractBasicInfoViewModel, Shared.Models.Contract>(basicInfo);
            contract.UserName = GetLoggedInUserName();
            return Json(PostApiResponse<bool>(Constants.Contract, Constants.IsContractNameExist, contract));
        }
    }
}
