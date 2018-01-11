/************************************************************************************************************/
/**  Author         : Manikandan
/**  Created        : 17-Feb-2016
/**  Summary        : Manage Facility Details
/**  User Story Id  : 
/** Modification History ************************************************************************************
 *  Date Modified   :
 *  Author          :
 *  Summary         :
/************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using SSI.ContractManagement.Shared.Data.Repository;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Helpers.DataAccess;
using SSI.ContractManagement.Shared.Models;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace SSI.ContractManagement.Data.Repository
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public class FacilityRepository : BaseRepository, IFacilityRepository
    {
        /// <summary>
        /// The _database obj variable
        /// </summary>
        private Database _databaseObj;
        /// <summary>
        /// The _database command variable
        /// </summary>
        DbCommand _databaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="FacilityRepository"/> class.
        /// </summary>
        public FacilityRepository()
        {
            // Create a database object
            // Specify the name of database
            _databaseObj = DatabaseFactory.CreateDatabase("CMMembershipConnectionString");
        }

        /// <summary>
        /// Save / Update Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public string SaveFacility(Facility facility)
        {
            string message = "0";
            //Checks if input request is not null
            if (facility != null)
            {
                // Initialize the Stored Procedure
                _databaseCommand = _databaseObj.GetStoredProcCommand("AddEditFacility");

                // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityId", DbType.Int32, facility.FacilityId);
                _databaseObj.AddInParameter(_databaseCommand, "@DisplayName", DbType.String, facility.DisplayName);
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityName", DbType.String, facility.FacilityName);
                _databaseObj.AddInParameter(_databaseCommand, "@Address", DbType.String, facility.Address);
                _databaseObj.AddInParameter(_databaseCommand, "@City", DbType.String, facility.City);
                _databaseObj.AddInParameter(_databaseCommand, "@StateId", DbType.String, facility.StateId);
                _databaseObj.AddInParameter(_databaseCommand, "@Zip", DbType.String, facility.Zip);
                _databaseObj.AddInParameter(_databaseCommand, "@Domains", DbType.String, facility.Domains);
                _databaseObj.AddInParameter(_databaseCommand, "@Phone", DbType.String, facility.Phone);
                _databaseObj.AddInParameter(_databaseCommand, "@Fax", DbType.String, facility.Fax);
                _databaseObj.AddInParameter(_databaseCommand, "@IsActive", DbType.Boolean, facility.IsActive);
                _databaseObj.AddInParameter(_databaseCommand, "@EarlyStatementDate", DbType.DateTime, facility.EarlyStatementDate);
                _databaseObj.AddInParameter(_databaseCommand, "@PasswordExpirationDays", DbType.Int32, facility.PasswordExpirationDays);
                _databaseObj.AddInParameter(_databaseCommand, "@InvalidPasswordAttempts", DbType.Int32, facility.InvalidPasswordAttempts);
                _databaseObj.AddInParameter(_databaseCommand, "@SSINumber", DbType.String, facility.SsiNo);
                _databaseObj.AddInParameter(_databaseCommand, "@FacilityBubbleId", DbType.Int32, facility.FacilityBubbleId);
                _databaseObj.AddInParameter(_databaseCommand, "@UserName", DbType.String, facility.RequestedUserName);

                List<int> featurecontrolids = facility.FacilityFeatureControl.Where(y => y.IsSelected.Equals(true)).Select(x => x.FeatureControlId).ToList();

                _databaseObj.AddInParameter(_databaseCommand, "@FeatureControl", DbType.String, string.Join(Constants.Comma, featurecontrolids));

                DataSet facilityDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
                if (facilityDataSet.IsTableDataPopulated())
                {
                    //If Column Count is >2 then need to Insert/update DisplayName or Ssinumbers in Bubble database
                    if (facilityDataSet.Tables[0].Columns.Count > 2)
                    {
                        if (!string.IsNullOrEmpty(
                                Convert.ToString(facilityDataSet.Tables[0].Rows[0]["ConnectionString"])))
                        {
                            _databaseObj = new SqlDatabase(Convert.ToString(facilityDataSet.Tables[0].Rows[0]["ConnectionString"]));
                            int facilityId = Convert.ToInt32(facilityDataSet.Tables[0].Rows[0]["FacilityId"]);
                            //Save faclity info in Bubble database
                            if (Convert.ToBoolean(facilityDataSet.Tables[0].Rows[0]["IsDisplayNameChanged"]))
                            {
                                DbCommand databaseCommandDisplayName = _databaseObj.GetStoredProcCommand("AddEditFacilityInfo");
                                _databaseObj.AddInParameter(databaseCommandDisplayName, "@FacilityID", DbType.Int64, facilityId);
                                _databaseObj.AddInParameter(databaseCommandDisplayName, "@FacilityText", DbType.String, facility.DisplayName);
                                _databaseObj.ExecuteNonQuery(databaseCommandDisplayName);
                            }
                            //Save SSI Numbers in Bubble database
                            if (Convert.ToBoolean(facilityDataSet.Tables[0].Rows[0]["IsSSINumberChanges"]))
                            {
                                DbCommand databaseCommandSsiNumber = _databaseObj.GetStoredProcCommand("AddEditFacilitySSINumberMapping");
                                _databaseObj.AddInParameter(databaseCommandSsiNumber, "@Facilityid", DbType.Int32, facilityId);
                                _databaseObj.AddInParameter(databaseCommandSsiNumber, "@Ssinumber", DbType.String, facility.SsiNo);
                                _databaseObj.ExecuteNonQuery(databaseCommandSsiNumber);
                            }
                        }
                    }
                    else
                    {
                        message = Convert.ToString(facilityDataSet.Tables[0].Rows[0][0]);
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// Get the facility by identifier
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        public Facility GetFacilityById(Facility facility)
        {
            var facilityDetails = new Facility();

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetFacilityById");
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityId", DbType.Int32, facility.FacilityId);

            // Retrieve the results of the Stored Procedure 
            DataSet facilityDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (facilityDataSet.IsTableDataPopulated())
            {
                if (facility.FacilityId == 0 && facilityDataSet.Tables[0].Rows.Count > 0)
                {
                    DataRow[] facilityDataRows = facilityDataSet.Tables[0].Select();
                    var featureControl = facilityDataRows.Select(facilityRow => new FeatureControl
                    {
                        FeatureControlId = Convert.ToInt32(facilityRow["FeatureControlId"]),
                        Name = Convert.ToString(facilityRow["Name"])
                    }).ToList();

                    facilityDetails.FacilityFeatureControl = featureControl;
                }
                else
                {
                    // Mapping Facility details
                    if (facilityDataSet.Tables[0].Rows.Count > 0)
                    {
                        facilityDetails.FacilityId = Convert.ToInt32(facilityDataSet.Tables[0].Rows[0]["FacilityId"]);
                        facilityDetails.DisplayName = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["DisplayName"]);
                        facilityDetails.FacilityName =
                            Convert.ToString(facilityDataSet.Tables[0].Rows[0]["FacilityName"]);
                        facilityDetails.Address = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["Address"]);
                        facilityDetails.City = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["City"]);
                        facilityDetails.StateId = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["StateId"]);
                        facilityDetails.Zip = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["Zip"]);
                        facilityDetails.Domains = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["Domains"]);
                        facilityDetails.Phone = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["Phone"]);
                        facilityDetails.Fax = Convert.ToString(facilityDataSet.Tables[0].Rows[0]["Fax"]);
                        facilityDetails.IsActive = Convert.ToBoolean(facilityDataSet.Tables[0].Rows[0]["IsActive"]);
                        facilityDetails.PasswordExpirationDays = Convert.ToInt32(facilityDataSet.Tables[0].Rows[0]["PasswordExpirationDays"]);
                        facilityDetails.InvalidPasswordAttempts = Convert.ToInt32(facilityDataSet.Tables[0].Rows[0]["InvalidPasswordAttempts"]);
                        facilityDetails.EarlyStatementDate =
                            GetValue<DateTime?>(facilityDataSet.Tables[0].Rows[0]["EarlyStatementDate"],
                                typeof(DateTime));

                    }

                    // Mapping SSI Numbers
                    if (facilityDataSet.Tables[1].Rows.Count > 0)
                    {
                        DataRow[] ssiNumbers = facilityDataSet.Tables[1].Select();
                        facilityDetails.SsiNo = string.Join(Constants.Comma, ssiNumbers.Select(ssiNumber => Convert.ToString(ssiNumber["SSINumber"])).ToList());
                    }

                    // Mapping facility Bubbles
                    if (facilityDataSet.Tables[2].Rows.Count > 0)
                    {
                        facilityDetails.FacilityBubbleId = Convert.ToInt32(facilityDataSet.Tables[2].Rows[0]["FacilityBubbleId"]);
                    }

                    // Getting Feature Control
                    if (facilityDataSet.Tables[3].Rows.Count > 0)
                    {
                        DataRow[] facilityDataRows = facilityDataSet.Tables[3].Select();
                        var featureControl = facilityDataRows.Select(facilityRow => new FeatureControl
                        {
                            FeatureControlId = Convert.ToInt32(facilityRow["FeatureControlId"]),
                            Name = Convert.ToString(facilityRow["Name"])
                        }).ToList();
                        facilityDetails.FacilityFeatureControl = featureControl;
                    }

                    // Mapping Feature Control
                    if (facilityDataSet.Tables[4].Rows.Count > 0)
                    {
                        DataRow[] featureControlDataRows = facilityDataSet.Tables[4].Select();
                        foreach (var featureControl in featureControlDataRows.SelectMany(featureControlRow => facilityDetails.FacilityFeatureControl.Where(featureControl =>
                            featureControl.FeatureControlId.Equals(Convert.ToInt32(featureControlRow["FeatureControlId"])))))
                        {
                            featureControl.IsSelected = true;
                        }
                    }
                }
            }

            return facilityDetails;
        }


        /// <summary>
        /// Get All Facilities
        /// </summary>
        /// <param name="facilityInfo"></param>
        /// <returns></returns>
        public FacilityContainer GetAllFacilities(FacilityContainer facilityInfo)
        {
            var facility = new FacilityContainer();

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAllFacilities");
            _databaseObj.AddInParameter(_databaseCommand, "@Take", DbType.String, facilityInfo.PageSetting.Take);
            _databaseObj.AddInParameter(_databaseCommand, "@Skip", DbType.String, facilityInfo.PageSetting.Skip);
            _databaseObj.AddInParameter(_databaseCommand, "@IsActive", DbType.Boolean, facilityInfo.IsActive);
            _databaseObj.AddInParameter(_databaseCommand, "@UserId", DbType.Int32, facilityInfo.UserId);

            // Retrieve the results of the Stored Procedure 
            DataSet facilityDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (facilityDataSet.IsTableDataPopulated())
            {
                DataRow[] dataRows = facilityDataSet.Tables[0].Select();
                List<Facility> facilities = new List<Facility>();
                facilities.AddRange(dataRows.Select(facilityRow => new Facility
                {
                    FacilityId = Convert.ToInt32(facilityRow["FacilityId"]),
                    FacilityName = Convert.ToString(facilityRow["FacilityName"]),
                    SsiNo = Convert.ToString(facilityRow["SSINumber"]),
                    City = Convert.ToString(facilityRow["City"]),
                    StateId = Convert.ToString(facilityRow["StateId"]),
                    IsActive = Convert.ToBoolean(facilityRow["IsActive"]),
                    NoofUsers = Convert.ToInt32(facilityRow["UserCount"])
                }));
                facility.Facilities = facilities;
                facility.TotalRecords = Convert.ToInt32(facilityDataSet.Tables[1].Rows[0][0]);
            }
            return facility;
        }

        /// <summary>
        /// Delete Facility
        /// </summary>
        /// <param name="facility"></param>
        /// <returns></returns>
        //               - use  if (_databaseObj.ExecuteNonQuery(_databaseCommand) == -1) return true; return false; 
        public bool DeleteFacility(Facility facility)
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("DeleteFacilityByID");

            // Pass parameters to Stored Procedure(i.e., @ParamName), add values for
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityId ", DbType.Int32, facility.FacilityId);
            _databaseObj.AddInParameter(_databaseCommand, "@UserName ", DbType.String, facility.RequestedUserName);

            //returns response to Business layer
            return (_databaseObj.ExecuteNonQuery(_databaseCommand) == -1);
        }

        /// <summary>
        /// Get Active States
        /// </summary>
        /// <returns></returns>
        public List<string> GetActiveStates()
        {
            //holds the response data
            var returnvalue = new List<string>();

            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetActiveStates");

            // Retrieve the results of the Stored Procedure 
            DataSet stateDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (stateDataSet.IsTableDataPopulated())
            {
                DataRow[] stateDataRows = stateDataSet.Tables[0].Select();
                returnvalue.AddRange(stateDataRows.Select(state => Convert.ToString(state["StateId"])));
            }
            return returnvalue;
        }

        /// <summary>
        /// Get Bubbles
        /// </summary>
        /// <returns></returns>
        public List<FacilityBubble> GetBubbles()
        {
            // Initialize the Stored Procedure
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAllFacilityBubble");

            // Retrieve the results of the Stored Procedure 
            DataSet bubbleDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (bubbleDataSet.IsTableDataPopulated() && bubbleDataSet.Tables[0].Rows.Count > 0)
            {
                List<FacilityBubble> facilityBubbles = (from DataRow row in bubbleDataSet.Tables[0].Rows
                                                        select new FacilityBubble
                                                 {
                                                     FacilityBubbleId = GetValue<int>(row["FacilityBubbleId"], typeof(int)),
                                                     Description = GetStringValue(row["Description"]),
                                                     ConnectionString = GetStringValue(row["ConnectionString"])
                                                 }).ToList();
                //returns response
                return facilityBubbles;
            }
            return null;
        }

        /// <summary>
        /// Gets all Facility models.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public List<Facility> GetAllFacilityList(User data)
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetAllFacility");
            _databaseObj.AddInParameter(_databaseCommand, "@LoginUserId ", DbType.Int32, data.UserId);
            DataSet facilityModelDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            var facilityModelsList = new List<Facility>();
            if (facilityModelDataSet != null && facilityModelDataSet.Tables[0].Rows.Count > 0)
            {
                DataRow[] dataRows = facilityModelDataSet.Tables[0].Select();
                facilityModelsList.AddRange(dataRows.Select(facilityRow => new Facility
                {
                    FacilityId = GetValue<int>(facilityRow["FacilityId"], typeof(int)),
                    FacilityName = GetStringValue(facilityRow["FacilityName"])

                }));
            }
            return facilityModelsList;
        }


        /// <summary>
        /// Gets the facilities data source.
        /// </summary>
        /// <param name="bubbleConnectionString"></param>
        /// <returns></returns>
        public List<Facility> GetFacilitiesDataSource(string bubbleConnectionString)
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetFacilityDataSource");
            _databaseObj.AddInParameter(_databaseCommand, "@BubbleConnectionString", DbType.String, bubbleConnectionString);
            DataSet facilityModelDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (facilityModelDataSet.IsTableDataPopulated() && facilityModelDataSet.Tables[0].Rows.Count > 0)
            {
                List<Facility> facilities = (from DataRow row in facilityModelDataSet.Tables[0].Rows
                                             select new Facility
                                                               {
                                                                   FacilityId = GetValue<int>(row["FacilityId"], typeof(int))
                                                               }).ToList();

                return facilities;
            }
            return null;
        }



        /// <summary>
        /// Gets the facility medicare details.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public Facility GetFacilityMedicareDetails(int facilityId)
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetFacilityMedicareDetails");
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int32, facilityId);
            DataSet facilityMedicareDataSet = _databaseObj.ExecuteDataSet(_databaseCommand);
            if (facilityMedicareDataSet.IsTableDataPopulated() && facilityMedicareDataSet.Tables[0].Rows.Count > 0)
            {
                Facility facilityDetails = new Facility { FacilityId = facilityId };
                for (int i = 0; i < facilityMedicareDataSet.Tables[0].Rows.Count; i++)
                {
                    //REVIEW-RAGINI-APR10 - What is 1 or 2,logic written below using magic integers ???
                    if (GetValue<int>(facilityMedicareDataSet.Tables[0].Rows[i]["FeatureControlID"], typeof(int)) == 1)
                    {
                        facilityDetails.IsMedicareIpAcute = GetValue<bool>(facilityMedicareDataSet.Tables[0].Rows[i]["IsExist"], typeof(bool));
                    }
                    else if (GetValue<int>(facilityMedicareDataSet.Tables[0].Rows[i]["FeatureControlID"], typeof(int)) == 2) 
                    {
                        facilityDetails.IsMedicareOpApc = GetValue<bool>(facilityMedicareDataSet.Tables[0].Rows[i]["IsExist"], typeof(bool));
                    }
                }
                return facilityDetails;
            }
            return null;
        }


        /// <summary>
        /// Gets the facility connection.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        /// <returns></returns>
        public string GetFacilityConnection(int facilityId)
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetFacilityConnection");
            _databaseObj.AddInParameter(_databaseCommand, "@FacilityID", DbType.Int32, facilityId);
            return Convert.ToString(_databaseObj.ExecuteScalar(_databaseCommand));
        }

        /// <summary>
        /// Gets the facilities data source.
        /// </summary>
        /// <returns></returns>
        public string GetBubbleConnectionString(string bubbleName)
        {
            _databaseCommand = _databaseObj.GetStoredProcCommand("GetBubbleDataSource");
            _databaseObj.AddInParameter(_databaseCommand, "@BubbleDescription", DbType.String, bubbleName);
            DataSet bubbleData = _databaseObj.ExecuteDataSet(_databaseCommand);

            if (bubbleData.IsTableDataPopulated() && bubbleData.Tables[0].Rows.Count > 0)
            {
                return GetStringValue(bubbleData.Tables[0].Rows[0]["ConnectionString"]);
            }
            return null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public void Dispose()
        {
            _databaseCommand.Dispose();
            _databaseObj = null;
        }
    }
}
