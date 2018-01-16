using System.Web.SessionState;
using System.Web.Mvc;
using SSI.ContractManagement.Web.ActionFilters;


namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    [Authorize]
    [AjaxSessionActionFilter]
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class CommonController : BaseController
    {
    }
}