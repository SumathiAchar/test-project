using System.Collections.Generic;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Web.Areas.Contract.Models
{
    public class PaymentTypePerDiemViewModel : PaymentTypeBaseViewModel
    {
        public List<PerDiemSelection> PerDiemSelections { get; set; }
    }
}