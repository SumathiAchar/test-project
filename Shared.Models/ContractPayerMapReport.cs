using System.Collections.Generic;

namespace SSI.ContractManagement.Shared.Models
{
    public class ContractPayerMapReport : ModelingReport
    {
        public string StatementThrough { get; set; }

        public string BilledDate { get; set; }

        public string ReportType { get; set; }

        public List<ContractPayerMapReport> ContractPayerMapReports { get; set; }
        
        public short Priority { get; set; }
    }
}