using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SSI.ContractManagement.Shared.Helpers;
using SSI.ContractManagement.Shared.Models;
using SSI.ContractManagement.Web.ActionFilters;
using SSI.ContractManagement.Web.Areas.Common.Models;
using SSI.ContractManagement.Web.Areas.UserManagement.Models;

namespace SSI.ContractManagement.Web.Areas.Common.Controllers
{
    [Authorize]
    [AjaxSessionActionFilter]
    [Serializable]
    public class SessionStoreController : BaseController
    {
        /// <summary>
        /// Sets the session.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="isFromSecurityPage"></param>
        /// <returns></returns>
        public ActionResult SetSession(string returnUrl, bool isFromSecurityPage)
        {
            UserInfo userInfo = TempData[Constants.UserInfo] as UserInfo;
            if (userInfo != null)
            {
                userInfo.IsFromSecurityPage = isFromSecurityPage;
                //Auto refresh checbox is checked when user logs in
                SetAutoRefresh(true);
                SetUserInfo(userInfo);

                if (Url.IsLocalUrl(returnUrl) && returnUrl != Constants.LogOffUrl
                    && !returnUrl.Contains(Constants.LogOffUrlWithSessionTimeOut) && !returnUrl.Contains(Constants.LogOffUrlWithOutSessionTimeOut) && !returnUrl.Contains(Constants.LogInUrl))
                {
                    return Redirect(returnUrl);
                }
                InsertLogOnAuditLog(userInfo);
            }
            return (!isFromSecurityPage) ? RedirectToAction(Constants.HomeIndex, Constants.ContractContainer, new { area = Constants.Contract }) : null;
        }

        /// <summary>
        /// Inserts the log on audit log.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        private void InsertLogOnAuditLog(UserInfo userInfo)
        {
            //FIXED-2016-R2-S3 : Use Enums.UserRoles to compare userInfo.UserTypeId 
            //Audit logging for FacilityAdmin and User login but not for SSIAdmin login(UserTypeId = 1)
            if (userInfo != null && userInfo.UserTypeId != (int)Enums.UserRoles.SsiAdmin)
            {
                LogOn logOn = new LogOn
                {
                    UserName = userInfo.UserName,
                    UserFacilities = AutoMapper.Mapper.Map<List<FacilityViewModel>, List<Facility>>(userInfo.AssignedFacilities)
                };
                PostApiResponse<bool>(Constants.LogOn, Constants.InsertAuditLog, logOn, true);
            }
        }

        /// <summary>
        /// Sets the reassigned claims.
        /// </summary>
        /// <param name="retainedCliams">The retained claims.</param>
        public void SetReassignedClaims(List<RetainedClaim> retainedCliams)
        {
            Session[Constants.ReassignedClaims] = retainedCliams;
        }

        /// <summary>
        /// Gets the name of the logged in user.
        /// </summary>
        /// <returns></returns>
        public List<RetainedClaim> GetReassignedClaims()
        {
            if (Session[Constants.ReassignedClaims] != null)
                return (List<RetainedClaim>)Session[Constants.ReassignedClaims];
            return new List<RetainedClaim>();
        }

        /// <summary>
        /// Sets the user information.
        /// </summary>
        /// <param name="userInfo">The user information.</param>
        public void SetUserInfo(UserInfo userInfo)
        {
            Session[Constants.UserFacilitiesSessionString] = userInfo;
        }

        /// <summary>
        /// Gets the logged in user key.
        /// </summary>
        /// <returns></returns>
        public string GetLoggedInUserKey()
        {
            UserInfo userInfo = Session[Constants.UserFacilitiesSessionString] as UserInfo;
            return userInfo != null ? userInfo.UserKey : null;
        }

        /// <summary>
        /// Gets or sets the _last expanded node unique identifier.
        /// </summary>
        /// <value>
        /// The _last expanded node unique identifier.
        /// </value>
        // ReSharper disable once InconsistentNaming
        private long? _lastExpandedNodeId { get; set; }

