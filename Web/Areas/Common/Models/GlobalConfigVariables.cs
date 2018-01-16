using System;
using System.Configuration;

namespace SSI.ContractManagement.Web.Areas.Common.Models
{
    public static class GlobalConfigVariables
    {
        /// <summary>
        /// The _CMS application URL
        /// </summary>
        private static int? _numberOfDaysToDismissAlerts;

        private static int? _numberOfDaysToDismissCompletedJobs;

        private static int? _refreshTime;
        private static bool? _isAutoRefreshEnabled;
        private static int? _defaultThresholdDaysToExpireAlters;

        /// <summary>
        /// Gets the number of days automatic dismiss alerts.
        /// </summary>
        /// <value>
        /// The number of days automatic dismiss alerts.
        /// </value>
        public static int NumberOfDaysToDismissAlerts
        {
            get
            {
                if (_numberOfDaysToDismissAlerts.HasValue && _numberOfDaysToDismissAlerts.Value != 0)
                {
                    return _numberOfDaysToDismissAlerts.Value;
                }
                _numberOfDaysToDismissAlerts = 1;
                if (ConfigurationManager.AppSettings["NumberOfDaysToDismissAlerts"] != null)
                {
                    int numberValue;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["NumberOfDaysToDismissAlerts"], out numberValue))
                    {
                        _numberOfDaysToDismissAlerts = numberValue;
                    }
                }
                return _numberOfDaysToDismissAlerts.Value;
            }
        }

        /// <summary>
        /// Gets the number of days to dismiss completed jobs.
        /// </summary>
        /// <value>
        /// The number of days to dismiss completed jobs.
        /// </value>
        public static int NumberOfDaysToDismissCompletedJobs
        {
            get
            {
                if (_numberOfDaysToDismissCompletedJobs.HasValue && _numberOfDaysToDismissCompletedJobs.Value != 0)
                {
                    return _numberOfDaysToDismissCompletedJobs.Value;
                }
                _numberOfDaysToDismissCompletedJobs = 2;
                if (ConfigurationManager.AppSettings["NumberOfDaysToDimissCompletedJobs"] != null)
                {
                    int numberValue;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["NumberOfDaysToDimissCompletedJobs"],
                        out numberValue))
                    {
                        _numberOfDaysToDismissCompletedJobs = numberValue;
                    }
                }
                return _numberOfDaysToDismissCompletedJobs.Value;
            }
        }

        /// <summary>
        /// Gets the Auto Refresh Value for job dash board.
        /// </summary>
        /// <value>
        /// _refreshTime
        /// </value>
        public static int AutoRefreshJobStatus
        {
            get
            {
                if (_refreshTime.HasValue && _refreshTime.Value != 0)
                {
                    return _refreshTime.Value;
                }
                _refreshTime = 20000; //20 Seconds / 20000 Milliseconds
                if (ConfigurationManager.AppSettings["AutoRefresh"] != null)
                {
                    int numberValue;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["AutoRefresh"], out numberValue))
                    {
                        _refreshTime = numberValue;
                    }
                }
                return _refreshTime.Value;
            }
        }


        /// <summary>
        /// Gets a value indicating whether this instance is automatic refresh enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is automatic refresh enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAutoRefreshEnabled
        {
            get
            {
                if (ConfigurationManager.AppSettings["IsAutoRefreshEnabled"] != null)
                {
                    bool numberValue;
                    if (Boolean.TryParse(ConfigurationManager.AppSettings["IsAutoRefreshEnabled"], out numberValue))
                    {
                        _isAutoRefreshEnabled = numberValue;
                    }
                    else
                    {
                        return false;
                    }
                }
                return _isAutoRefreshEnabled != null && _isAutoRefreshEnabled.Value;
            }
        }

        public static int DefaultThresholdDaysToExpireAlters
        {
            get
            {
                if (_defaultThresholdDaysToExpireAlters.HasValue && _defaultThresholdDaysToExpireAlters.Value != 0)
                {
                    return _defaultThresholdDaysToExpireAlters.Value;
                }
                _defaultThresholdDaysToExpireAlters = 60;
                if (ConfigurationManager.AppSettings["DefaultThresholdDaysToExpireAlters"] != null)
                {
                    int numberValue;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["DefaultThresholdDaysToExpireAlters"],
                        out numberValue))
                    {
                        _defaultThresholdDaysToExpireAlters = numberValue;
                    }
                }
                return _defaultThresholdDaysToExpireAlters.Value;
            }
        }

        private static int? _alertPageSize;

        public static int AlertPageSize
        {
            get
            {
                if (_alertPageSize.HasValue && _alertPageSize.Value != 0)
                {
                    return _alertPageSize.Value;
                }
                _alertPageSize = 50;
                if (ConfigurationManager.AppSettings["AlertPerPage"] != null)
                {
                    int size;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["AlertPerPage"], out size))
                    {
                        _alertPageSize = size;
                    }
                }
                return _alertPageSize.Value;
            }
        }

        public static int CheckInternetInterval
        {
            get
            {
                return 3000;
            }
        }
    }
}