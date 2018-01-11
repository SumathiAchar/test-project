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
    public class LetterTemplateRepository : BaseRepository, ILetterTemplateRepository
    {
        private Database _databaseObj;
        DbCommand _databaseCommandObj;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterTemplateRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public LetterTemplateRepository(string connectionString)
        {
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }
       
       
        /// <summary>
        /// Adds the edit letter template.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public long Save(LetterTemplate letterTemplate)
        {
            //Checks if input request is not null
            if (letterTemplate != null)
            {
                // Initialize the Stored Procedure
                _databaseCommandObj = _databaseObj.GetStoredProcCommand("AddEditLetterTemplate");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommandObj, "@LetterTemplateID", DbType.Int64, letterTemplate.LetterTemplateId);
                _databaseObj.AddInParameter(_databaseCommandObj, "@Name", DbType.String, letterTemplate.Name);
                _databaseObj.AddInParameter(_databaseCommandObj, "@TemplateText", DbType.String, letterTemplate.TemplateText);
                _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, letterTemplate.UserName);
                _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.String, letterTemplate.FacilityId);

                // Retrieve the results of the Stored Procedure in Data table
                return long.Parse(_databaseObj.ExecuteScalar(_databaseCommandObj).ToString());

            }
           
            return 0;
        }

        /// <summary>
        /// Gets the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public LetterTemplate GetLetterTemplateById(LetterTemplate letterTemplate)
        {

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetLetterTemplateById");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@LetterTemplateID ", DbType.Int64, letterTemplate.LetterTemplateId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName ", DbType.String, letterTemplate.UserName);

            // Retrieve the results of the Stored Procedure in Dataset
            DataSet letterTemplateDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);
            if (letterTemplateDataSet != null && letterTemplateDataSet.Tables.Count > 0)
            {
                //populating LetterTemplate data
                if (letterTemplateDataSet.Tables[0] != null && letterTemplateDataSet.Tables[0].Rows != null && letterTemplateDataSet.Tables[0].Rows.Count > 0)
                {
                    LetterTemplate letterTemplateResult = new LetterTemplate
                    {
                        LetterTemplateId = GetValue<long>(letterTemplateDataSet.Tables[0].Rows[0]["LetterTemplateID"], typeof(long)),
                        Name = GetStringValue(letterTemplateDataSet.Tables[0].Rows[0]["Name"]),
                        TemplateText = GetStringValue(letterTemplateDataSet.Tables[0].Rows[0]["TemplateText"]),
                        FacilityId = GetValue<int>(letterTemplateDataSet.Tables[0].Rows[0]["FacilityId"], typeof(int))
                    };
                    return letterTemplateResult;
                }
            }

            return null;
        }

        /// <summary>
        /// Determines whether [is letter name exists] [the specified letter template].
        /// </summary>
        /// <param name="letterTemplate">The letter template.</param>
        /// <returns></returns>
        public bool IsLetterNameExists(LetterTemplate letterTemplate)
        {
            bool isMatched = false;

            _databaseCommandObj = _databaseObj.GetStoredProcCommand("CheckDuplicateLetterTemplateName");
            _databaseObj.AddInParameter(_databaseCommandObj, "@Name", DbType.String, letterTemplate.Name);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityId", DbType.String, letterTemplate.FacilityId);
            int matchedCount = (int)_databaseObj.ExecuteScalar(_databaseCommandObj);
            if (matchedCount > 0)
                isMatched = true;

            return isMatched;
        }

        /// <summary>
        /// Gets all letter templates.
        /// </summary>
        /// <param name="letterTemplateInfo">The letter template information.</param>
        /// <returns></returns>
        public LetterTemplateContainer GetLetterTemplates(LetterTemplateContainer letterTemplateInfo)
        {
            LetterTemplateContainer templates = new LetterTemplateContainer();

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("GetLetterTemplates");
            _databaseObj.AddInParameter(_databaseCommandObj, "@Take", DbType.String, letterTemplateInfo.PageSetting.Take);
            _databaseObj.AddInParameter(_databaseCommandObj, "@Skip", DbType.String, letterTemplateInfo.PageSetting.Skip);
            _databaseObj.AddInParameter(_databaseCommandObj, "@FacilityID", DbType.Int64, letterTemplateInfo.FacilityId);
         // Retrieve the results of the Stored Procedure 
            DataSet letterTemplateDataSet = _databaseObj.ExecuteDataSet(_databaseCommandObj);

            if (letterTemplateDataSet.IsTableDataPopulated())
            {
                DataRow[] dataRows = letterTemplateDataSet.Tables[0].Select();
                List<LetterTemplate> letterTemplates = new List<LetterTemplate>();
                letterTemplates.AddRange(dataRows.Select(letterTemplate => new LetterTemplate
                {
                    LetterTemplateId = GetValue<long>(letterTemplate["LetterTemplateId"], typeof(long)),
                    Name = GetStringValue(letterTemplate["Name"]),
                    UserName = GetStringValue(letterTemplate["UserName"]),
                    TemplateText = GetStringValue(letterTemplate["TemplateText"]),
                    InsertDate = GetValue<DateTime>(letterTemplate["InsertDate"], typeof(DateTime)),
                }));
                templates.LetterTemplates = letterTemplates;
                templates.TotalRecords = Convert.ToInt32(letterTemplateDataSet.Tables[1].Rows[0][0]);
            }


            return templates;
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


        /// <summary>
        /// Deletes the letter template by identifier.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        public bool Delete(LetterTemplate letterTemplateToDelete)
        {
            //holds the response data
            bool returnvalue = false;

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("DeleteLetterTemplateByID");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@LetterTemplateId ", DbType.Int64, letterTemplateToDelete.LetterTemplateId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, letterTemplateToDelete.UserName);
            // Retrieve the results of the Stored Procedure in Data table
            int updatedRow = _databaseObj.ExecuteNonQuery(_databaseCommandObj);
            //returns response to Business layer
            if (updatedRow > 0)
                returnvalue = true;
            return returnvalue;
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="letterTemplateToDelete">The letter template to delete.</param>
        /// <returns></returns>
        public bool InsertAuditLog(LetterTemplate letterTemplateToDelete)
        {
            //holds the response data
            bool returnValue = false;

            // Initialize the Stored Procedure
            _databaseCommandObj = _databaseObj.GetStoredProcCommand("InsertLetterTemplateAuditLog");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommandObj, "@LetterTemplateId ", DbType.Int64, letterTemplateToDelete.LetterTemplateId);
            _databaseObj.AddInParameter(_databaseCommandObj, "@UserName", DbType.String, letterTemplateToDelete.UserName);
            // Retrieve the results of the Stored Procedure in Data table
            int updatedRow = _databaseObj.ExecuteNonQuery(_databaseCommandObj);
            //returns response to Business layer
            if (updatedRow > 0)
                returnValue = true;

            return returnValue;
        }
    }
}
