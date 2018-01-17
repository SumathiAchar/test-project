using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;

namespace SSI.ContractManagement.Web.Areas.PaymentTable.Controllers
{
    public class TableBaseController : CommonController
    {
        /// <summary>
        /// Get TableName Selection basing on claimFieldId & contractId
        /// </summary>
        public JsonResult GetTableNames(long paymentTypeId, string userText)
        {
            PaymentTypeAscFeeSchedule modelPaymentTypeAscFeeSchedule = new PaymentTypeAscFeeSchedule
            {
                FacilityId = GetCurrentFacilityId(),
                UserName = GetCurrentUserName(),
                UserText = userText
            };

            switch ((Enums.PaymentTypeCodes)paymentTypeId)
            {
                case Enums.PaymentTypeCodes.AscFeeSchedule:
                    modelPaymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.AscFeeSchedule).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.DrgPayment:
                    modelPaymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.DrgWeightTable).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.FeeSchedule:
                    modelPaymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.FeeSchedule).FieldIdentityNumber;
                    break;
                case Enums.PaymentTypeCodes.CustomTableFormulas:
                    modelPaymentTypeAscFeeSchedule.ClaimFieldId =
                        EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.CustomPaymentType)
                            .FieldIdentityNumber;
                    break;
                default:
                    modelPaymentTypeAscFeeSchedule.ClaimFieldId = EnumHelperLibrary.GetFieldInfoFromEnum(Enums.ClaimFieldTypes.None).FieldIdentityNumber;
                    break;
            }
            
            List<PaymentTypeTableSelection> paymentTypeTableSelection = PostApiResponse<List<PaymentTypeTableSelection>>("PaymentTypeASCFeeSchedule", "GetTableNameSelection", modelPaymentTypeAscFeeSchedule);
            List<SelectListItem> paymentTableSelection = new List<SelectListItem>();
            if (paymentTypeTableSelection != null && paymentTypeTableSelection.Count > 0)
            {
                paymentTableSelection.AddRange(paymentTypeTableSelection.Select(item => new SelectListItem { Text = item.TableName, Value = item.ClaimFieldDocId.ToString(CultureInfo.InvariantCulture) }));
            }
            return Json(paymentTableSelection, JsonRequestBehavior.AllowGet);
            
        }
    }
}