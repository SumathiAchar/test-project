using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;
using System.Globalization;
using System.Linq;

namespace SSI.ContractManagement.Web.Report
{
    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class ClaimVarianceDetails : Telerik.Reporting.Report
    {
        private readonly VarianceReportViewModel _varianceReport;

        public ClaimVarianceDetails(VarianceReportViewModel varianceReport)
        {
            InitializeComponent();
            _varianceReport = varianceReport;
            DataSource = varianceReport.VarianceReports;
            contractDetails.DataSource = varianceReport.Contracts;
            table5.DataSource = varianceReport.ClaimsAdjudicated;
            table10.DataSource = varianceReport.PaymentsLinked;
            table6.DataSource = varianceReport.ClaimCharges;
            table7.DataSource = varianceReport.ClaimVariances;
            table9.DataSource = varianceReport.VarianceRanges;
            Name = Constants.ClaimVarianceReportFileBaseName;

            txtBox_ClaimCount.Value = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated || string.IsNullOrEmpty(x.ContractName) ? 0 : x.ClaimCount).ToString(CultureInfo.InvariantCulture);

            var totalClaimCharges = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.TotalClaimCharges);
            txtBox_TotalCliamCharges.Value = totalClaimCharges == 0 ? String.Empty : String.Format("{0:C}", totalClaimCharges);

            var calculatedAllowedClaimLevel = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAllowed);
            txtBox_CalculatedAllowedClaimLevel.Value = calculatedAllowedClaimLevel == 0 ? String.Empty : String.Format("{0:C}", calculatedAllowedClaimLevel);

            var actualPaymentClaimLevel = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualPayment);
            txtBox_ActualPaymentClaimLevel.Value = actualPaymentClaimLevel == 0 ? String.Empty : String.Format("{0:C}", actualPaymentClaimLevel);


            var patientResponsibility = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PatientResponsibility);
            textBox_PatientResponsibility.Value = patientResponsibility == 0 ? String.Empty : String.Format("{0:C}", patientResponsibility);


            var paymentVariance = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PaymentVariance);
            txtBox_PaymentVariance.Value = paymentVariance == 0 ? String.Empty : String.Format("{0:C}", paymentVariance);


            var calculatedAdjustment = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAdjustment);
            txtBox_CalculatedAdjustment.Value = calculatedAdjustment == 0 ? String.Empty : String.Format("{0:C}", calculatedAdjustment);


            var actualAdjustment = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualAdjustment);
            txtBox_ActualAdjustment.Value = actualAdjustment == 0 ? String.Empty : String.Format("{0:C}", actualAdjustment);


            var contractualVariance = varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ContractualVariance);
            txtBox_contractualVariance.Value = contractualVariance == 0 ? String.Empty : String.Format("{0:C}", contractualVariance);

            totalClaims.Value = Convert.ToString(varianceReport.TotalClaims);
        }

        private void PageHeaderSection_ItemDataBinding(object sender, EventArgs eventArgs)
        {
            facilityName.Value = string.Format(Constants.FacilityName, _varianceReport.FacilityName);
            reportDate.Value = _varianceReport.CurrentDateTime;
            userName.Value = string.Format(Constants.LoggedInUser, _varianceReport.LoggedInUser);
        }
    }
}