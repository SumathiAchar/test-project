using System;
using System.Collections.Generic;
using System.Linq;
using Kendo.DynamicLinq;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.Areas.Adjudication.Models;

namespace SSI.ContractManagement.Web.WebUtil
{
    public static class CommonUtil
    {

        /// <summary>
        /// Gets the page setting.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="defaultSortField">The default sort field.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <returns></returns>
        public static PageSetting GetPageSetting(int take, int skip, IEnumerable<Sort> sort, Filter filter, string defaultSortField, List<string> fieldNames, long claimFieldId)
        {
            PageSetting listSetting = new PageSetting { Take = take, Skip = skip };
            Sort sortField = sort != null ? sort.FirstOrDefault() : new Sort();
            if (sortField != null && (fieldNames.Contains(sortField.Field) || claimFieldId == (byte)Enums.ClaimFieldTypes.CustomPaymentType)) //Handle SQL Injection
            {
                listSetting.SortDirection = sortField.Dir;
                listSetting.SortField = sortField.Field != null ? sortField.Field.Replace(Constants.ColumnHeaderNameSeparator, Constants.Space) : sortField.Field;
            }
            else
            {
                listSetting.SortField = defaultSortField.Replace(Constants.ColumnHeaderNameSeparator, string.Empty);
                listSetting.SortDirection = string.Empty;
            }

            if (filter != null && filter.Filters != null)
            {
                listSetting.SearchCriteriaList = new List<SearchCriteria>();
                foreach (SearchCriteria searchCriteria in from currentFilter in filter.Filters
                                                          where fieldNames.Contains(currentFilter.Field) || claimFieldId == (int)Enums.ClaimFieldTypes.CustomPaymentType
                                                          select new SearchCriteria
                                                              {
                                                                  FilterName = currentFilter.Field != null ? currentFilter.Field.Replace(Constants.ColumnHeaderNameSeparator, Constants.Space) : currentFilter.Field,
                                                                  FilterValue = EditFilterValue(currentFilter.Value, claimFieldId),
                                                                  Operator =
                                                                      (byte)
                                                                          ((Enums.FilterOperator)
                                                                              Enum.Parse(typeof(Enums.FilterOperator), currentFilter.Operator))
                                                              })
                {
                    listSetting.SearchCriteriaList.Add(searchCriteria);
                }
            }

            return listSetting;
        }

        /// <summary>
        /// Edits the filter value.
        /// </summary>
        /// <param name="filterValue">The filterValue.</param>
        /// <param name="claimFieldId">The claim field identifier.</param>
        /// <returns></returns>
        private static object EditFilterValue(object filterValue, long claimFieldId)
        {
            if ((long)Enums.ClaimFieldTypes.DrgWeightTable == claimFieldId)
            {
                string tempValue = filterValue.ToString();
                while (tempValue.Length < 3)
                {
                    tempValue = Constants.PrefixValue + tempValue;
                }
                filterValue = tempValue;
            }
            return filterValue;
        }


