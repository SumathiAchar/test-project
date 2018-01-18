using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;

namespace SSI.ContractManagement.Web.Report
{
    /// <summary>
    /// Summary description for ContractModeling.
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public partial class ContractModelingExcelView : Telerik.Reporting.Report
    {
        private readonly ModelingReportViewModel _modelingReportViewModels;

        /// <summary>
        /// Constructor which reveives modeling details
        /// </summary>
        /// <param name="modelingReportViewModels">modeling data of the model</param>
        public ContractModelingExcelView(ModelingReportViewModel modelingReportViewModels)
        {
            InitializeComponent();

            DataSource = modelingReportViewModels.ModelingReports;
            _modelingReportViewModels = modelingReportViewModels;
            Name = _modelingReportViewModels.IsActive
                ? string.Format("{0}({1})", Constants.ModelingReportFileBaseName, Constants.ModelingActiveReport)
                : string.Format("{0}({1})", Constants.ModelingReportFileBaseName, Constants.ModelingInActiveReport);
        }

        /// <summary>
        /// Describes the header section of the page
        /// </summary>
        /// <param name="sender">Object which sent the request</param>
        /// <param name="eventArgs">event argument</param>
        private void PageHeader_ItemDataBound(object sender, EventArgs eventArgs)
        {
            facilityName.Value = string.Format(Constants.FacilityName, _modelingReportViewModels.FacilityName);
            reportType.Value = string.Format("({0})", _modelingReportViewModels.IsActive ? Constants.ModelingActiveReport : Constants.ModelingInActiveReport);
            reportDate.Value = _modelingReportViewModels.CurrentDateTime;
            userName.Value = string.Format(Constants.LoggedInUser, _modelingReportViewModels.LoggedInUser);
        }

    }
}