        /// <summary>
        /// Gets or sets the last expanded node unique identifier which is stored in Session. This nodeID defines that which node was selected before, and expand the tree view next time when page gets load.
        /// </summary>
        /// <value>
        /// The last expanded node unique identifier.
        /// </value>
        protected long? LastExpandedNodeId
        {
            get
            {
                if (_lastExpandedNodeId.HasValue)
                {
                    return _lastExpandedNodeId;
                }
                if (Session[Constants.LastExpandedNodeIdSessionString] != null)
                {
                    long nodeId;
                    if (long.TryParse(Session[Constants.LastExpandedNodeIdSessionString].ToString(), out nodeId))
                    {
                        _lastExpandedNodeId = nodeId;
                    }
                }
                return _lastExpandedNodeId;
            }
            set
            {
                _lastExpandedNodeId = value;
                Session[Constants.LastExpandedNodeIdSessionString] = value;
            }
        }

        /// <summary>
        /// Gets or sets the _last highlighted node identifier.
        /// </summary>
        /// <value>
        /// The _last highlighted node identifier.
        /// </value>
        // ReSharper disable once InconsistentNaming
        private long? _lastHighlightedNodeId { get; set; }

        /// <summary>
        /// Gets or sets the last highlighted node identifier.
        /// </summary>
        /// <value>
        /// The last highlighted node identifier.
        /// </value>
        protected long? LastHighlightedNodeId
        {
            get
            {
                if (_lastHighlightedNodeId.HasValue)
                {
                    return _lastHighlightedNodeId;
                }
                if (Session[Constants.LastHighlightedNodeIdSessionString] != null)
                {
                    long nodeId;
                    if (long.TryParse(Session[Constants.LastHighlightedNodeIdSessionString].ToString(), out nodeId))
                    {
                        _lastHighlightedNodeId = nodeId;
                    }
                }
                return _lastHighlightedNodeId;
            }
            set
            {
                _lastHighlightedNodeId = value;
                Session[Constants.LastHighlightedNodeIdSessionString] = value;
            }
        }

        // ReSharper disable once InconsistentNaming
        private long? _lastDeletedNodeId { get; set; }

        /// <summary>
        /// Gets or sets the last highlighted node identifier.
        /// </summary>
        /// <value>
        /// The last highlighted node identifier.
        /// </value>
        protected long? LastDeletedNodeId
        {
            get
            {
                if (_lastDeletedNodeId.HasValue)
                {
                    return _lastDeletedNodeId;
                }
                if (Session[Constants.LastDeletedNodeId] != null)
                {
                    long nodeId;
                    if (long.TryParse(Session[Constants.LastDeletedNodeId].ToString(), out nodeId))
                    {
                        _lastDeletedNodeId = nodeId;
                    }
                }
                return _lastDeletedNodeId;
            }
            set
            {
                _lastDeletedNodeId = value;
                Session[Constants.LastDeletedNodeId] = value;
            }
        }

        /// <summary>
        /// Gets or sets the _last requested node.
        /// </summary>
        /// <value>
        /// The _last requested node.
        /// </value>
        // ReSharper disable once InconsistentNaming
        private ContractHierarchy _lastRequestedNode { get; set; }


        /// <summary>
        /// Gets or sets the last requested node.
        /// </summary>
        /// <value>
        /// The last requested node.
        /// </value>
        protected ContractHierarchy LastRequestedNode
        {
            get
            {
                if (_lastRequestedNode != null)
                {
                    return _lastRequestedNode;
                }
                if (Session[Constants.LastRequestedNodeSessionString] != null)
                {
                    _lastRequestedNode = Session[Constants.LastRequestedNodeSessionString] as ContractHierarchy;
                }
                return _lastRequestedNode;
            }
            set
            {
                _lastRequestedNode = value;
                Session[Constants.LastRequestedNodeSessionString] = value;
            }
        }

        /// <summary>
        /// Sets the current facility identifier.
        /// </summary>
        /// <param name="facilityId">The facility identifier.</param>
        public void SetCurrentFacilityId(int facilityId)
        {
            Session[Constants.CurrentFacilityIdSessionString] = facilityId;
        }

        /// <summary>
        /// Sets the auto refresh information.
        /// </summary>
        /// <param name="isAutoRefresh">The user information.</param>
        public void SetAutoRefresh(bool isAutoRefresh)
        {
            Session[Constants.IsAutoRefresh] = isAutoRefresh;
        }

    }
}