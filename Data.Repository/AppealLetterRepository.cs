using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class AppealLetterRepository : BaseRepository, IAppealLetterRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;


        /// <summary>
        /// Initializes a new instance of the <see cref="AppealLetterRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public AppealLetterRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// Gets the appeal letter.
        /// </summary>
        /// <param name="appealLetter">The appeal letter container.</param>
        /// <returns></returns>
        public AppealLetter GetAppealLetter(AppealLetter appealLetter)
        {
            if (appealLetter != null)
            {

                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAppealLetter");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@LetterTemplateID", DbType.Int64, appealLetter.LetterTemplateId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@ModelID", DbType.Int64, appealLetter.NodeId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateType", DbType.Int32, appealLetter.DateType);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateFrom", DbType.DateTime, appealLetter.StartDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@DateTo", DbType.DateTime, appealLetter.EndDate);
                _databaseObj.AddInParameter(_databaseCommandObj, "@SelectCriteria", DbType.String, appealLetter.ClaimSearchCriteria);
                //Added RequestedUserID and RequestedUserName with reference to HIPAA logging feature
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserID", DbType.String, appealLetter.RequestedUserId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@RequestedUserName", DbType.String, appealLetter.RequestedUserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@MaxRecordLimit", DbType.Int32, appealLetter.MaxNoOfRecords);

                // Retrieve the results of the Stored Procedure in Data Set=====================================================================
                DataSet appealLetterDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
                if (appealLetterDataSet.IsTableDataPopulated(0))
                {
                    if (appealLetterDataSet.Tables.Count == 1)
                    {
                        appealLetter.ReportThreshold = -1;
                    }
                    if (appealLetterDataSet.Tables.Count == 2)
                    {
                        //Get letter template text
                        appealLetter.LetterTemplaterText = GetStringValue(appealLetterDataSet.Tables[0].Rows[0]["LetterTemplaterText"]);

                        //Get letter report data (claim data)
                        DataTable dataTableClaimData = appealLetterDataSet.Tables[1];

                        appealLetter.AppealLetterClaims = new List<AppealLetterClaim>();
                        appealLetter.AppealLetterClaims.AddRange(
                            dataTableClaimData.Rows.Cast<object>().Select((t, indexCount) => new AppealLetterClaim
                            {
                                BillDate = GetValue<DateTime?>(dataTableClaimData.Rows[indexCount]["BillDate"], typeof(DateTime)),                                
                                BillType = GetStringValue(dataTableClaimData.Rows[indexCount]["BillType"]),
                                ClaimId = GetValue<long>(dataTableClaimData.Rows[indexCount]["ClaimID"], typeof(long)),
                                ContractName = GetStringValue(dataTableClaimData.Rows[indexCount]["ContractName"]),
                                Drg = GetStringValue(dataTableClaimData.Rows[indexCount]["DRG"]),
                                ExpectedAllowed = DBNull.Value == dataTableClaimData.Rows[indexCount]["ExpectedAllowed"]
                                        ? (double?)null
                                        : Math.Round(
                                            double.Parse(
                                                dataTableClaimData.Rows[indexCount]["ExpectedAllowed"].ToString()), 2),
                                Ftn = GetStringValue(dataTableClaimData.Rows[indexCount]["FTN"]),

                                PrimaryGroupNumber = GetStringValue(dataTableClaimData.Rows[indexCount]["PrimaryGroupNumber"]),
                                SecondaryGroupNumber = GetStringValue(dataTableClaimData.Rows[indexCount]["SecondaryGroupNumber"]),
                                TertiaryGroupNumber = GetStringValue(dataTableClaimData.Rows[indexCount]["TertiaryGroupNumber"]),

                                PrimaryMemberId = GetStringValue(dataTableClaimData.Rows[indexCount]["PrimaryMemberID"]),
                                SecondaryMemberId = GetStringValue(dataTableClaimData.Rows[indexCount]["SecondaryMemberID"]),
                                TertiaryMemberId = GetStringValue(dataTableClaimData.Rows[indexCount]["TertiaryMemberID"]),
                                Npi = GetStringValue(dataTableClaimData.Rows[indexCount]["NPI"]),
                                MedRecNumber = GetStringValue(dataTableClaimData.Rows[indexCount]["MedRecNumber"]),
                                Icn = GetStringValue(dataTableClaimData.Rows[indexCount]["ICN"]),
                                PatientAccountNumber = GetStringValue(dataTableClaimData.Rows[indexCount]["PatientAccountNumber"]),
                                PatientDob = GetValue<DateTime?>(dataTableClaimData.Rows[indexCount]["PatientDOB"], typeof(DateTime)),
                                PatientFirstName = GetStringValue(dataTableClaimData.Rows[indexCount]["PatientFirstName"]),
                                PatientLastName = GetStringValue(dataTableClaimData.Rows[indexCount]["PatientLastName"]),
                                PatientMiddleName = GetStringValue(dataTableClaimData.Rows[indexCount]["PatientMiddle"]),
                                PatientResponsibility = DBNull.Value == dataTableClaimData.Rows[indexCount]["PatientResponsibility"]
                                       ? (double?)null
                                       : Math.Round(
                                           double.Parse(
                                               dataTableClaimData.Rows[indexCount]["PatientResponsibility"].ToString()), 2),
                                PayerName = GetStringValue(dataTableClaimData.Rows[indexCount]["PriPayerName"]),
                                ProviderName = GetStringValue(dataTableClaimData.Rows[indexCount]["ProviderName"]),
                                RemitCheckDate = GetValue<DateTime?>(dataTableClaimData.Rows[indexCount]["RemitCheckDate"], typeof(DateTime)),
                                RemitPayment = DBNull.Value == dataTableClaimData.Rows[indexCount]["RemitPayment"]
                                       ? (double?)null
                                       : Math.Round(
                                           double.Parse(
                                               dataTableClaimData.Rows[indexCount]["RemitPayment"].ToString()), 2),
                                StatementFrom = GetValue<DateTime>(dataTableClaimData.Rows[indexCount]["StatementFrom"], typeof(DateTime)),
                                StatementThru = GetValue<DateTime>(dataTableClaimData.Rows[indexCount]["StatementThru"], typeof(DateTime)),
                                Los = GetValue<int>(dataTableClaimData.Rows[indexCount]["Los"], typeof(int)),
                                Age = GetValue<byte>(dataTableClaimData.Rows[indexCount]["Age"], typeof(byte)),
                                ClaimTotal =
                                    DBNull.Value == dataTableClaimData.Rows[indexCount]["ClaimTotal"]
                                        ? (double?)null
                                        : Math.Round(
                                            double.Parse(
                                                dataTableClaimData.Rows[indexCount]["ClaimTotal"].ToString()), 2)

                            }));
                    }
                }
            }
            return appealLetter;
        }


        /// <summary>
        /// Gets the appeal templates.
        /// </summary>
        /// <returns></returns>
        public List<LetterTemplate> GetAppealTemplates(LetterTemplate appealLetterInfo)
        {
            List<LetterTemplate> letterTemplates = new List<LetterTemplate>();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetAppealTemplates");
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.Int64, appealLetterInfo.FacilityId);
            // Retrieve the results of the Stored Procedure 
            DataSet letterTemplateDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (letterTemplateDataSet.IsTableDataPopulated())
            {
                letterTemplates = (from DataRow row in letterTemplateDataSet.Tables[0].Rows
                                   select new LetterTemplate
                                   {
                                       LetterTemplateId = GetValue<long>(row["LetterTemplateID"], typeof(long)),
                                       Name = GetStringValue(row["Name"])
                                   }).ToList();
            }

            return letterTemplates;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseObj = null;
            _databaseCommandObj.Dispose();
        }
    }
}
