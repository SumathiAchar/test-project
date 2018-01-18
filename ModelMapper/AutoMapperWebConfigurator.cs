using AutoMapper;
using SSI.ContractManagement.Web.Areas.Adjudication.ModelMapper;
using SSI.ContractManagement.Web.Areas.Contract.ModelMapper;
using SSI.ContractManagement.Web.Areas.Report.ModelMapper;
using SSI.ContractManagement.Web.Areas.Treeview.ModelMapper;
using SSI.ContractManagement.Web.Areas.UserManagement.ModelMapper;

namespace SSI.ContractManagement.Web.ModelMapper
{
    public static class AutoMapperWebConfigurator
    {
        public static void Configure()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new ContractDocsMapper());
                cfg.AddProfile(new ContractMapper());
                cfg.AddProfile(new ContractNotesMapper());
                cfg.AddProfile(new ContractPayerInfoMapper());
                cfg.AddProfile(new ContractServiceLineClaimFieldSelectionMapper());
                cfg.AddProfile(new ContractServiceLineCodeMapper());
                cfg.AddProfile(new ContractServiceLineTableSelectionMapper());
                cfg.AddProfile(new ContractServiceTypeMapper());
                cfg.AddProfile(new TreeViewMapper());
                cfg.AddProfile(new ContractFiltersMapper());

                cfg.AddProfile(new PaymentTypeAscFeeScheduleMapper());
                cfg.AddProfile(new PaymentTypeCapMapper());
                cfg.AddProfile(new PaymentTypeDrgPaymentMapper());
                cfg.AddProfile(new PaymentTypeFeeSchedulesMapper());
                cfg.AddProfile(new PaymentTypeLesserOfMapper());
                cfg.AddProfile(new PaymentTypeMedicareIpPaymentMapper());
                cfg.AddProfile(new PaymentTypeMedicareLabFeeSchedulePaymentMapper());
                cfg.AddProfile(new PaymentTypeMedicareOpPaymentMapper());
                cfg.AddProfile(new PaymentTypePerCaseMapper());
                cfg.AddProfile(new PaymentTypePercentageChargeMapper());
                cfg.AddProfile(new PaymentTypePercentageChargeMapper());
                cfg.AddProfile(new PaymentTypePerDiemMapper());
                cfg.AddProfile(new PaymentTypePerVisitMapper());
                cfg.AddProfile(new PaymentTypeStopLossMapper());
                cfg.AddProfile(new SelectClaimsMapper());
                cfg.AddProfile(new ModelingReportMapper());
                cfg.AddProfile(new VarianceReportMapper());
                cfg.AddProfile(new ClaimAdjudicationReportMapper());
                cfg.AddProfile(new JobStatusMapper());
                cfg.AddProfile(new ClaimDataMapper());
                cfg.AddProfile(new ClaimChargeDataMapper());
                cfg.AddProfile(new LetterTemplateMapper());
                cfg.AddProfile(new ContractPayerMapReportMapper());
                cfg.AddProfile(new PaymentTypeCustomTableMapper());
                cfg.AddProfile(new ModelComparisonReportMapper());
                cfg.AddProfile(new AuditLogReportMapper());
                cfg.AddProfile(new FacilityMapper());
				cfg.AddProfile(new UserMapper());
                cfg.AddProfile(new AccountMapper());
                cfg.AddProfile(new PaymentTypeMedicareSequesterMapper());
                cfg.AddProfile(new ClaimColumnOptionsMapper());
            });
        }
    }
}
    
