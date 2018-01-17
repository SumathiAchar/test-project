using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Contract
{
    // ReSharper disable once UnusedMember.Global
    // This class is used to get Contract Area on application start up.
    public class ContractAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contract";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Contract_default",
                "Contract/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
