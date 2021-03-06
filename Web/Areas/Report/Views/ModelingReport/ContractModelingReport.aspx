<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<SSI.ContractManagement.Web.Areas.Report.Models.ModelingReportViewModel>" %>

<%@ Import Namespace="SSI.ContractManagement.Web.Report" %>
<%@ Register TagPrefix="telerik" Assembly="Telerik.ReportViewer.WebForms" Namespace="Telerik.ReportViewer.WebForms" %>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>Report</title>
</head>
<body>
    <script runat="server">
        public override void VerifyRenderingInServerForm(Control control)
        {
            // to avoid the server form (<form runat="server"> requirement
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var instanceReportSource = new Telerik.Reporting.InstanceReportSource { ReportDocument = new ContractModeling(Model) };

            primaryreportviewer.ReportSource = instanceReportSource;
            primaryreportviewer.RefreshReport();
        }

    </script>
    <form id="" method="post" action="">
        <telerik:ReportViewer ID="primaryreportviewer" Width="100%" Height="570px" runat="server">
        </telerik:ReportViewer>
    </form>
</body>
</html>
<style>
    html, body, #main, #primaryreportviewer
    {
        width:100%;
        height:100%;
    }
</style>
