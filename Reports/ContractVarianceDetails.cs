using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;
using System.Globalization;
using System.Linq;

namespace SSI.ContractManagement.Web.Report
{
    /// <summary>
    /// Summary description for ContractVarianceDetails.
    /// </summary>
    public partial class ContractVarianceDetails : Telerik.Reporting.Report
    {
        private readonly VarianceReportViewModel _varianceReport;

        /// <summary>
        /// Constructor which receives variance details
        /// </summary>
        /// <param name="varianceReportViewModel">variance details</param>
        public ContractVarianceDetails(VarianceReportViewModel varianceReportViewModel)
        {
            InitializeComponent();
            _varianceReport = varianceReportViewModel;
            Name = Constants.ContractVarianceReportFileBaseName;

            TblContractLableVarianceData.DataSource = _varianceReport.Contracts;

            txtBox_ClaimCount.Value = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated || string.IsNullOrEmpty(x.ContractName) ? 0 : x.ClaimCount).ToString(CultureInfo.InvariantCulture);

            var totalClaimCharges = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.TotalClaimCharges);
            txtBox_TotalCliamCharges.Value = totalClaimCharges == 0 ? String.Empty : String.Format("{0:C}", totalClaimCharges);

            var calculatedAllowedClaimLevel = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAllowed);
            txtBox_CalculatedAllowedClaimLevel.Value = calculatedAllowedClaimLevel == 0 ? String.Empty : String.Format("{0:C}", calculatedAllowedClaimLevel);

            var actualPaymentClaimLevel = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualPayment);
            txtBox_ActualPaymentClaimLevel.Value = actualPaymentClaimLevel == 0 ? String.Empty : String.Format("{0:C}", actualPaymentClaimLevel);

            var patientResponsibility = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PatientResponsibility);
            textBox_PatientResponsibility.Value = patientResponsibility == 0 ? String.Empty : String.Format("{0:C}", patientResponsibility);

            var paymentVariance = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PaymentVariance);
            txtBox_PaymentVariance.Value = paymentVariance == 0 ? String.Empty : String.Format("{0:C}", paymentVariance);

            var calculatedAdjustment = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAdjustment);
            txtBox_CalculatedAdjustment.Value = calculatedAdjustment == 0 ? String.Empty : String.Format("{0:C}", calculatedAdjustment);

            var actualAdjustment = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualAdjustment);
            txtBox_ActualAdjustment.Value = actualAdjustment == 0 ? String.Empty : String.Format("{0:C}", actualAdjustment);

            var contractualVariance = varianceReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ContractualVariance);
            txtBox_contractualVariance.Value = contractualVariance == 0 ? String.Empty : String.Format("{0:C}", contractualVariance);
        }

        /// <summary>
        /// Describes the header section of the page
        /// </summary>
        /// <param name="sender">Object which sent the request </param>
        /// <param name="eventArgs">event argument</param>
        private void PageHeaderSection_ItemDataBinding(object sender, EventArgs eventArgs)
        {
            facilityName.Value = string.Format(Constants.FacilityName, _varianceReport.FacilityName);
            reportDate.Value = _varianceReport.CurrentDateTime;
            userName.Value = string.Format(Constants.LoggedInUser, _varianceReport.LoggedInUser);
        }
    }
}