using System.Collections.Generic;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class ContractPayerMapReportViewModel : ModelingReportViewModel
    {
        public string StatementThrough { get; set; }

        public string BilledDate { get; set; }

        public string ReportType { get; set; }

        public List<ContractPayerMapReportViewModel> ContractPayerMapReportViewModels { get; set; }

        // ReSharper disable once UnusedMember.Global
        // This property is used in Report
        public short Priority { get; set; }

        public new string CurrentDateTime { get; set; }
    }
}