using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.ErrorLog;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.PaymentTable.Models;
using SSI.ContractManagement.Web.WebUtil;

namespace SSI.ContractManagement.Web.Areas.PaymentTable.Controllers
{
    public class PaymentTableController : CommonController
    {
        /// <summary>
        /// Payments the table.
        /// </summary>
        /// <param name="nodeId">The node identifier.</param>
        /// <returns></returns>
        public ActionResult PaymentTable(long? nodeId)
        {
            ViewBag.NodeId = nodeId;
            return View();
        }

        /// <summary>
        /// Saves the payment tables.
        /// </summary>
        /// <param name="formCollection">The form collection.</param>
        /// <returns></returns>
        //FIXED-JITEN-APRIL15 - Save action does lots of stuff. it does lots of validation, save. use different utility classes for validation 
        //FIXED-SEP15 Use actual variable name in place of var
       [HttpPost, ValidateInput(false)]
        public ActionResult Save(FormCollection formCollection)
        {
            string responseStatus = string.Empty;
            try
            {
                if (Request.Files != null && Request.Files.Count > 0)
                {
                    string tableName = formCollection[Constants.FormCollectionTableName];
                    long claimFieldId = formCollection[Constants.FormCollectionClaimFieldId] != string.Empty
                        ? long.Parse(formCollection[Constants.FormCollectionClaimFieldId])
                        : 0;
                    int facilityId = GetCurrentFacilityId();
                    HttpPostedFileBase httpPostedFileBase = Request.Files[Constants.FormCollectionImportTable];
                    if (httpPostedFileBase != null)
                    {
                        if (httpPostedFileBase.ContentLength > GlobalConfigVariable.PaymentTableFileSize)
                        {
                            responseStatus = string.Format(Constants.FileMaxSize,
                                GlobalConfigVariable.PaymentTableFileSize/(Constants.KiloBytes*Constants.KiloBytes));
                            return new ContentResult {Content = responseStatus};
                        }
                        string fileName = Path.GetFileName(httpPostedFileBase.FileName);
                        // store the file inside ~/App_Data/uploads folder
                        if (fileName != null)
                        {
                            //check for duplicate table name
                            ClaimFieldDoc newClaimFieldDoc = new ClaimFieldDoc
                            {
                                TableName = tableName,
                                FacilityId = facilityId
                            };

                            //Check table name is already exist in DB or not
                            if (PostApiResponse<bool>(Constants.PaymentTable, Constants.TableNameExists,
                                newClaimFieldDoc))
                            {
                                responseStatus = Constants.DuplicateTableNameMessage;
                            }
                            else
                            {
                                //Save file into local folder
                                string filePath =
                                    Path.Combine(Server.MapPath(GlobalConfigVariable.AppDataVirtualPath),
                                        fileName);
                                httpPostedFileBase.SaveAs(filePath);

                                responseStatus = ValidateAndSave(responseStatus, filePath, fileName, claimFieldId,
                                    tableName, facilityId);
                            }
                        }
                    }
                }
                else
                    responseStatus = Constants.DocumentError;
            }
            catch (Exception ex)
            {
                Log.LogError(string.Empty, string.Empty, ex);
                responseStatus = Constants.TableFailedToUpload;
            }
            //return error message
            return new ContentResult { Content = responseStatus };
        }

