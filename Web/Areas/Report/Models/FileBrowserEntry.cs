
using SSI.ContractManagement.Web.Areas.Common.Models;

namespace SSI.ContractManagement.Web.Areas.Report.Models
{
    /// <summary>
    /// Holds File metadata
    /// </summary>
    public class FileBrowserEntry
    {
        public string Name { get; set; }
        public EntryType Type { get; set; }
        public long Size { get; set; }
    }
}