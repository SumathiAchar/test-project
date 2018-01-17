using System.Web.Mvc;

namespace SSI.ContractManagement.Web.Areas.PaymentTable
{
    // ReSharper disable once UnusedMember.Global
    // This class is used to get Payment Table Area on application start up.
    public class PaymentTableAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PaymentTable";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PaymentTable_default",
                "PaymentTable/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
