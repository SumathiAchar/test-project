using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;

namespace SSI.ContractManagement.Web.Report
{
    /// <summary>
    /// Summary description for ContractModeling.
    /// </summary>
    public partial class PayerMapping : Telerik.Reporting.Report
    {
        private readonly ContractPayerMapReportViewModel _contractPayerMapReportViewModel;

        /// <summary>
        /// Constructor which reveives modeling details
        /// </summary>
        /// <param name="contractPayerMapReportViewModel">modeling data of the model</param>
        public PayerMapping(ContractPayerMapReportViewModel contractPayerMapReportViewModel)
        {
            InitializeComponent();

            DataSource = contractPayerMapReportViewModel.ContractPayerMapReportViewModels;
            _contractPayerMapReportViewModel = contractPayerMapReportViewModel;
        }

        /// <summary>
        /// Describes the header section of the page
        /// </summary>
        /// <param name="sender">Object which sent the request</param>
        /// <param name="eventArgs">event argument</param>
        private void PageHeader_ItemDataBound(object sender, EventArgs eventArgs)
        {
            facilityName.Value = string.Format(Constants.FacilityName, _contractPayerMapReportViewModel.FacilityName);
            reportType.Value = string.Format("{0}{1}{2}", _contractPayerMapReportViewModel.ReportType, Constants.DoubleNewLine, _contractPayerMapReportViewModel.ModelName);
            reportDate.Value = _contractPayerMapReportViewModel.CurrentDateTime;
            userName.Value = string.Format(Constants.LoggedInUser, _contractPayerMapReportViewModel.LoggedInUser);
        }
    }
}