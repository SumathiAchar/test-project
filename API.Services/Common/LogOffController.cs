using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SSI.ContractManagement.BusinessLogic;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;

namespace SSI.ContractManagement.Api.Services.Common
{
    
    public class LogOffController : ApiController
    {
        private LogOffLogic _logOffLogic;

        /// <summary>
        /// Prevents a default instance of the <see cref="LogOffController"/> class from being created.
        /// </summary>
        public LogOffController()
        {
            _logOffLogic = new LogOffLogic(Constants.ConnectionString);
        }

        /// <summary>
        /// Inserts the audit log.
        /// </summary>
        /// <param name="logOffInfo">The log off information.</param>
        /// <returns></returns>
        [HttpPost]
        public void InsertAuditLog(LogOff logOffInfo)
        {
            if (logOffInfo != null)
            {
                //Fetches the distinct Facility Connection string
                List<string> facilityConnectionStrings =
                    logOffInfo.UserFacilities.Select(facility => facility.DataSource).Distinct().ToList();

                //Logs into database looping through each conectionstrings
                foreach (string connectionString in facilityConnectionStrings)
                {
                    logOffInfo.FacilityIds = String.Join(Constants.Comma,
                        logOffInfo.UserFacilities.Where(facility => facility.DataSource == connectionString)
                            .Select(facility => facility.FacilityId)
                            .ToList());
                    _logOffLogic = new LogOffLogic(connectionString);
                    _logOffLogic.InsertAuditLog(logOffInfo);
                }
            }
         }
    }
}