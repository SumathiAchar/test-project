using System;
using System.Collections.Generic;
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    public class ReportInfoViewModel:BaseViewModel
    {
        public List<ClaimAdjudicationReportViewModel> ClaimAdjudicationReportList { get; set; }
        public long NodeId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRowCount { get; set; }
        public int DateType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ClaimSearchCriteria { get; set; }
        // ReSharper disable once UnusedMember.Global
        // This property is used in Report
        public int ReportType { get; set; }
    }
}