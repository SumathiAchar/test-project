using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.Treeview
{
    // ReSharper disable once UnusedMember.Global
    // This class is used to get Tree view Area on application start up.
    public class TreeviewAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Treeview";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Treeview_default",
                "Treeview/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
