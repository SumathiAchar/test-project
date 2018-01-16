using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Contract.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Controllers
{
    public class PaymentTypeCustomTableController : CommonController
    {
        public ActionResult PaymentTypeCustomTable(long? contractId, long? serviceTypeId, int paymentTypeId, bool isEdit)
        {
            PaymentTypeCustomTableViewModel paymentTypeCustomTableViewModel = new PaymentTypeCustomTableViewModel();
            if (isEdit)
            {
                PaymentTypeCustomTable paymentTypeCustomTableForPost = new PaymentTypeCustomTable
                {
                    ServiceTypeId = serviceTypeId,
                    ContractId = contractId,
                    PaymentTypeId = paymentTypeId,
                    UserName = GetCurrentUserName()
                };

                //Get the Name of User logged in
                PaymentTypeCustomTable customPaymentTypeInfo = PostApiResponse<PaymentTypeCustomTable>(Constants.PaymentTypeCustomTable,
                                                                    Constants.GetPaymentTypeCustomTable,
                                                                    paymentTypeCustomTableForPost);

                paymentTypeCustomTableViewModel = AutoMapper.Mapper.Map<PaymentTypeCustomTable, PaymentTypeCustomTableViewModel>(customPaymentTypeInfo);
            }
            paymentTypeCustomTableViewModel.ContractId = contractId;
            paymentTypeCustomTableViewModel.ServiceTypeId = serviceTypeId;
            paymentTypeCustomTableViewModel.PaymentTypeId = paymentTypeId;
            paymentTypeCustomTableViewModel.IsEdit = isEdit;
            paymentTypeCustomTableViewModel.ModuleId = Convert.ToByte(EnumHelperLibrary.GetFieldInfoFromEnum(Enums.Modules.CustomPaymentModeling).FieldIdentityNumber);
            return View(paymentTypeCustomTableViewModel);
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetHeaders(long? documentId)
        {
            string documentHeaders = string.Empty;
            if (documentId != null)
            {
                documentHeaders = GetApiResponse<string>(Constants.PaymentTypeCustomTable, "GetHeaders", documentId);
            }
            return Json(new { documentHeader = documentHeaders }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Adds the edit.
        /// </summary>
        /// <param name="paymentTypeCustomTableViewModel">The payment type custom table view model.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddEdit(PaymentTypeCustomTableViewModel paymentTypeCustomTableViewModel)
        {
            long paymentDetailId = 0;
            bool isValidated = true;
            bool isMultiplier = false;
            string[] resultFormula = new string[7];
            if (paymentTypeCustomTableViewModel.DocumentId != null)
            {
                PaymentTypeCustomTable customTable =
                    AutoMapper.Mapper.Map<PaymentTypeCustomTableViewModel, PaymentTypeCustomTable>(
                        paymentTypeCustomTableViewModel);
                customTable.Expression = Utilities.Replace(customTable.Expression, Constants.NewLine, string.Empty);

                customTable.ExpandedExpression = Utilities.Replace(customTable.ExpandedExpression, Constants.NewLine,
                    string.Empty);
                Dictionary<string, string> tableHeader = new Dictionary<string, string>();
                for (int i = 0; i < paymentTypeCustomTableViewModel.TableHeaders.Count; i++)
                {
                    tableHeader[
                        paymentTypeCustomTableViewModel.TableHeaders.ElementAt(i)
                            .Key.Replace(Constants.ReplaceDotString, Constants.Dot)] =
                        paymentTypeCustomTableViewModel.TableHeaders.ElementAt(i).Value;
                }
                paymentTypeCustomTableViewModel.TableHeaders = tableHeader;

                //send particular id for highlighting the text box if formula validation fails.
                if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.Expression,
                  paymentTypeCustomTableViewModel.TableHeaders))
                {
                    resultFormula[0] = "#txtChooseFormula";
                    isMultiplier = true;
                    isValidated = false;
                }
                //send particular id for highlighting the text box if multiplier first validation fails.
                if (customTable.MultiplierFirst != null)
                {
                    customTable.MultiplierFirst = Utilities.Replace(customTable.MultiplierFirst, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.MultiplierFirst,
                       paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[1] = "#txt-first-multiplier";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                //send particular id for highlighting the text box if multiplier second validation fails.
                if (customTable.MultiplierSecond != null)
                {
                    customTable.MultiplierSecond = Utilities.Replace(customTable.MultiplierSecond, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.MultiplierSecond,
                       paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[2] = "#txt-second-multiplier";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                //send particular id for highlighting the text box if multiplier third validation fails.
                if (customTable.MultiplierThird != null)
                {
                    customTable.MultiplierThird = Utilities.Replace(customTable.MultiplierThird, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.MultiplierThird,
                       paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[3] = "#txt-third-multiplier";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                //send particular id for highlighting the text box if multiplier forth validation fails.
                if (customTable.MultiplierFourth != null)
                {
                    customTable.MultiplierFourth = Utilities.Replace(customTable.MultiplierFourth, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.MultiplierFourth,
                       paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[4] = "#txt-fourth-multiplier";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                //send particular id for highlighting the text box if multiplier others validation fails.
                if (customTable.MultiplierOther != null)
                {
                    customTable.MultiplierOther = Utilities.Replace(customTable.MultiplierOther, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.MultiplierOther,
                       paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[5] = "#txt-others-multiplier";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                //send particular id for highlighting the text box if observe service unit limit validation fails.
                if (customTable.ObserveServiceUnitLimit != null)
                {
                    customTable.ObserveServiceUnitLimit = Utilities.Replace(customTable.ObserveServiceUnitLimit, Constants.NewLine, string.Empty);
                    if (!Utilities.ValidateExpression(paymentTypeCustomTableViewModel.ObserveServiceUnitLimit,
                        paymentTypeCustomTableViewModel.TableHeaders))
                    {
                        resultFormula[6] = "#input-limit";
                        isMultiplier = true;
                        isValidated = false;
                    }
                }
                if (isValidated)
                {
                    customTable.UserName = GetCurrentUserName();
                    paymentDetailId = PostApiResponse<long>(Constants.PaymentTypeCustomTable,
                           Convert.ToString(Enums.Action.AddEdit), customTable);
                }
            }
            return Json(new { success = isMultiplier, Id = paymentDetailId, documentId = paymentTypeCustomTableViewModel.DocumentId, resultFormula });
        }
    }
}
