using System.Web.Mvc;
using System.Web.Routing;

// ReSharper disable once CheckNamespace
//This namespace comes under the start up application file.
namespace SSI.ContractManagement.Web.App_Start
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("Content/{*pathInfo}");
            routes.IgnoreRoute("Scripts/{*pathInfo}");
           
            routes.MapRoute("Default", "{controller}/{action}/{id}",
                new {controller = "ContractContainer", action = "HomeIndex", id = UrlParameter.Optional}
                );
        }
    }
}