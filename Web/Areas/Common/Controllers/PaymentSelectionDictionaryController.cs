using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    //TODO: We can delete this later if we are not going to use this
    public class PaymentSelectionDictionaryController : Controller
    {
        /// <summary>
        /// Payment selection dictionary
        /// </summary>
        /// <returns></returns>
        public ActionResult PaymentSelectionDictionary()
        {
            return View();
        }

    }
}