        /// <summary>
        /// Gets the page setting.
        /// </summary>
        /// <param name="take">The take.</param>
        /// <param name="skip">The skip.</param>
        /// <param name="sort">The sort.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="fieldNames">The field names.</param>
        /// <param name="modelId">The Model Id.</param>
        /// <returns></returns>
        public static PageSetting GetPageSettingClaim(int take, int skip, IEnumerable<Sort> sort, Filter filter, List<string> fieldNames, long? modelId)
        {
            PageSetting listSetting = new PageSetting { Take = take, Skip = skip };
            Sort sortField = sort != null ? sort.FirstOrDefault() : new Sort();
            if (sortField != null && (fieldNames.Contains(sortField.Field) || modelId != null)) //Handle SQL Injection
            {
                listSetting.SortDirection = sortField.Dir;
                listSetting.SortField = OpenClaimColumnReplacementForSorting(sortField.Field);
            }
            else
            {
                if (sortField != null) listSetting.SortField = OpenClaimColumnReplacementForSorting(sortField.Field);
                listSetting.SortDirection = string.Empty;
            }

            if (filter != null && filter.Filters != null)
            {
                listSetting.SearchCriteriaList = new List<SearchCriteria>();
                foreach (SearchCriteria searchCriteria in from currentFilter in filter.Filters
                                                          select new SearchCriteria
                                                          {
                                                              FilterName = currentFilter.Field != null ? ColumnReplacement(currentFilter.Field) : currentFilter.Field,
                                                              FilterValue = currentFilter.Value,
                                                              Operator =
                                                                  (byte)
                                                                      ((Enums.FilterOperator)
                                                                          Enum.Parse(typeof(Enums.FilterOperator), currentFilter.Operator))
                                                          })
                {
                    listSetting.SearchCriteriaList.Add(searchCriteria);
                }
            }

            return listSetting;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static string ColumnReplacement(string fieldName)
        {
            switch (fieldName)
            {
                case Constants.PropertyPatientAccountNumber:
                    return Constants.PropertyPatAcctNum;
                case Constants.AdjudicatedDateValue:
                    return Constants.InsertDate;
                case Constants.SsiNumber:
                    return Constants.PropertySsinumber;
                case Constants.PropertyClaimType:
                    return Constants.PropertyClaimType;
                case Constants.PayerSequence:
                    return Constants.PayerSequence;
                case Constants.PropertyTotalCharges:
                    return Constants.PropertyTotalCharges;
                case Constants.StatementFromValue:
                    return Constants.PropertyStatementFrom;
                case Constants.StatementThruValue:
                    return Constants.PropertyStatementThru;
                case Constants.ClaimDate:
                    return Constants.ClaimDate;
                case Constants.LastFiledDate:
                    return Constants.LastFiledDate;
                case Constants.PropertyBillType:
                    return Constants.PropertyBillType;
                case Constants.PropertyDrg:
                    return Constants.DrgWithCaps;
                case Constants.PriIcddCode:
                    return Constants.IcddCode;
                case Constants.PriIcdpCode:
                    return Constants.IcdpCode;
                case Constants.PropertyPriPayerName:
                    return Constants.PropertyPriPayerName;
                case Constants.SecPayerName:
                    return Constants.SecPayerName;
                case Constants.TerPayerName:
                    return Constants.TerPayerName;
                case Constants.PropertyIsRemitLinked:
                    return Constants.PropertyIsRemitLinked;
                case Constants.ClaimStatus:
                    return Constants.ClaimStat;
                case Constants.AdjudicatedValue:
                    return Constants.AdjudicatedValue;
                case Constants.ActualPayment:
                    return Constants.ActualPayment;
                case Constants.ActualAdjustment:
                    return Constants.ActualContractualAdjustment;
                case Constants.CalculatedAdjustment:
                    return Constants.ExpectedContractualAdjustment;
                case Constants.ClaimPatientResponsibility:
                    return Constants.ClaimPatientResponsibility;
                case Constants.ContractualVariance:
                    return Constants.ContractualVariance;
                case Constants.PaymentVariance:
                    return Constants.PaymentVariance;
                case Constants.RemitAllowedAmt:
                    return Constants.CalAllow;
                case Constants.PropertyIcn:
                    return Constants.IcnWithCaps;
                case Constants.ClaimLink:
                    return Constants.PropertyClaimLink;
                case Constants.LinkedRemitId:
                    return Constants.PropertyLastRemitId;
                case Constants.ClaimIdValue:
                    return Constants.PropertyClaimId;
                case Constants.PropertyDischargeStatus:
                    return Constants.PropertyDischargeStatus;
                case Constants.PropertyCustomField1:
                    return Constants.PropertyCustomField1;
                case Constants.PropertyCustomField2:
                    return Constants.PropertyCustomField2;
                case Constants.PropertyCustomField3:
                    return Constants.PropertyCustomField3;
                case Constants.PropertyCustomField4:
                    return Constants.PropertyCustomField4;
                case Constants.PropertyCustomField5:
                    return Constants.PropertyCustomField5;
                case Constants.PropertyCustomField6:
                    return Constants.PropertyCustomField6;
                case Constants.PropertyNpi:
                    return Constants.NpiCaps;
                case Constants.MemberId:
                    return Constants.CertificationNumber;
                case Constants.PropertyMrn:
                    return Constants.PropertyMedRecNo;
                case Constants.LastFiledDateValue:
                    return Constants.LastFiledDate;
                case Constants.ClaimDateValue:
                    return Constants.ClaimDate;
                case Constants.RemitNonCovered:
                    return Constants.PropertyCalNonCov;
                case Constants.BillDateValue:
                    return Constants.PropertyBillDate;
              

            }
            return fieldName;
        }

        /// <summary>
        /// Open Claim Column Replacement.
        /// </summary>
        /// <param name="fieldName">The fieldName.</param>
        /// <returns></returns>
        private static string OpenClaimColumnReplacement(string fieldName)
        {
            if (fieldName != null)
            {
                switch (fieldName)
                {
                    case Constants.PropertyPatientAccountNumber:
                        return Constants.PatientAccountNumberWithSpace;

                    case Constants.AdjudicatedDate:
                        return Constants.AdjudicatedDateWithSpace;

                    case Constants.SsiNumberWithCaps:
                        return Constants.SsiNumberWithSpace;

                    case Constants.PropertyClaimType:
                        return Constants.ClaimTypeWithSpace;

                    case Constants.PropertyClaimState:
                        return Constants.ClaimStateWithSpace;

                    case Constants.PayerSequence:
                        return Constants.PayerSequenceWithSpace;

                    case Constants.PropertyTotalCharges:
                        return Constants.ClaimTotalWithSpace;

                    case Constants.PropertyStatementFrom:
                        return Constants.StatementFromWithSpace;

                    case Constants.PropertyStatementThru:
                        return Constants.StatementThruWithSpace;

                    case Constants.ClaimDate:
                        return Constants.ClaimDateWithSpace;

                    case Constants.PropertyBillDate:
                        return Constants.BillDateWithSpace;

                    case Constants.LastFiledDate:
                        return Constants.LastFiledDateWithSpace;

                    case Constants.PropertyBillType:
                        return Constants.BillTypeWithSpace;

                    case Constants.DrgWithCaps:
                        return Constants.DrgWithCaps;

                    case Constants.PriIcddCodeCaps:
                        return Constants.PriIcddCodeWithSpace;

                    case Constants.PriIcdPCodeCaps:
                        return Constants.PriIcdpCodeWithSpace;

                    case Constants.PropertyPriPayerName:
                        return Constants.PriPayerNameWithSpace;

                    case Constants.SecPayerName:
                        return Constants.SecPayerNameWithSpace;

                    case Constants.TerPayerName:
                        return Constants.TerPayerNameWithSpace;

                    case Constants.PropertyIsRemitLinked:
                        return Constants.IsRemitLinkedWithSpace;

                    case Constants.ClaimStatus:
                        return Constants.ClaimStatusWithSpace;

                    case Constants.AdjudicatedValue:
                        return Constants.AdjudicatedValueWithSpace;

                    case Constants.ActualPayment:
                        return Constants.ActualPaymentWithSpace;

                    case Constants.ActualContractualAdjustment:
                        return Constants.ActualAdjWithSpace;

                    case Constants.ExpectedContractualAdjustment:
                        return Constants.CalculatedAdjWithSpace;

                    case Constants.ClaimPatientResponsibility:
                        return Constants.PatientResponsibilityWithSpace;

                    case Constants.ContractualVariance:
                        return Constants.ContractualVarianceWithSpace;

                    case Constants.PaymentVariance:
                        return Constants.PaymentVarianceWithSpace;

                    case Constants.RemitAllowedAmt:
                        return Constants.RemitAllowedAmtWithSpace;

                    case Constants.PropertyClaimLink:
                        return Constants.ClaimLinkWithSpace;

                    case Constants.PropertyLinkedRemitId:
                        return Constants.LinkedRemitIdWithSpace;

                    case Constants.ClaimId:
                        return Constants.ClaimIdWithSpace;

                    case Constants.PropertyDischargeStatus:
                        return Constants.DischargeStatusWithSpace;

                    case Constants.MemberIdCaps:
                        return Constants.MemberIdWithSpace;

                    case Constants.PropertyLos:
                        return Constants.LosWithCaps;

                    case Constants.RemitNonCovered:
                        return Constants.RemitNonCoveredWithSpace;

                    case Constants.CheckDate:
                        return Constants.CheckDateWithSpace;

                    case Constants.CheckNumber:
                        return Constants.CheckNumberWithSpace;

                    case Constants.AdjudicatedContractName:
                        return Constants.AdjudicatedContractNameWithSpace;
                    
                    case Constants.InsuredsGroupNumber:
                        return Constants.InsuredsGroupNumberWithSpace;
                }
                return fieldName;
            }
            return null;
        }

        /// <summary>
        /// Open Claim Column Replacement For Sorting
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        private static string OpenClaimColumnReplacementForSorting(string fieldName)
        {

            switch (fieldName)
            {
                case Constants.ClaimIdValue:
                    return Constants.ClaimId;
                case Constants.Reviewed:
                    return Constants.IsReviewed;
                case Constants.ClaimStat:
                    return Constants.ClaimStatus;
                case Constants.AdjudicatedDateValue:
                    return Constants.AdjudicatedDate;
                case Constants.StatementFromValue:
                    return Constants.PropertyStatementFrom;
                case Constants.StatementThruValue:
                    return Constants.PropertyStatementThru;
                case Constants.BillDateValue:
                    return Constants.PropertyBillDate;

                case Constants.ClaimDateValue:
                    return Constants.ClaimDate;

                case Constants.LastFiledDateValue:
                    return Constants.LastFiledDate;

                case Constants.CalculatedAdjustment:
                    return Constants.ExpectedContractualAdjustment;

                case Constants.ActualAdjustment:
                    return Constants.ActualContractualAdjustment;
            }

            return fieldName;
        }

        /// <summary>
        /// Replace Open Claim Column Name.
        /// </summary>
        /// <param name="claimColumnOptionsInfo">The filterValue.</param>
        /// <returns></returns>
        public static void ReplaceOpenClaimColumName(ClaimColumnOptionsViewModel claimColumnOptionsInfo)
        {
            foreach (var availableColumnList in claimColumnOptionsInfo.AvailableColumnList)
            {
                string replaceAvailableColumn = availableColumnList.ColumnName.Replace(availableColumnList.ColumnName, OpenClaimColumnReplacement(availableColumnList.ColumnName));
                availableColumnList.ColumnName = replaceAvailableColumn;
            }
            foreach (var selectedColumnList in claimColumnOptionsInfo.SelectedColumnList)
            {
                string replaceSelectedColumn = selectedColumnList.ColumnName.Replace(selectedColumnList.ColumnName, OpenClaimColumnReplacement(selectedColumnList.ColumnName));
                selectedColumnList.ColumnName = replaceSelectedColumn;
            }
        }
    }
}