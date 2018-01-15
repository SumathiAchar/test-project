using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;

namespace SSI.ContractManagement.Web.ActionFilters
{
    public class AjaxSessionActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            var response = filterContext.HttpContext.Response;
            
            var session = filterContext.HttpContext.Session;

            if (session != null && session[Constants.IsUserLoggedIn] == null)
            {
                if (request.IsAjaxRequest())
                {
                    response.StatusCode = Constants.AjaxStatusCode; 
                }
                else
                {
                    var url = new UrlHelper(filterContext.HttpContext.Request.RequestContext);
                    // ReSharper disable once AssignNullToNotNullAttribute
                    response.Redirect(url.Action("Login", "Account", new { area = "" }));
                }

                filterContext.Result = new EmptyResult();
            }

            base.OnActionExecuting(filterContext);
        }
    }
}