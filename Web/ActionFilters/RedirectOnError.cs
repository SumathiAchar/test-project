using System.Web.Mvc;
using System.Web.Routing;


namespace SSI.ContractManagement.Web.ActionFilters
{
    /// <summary>
    /// Handle Antiforgery token exception and redirect to customer area if the user is already authenticated
    /// </summary>
    public class RedirectOnError : HandleErrorAttribute
    {
        //ToDo :This method will handle AntiForgery Exception which gets triggered while working on multiple tabs in a browser. 
        //ToDo: This might violate antiForgery security feature. Once appropriate fix is found this method needs to be removed.
        /// <summary>
        /// Override the on exception method and check if the user is authenticated and redirect the user 
        /// to the customer service index otherwise continue with the base implamentation
        /// </summary>
        /// <param name="filterContext">Current Exception Context of the request</param>
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is HttpAntiForgeryException && filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                // Set response code back to normal
                filterContext.HttpContext.Response.StatusCode = 200;

                // Handle the exception
                filterContext.ExceptionHandled = true;

                UrlHelper urlH = new UrlHelper(filterContext.HttpContext.Request.RequestContext);

                // Create a new request context
                RequestContext rc = new RequestContext(filterContext.HttpContext, filterContext.RouteData);

                // Create a new return url
                var virtualPathData = RouteTable.Routes.GetVirtualPath(rc, new RouteValueDictionary(new { Controller = "Account", action = "Login" }));
                if (virtualPathData != null)
                {
                    string url = virtualPathData.VirtualPath;

                    // Check if there is a request url
                    if (filterContext.HttpContext.Request.Params["ReturnUrl"] != null && urlH.IsLocalUrl(filterContext.HttpContext.Request.Params["ReturnUrl"]))
                    {
                        url = filterContext.HttpContext.Request.Params["ReturnUrl"];
                    }

                    // Redirect the user back to the customer service index page
                    filterContext.HttpContext.Response.Redirect(url, true);
                }
            }
            else
            {
                // Continue to the base
                base.OnException(filterContext);
            }
        }
    }
}