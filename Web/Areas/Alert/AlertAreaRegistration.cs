using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Alert
{
    // ReSharper disable once UnusedMember.Global
    // This class is used to get Alert Area on application start up.
    public class AlertAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Alert";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Alert_default",
                "Alert/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