       /// <summary>
       /// Validates the and save.
       /// </summary>
       /// <param name="responseStatus">The response status.</param>
       /// <param name="filePath">The file path.</param>
       /// <param name="fileName">Name of the file.</param>
       /// <param name="claimFieldId">The claim field identifier.</param>
       /// <param name="tableName">Name of the table.</param>
       /// <param name="facilityId">The facility identifier.</param>
       /// <returns></returns>
        private string ValidateAndSave(string responseStatus, string filePath, string fileName, long claimFieldId,
            string tableName, int facilityId)
        {
            if (Path.GetExtension(fileName) != null)
            {
                // Validating the uploaded file
                // ReSharper disable once PossibleNullReferenceException
                //FIXED-SEP15 Create variable for Path.GetExtension(fileName).Remove(0, 1) and replace magic number 0,1 with constant.
                string fileExtension = Path.GetExtension(fileName).Remove(Constants.Zero, Constants.One);
                PaymentTableViewModel paymentTableViewModel = PaymentTableUtil.ValidateData(filePath, fileExtension,
                    claimFieldId);

                if (string.IsNullOrEmpty(paymentTableViewModel.Message))
                {
                    //Read claim fields details from imported file xls/xlsx/Csv
                    ClaimFieldDoc claimFieldDoc = PaymentTableUtil.GetPaymentTableClaimFields(tableName,
                        // ReSharper disable once PossibleNullReferenceException
                        fileName, fileExtension, claimFieldId, facilityId, paymentTableViewModel.CalimFieldValues, GetCurrentUserName());

                    //If imported table is not empty
                    if (claimFieldDoc.ClaimFieldValues.Count > 0)
                    {
                        //Save Document data into DB
                        responseStatus = AddClaimDocumentItemsByBatch(claimFieldDoc) > 0
                            ? Constants.TableSuccessfullyUploaded
                            : Constants.TableFailedToUpload;
                    }
                    else
                    {
                        responseStatus = Constants.EmptyFile;
                    }
                }
                else
                {
                    responseStatus = paymentTableViewModel.Message;
                }
            }
            return responseStatus;
        }

        /// <summary>
        /// Adds the claim document items by batch.
        /// </summary>
        /// <param name="claimFieldDoc">The claim field document.</param>
        /// <returns></returns>
        private long AddClaimDocumentItemsByBatch(ClaimFieldDoc claimFieldDoc)
        {
            long claimFieldDocId = 0;
            int iterations;
            int totalClaimFieldValues;
            int claimFieldValuesPerCall = Convert.ToInt32(GlobalConfigVariable.ClaimFieldValuesPerCall);

            foreach (ClaimFieldValue claimFieldValues in claimFieldDoc.ClaimFieldValues)
            {
                if (claimFieldValues.Value.Contains(Constants.Comma))
                {
                    string[] values = claimFieldValues.Value.Split('\n');
                    for (int count = 0; count < values.Length; count++)
                    {
                        values[count] = values[count].Trim();
                    }
                    claimFieldValues.Value = string.Join("\n", values);
                }
            }
            // Taking the max records count for insert custom payment
            int totalClaimFieldCustomRecords = Convert.ToInt32(GlobalConfigVariable.ClaimFieldValuesCustomMaxRecords);
            // If we are uploading CustomPaymentType
            if (claimFieldDoc.ClaimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType)
            {
                // Taking the max length of varchar characters to insert 
                claimFieldValuesPerCall = Convert.ToInt32(GlobalConfigVariable.ClaimFieldValuesCustomLength);

                // Getting the total length of comma separated string 
                int totalClaimFieldLength = claimFieldDoc.ClaimFieldValues[0].Value.Length;
                int remainingClaimFieldLength = totalClaimFieldLength;
                if (totalClaimFieldLength > claimFieldValuesPerCall)
                {
                    // Dividing the total string to every claimFieldValuesPerCall length 
                    List<ClaimFieldValue> claimFieldValueList = new List<ClaimFieldValue>();
                    for (int count = 0; count < totalClaimFieldLength; count = count + claimFieldValuesPerCall)
                    {
                        claimFieldValueList.Add(
                            new ClaimFieldValue
                            {
                                Value =
                                    claimFieldDoc.ClaimFieldValues[0].Value.Substring(count,
                                        remainingClaimFieldLength >= claimFieldValuesPerCall
                                            ? claimFieldValuesPerCall
                                            : remainingClaimFieldLength)
                            });
                        remainingClaimFieldLength = remainingClaimFieldLength - claimFieldValuesPerCall;
                    }
                    claimFieldDoc.ClaimFieldValues = claimFieldValueList;
                }
                totalClaimFieldValues = claimFieldDoc.ClaimFieldValues.Count;
                // Calculating the total loops
                iterations = totalClaimFieldValues % totalClaimFieldCustomRecords == 0
                   ? totalClaimFieldValues / totalClaimFieldCustomRecords
                   : totalClaimFieldValues / totalClaimFieldCustomRecords + 1;
            }
            // Other than custom payment
            else
            {
                totalClaimFieldValues = claimFieldDoc.ClaimFieldValues.Count;
                iterations = totalClaimFieldValues % claimFieldValuesPerCall == 0
                   ? totalClaimFieldValues / claimFieldValuesPerCall
                   : totalClaimFieldValues / claimFieldValuesPerCall + 1;
            }

            if (totalClaimFieldValues > claimFieldValuesPerCall || claimFieldDoc.ClaimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType)
            {
                for (int iteration = 0; iteration < iterations; iteration++)
                {
                    // Taking some of the records to insert
                    List<ClaimFieldValue> claimFieldValues =
                        claimFieldDoc.ClaimFieldValues.Skip((
                            claimFieldDoc.ClaimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                            ? totalClaimFieldCustomRecords
                            : claimFieldValuesPerCall) * iteration)
                            .Take((claimFieldDoc.ClaimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType
                            ? totalClaimFieldCustomRecords
                            : claimFieldValuesPerCall))
                            .ToList();

                    ClaimFieldDoc tempraryDoc = new ClaimFieldDoc
                    {
                        ColumnHeaderFirst = claimFieldDoc.ColumnHeaderFirst,
                        ColumnHeaderSecond = claimFieldDoc.ColumnHeaderSecond,
                        FileName = claimFieldDoc.FileName,
                        TableName = claimFieldDoc.TableName,
                        ClaimFieldId = claimFieldDoc.ClaimFieldId,
                        NodeId = claimFieldDoc.NodeId,
                        ClaimFieldValues = claimFieldValues,
                        UserName = GetCurrentUserName(),
                        FacilityId = claimFieldDoc.FacilityId
                    };

                    claimFieldDocId = PostApiResponse<long>(Constants.ClaimFieldDoc, Convert.ToString(Enums.Action.AddClaimFieldDocs), tempraryDoc);

                    //If any error occur than no need to save remaining data  - DB side all saved data will be deleted if exception will come.
                    if (claimFieldDocId == 0)
                        break;
                }
            }
            else
            {
                claimFieldDocId = PostApiResponse<long>(Constants.ClaimFieldDoc, Convert.ToString(Enums.Action.AddClaimFieldDocs), claimFieldDoc);
            }
            return claimFieldDocId;
        }

