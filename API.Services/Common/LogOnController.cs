using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    public class LogOnController : ApiController
    {
        private LogOnLogic _logOnLogic;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogOnController"/> class.
        /// </summary>
        public LogOnController()
        {
           _logOnLogic = new LogOnLogic(Constants.ConnectionString);
        }

         /// <summary>
         /// Inserts the audit log.
         /// </summary>
         /// <param name="logOnInfo">The log in information.</param>
         /// <returns></returns>
        [HttpPost]
        public void InsertAuditLog(LogOn logOnInfo)
         {
             if (logOnInfo != null)
             {
                 //Fetches the distinct Facility Connection string
                 List<string> facilityConnectionStrings =
                     logOnInfo.UserFacilities.Select(facility => facility.DataSource).Distinct().ToList();

                 //Logs into database looping through each conectionstrings
                 foreach (string connectionString in facilityConnectionStrings)
                 {
                     logOnInfo.FacilityIds = String.Join(Constants.Comma,
                         logOnInfo.UserFacilities.Where(facility => facility.DataSource == connectionString)
                             .Select(facility => facility.FacilityId)
                             .ToList());
                     _logOnLogic = new LogOnLogic(connectionString);
                     _logOnLogic.InsertAuditLog(logOnInfo);
                 }
             }
         }
    }
}