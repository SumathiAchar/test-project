using AutoMapper;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;
using SSI.ContractManagement.Web.Areas.Common.Controllers;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.Report.Models;
using SSI.ContractManagement.Web.WebUtil;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace SSI.ContractManagement.Web.Areas.Adjudication.Controllers
{
    public class JobDataController : SessionStoreController
    {

        /// <summary>
        /// Jobs the status.
        /// </summary>
        /// <returns></returns>
        public ActionResult JobStatus()
        {
            return View(Constants.Jobstatus);
        }

        /// <summary>
        /// Opens the claim grid view.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="zoneDateTime"></param>
        /// <returns></returns>
        public ActionResult OpenClaimGridView(string selectionCriteria, DateTime? dateFrom, DateTime? dateTo, long? modelId, int? dateType, string zoneDateTime)
        {
            StringBuilder requestCriteria = new StringBuilder();
            requestCriteria.Append(Constants.AdjudicationRequestCriteria);
            requestCriteria.Append(Convert.ToByte(Enum.Parse(typeof(Enums.ClaimFieldOperator), Enums.ClaimFieldOperator.GreaterThan.ToString())));
            requestCriteria.Append("|");
            requestCriteria.Append(selectionCriteria);
            ViewBag.SelectionCriteria = requestCriteria.ToString();

            if (dateFrom == DateTime.MinValue ||
                     dateTo == DateTime.MinValue || dateFrom == null ||
                                                                  dateTo == null)
            {
                dateTo = DateTime.Now;
                dateFrom =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
            }

            ViewBag.dateFrom = Convert.ToDateTime(dateFrom);
            ViewBag.dateTo = Convert.ToDateTime(dateTo);
            ViewBag.modelId = modelId;
            ViewBag.dateType = dateType;
            ViewBag.zoneDateTime = zoneDateTime;
            ViewBag.isSelectClaims = false;
            return View();
        }

        /// <summary>
        /// Gets the job status.
        /// </summary>
        /// <returns></returns>
        public ActionResult GetJobStatus()
        {
            List<EnumHelper> jobStatus = EnumHelperLibrary.GetFieldInfoFromEnum<Enums.JobStatus>();

            if (jobStatus != null && jobStatus.Any())
            {

                jobStatus.RemoveAll(
                    a =>
                        a.FieldIdentityNumber ==
                        Convert.ToInt16(Enum.Parse(typeof(Enums.JobStatus), Enums.JobStatus.Cancelled.ToString())) ||
                        a.FieldIdentityNumber ==
                        Convert.ToInt16(Enum.Parse(typeof(Enums.JobStatus), Enums.JobStatus.Debug.ToString())));
            }
            return Json(new { jobStatus }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieves all available jobs
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllJobs(string jobStatusId, int take, int skip)
        {
            PageSetting pageSetting = new PageSetting { Skip = skip, Take = take };
            // bool isVerified = false;
            TrackTask data = new TrackTask
            {
                PageSetting = pageSetting,
                UserName = GetCurrentUserName(),
                FacilityId = GetCurrentFacilityId(),
                ThresholdDaysToExpireJobs = GlobalConfigVariables.NumberOfDaysToDismissCompletedJobs,
                Status = jobStatusId
            };

            List<TrackTask> jobsList = PostApiResponse<List<TrackTask>>(Constants.JobsData, Constants.GetAllJobs, data);
            List<JobStatusViewModel> jobs = new List<JobStatusViewModel>();
            if (jobsList != null && jobsList.Count > 0)
            {
                jobs = jobsList.Select(Mapper.Map<TrackTask, JobStatusViewModel>).ToList();
            }
            var jobStatusViewModel = jobs.FirstOrDefault();
            return Json(jobStatusViewModel != null ? new { data = jobs, total = jobStatusViewModel.TotalJobs } : new { data = jobs, total = 0 });
        }

        /// <summary>
        /// Updates Job status
        /// </summary>
        /// <param name="taskid"></param>
        /// <param name="status"></param>
        public ActionResult UpdateJobStatus(string taskid, string status)
        {
            TrackTask job = new TrackTask { TaskId = taskid, Status = status, UserName = GetCurrentUserName() };
            int jobsstatus = PostApiResponse<int>(Constants.JobsData, Constants.UpdateJobStatus, job);
            return Json(jobsstatus == 1 ? new { IsSuccess = true } : new { IsSuccess = false });
        }

        /// <summary>
        /// Updates Job status
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllJobsAlertCount()
        {
            int facilityId = GetCurrentFacilityId();
            if (facilityId != 0)
            {
                //FIXED-FEB16 Use Object initializer
                TrackTask data = new TrackTask
                {
                    FacilityId = facilityId,
                    UserName = GetCurrentUserName(),
                    ThresholdDaysToExpireJobs = GlobalConfigVariables.NumberOfDaysToDismissCompletedJobs
                };
                int jobsstatus = PostApiResponse<int>(Constants.JobsData, Constants.JobCountAlert, data);
                int contractAlertCount = ContractAlertCount(facilityId);
                return Json(new { data = string.Format("{0}-{1}", jobsstatus, contractAlertCount) },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { data = string.Format("{0}-{1}", facilityId, facilityId) },
                   JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the jobs count.
        /// </summary>
        /// <param name="jobUpdateId">The job update identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateJobsCount(int jobUpdateId)
        {
            TrackTask job = new TrackTask { TaskId = jobUpdateId.ToString(CultureInfo.InvariantCulture) };
            bool jobsstatus = PostApiResponse<bool>(Constants.JobsData, Constants.UpdateJobVerifiedStatus, job);
            return Json(new { data = jobsstatus });
        }

        /// <summary>
        /// Gets all open claim.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="dateFrom">The date from.</param>
        /// <param name="dateTo">The date to.</param>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="dateType">Type of the date.</param>
        /// <param name="zoneDateTime"></param>
        /// <param name="isSelectClaims"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllOpenClaim(string selectionCriteria, int take, int skip, IEnumerable<Sort> sort, Kendo.DynamicLinq.Filter filter, DateTime? dateFrom, DateTime? dateTo, long? modelId, int? dateType, string zoneDateTime, bool isSelectClaims)
        {
            PageSetting pageSetting = CommonUtil.GetPageSettingClaim(take, skip, sort, filter, Constants.OpenClaimFields, modelId);
            var currentUserName = GetCurrentUserName();
            var userGuid = GetUserKey();//user key replaced
            if (dateFrom == DateTime.MinValue ||
                     dateTo == DateTime.MinValue || dateFrom == null ||
                                                                  dateTo == null)
            {
                dateTo = DateTime.Now;
                dateFrom =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                if (!string.IsNullOrEmpty(selectionCriteria) &&
                     selectionCriteria.Contains(Constants.AdjudicationRequestCriteria))
                    dateType = -1;
            }
            ClaimAdjudicationReport adjudicationReportData = new ClaimAdjudicationReport
            {
                ClaimSearchCriteria = selectionCriteria,
                UserName = GetLoggedInUserName(),
                RequestedUserId = string.IsNullOrEmpty(currentUserName) ? Guid.Empty.ToString() : userGuid,
                RequestedUserName = currentUserName,
                MaxLinesForPdfReport = GlobalConfigVariable.MaxPagesForTelerikReport * GlobalConfigVariable.MaxNumberOfLinesInPdfPage,
                Take = take,
                Skip = skip,
                DateType = Convert.ToInt32(dateType),
                StartDate = Convert.ToDateTime(dateFrom),
                EndDate = Convert.ToDateTime(dateTo),
                ModelId = Convert.ToInt64(modelId),
                CurrentDateTime = zoneDateTime,
                IsSelectClaims = isSelectClaims,
                UserId = GetUserInfo().UserId,
                PageSetting = pageSetting
            };

            ClaimAdjudicationReport adjudicationReportInfo = PostApiResponse<ClaimAdjudicationReport>(Constants.ClaimAdjudicationReport, Constants.GetSelectedClaim, adjudicationReportData);
            ClaimAdjudicationReportViewModel adjudicationReportList =
                Mapper.Map<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>(adjudicationReportInfo);

            //Need to get claim-id as string. If claim-id is exceeding 16 digit then it was not showing correctly. IF we will remove then it will not display properly into open-claims grid
            foreach (ClaimDataViewModel claim in adjudicationReportList.ClaimData)
                claim.ClaimIdValue = claim.ClaimId.ToString(CultureInfo.InvariantCulture);

            adjudicationReportList.LoggedInUser = currentUserName;
            if (adjudicationReportList.ReportThreshold == Constants.ReportThreshold)
            {
                var data = adjudicationReportList.ReportThreshold;
                return Json(new { total = data });
            }

            DataSourceResult claimAdjudicationReportResult = new DataSourceResult
            {
                Data = adjudicationReportList.ClaimData,
                Total = adjudicationReportInfo.TotalRecords
            };
            return Json(new { data = claimAdjudicationReportResult, total = adjudicationReportList.ReportThreshold });
        }

        /// <summary>
        /// Downloads the claims.
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="selectionCriteria"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <param name="modelId"></param>
        /// <param name="dateType"></param>
        /// <param name="currentDateTime"></param>
        /// <param name="isSelectClaims"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DownloadClaims(IEnumerable<Sort> sort, Kendo.DynamicLinq.Filter filter, string selectionCriteria, DateTime? dateFrom, DateTime? dateTo, long? modelId, int? dateType, string currentDateTime, bool isSelectClaims)
        {
            PageSetting pageSetting = CommonUtil.GetPageSettingClaim(10, -1, sort, filter, Constants.OpenClaimFields, modelId);
            var currentuserName = GetCurrentUserName();
            var userGuId = GetUserKey();//replacing for user keys
            if (dateFrom == DateTime.MinValue ||
                    dateTo == DateTime.MinValue || dateFrom == null ||
                                                                 dateTo == null)
            {
                dateTo = DateTime.Now;
                dateFrom =
                    DateTime.Now.AddYears(-GlobalConfigVariable.PullDataForNumberOfYears);
                if (!string.IsNullOrEmpty(selectionCriteria) &&
                    selectionCriteria.Contains(Constants.AdjudicationRequestCriteria))
                dateType = -1;

            }
            ClaimAdjudicationReport adjudicationReport = new ClaimAdjudicationReport
            {
                ClaimSearchCriteria = selectionCriteria,
                UserName = GetLoggedInUserName(),
                RequestedUserId = string.IsNullOrEmpty(currentuserName) ? Guid.Empty.ToString() : userGuId,
                RequestedUserName = currentuserName,
                MaxLinesForPdfReport = GlobalConfigVariable.MaxPagesForTelerikReport * GlobalConfigVariable.MaxNumberOfLinesInPdfPage,
                StartDate = Convert.ToDateTime(dateFrom),
                EndDate = Convert.ToDateTime(dateTo),
                ModelId = Convert.ToInt64(modelId),
                DateType = Convert.ToInt32(dateType),
                CurrentDateTime = currentDateTime,
                IsSelectClaims = isSelectClaims,
                UserId = GetUserInfo().UserId,
                PageSetting = pageSetting
            };


            ClaimAdjudicationReport claimAdjudicationReport = PostApiResponse<ClaimAdjudicationReport>(Constants.ClaimAdjudicationReport, Constants.GetSelectedClaim, adjudicationReport);
            ClaimAdjudicationReportViewModel claimAdjudicationReportList =
                Mapper.Map<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>(claimAdjudicationReport);
            claimAdjudicationReportList.LoggedInUser = currentuserName;

            string fileName = new ClaimAdjudicationReportUtil().GetExportedFileName(Enums.DownloadFileType.Xls,
                claimAdjudicationReportList, GlobalConfigVariable.ReportsFilePath, currentDateTime);

            return Json(fileName);
        }

        /// <summary>
        /// Download the report for a requested format
        /// </summary>
        /// <param name="reportFileName">Holds report file name</param>
        /// <param name="currentDateTime"></param>
        /// <returns>files result. is used to download the file</returns>
        public ActionResult DownloadReport(string reportFileName, string currentDateTime)
        {
            string fileName;
            string contentType;
            string filePath = ReportUtility.GetReportFileDetailsToDownload(Constants.ClaimVarianceReportFileBaseName, reportFileName, out fileName, Enums.DownloadFileType.Csv, out contentType, currentDateTime);
            return File(filePath, contentType, fileName);
        }

        /// <summary>
        /// Determines whether [is model exist] [the specified model identifier].
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        public ActionResult IsModelExist(long modelId)
        {
            TrackTask job = new TrackTask { ModelId = modelId };
            return Json(new { IsSuccess = PostApiResponse<bool>(Constants.JobsData, Constants.IsModelExist, job) });
        }

        /// <summary>
        /// Res the adjudicate.
        /// </summary>
        /// <param name="taskId">The task identifier.</param>
        /// <returns></returns>
        public ActionResult ReAdjudicate(string taskId)
        {
            return Json(new { returnValue = PostApiResponse<long>(Constants.JobsData, Constants.ReAdjudicate, new TrackTask { TaskId = taskId, UserName = GetCurrentUserName() }) });
        }

        public int ContractAlertCount(int currentFacilityId)
        {
            ContractAlert alertCount = new ContractAlert
            {
                NumberOfDaysToDismissAlerts = GlobalConfigVariables.NumberOfDaysToDismissAlerts,
                FacilityId = currentFacilityId
            };
            int totalAlertCount = PostApiResponse<int>(Constants.ContractAlert, Constants.ContractAlertCount, alertCount);
            return totalAlertCount;
        }

        /// <summary>
        /// Gets Open Claim Columns By UserId
        /// </summary>
        /// <param name="claimColumnOptions">The claimColumnOptions.</param>
        /// <returns></returns>
        public ActionResult OpenClaimColumnOption(ClaimColumnOptionsViewModel claimColumnOptions)
        {
            ClaimColumnOptions claimColumns = PostApiResponse<ClaimColumnOptions>(Constants.JobsData, Constants.GetOpenClaimColumnOptionByUserId, claimColumnOptions);

            ClaimColumnOptionsViewModel claimColumnOptionsInfo = Mapper.Map<ClaimColumnOptions, ClaimColumnOptionsViewModel>(claimColumns);
            CommonUtil.ReplaceOpenClaimColumName(claimColumnOptionsInfo);

            return View(claimColumnOptionsInfo);
        }

        /// <summary>
        /// Saves the available columns.
        /// </summary>
        /// <param name="claimColumnOptionIds">The selected fields.</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveClaimColumnOptions(string claimColumnOptionIds, int userId)
        {
            ClaimColumnOptions claimColumnsInfo = new ClaimColumnOptions { ClaimColumnOptionIds = claimColumnOptionIds, UserId = userId };
            bool claimColumnsSuccess = PostApiResponse<bool>(Constants.JobsData, Constants.SaveClaimColumnOptions, claimColumnsInfo);
            return Json(claimColumnsSuccess);
        }

        /// <summary>
        /// Get Column Names based on UserId
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOpenClaimColumnNamesBasedOnUserId()
        {
            ClaimAdjudicationReport adjudicationReportData = new ClaimAdjudicationReport
            {
                UserId = GetUserInfo().UserId
            };

            ClaimAdjudicationReport adjudicationReportInfo = PostApiResponse<ClaimAdjudicationReport>(Constants.ClaimAdjudicationReport,
                Constants.GetOpenClaimColumnNamesBasedOnUserId, adjudicationReportData);

            ClaimAdjudicationReportViewModel adjudicationReportList =
                Mapper.Map<ClaimAdjudicationReport, ClaimAdjudicationReportViewModel>(adjudicationReportInfo);

            return Json(new { columnNames = adjudicationReportList.ColumnNames }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the automatic refresh.
        /// </summary>
        /// <param name="isAutoRefresh">if set to <c>true</c> [automatic refresh].</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult UpdateAutoRefresh(bool isAutoRefresh)
        {
            SetAutoRefresh(isAutoRefresh);
            return Json(isAutoRefresh);
        }
    }
}