        //FIXED-JITEN MAY15 - Use ClaimFieldController GetClaimFieldsByModule action. Pass moduleid from UI. Create module specific master data in DB
        
        //FIXED-JITEN MAY15 - Use ClaimFieldController GetClaimFieldsByModule action. Pass moduleid from UI. Create module specific master data in DB
        
        /// <summary>
        /// Gets the name of the m care lab fee schedule table.
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMedicareLabFeeScheduleTableNames(string userText)
        {
            MedicareLabFeeSchedule mCareFeeSchedule = new MedicareLabFeeSchedule
            {
                UserText = userText
            };
            List<MedicareLabFeeSchedule> mCareLabFeeSchedules =
                PostApiResponse<List<MedicareLabFeeSchedule>>(Constants.MedicareLabFeeSchedule,
                    Constants.GetMedicareLabFeeScheduleTableNames, mCareFeeSchedule);
            return Json(mCareLabFeeSchedules, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Gets the m care lab fee schedule table data.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultSortField">The default sort field.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMedicareLabFeeSchedule(int date, int take, int skip, IEnumerable<Sort> sort,
            Kendo.DynamicLinq.Filter filter, string defaultSortField)
        {
            PageSetting pageSetting = CommonUtil.GetPageSetting(take, skip, sort, filter, defaultSortField,
                Constants.MedicareLabFeeScheduleFields, (long)Enums.ClaimFieldTypes.MedicareLabFeeSchedule);
            UserInfo userInfoViewModel = GetUserInfo();
            if (userInfoViewModel != null && userInfoViewModel.AssignedFacilities!=null && userInfoViewModel.AssignedFacilities.Any())
            {
                MedicareLabFeeSchedule mCareLabFeeSchedule = new MedicareLabFeeSchedule
                {
                    Date = date,
                    PageSetting = pageSetting,
                    UserName = userInfoViewModel.UserName,
                    FacilityId = GetCurrentFacilityId()
                };
                MedicareLabFeeScheduleResult medicareLabFeeScheduleResult =
                    PostApiResponse<MedicareLabFeeScheduleResult>(Constants.MedicareLabFeeSchedule,
                        Constants.GetMedicareLabFeeSchedule,
                        mCareLabFeeSchedule);
                if (medicareLabFeeScheduleResult != null)
                {
                    DataSourceResult dataSourceResult = new DataSourceResult
                    {
                        Total = medicareLabFeeScheduleResult.Total,
                        Data = medicareLabFeeScheduleResult.MedicareLabFeeScheduleList
                    };
                    return Json(dataSourceResult);
                }
            }
            return Json(null);
        }

        /// <summary>
        /// Gets the payment table.
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document identifier.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultSortField">The default sort field.</param>
        /// <param name="claimFieldId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetPaymentTable(long claimFieldDocId, int take, int skip, IEnumerable<Sort> sort,
            Kendo.DynamicLinq.Filter filter, string defaultSortField, long claimFieldId)
        {
            PageSetting pageSetting = CommonUtil.GetPageSetting(take, skip, sort, filter, defaultSortField,
                Constants.PaymentTableFields, claimFieldId);

            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc
            {
                ClaimFieldDocId = claimFieldDocId,
                PageSetting = pageSetting,
                ClaimFieldId = claimFieldId,
                UserName = GetCurrentUserName(),
                SessionTimeOut = Convert.ToInt32(GlobalConfigVariable.CommandTimeout)
            };

            PaymentTableContainer paymentTableContainer = PostApiResponse<PaymentTableContainer>(Constants.PaymentTable,
                Constants.GetPaymentTable, claimFieldDoc);

            if (paymentTableContainer != null)
            {
                return Json(new DataSourceResult
                {
                    Total = paymentTableContainer.Total,
                    Data = paymentTableContainer.ClaimFieldValues
                });
            }
            return Json(null);
        }

