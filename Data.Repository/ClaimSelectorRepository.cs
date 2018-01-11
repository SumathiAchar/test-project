/************************************************************************************************************/
/**  Author         : Prasad Dintakurti
/**  Created        : 04-Sep-2013
/**  Summary        : Handles Add/Modify Select Claims Repository DataAccess Layer

/************************************************************************************************************/

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using SSI.ContractManagement.Shared.Data.Repository;
//FIXED-MAR16 - Remove unwanted using directive
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Helpers.StringExtension;
using SSI.ContractManagement.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class ClaimSelectorRepository : BaseRepository, IClaimSelectorRepository
    {
        // Variables
        private readonly Database _databaseObj;
        DbCommand _databaseCommand;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimSelectorRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public ClaimSelectorRepository(string connectionString)
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = new GenericDatabase(connectionString, System.Data.SqlClient.SqlClientFactory.Instance);
        }

        /// <summary>
        /// AddEdit Add Edit Select Claims
        /// </summary>
        /// <param name="claimSelector"></param>
        /// <returns></returns>
        public long AddEditSelectClaims(ClaimSelector claimSelector)
        {
            //Checks if input request is not null
            if (claimSelector != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("AddAdjudicationTasks");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@RequestName", DbType.String, claimSelector.RequestName.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@SelectCriteria", DbType.String, claimSelector.ClaimFieldList.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, claimSelector.FacilityId);
                _databaseObj.AddInParameter(_databaseCommand, "@ModelID", DbType.Int64, claimSelector.ModelId);
                _databaseObj.AddInParameter(_databaseCommand, "@DateType", DbType.String, claimSelector.DateType);
                _databaseObj.AddInParameter(_databaseCommand, "@DateFrom", DbType.DateTime, claimSelector.DateFrom);
                _databaseObj.AddInParameter(_databaseCommand, "@DateTo", DbType.DateTime, claimSelector.DateTo);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, claimSelector.UserName.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@IsUserDefined", DbType.Boolean, claimSelector.IsUserDefined);
                _databaseObj.AddInParameter(_databaseCommand, "@RunningStatus", DbType.Int32, claimSelector.RunningStatus);
                _databaseObj.AddInParameter(_databaseCommand, "@Priority", DbType.Int32, 0);
                _databaseObj.AddInParameter(_databaseCommand, "@IdJob", DbType.Int32, 0);
                _databaseObj.AddInParameter(_databaseCommand, "@StartTime", DbType.DateTime, null);
                _databaseObj.AddInParameter(_databaseCommand, "@EndTime", DbType.DateTime, null);
                // Out parameter for getting the newly inserted TaskId
                _databaseObj.AddOutParameter(_databaseCommand, "@InsertedID", DbType.Int32, Int32.MaxValue);
                // Added for Config value
                _databaseCommand.CommandTimeout = claimSelector.CommandTimeoutForSelectClaimIdsforAdjudicate;
                // Retrieve the results of the Stored Procedure in Datatable
                _databaseObj.ExecuteNonQuery(_databaseCommand);
                return long.Parse(_databaseObj.GetParameterValue(_databaseCommand, "@InsertedID").ToString());


            }
            return 0;
        }

        /// <summary>
        /// Get All Adjudicate dataDeleteUnMatchedClaims
        /// </summary>
        /// <returns></returns>
        public List<ClaimSelector> AdjudicateAllFacilityContract()
        {
            List<ClaimSelector> selectClaimDataList = null;

            //Get All Adjudicate data
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAdjudicateAllFacilityContractData");
            _databaseCommand.CommandTimeout = 250;
            DataSet selectClaimData = _databaseObj.ExecuteDataSet(_databaseCommand);
            if (selectClaimData != null)
            {
                selectClaimDataList = AdjudicateAllFacilityContractData(selectClaimData);
            }

            return selectClaimDataList;
        }

        /// <summary>
        /// Gets the ssi number for background ajudication.
        /// </summary>
        /// <returns></returns>
        public List<int> GetSsiNumberForBackgroundAjudication()
        {
            List<int> ssiNumberList = new List<int>();
            //Get All Adjudicate data
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetSSINumberForBackgroundAjudication");
            _databaseCommand.CommandTimeout = 3000;
            DataSet ssiNumberData = _databaseObj.ExecuteDataSet(_databaseCommand);
            if (ssiNumberData.IsTableDataPopulated(0))
            {
                //Loop through to get all ModelId and FacilityId
                for (int i = 0; i < ssiNumberData.Tables[0].Rows.Count; i++)
                {
                    //Add All selectclaim data in list
                    ssiNumberList.Add(Convert.ToInt32(
                        ssiNumberData.Tables[0].Rows[i]["SSINumber"]));
                }
            }

            return ssiNumberList;
        }

        /// <summary>
        /// AddEdit Add Edit Select Claims
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public void UpdateIsLastAdjudicateProcessed(long taskId)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("UpdateIsLastAdjudicateProcess");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@TaskID", DbType.Int64, taskId);
            // Retrieve the results of the Stored Procedure in Datatable
            _databaseObj.ExecuteNonQuery(_databaseCommand);

        }


        /// <summary>
        /// Selects the claim id's for adjudicate.
        /// </summary>
        /// <param name="taskId">The task unique identifier.</param>
        /// <returns></returns>
        public long SelectClaimIdsToAdjudicate(long? taskId)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("SelectClaimsForAdjudication");
            _databaseCommand.CommandTimeout = 5400;
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@TaskID", DbType.Int64, taskId);

            // Retrieve the results of the Stored Procedure in Datatable
            return long.Parse(_databaseObj.ExecuteScalar(_databaseCommand).ToString());

        }

        /// <summary>
        /// Gets the claim idsfor adjudicate.
        /// </summary>
        /// <returns>Claims Count</returns>
        public long GetClaimsCountForAdjudication(ClaimSelector claimSelector)
        {

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetClaimsCountForAdjudication");
            _databaseObj.AddInParameter(_databaseCommand, "@DateType", DbType.Int16, claimSelector.DateType);
            _databaseObj.AddInParameter(_databaseCommand, "@DateFrom", DbType.DateTime, claimSelector.DateFrom);
            _databaseObj.AddInParameter(_databaseCommand, "@DateTo", DbType.DateTime, claimSelector.DateTo);
            _databaseObj.AddInParameter(_databaseCommand, "@SelectCriteria", DbType.String, claimSelector.ClaimFieldList);
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, claimSelector.FacilityId);
            _databaseObj.AddInParameter(_databaseCommand, "@ModelID", DbType.Int64, claimSelector.ModelId);
            _databaseCommand.CommandTimeout = claimSelector.CommandTimeoutForSelectClaimIdsforAdjudicate;
            // Retrieve the results of the Stored Procedure in Data table
            return (int)_databaseObj.ExecuteScalar(_databaseCommand);

        }

        

        /// <summary>
        /// For getting SelectClaim Data
        /// </summary>
        /// <param name="dataSetClaimFullData"></param>
        /// <returns>List of ClaimData</returns>
        private List<ClaimSelector> AdjudicateAllFacilityContractData(DataSet dataSetClaimFullData)
        {
            List<ClaimSelector> ssiNumberList = new List<ClaimSelector>();
            if (dataSetClaimFullData.IsTableDataPopulated(0))
            {
                //Loop through to get all ModelId and FacilityId
                for (int i = 0; i < dataSetClaimFullData.Tables[0].Rows.Count; i++)
                {
                    ClaimSelector selectClaimsInfo = new ClaimSelector
                    {
                        ModelId =
                            Convert.ToInt64(
                                dataSetClaimFullData.Tables[0].Rows[i]["NodeId"]),
                        FacilityId =
                            Convert.ToInt32(dataSetClaimFullData.Tables[0].Rows[i]["FacilityId"]),
                        //Creating RequestName
                        FacilityList = new List<int> { Convert.ToInt32(dataSetClaimFullData.Tables[0].Rows[i]["FacilityIdOfNode"]) },
                        RequestName = string.Format("TrackTask_{0}_{1}", Convert.ToInt64(dataSetClaimFullData.Tables[0].Rows[i]["FacilityId"]), DateTime.UtcNow),
                        IsUserDefined = false
                    };
                    
                    //Add All select claim data in list
                    ssiNumberList.Add(selectClaimsInfo);
                }
            }

            return ssiNumberList;
        }

        /// <summary>
        /// Updates task status in DB
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        public void UpdateJobStatus(TrackTask job)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("UpdateJobStatus");
            _databaseObj.AddInParameter(_databaseCommand, "@TaskId", DbType.Int64, job.TaskId);
            _databaseObj.AddInParameter(_databaseCommand, "@RunningStatus", DbType.Int16, job.Status);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, job.UserName);
            //// Retrieve the results of the Stored Procedure 
            _databaseObj.ExecuteScalar(_databaseCommand);
        }

        /// <summary>
        /// Disposes the objects
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {

        }

        /// <summary>
        /// Check Adjudication request name exist or not.
        /// </summary>
        /// <param name="claimSelector">The select claims.</param>
        /// <returns>true/false</returns>
        public bool CheckAdjudicationRequestNameExist(ClaimSelector claimSelector)
        {

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("CheckAdjudicationRequestNameExist");
            _databaseObj.AddInParameter(_databaseCommand, "@RequestName", DbType.String, claimSelector.RequestName);
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int64, claimSelector.FacilityId);

            _databaseCommand.CommandTimeout = claimSelector.CommandTimeoutForCheckAdjudicationRequestNameExist;
            int adjRequestNameCount = (int)_databaseObj.ExecuteScalar(_databaseCommand);
            if (adjRequestNameCount > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Selects the claim id's for adjudicate.
        /// </summary>
        /// <param name="facilityId">The task unique identifier.</param>
        /// <param name="batchSize">The batch size for background adjudication.</param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public long GetBackgroundAdjudicationTask(long? facilityId, int batchSize, int timeout)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("CreateBackgroundAdjudicationTask");
            _databaseCommand.CommandTimeout = timeout;
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityId", DbType.Int64, facilityId);
            _databaseObj.AddInParameter(_databaseCommand, "@BatchSizeForBackgroundAdjudication", DbType.Int32, batchSize);

            // Retrieve the results of the Stored Procedure in Data table
            return Convert.ToInt64(_databaseObj.ExecuteScalar(_databaseCommand));
        }

        /// <summary>
        /// Gets the adjudicated facilities.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TrackTask> GetAdjudicatedTasks(string facilityIds)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAdjudicatedTasks");
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityIds", DbType.String, facilityIds);
            DataSet adjudicatedTasksDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
            if (adjudicatedTasksDataSet.IsTableDataPopulated() && adjudicatedTasksDataSet.Tables[0].Rows.Count > 0)
            {
                //Loop through to get all track facility details
                List<TrackTask> trackTaskList = (from DataRow row in adjudicatedTasksDataSet.Tables[0].Rows
                                              select new TrackTask
                                              {
                                                  FacilityId = GetValue<int>(row["FacilityId"], typeof(int)),//TODO Janaki
                                                  StatusCode = GetValue<int>(row["RunningStatus"], typeof(int)),
                                                  InsertDate = GetValue<DateTime>(row["InsertDate"], typeof(DateTime))
                                              }).ToList();
                return trackTaskList;
            }
            return null;

        }

        /// <summary>
        /// Reviews the claim.
        /// </summary>
        /// <param name="claimsReviewedList">The claims reviewed list.</param>
        /// <returns></returns>
        public bool ReviewClaim(IEnumerable<ClaimsReviewed> claimsReviewedList)
        {
            if (claimsReviewedList != null)
            {
                XmlSerializer serializer = new XmlSerializer(claimsReviewedList.GetType());
                StringWriter stringWriter = new StringWriter();
                XmlWriterSettings settings = new XmlWriterSettings {Indent = true, OmitXmlDeclaration = true};
                XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);
                XmlSerializerNamespaces emptyNs = new XmlSerializerNamespaces(new[] {XmlQualifiedName.Empty});
                serializer.Serialize(xmlWriter, claimsReviewedList, emptyNs);
                //holds the final payer response
                var finalStrXml = stringWriter.ToString();

                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("ClaimsReviewed");
                _databaseObj.AddInParameter(_databaseCommand, "@XmlclaimsReviewedList", DbType.String, finalStrXml);
                return _databaseObj.ExecuteNonQuery(_databaseCommand) > 0;
            }
            return false;
        }

        // FIXED-NOV15 Move this method into SelectClaimRepository
        /// <summary>
        /// Reviewed all claims.
        /// </summary>
        /// <param name="selectionCriteria">The selection criteria.</param>
        /// <returns></returns>
        public bool ReviewedAllClaims(SelectionCriteria selectionCriteria)
        {
            if (selectionCriteria != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("ClaimsAllReviewed");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@SelectCriteria ", DbType.String, selectionCriteria.ClaimSearchCriteria);
                _databaseObj.AddInParameter(_databaseCommand, "@ModelId", DbType.Int64, selectionCriteria.ModelId);
                _databaseObj.AddInParameter(_databaseCommand, "@DateType ", DbType.Int32, selectionCriteria.DateType);
                _databaseObj.AddInParameter(_databaseCommand, "@StartDate ", DbType.DateTime, selectionCriteria.StartDate);
                _databaseObj.AddInParameter(_databaseCommand, "@EndDate ", DbType.DateTime, selectionCriteria.EndDate);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, selectionCriteria.UserName);
                // Retrieve the results of the Stored Procedure
                return _databaseObj.ExecuteNonQuery(_databaseCommand) > 0;
            }

            return false;

        }

        /// <summary>
        /// Adds the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        public ClaimNote AddClaimNote(ClaimNote claimNote)
        {
            ClaimNote responseClaimNote = new ClaimNote();
            //Checks if input request is not null
            if (claimNote != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("AddClaimNote");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@ClaimID ", DbType.Int64, claimNote.ClaimId);
                _databaseObj.AddInParameter(_databaseCommand, "@ClaimNoteText", DbType.String, claimNote.ClaimNoteText.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, claimNote.UserName.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityName", DbType.String, claimNote.FacilityName);
                // Retrieve the results of the Stored Procedure in Datatable
                DataSet contractNotesDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

                if (contractNotesDataSet.IsTableDataPopulated(0))
                {
                    string localDateTime = Utilities.GetLocalTimeString(claimNote.CurrentDateTime,
                        Convert.ToDateTime(contractNotesDataSet.Tables[0].Rows[0]["InsertDate"].ToString()));
                    responseClaimNote.ClaimNoteId = long.Parse(contractNotesDataSet.Tables[0].Rows[0]["InsertedId"].ToString());
                    responseClaimNote.InsertDate = Convert.ToDateTime(localDateTime);
                }
            }
            //returns response to Business layer
            return responseClaimNote;
        }

        /// <summary>
        /// Deletes the claim note.
        /// </summary>
        /// <param name="claimNote">The claim note.</param>
        /// <returns></returns>
        public bool DeleteClaimNote(ClaimNote claimNote)
        {
            //holds the response data
            bool returnvalue = false;

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("DeleteClaimNoteByID");
            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@ClaimNoteID ", DbType.Int64, claimNote.ClaimNoteId);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName ", DbType.String, claimNote.UserName);
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityName", DbType.String, claimNote.FacilityName);
            // Retrieve the results of the Stored Procedure in Datatable
            int updatedRow = _databaseObj.ExecuteNonQuery(_databaseCommand);
            //returns response to Business layer
            if (updatedRow > 0)
                returnvalue = true;
            //returns false if any exception occurs
            return returnvalue;
        }

        /// <summary>
        /// Gets the claim notes.
        /// </summary>
        /// <param name="claimNotesContainer">The claim notes container.</param>
        /// <returns></returns>
        public ClaimNotesContainer GetClaimNotes(ClaimNotesContainer claimNotesContainer)
        {
            ClaimNotesContainer claimNoteContainer = new ClaimNotesContainer();
            List<ClaimNote> claimNotes=new List<ClaimNote>();
            //Checks if input request is not null
            if (claimNotesContainer != null)
            {
                _databaseCommand = _databaseObj.GetStoredProcCommand("GetClaimNotes");
                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@ClaimID ", DbType.Int64, claimNotesContainer.ClaimId);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, claimNotesContainer.UserName.ToTrim());
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityName", DbType.String, claimNotesContainer.FacilityName);

                // Retrieve the results of the Stored Procedure in Datatable
                DataSet claimtNotesDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
                
                if (claimtNotesDataSet.IsTableDataPopulated(0))
                {
                    DataRowCollection claimNoteDataRowCollection = claimtNotesDataSet.Tables[0].Rows;

                     claimNotes = (from DataRow row in claimNoteDataRowCollection
                        select new ClaimNote
                        {
                            ClaimId = GetValue<long>(row["ClaimID"], typeof(long)),
                            ClaimNoteText = GetStringValue(row["ClaimNoteText"]),
                            ClaimNoteId = GetValue<long>(row["ClaimNoteID"], typeof(long)),
                            UserName = GetStringValue(row["UserName"]),
                            ShortDateTime = Convert.ToDateTime(
                                       Utilities.GetLocalTimeString(claimNotesContainer.CurrentDateTime,
                           Convert.ToDateTime(row["InsertDate"].ToString()))).ToString(Constants.DateTimeFormatWithSecond),
                        }).ToList();

                }
            }
            claimNoteContainer.ClaimNotes = claimNotes;
            //returns response to Business layer
            return claimNoteContainer;
        }
    }
}

