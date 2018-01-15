using System.Web.Optimization;

namespace SSI.ContractManagement.Web
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // No need to Instantiated
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //scripts
            bundles.Add(new ScriptBundle("~/Content/js/secondscript")
                .Include("~/Content/js/jquery.min.js",
                    "~/Content/js/kendo/kendo.all.min.js",
                    "~/Content/js/kendo/kendo.menu.ex.js",
                    "~/Content/js/jquery.jeditable.js",
                    "~/Content/js/login.js",
                    "~/Content/js/Menubinding.js",
                    "~/Content/js/Common.js",
                    "~/Content/js/CustomAlert.js",
                    "~/Content/js/treeview.js",
                    "~/Content/js/jquery.scrollTo-min.js",
                    "~/Content/js/ServiceLineAutomationTool.js",
                    "~/Content/js/PaymentTypeAutomationTool.js",
                    "~/Content/js/EditServiceLine.js",
                    "~/Content/js/EditPaymentType.js",
                    "~/Content/js/DeleteFilters.js",
                    "~/Content/js/tinyshort.js",
                    "~/Content/js/AjaxGlobalHandler.js",
                    "~/Content/js/placeholders.min.js",
                    "~/Content/js/FileSaver.js",
                    "~/Content/js/jquery.wordexport.js"
                ));

            bundles.Add(new ScriptBundle("~/Scripts/mainscript")
                .Include("~/Scripts/jquery-1.7.1.min.js",
                    "~/Scripts/jquery-ui-1.8.20.js",
                    "~/Scripts/modernizr-2.5.3.js",
                    "~/Scripts/idle-timer.min.js",
                    "~/Scripts/AjaxLogin.js",
                    "~/Scripts/cookies.min.js"
                ));

            //styles
            bundles.Add(new StyleBundle("~/Content/css/cmsstyle").Include(
                "~/Content/css/stylesheet.css"
                ));

            bundles.Add(new StyleBundle("~/Content/styles/kendostyle")
                .Include("~/Content/styles/kendo.common.min.css",
                    "~/Content/styles/kendo.menu.ex.css",
                    "~/Content/styles/kendo.default.min.css"
                ));

            bundles.Add(new StyleBundle("~/Content/themes/base/basestyle").Include(
                "~/Content/themes/base/jquery.ui.core.css",
                "~/Content/themes/base/jquery.ui.resizable.css",
                "~/Content/themes/base/jquery.ui.selectable.css",
                "~/Content/themes/base/jquery.ui.accordion.css",
                "~/Content/themes/base/jquery.ui.autocomplete.css",
                "~/Content/themes/base/jquery.ui.button.css",
                "~/Content/themes/base/jquery.ui.dialog.css",
                "~/Content/themes/base/jquery.ui.slider.css",
                "~/Content/themes/base/jquery.ui.tabs.css",
                "~/Content/themes/base/jquery.ui.datepicker.css",
                "~/Content/themes/base/jquery.ui.progressbar.css",
                "~/Content/themes/base/jquery.ui.theme.css"
            ));

            //
            bundles.Add(new ScriptBundle("~/Content/js/accountsettingscriptandstyle")
              .Include("~/Content/js/jquery.min.js",
                        "~/Content/js/kendo/kendo.all.min.js",
                        "~/Content/js/kendo/kendo.menu.ex.js",
                        "~/Content/js/jquery.jeditable.js",
                        "~/Content/js/Menubinding.js",
                        "~/Content/js/Common.js",
                        "~/Content/js/CustomAlert.js",
                        "~/Content/js/jquery.scrollTo-min.js",
                        "~/Content/js/tinyshort.js",
                        "~/Content/js/AjaxGlobalHandler.js",
                        "~/Content/js/placeholders.min.js"
              ));
        }
    }
}