        /// <summary>
        /// Deletes the specified claim field document identifier.
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(long claimFieldDocId)
        {
            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc
            {
                ClaimFieldDocId = claimFieldDocId,
                UserName =GetCurrentUserName()
            };
            //FIXED-SEP15 use Enum for method -- "Delete"
            return Json(new { success = PostApiResponse<bool>(Constants.ClaimFieldDoc, Convert.ToString(Enums.Action.Delete), claimFieldDoc) });
        }


        /// <summary>
        /// Determines whether [is document in use] [the specified claim field document identifier].
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult IsDocumentInUse(long claimFieldDocId)
        {
            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc
            {
                ClaimFieldDocId = claimFieldDocId,
                UserName = GetCurrentUserName()
            };
            return Json(new { contractDetails = PostApiResponse<List<ContractLog>>(Constants.ClaimFieldDoc, Constants.IsDocumentInUse, claimFieldDoc) });
        }

        /// <summary>
        /// Rename the Payment table.
        /// </summary>
        /// <returns></returns>
        public ActionResult RenamePaymentTable()
        {
            return View();
        }

        /// <summary>
        /// Rename Payment Table.
        /// </summary>
        /// <param name="claimFieldDocId">The claim field document.</param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RenamePaymentTable(long claimFieldDocId, string tableName)
        {
            ClaimFieldDoc claimFieldDoc = new ClaimFieldDoc
            {
                ClaimFieldDocId = claimFieldDocId,
                TableName = tableName,
                UserName = GetCurrentUserName(),
                FacilityId = GetCurrentFacilityId()
            };
            ClaimFieldDoc claimFieldDocs = PostApiResponse<ClaimFieldDoc>(Constants.ClaimFieldDoc,
                Constants.RenamePaymentTable, claimFieldDoc);
            return Json(claimFieldDocs);
        }
    }
}
