using System.Globalization;
using System.Linq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Web.Areas.Report.Models;
using System;

namespace SSI.ContractManagement.Web.Report
{
    /// <summary>
    /// Summary description for Report1.
    /// </summary>
    public partial class ClaimAdjudicationDetails : Telerik.Reporting.Report
    {
        private readonly ClaimAdjudicationReportViewModel _claimAdjudicationReport;

        public ClaimAdjudicationDetails(ClaimAdjudicationReportViewModel claimAdjudicationReportViewModel)
        {
            //claimAdjudicationReportViewModel.ClaimAdjudicationReports[1].Test = new List<string>{};
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            DataSource = claimAdjudicationReportViewModel.ClaimAdjudicationReports;
            table5.DataSource = claimAdjudicationReportViewModel.ClaimsAdjudicated;
            table10.DataSource = claimAdjudicationReportViewModel.PaymentsLinked;
            table6.DataSource = claimAdjudicationReportViewModel.ClaimCharges;
            table7.DataSource = claimAdjudicationReportViewModel.ClaimVariances;
            table9.DataSource = claimAdjudicationReportViewModel.VarianceRanges;
            contractDetails.DataSource = claimAdjudicationReportViewModel.Contracts;
            _claimAdjudicationReport = claimAdjudicationReportViewModel;

            textBox_AdjudicatedCount.Value =
                claimAdjudicationReportViewModel.ClaimAdjudicationReports.Sum(
                    x => x.AdjudicationStatus.Contains("Adjudicated ") ? 0 :x.ClaimCount).ToString(CultureInfo.InvariantCulture);

            txtBox_ClaimCount.Value = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated || string.IsNullOrEmpty(x.ContractName) ? 0 : x.ClaimCount).ToString(CultureInfo.InvariantCulture);

            var totalClaimCharges = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.TotalClaimCharges);
            txtBox_TotalCliamCharges.Value = totalClaimCharges == 0 ? String.Empty : String.Format("{0:C}", totalClaimCharges);

            var calculatedAllowedClaimLevel = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAllowed);
            txtBox_CalculatedAllowedClaimLevel.Value = calculatedAllowedClaimLevel == 0 ? String.Empty : String.Format("{0:C}", calculatedAllowedClaimLevel);

            var actualPaymentClaimLevel = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualPayment);
            txtBox_ActualPaymentClaimLevel.Value = actualPaymentClaimLevel == 0 ? String.Empty : String.Format("{0:C}", actualPaymentClaimLevel);


            var patientResponsibility = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PatientResponsibility);
            textBox_PatientResponsibility.Value = patientResponsibility == 0 ? String.Empty : String.Format("{0:C}", patientResponsibility);


            var paymentVariance = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.PaymentVariance);
            txtBox_PaymentVariance.Value = paymentVariance == 0 ? String.Empty : String.Format("{0:C}", paymentVariance);


            var calculatedAdjustment = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.CalculatedAdjustment);
            txtBox_CalculatedAdjustment.Value = calculatedAdjustment == 0 ? String.Empty : String.Format("{0:C}", calculatedAdjustment);


            var actualAdjustment = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ActualAdjustment);
            txtBox_ActualAdjustment.Value = actualAdjustment == 0 ? String.Empty : String.Format("{0:C}", actualAdjustment);


            var contractualVariance = claimAdjudicationReportViewModel.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated ? 0 : x.ContractualVariance);
            txtBox_contractualVariance.Value = contractualVariance == 0 ? String.Empty : String.Format("{0:C}", contractualVariance);
            totalClaims.Value = Convert.ToString(_claimAdjudicationReport.TotalRecords);
            //varianceReport.Contracts.Sum(x => x.ContractName.ToLower().Contains(Constants.NoRemit) || x.ContractName == Constants.UnAdjudicated || string.IsNullOrEmpty(x.ContractName) ? 0 : x.ClaimCount).ToString(CultureInfo.InvariantCulture);
        }

        private void PageHeaderSection_ItemDataBinding(object sender, EventArgs eventArgs)
        {
            facilityName.Value = string.Format(Constants.FacilityName, _claimAdjudicationReport.FacilityName);
            reportDate.Value = _claimAdjudicationReport.CurrentDateTime;
            userName.Value = string.Format(Constants.LoggedInUser, _claimAdjudicationReport.LoggedInUser);
            
        }

    }

}