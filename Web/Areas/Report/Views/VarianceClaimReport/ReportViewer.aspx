<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.Mvc.ViewPage<SSI.ContractManagement.Web.Areas.Report.Models.VarianceReportViewModel>" %>

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
            var instanceReportSource = new Telerik.Reporting.InstanceReportSource { ReportDocument = new ClaimVarianceDetails(Model) };

            primaryreportviewer.ReportSource = instanceReportSource;
            primaryreportviewer.RefreshReport();
        }
    </script>

    <form id="main" method="post" action="">
        <telerik:ReportViewer ID="primaryreportviewer" runat="server" Width="100%" Height="570px"></telerik:ReportViewer>
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
