using System;
using System.Configuration;

namespace SSI.ContractManagement.Shared.Helpers
{
    public static class GlobalConfigVariable
    {
        /// <summary>
        /// The no of records for adjudicate
        /// </summary>
        private static int? _noOfRecordsForAdjudicate;

        /// <summary>
        /// Gets the no of records for adjudicate.
        /// </summary>
        /// <value>
        /// The no of records for adjudicate.
        /// </value>
        public static int NoOfRecordsForAdjudicate
        {
            get
            {
                if (_noOfRecordsForAdjudicate.HasValue && _noOfRecordsForAdjudicate.Value != 0)
                {
                    return _noOfRecordsForAdjudicate.Value;
                }
                _noOfRecordsForAdjudicate = 150;
                if (ConfigurationManager.AppSettings["NoOfRecordsForAdjudicate"] != null)
                {

                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["NoOfRecordsForAdjudicate"], out count))
                    {
                        _noOfRecordsForAdjudicate = count;
                    }
                }
                return _noOfRecordsForAdjudicate.Value;
            }
        }

        /// <summary>
        /// The _background user name for schedule job
        /// </summary>
        private static string _backgroundUserNameForScheduleJob;


        /// <summary>
        /// Gets the background user name for schedule job.
        /// </summary>
        /// <value>
        /// The background user name for schedule job.
        /// </value>
        public static string BackgroundUserNameForScheduleJob
        {
            get
            {
                if (!(string.IsNullOrEmpty(_backgroundUserNameForScheduleJob) && string.IsNullOrWhiteSpace(_backgroundUserNameForScheduleJob)))
                {
                    return _backgroundUserNameForScheduleJob;
                }
                if (ConfigurationManager.AppSettings["BackgroundUserNameForScheduleJob"] != null)
                {
                    _backgroundUserNameForScheduleJob = ConfigurationManager.AppSettings["BackgroundUserNameForScheduleJob"];
                }
                return _backgroundUserNameForScheduleJob;
            }
        }

        /// <summary>
        /// The _is adjudication using windows service
        /// </summary>
        private static bool? _isAdjudicationUsingWindowsService;

        /// <summary>
        /// Gets a value indicating whether [is adjudication using windows service].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is adjudication using windows service]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAdjudicationUsingWindowsService
        {
            get
            {
                if (_isAdjudicationUsingWindowsService.HasValue)
                {
                    return _isAdjudicationUsingWindowsService.Value;
                }
                _isAdjudicationUsingWindowsService = false;
                if (ConfigurationManager.AppSettings["AdjudicationUsingWindowsService"] != null)
                {
                    bool keyValueFound;
                    if (bool.TryParse(ConfigurationManager.AppSettings["AdjudicationUsingWindowsService"], out keyValueFound))
                    {
                        _isAdjudicationUsingWindowsService = keyValueFound;
                    }
                }
                return _isAdjudicationUsingWindowsService.Value;
            }
        }

        /// <summary>
        /// The _pull data for number of years
        /// </summary>
        private static int? _pullDataForNumberOfYears;
        /// <summary>
        /// Gets the pull data for number of years.
        /// </summary>
        /// <value>
        /// The pull data for number of years.
        /// </value>
        public static int PullDataForNumberOfYears
        {
            get
            {
                if (_pullDataForNumberOfYears.HasValue && _pullDataForNumberOfYears.Value != 0)
                {
                    return _pullDataForNumberOfYears.Value;
                }
                _pullDataForNumberOfYears = 3;
                if (ConfigurationManager.AppSettings["PullDataForNumberOfYears"] != null)
                {
                    int numberValue;
                    if (int.TryParse(ConfigurationManager.AppSettings["PullDataForNumberOfYears"], out numberValue))
                    {
                        _pullDataForNumberOfYears = numberValue;
                    }
                }
                return _pullDataForNumberOfYears.Value;
            }
        }

        /// <summary>
        /// The _is microdyn log enabled
        /// </summary>
        private static bool? _isMicrodynLogEnabled;


        /// <summary>
        /// Gets a value indicating whether [is microdyn log enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is microdyn log enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsMicrodynLogEnabled
        {
            get
            {
                if (_isMicrodynLogEnabled.HasValue)
                {
                    return _isMicrodynLogEnabled.Value;
                }
                _isMicrodynLogEnabled = false;
                if (ConfigurationManager.AppSettings["IsMicrodynLogEnabled"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsMicrodynLogEnabled"], out booleanValue))
                    {
                        _isMicrodynLogEnabled = booleanValue;
                    }
                }
                return _isMicrodynLogEnabled.Value;
            }
        }

        /// <summary>
        /// The _is microdyn enabled
        /// </summary>
        private static bool? _isMicrodynEnabled;


        /// <summary>
        /// Gets a value indicating whether [is microdyn log enabled].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is microdyn log enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsMicrodynEnabled
        {
            get
            {
                if (_isMicrodynEnabled.HasValue)
                {
                    return _isMicrodynEnabled.Value;
                }
                _isMicrodynEnabled = false;
                if (ConfigurationManager.AppSettings["IsMicrodynEnabled"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsMicrodynEnabled"], out booleanValue))
                    {
                        _isMicrodynEnabled = booleanValue;
                    }
                }
                return _isMicrodynEnabled.Value;
            }
        }

        private static bool? _contractLogInsert;

        /// <summary>
        /// Gets a value indicating whether [is contract log insert].
        /// </summary>
        /// <value>
        /// <c>true</c> if [is contract log insert]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsContractLogInsert
        {
            get
            {
                if (_contractLogInsert.HasValue)
                {
                    return _contractLogInsert.Value;
                }
                _contractLogInsert = false;
                if (ConfigurationManager.AppSettings["InsertContractLogs"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["InsertContractLogs"], out booleanValue))
                    {
                        _contractLogInsert = booleanValue;
                    }
                }
                return _contractLogInsert.Value;
            }
        }

        private static bool? _isDmEntry;

        /// <summary>
        /// Gets a value indicating whether [is dm entry].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is dm entry]; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDmEntry
        {
            get
            {
                if (_isDmEntry.HasValue)
                {
                    return _isDmEntry.Value;
                }
                _isDmEntry = false;
                if (ConfigurationManager.AppSettings["IsDMEntry"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsDMEntry"], out booleanValue))
                    {
                        _isDmEntry = booleanValue;
                    }
                }
                return _isDmEntry.Value;
            }
        }

        /// <summary>
        /// The no of claim fields selection
        /// </summary>
        private static int? _maxClaimFieldSelectionCount;

        /// <summary>
        /// Gets the count of claim field selection.
        /// </summary>
        /// <value>
        /// The no of selected claim fields.
        /// </value>
        public static int MaxClaimFieldSelectionCount
        {
            get
            {
                if (_maxClaimFieldSelectionCount.HasValue && _maxClaimFieldSelectionCount.Value != 0)
                {
                    return _maxClaimFieldSelectionCount.Value;
                }
                _maxClaimFieldSelectionCount = 250;
                if (ConfigurationManager.AppSettings["MaxClaimFieldSelectionCount"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxClaimFieldSelectionCount"], out count))
                    {
                        _maxClaimFieldSelectionCount = count;
                    }
                }
                return _maxClaimFieldSelectionCount.Value;
            }
        }



        /// <summary>
        /// Displaying Message
        /// </summary>
        private static string _tooLargeSelectionForClaimField;

        /// <summary>
        /// Gets the Maximum range message.
        /// </summary>
        /// <value>
        /// Error Message for too many claim field selection.
        /// </value>
        public static string TooLargeSelectionForClaimField
        {
            get
            {
                if (!string.IsNullOrEmpty(_tooLargeSelectionForClaimField) && string.IsNullOrWhiteSpace(_tooLargeSelectionForClaimField))
                {
                    return _tooLargeSelectionForClaimField;
                }
                _tooLargeSelectionForClaimField = "Claim Field range selection is too large to process.";
                if (ConfigurationManager.AppSettings["MessageForLargeClaimFieldRangeSelection"] != null)
                {
                    _tooLargeSelectionForClaimField = ConfigurationManager.AppSettings["MessageForLargeClaimFieldRangeSelection"];
                }
                return _tooLargeSelectionForClaimField;
            }
        }



        /// <summary>
        /// The max record limit for excel report
        /// </summary>
        private static int? _maxRecordLimitForExcelReport;


        /// <summary>
        /// Gets the maximum record limit for excel report.
        /// </summary>
        /// <value>
        /// The maximum record limit for excel report.
        /// </value>
        public static int MaxRecordLimitForExcelReport
        {
            get
            {
                if (_maxRecordLimitForExcelReport.HasValue && _maxRecordLimitForExcelReport.Value != 0)
                {
                    return _maxRecordLimitForExcelReport.Value;
                }
                _maxRecordLimitForExcelReport = 30000;
                if (ConfigurationManager.AppSettings["MaxRecordLimitForExcelReport"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxRecordLimitForExcelReport"], out count))
                    {
                        _maxRecordLimitForExcelReport = count;
                    }
                }
                return _maxRecordLimitForExcelReport.Value;
            }
        }


        /// <summary>
        /// The max record limit for telerik report
        /// </summary>
        private static int? _maxRecordLimitForTelericReport;


        /// <summary>
        /// Gets the maximum record limit for telerik report.
        /// </summary>
        /// <value>
        /// The maximum record limit for telerik report.
        /// </value>
        public static int MaxRecordLimitForTelericReport
        {
            get
            {
                if (_maxRecordLimitForTelericReport.HasValue && _maxRecordLimitForTelericReport.Value != 0)
                {
                    return _maxRecordLimitForTelericReport.Value;
                }
                _maxRecordLimitForTelericReport = 2000;
                if (ConfigurationManager.AppSettings["MaxRecordLimitForTelericReport"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxRecordLimitForTelericReport"], out count))
                    {
                        _maxRecordLimitForTelericReport = count;
                    }
                }
                return _maxRecordLimitForTelericReport.Value;
            }
        }

        /// <summary>
        /// Gets the maximum record limit for saving claim adjudication report.
        /// </summary>
        /// <value>
        /// The maximum record limit for saving claim adjudication report.
        /// </value>
        //private static int? _maxPagesForTelerikReport;
        public static int MaxRecordLimitForSavingClaimAdjudicationReport
        {
            get
            {
                if (_maxRecordLimitForTelericReport.HasValue && _maxRecordLimitForTelericReport.Value != 0)
                {
                    return _maxRecordLimitForTelericReport.Value;
                }
                _maxRecordLimitForTelericReport = 80000;
                if (ConfigurationManager.AppSettings["MaxRecordLimitForSavingClaimAdjudicationReport"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxRecordLimitForSavingClaimAdjudicationReport"], out count))
                    {
                        _maxRecordLimitForTelericReport = count;
                    }
                }
                return _maxRecordLimitForTelericReport.Value;
            }
        }

        /// <summary>
        /// The _max pages for telerik report
        /// </summary>
        private static int? _maxPagesForTelerikReport;

        /// <summary>
        /// Gets the maximum pages for telerik report.
        /// </summary>
        /// <value>
        /// The maximum pages for telerik report.
        /// </value>
        public static int MaxPagesForTelerikReport
        {
            get
            {
                if (_maxPagesForTelerikReport.HasValue && _maxPagesForTelerikReport.Value != 0)
                {
                    return _maxPagesForTelerikReport.Value;
                }
                _maxPagesForTelerikReport = 1500;
                if (ConfigurationManager.AppSettings["MaxPagesForTelerikReport"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxPagesForTelerikReport"], out count))
                    {
                        _maxPagesForTelerikReport = count;
                    }
                }
                return _maxPagesForTelerikReport.Value;
            }
        }

        /// <summary>
        /// The _max number of lines in PDF page
        /// </summary>
        private static int? _maxNumberOfLinesInPdfPage;

        /// <summary>
        /// Gets the maximum number of lines in PDF page.
        /// </summary>
        /// <value>
        /// The maximum number of lines in PDF page.
        /// </value>
        public static int MaxNumberOfLinesInPdfPage
        {
            get
            {
                if (_maxNumberOfLinesInPdfPage.HasValue && _maxNumberOfLinesInPdfPage.Value != 0)
                {
                    return _maxNumberOfLinesInPdfPage.Value;
                }
                _maxNumberOfLinesInPdfPage = 24;
                if (ConfigurationManager.AppSettings["MaxNumberOfLinesInPdfPage"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxNumberOfLinesInPdfPage"], out count))
                    {
                        _maxNumberOfLinesInPdfPage = count;
                    }
                }
                return _maxNumberOfLinesInPdfPage.Value;
            }
        }

        /// <summary>
        /// The _reports file path
        /// </summary>
        private static string _reportsFilePath;

        /// <summary>
        /// Gets the reports file path.
        /// </summary>
        /// <value>
        /// The reports file path.
        /// </value>
        public static string ReportsFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_reportsFilePath) && string.IsNullOrWhiteSpace(_reportsFilePath))
                {
                    return _reportsFilePath;
                }
                _reportsFilePath = "/";
                if (ConfigurationManager.AppSettings["ReportsFilePath"] != null)
                {
                    _reportsFilePath = ConfigurationManager.AppSettings["ReportsFilePath"];
                }
                return _reportsFilePath;
            }
        }

        /// <summary>
        /// The _letter template image path
        /// </summary>
        private static string _letterTemplateImagePath;

        /// <summary>
        /// Gets the letter template image path.
        /// </summary>
        /// <value>
        /// The letter template image path.
        /// </value>
        public static string LetterTemplateImagePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_letterTemplateImagePath) && string.IsNullOrWhiteSpace(_letterTemplateImagePath))
                {
                    return _letterTemplateImagePath;
                }
                _letterTemplateImagePath = "/";
                if (ConfigurationManager.AppSettings["LetterTemplateImagePath"] != null)
                {
                    _letterTemplateImagePath = ConfigurationManager.AppSettings["LetterTemplateImagePath"];
                }
                return _letterTemplateImagePath;
            }
        }

        /// <summary>
        /// The _command timeout for claim adjudication
        /// </summary>
        private static int? _commandTimeoutForClaimAdjudication;
        /// <summary>
        /// Gets the command timeout for claim adjudication.
        /// </summary>
        /// <value>
        /// The command timeout for claim adjudication.
        /// </value>
        public static int CommandTimeoutForClaimAdjudication
        {
            get
            {
                if (_commandTimeoutForClaimAdjudication.HasValue && _commandTimeoutForClaimAdjudication.Value != 0)
                {
                    return _commandTimeoutForClaimAdjudication.Value;
                }
                _commandTimeoutForClaimAdjudication = 7000;
                if (ConfigurationManager.AppSettings["CommandTimeoutForClaimAdjudication"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeoutForClaimAdjudication"], out count))
                    {
                        _commandTimeoutForClaimAdjudication = count;
                    }
                }
                return _commandTimeoutForClaimAdjudication.Value;
            }
        }

        /// <summary>
        /// The application home path
        /// </summary>
        private static string _userManualFilePath;

        /// <summary>
        /// Gets the user manual file path.
        /// </summary>
        /// <value>
        /// The user manual file path.
        /// </value>
        public static string UserManualFilePath
        {
            get
            {
                if (!string.IsNullOrEmpty(_userManualFilePath) && string.IsNullOrWhiteSpace(_userManualFilePath))
                {
                    return _userManualFilePath;
                }
                _userManualFilePath = "/";
                if (ConfigurationManager.AppSettings["UserManualFilePath"] != null)
                {
                    _userManualFilePath = ConfigurationManager.AppSettings["UserManualFilePath"];
                }
                return _userManualFilePath;
            }
        }

        /// <summary>
        /// The _app data virtual path
        /// </summary>
        private static string _appDataVirtualPath;

        /// <summary>
        /// Gets the application data virtual path.
        /// </summary>
        /// <value>
        /// The application data virtual path.
        /// </value>
        public static string AppDataVirtualPath
        {
            get
            {
                if (!(string.IsNullOrEmpty(_appDataVirtualPath) && string.IsNullOrWhiteSpace(_appDataVirtualPath)))
                {
                    return _appDataVirtualPath;
                }
                if (ConfigurationManager.AppSettings["AppDataVirtualPath"] != null)
                {
                    _appDataVirtualPath = ConfigurationManager.AppSettings["AppDataVirtualPath"];
                }
                return _appDataVirtualPath;
            }
        }

        /// <summary>
        /// The _is request log enabled
        /// </summary>
        private static bool? _isRequestLogEnabled;
        /// <summary>
        /// Gets a value indicating whether this instance is request log enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is request log enabled; otherwise, <c>false</c>.
        /// </value>
        public static bool IsRequestLogEnabled
        {
            get
            {
                if (_isRequestLogEnabled.HasValue)
                {
                    return _isRequestLogEnabled.Value;
                }
                _isRequestLogEnabled = false;
                if (ConfigurationManager.AppSettings["IsRequestLogEnabled"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsRequestLogEnabled"], out booleanValue))
                    {
                        _isRequestLogEnabled = booleanValue;
                    }
                }
                return _isRequestLogEnabled.Value;
            }
        }

        /// <summary>
        /// The _letter template maximum image size
        /// </summary>
        private static int? _letterTemplateMaxImageSize;
        /// <summary>
        /// Gets the size of the letter template maximum image.
        /// </summary>
        /// <value>
        /// The size of the letter template maximum image.
        /// </value>
        public static int LetterTemplateMaxImageSize
        {
            get
            {
                if (_letterTemplateMaxImageSize.HasValue && _letterTemplateMaxImageSize.Value != 0)
                {
                    return _letterTemplateMaxImageSize.Value;
                }
                _letterTemplateMaxImageSize = 256;
                if (ConfigurationManager.AppSettings["LetterTemplateMaxImageSize"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["LetterTemplateMaxImageSize"], out size))
                    {
                        _letterTemplateMaxImageSize = size;
                    }
                }
                return _letterTemplateMaxImageSize.Value;
            }
        }

        /// <summary>
        /// The _max no of data for letters
        /// </summary>
        private static int? _maxNoOfDataForLetters;
        /// <summary>
        /// Gets the maximum no of data for letters.
        /// </summary>
        /// <value>
        /// The maximum no of data for letters.
        /// </value>
        public static int MaxNoOfDataForLetters
        {
            get
            {
                if (_maxNoOfDataForLetters.HasValue && _maxNoOfDataForLetters.Value != 0)
                {
                    return _maxNoOfDataForLetters.Value;
                }
                _maxNoOfDataForLetters = 300;
                if (ConfigurationManager.AppSettings["MaxNoOfDataForLetters"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["MaxNoOfDataForLetters"], out size))
                    {
                        _maxNoOfDataForLetters = size;
                    }
                }
                return _maxNoOfDataForLetters.Value;
            }
        }

        /// <summary>
        /// The _idle timeout
        /// </summary>
        private static int? _idleTimeout;
        /// <summary>
        /// Gets the idle timeout.
        /// </summary>
        /// <value>
        /// The idle timeout.
        /// </value>
        public static int IdleTimeout
        {
            get
            {
                if (_idleTimeout.HasValue && _idleTimeout.Value != 0)
                {
                    return _idleTimeout.Value;
                }
                _idleTimeout = 3600000;
                if (ConfigurationManager.AppSettings["IdleTimeout"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["IdleTimeout"], out size))
                    {
                        _idleTimeout = size;
                    }
                }
                return _idleTimeout.Value;
            }
        }

        /// <summary>
        /// The _sleep interval when manual job is running
        /// </summary>
        private static int? _sleepIntervalWhenManualJobIsRunning;
        /// <summary>
        /// Gets the sleep interval when manuall job is running.
        /// </summary>
        /// <value>
        /// The sleep interval when manuall job is running.
        /// </value>
        public static int SleepIntervalWhenManualJobIsRunning
        {
            get
            {
                if (_sleepIntervalWhenManualJobIsRunning.HasValue && _sleepIntervalWhenManualJobIsRunning.Value != 0)
                {
                    return _sleepIntervalWhenManualJobIsRunning.Value;
                }
                _sleepIntervalWhenManualJobIsRunning = 120000;
                if (ConfigurationManager.AppSettings["SleepIntervalForWhenManuallJobIsRunning"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["SleepIntervalForWhenManuallJobIsRunning"], out size))
                    {
                        _sleepIntervalWhenManualJobIsRunning = size;
                    }
                }
                return _sleepIntervalWhenManualJobIsRunning.Value;
            }
        }

        /// <summary>
        /// The _sleep interval for background job
        /// </summary>
        private static int? _sleepIntervalForBackgroundJob;
        /// <summary>
        /// Gets the sleep interval for background job.
        /// </summary>
        /// <value>
        /// The sleep interval for background job.
        /// </value>
        public static int SleepIntervalWhenBackgroundJobCompletes
        {
            get
            {
                if (_sleepIntervalForBackgroundJob.HasValue && _sleepIntervalForBackgroundJob.Value != 0)
                {
                    return _sleepIntervalForBackgroundJob.Value;
                }
                _sleepIntervalForBackgroundJob = 3600000;
                if (ConfigurationManager.AppSettings["SleepIntervalForBackgroundJob"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["SleepIntervalForBackgroundJob"], out size))
                    {
                        _sleepIntervalForBackgroundJob = size;
                    }
                }
                return _sleepIntervalForBackgroundJob.Value;
            }
        }

        /// <summary>
        /// The _batch size for background adjudication
        /// </summary>
        private static int? _batchSizeForBackgroundAdjudication;
        /// <summary>
        /// Gets the batch size for background adjudication.
        /// </summary>
        /// <value>
        /// The batch size for background adjudication.
        /// </value>
        public static int BatchSizeForBackgroundAdjudication
        {
            get
            {
                if (_batchSizeForBackgroundAdjudication.HasValue && _batchSizeForBackgroundAdjudication.Value != 0)
                {
                    return _batchSizeForBackgroundAdjudication.Value;
                }
                _batchSizeForBackgroundAdjudication = 200;
                if (ConfigurationManager.AppSettings["BatchSizeForBackgroundAdjudication"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["BatchSizeForBackgroundAdjudication"], out size))
                    {
                        _batchSizeForBackgroundAdjudication = size;
                    }
                }
                return _batchSizeForBackgroundAdjudication.Value;
            }
        }

        /// <summary>
        /// The _event source
        /// </summary>
        private static string _eventSource;
        /// <summary>
        /// Gets the event source.
        /// </summary>
        /// <value>
        /// The event source.
        /// </value>
        public static string EventSource
        {
            get
            {
                if (!(string.IsNullOrEmpty(_eventSource) && string.IsNullOrWhiteSpace(_eventSource)))
                {
                    return _eventSource;
                }
                if (ConfigurationManager.AppSettings["EventSource"] != null)
                {
                    _eventSource = ConfigurationManager.AppSettings["EventSource"];
                }
                return _eventSource;
            }
        }

        /// <summary>
        /// The _lock out minutes
        /// </summary>
        private static int? _lockOutMinutes;
        /// <summary>
        /// Gets the lock out minutes.
        /// </summary>
        /// <value>
        /// The lock out minutes.
        /// </value>
        public static int LockOutMinutes
        {
            get
            {
                if (_lockOutMinutes.HasValue && _lockOutMinutes.Value != 0)
                {
                    return _lockOutMinutes.Value;
                }
                _lockOutMinutes = 30;
                if (ConfigurationManager.AppSettings["LockOutMinutes"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["LockOutMinutes"], out size))
                    {
                        _lockOutMinutes = size;
                    }
                }
                return _lockOutMinutes.Value;
            }
        }

        /// <summary>
        /// The _web apiurl
        /// </summary>
        private static string _webApiurl;
        /// <summary>
        /// Gets the web apiurl.
        /// </summary>
        /// <value>
        /// The web apiurl.
        /// </value>
        public static string WebApiurl
        {
            get
            {
                if (!(string.IsNullOrEmpty(_webApiurl) && string.IsNullOrWhiteSpace(_webApiurl)))
                {
                    return _webApiurl;
                }
                if (ConfigurationManager.AppSettings["WebAPIURL"] != null)
                {
                    _webApiurl = ConfigurationManager.AppSettings["WebAPIURL"];
                }
                return _webApiurl;
            }
        }


        /// <summary>
        /// The _HTTP client time out
        /// </summary>
        private static int? _httpClientTimeOut;
        /// <summary>
        /// Gets the HTTP client time out.
        /// </summary>
        /// <value>
        /// The HTTP client time out.
        /// </value>
        public static int HttpClientTimeOut
        {
            get
            {
                if (_httpClientTimeOut.HasValue && _httpClientTimeOut.Value != 0)
                {
                    return _httpClientTimeOut.Value;
                }
                _httpClientTimeOut = 1000000;
                if (ConfigurationManager.AppSettings["HttpClientTimeOut"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["HttpClientTimeOut"], out size))
                    {
                        _httpClientTimeOut = size;
                    }
                }
                return _httpClientTimeOut.Value;
            }
        }

        /// <summary>
        /// The _command timeout for claims selection
        /// </summary>
        private static int? _commandTimeoutForClaimsSelection;
        /// <summary>
        /// Gets the command timeout for claims selection.
        /// </summary>
        /// <value>
        /// The command timeout for claims selection.
        /// </value>
        public static int CommandTimeoutForClaimsSelection
        {
            get
            {
                if (_commandTimeoutForClaimsSelection.HasValue && _commandTimeoutForClaimsSelection.Value != 0)
                {
                    return _commandTimeoutForClaimsSelection.Value;
                }
                _commandTimeoutForClaimsSelection = 7200;
                if (ConfigurationManager.AppSettings["CommandTimeoutForClaimsSelection"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeoutForClaimsSelection"], out size))
                    {
                        _commandTimeoutForClaimsSelection = size;
                    }
                }
                return _commandTimeoutForClaimsSelection.Value;
            }
        }

        /// <summary>
        /// The _command timeout for check adjudication request name exist
        /// </summary>
        private static int? _commandTimeoutForCheckAdjudicationRequestNameExist;
        /// <summary>
        /// Gets the command timeout for check adjudication request name exist.
        /// </summary>
        /// <value>
        /// The command timeout for check adjudication request name exist.
        /// </value>
        public static int CommandTimeoutForCheckAdjudicationRequestNameExist
        {
            get
            {
                if (_commandTimeoutForCheckAdjudicationRequestNameExist.HasValue && _commandTimeoutForCheckAdjudicationRequestNameExist.Value != 0)
                {
                    return _commandTimeoutForCheckAdjudicationRequestNameExist.Value;
                }
                _commandTimeoutForCheckAdjudicationRequestNameExist = 2000;
                if (ConfigurationManager.AppSettings["CommandTimeoutForCheckAdjudicationRequestNameExist"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeoutForCheckAdjudicationRequestNameExist"], out size))
                    {
                        _commandTimeoutForCheckAdjudicationRequestNameExist = size;
                    }
                }
                return _commandTimeoutForCheckAdjudicationRequestNameExist.Value;
            }
        }

        /// <summary>
        /// The _command timeout
        /// </summary>
        private static int? _commandTimeout;
        /// <summary>
        /// Gets the command timeout.
        /// </summary>
        /// <value>
        /// The command timeout.
        /// </value>
        public static int CommandTimeout
        {
            get
            {
                if (_commandTimeout.HasValue && _commandTimeout.Value != 0)
                {
                    return _commandTimeout.Value;
                }
                _commandTimeout = 2000;
                if (ConfigurationManager.AppSettings["CommandTimeout"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeout"], out size))
                    {
                        _commandTimeout = size;
                    }
                }
                return _commandTimeout.Value;
            }
        }

        /// <summary>
        /// The _command timeout for contract hierarchy
        /// </summary>
        private static int? _commandTimeoutForContractHierarchy;
        /// <summary>
        /// Gets the command timeout for contract hierarchy.
        /// </summary>
        /// <value>
        /// The command timeout for contract hierarchy.
        /// </value>
        public static int CommandTimeoutForContractHierarchy
        {
            get
            {
                if (_commandTimeoutForContractHierarchy.HasValue && _commandTimeoutForContractHierarchy.Value != 0)
                {
                    return _commandTimeoutForContractHierarchy.Value;
                }
                _commandTimeoutForContractHierarchy = 3000;
                if (ConfigurationManager.AppSettings["CommandTimeoutForContractHierarchy"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["CommandTimeoutForContractHierarchy"], out size))
                    {
                        _commandTimeoutForContractHierarchy = size;
                    }
                }
                return _commandTimeoutForContractHierarchy.Value;
            }
        }

        /// <summary>
        /// The _is user name in title case
        /// </summary>
        private static bool? _isUserNameInTitleCase;
        /// <summary>
        /// Gets a value indicating whether this instance is user name in title case.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is user name in title case; otherwise, <c>false</c>.
        /// </value>
        public static bool IsUserNameInTitleCase
        {
            get
            {
                if (_isUserNameInTitleCase.HasValue)
                {
                    return _isUserNameInTitleCase.Value;
                }
                _isUserNameInTitleCase = false;
                if (ConfigurationManager.AppSettings["IsUserNameInTitleCase"] != null)
                {
                    bool booleanValue;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsUserNameInTitleCase"], out booleanValue))
                    {
                        _isUserNameInTitleCase = booleanValue;
                    }
                }
                return _isUserNameInTitleCase.Value;
            }
        }

        /// <summary>
        /// The _claim field values per call
        /// </summary>
        private static int? _claimFieldValuesPerCall;
        /// <summary>
        /// Gets the claim field values per call.
        /// </summary>
        /// <value>
        /// The claim field values per call.
        /// </value>
        public static int ClaimFieldValuesPerCall
        {
            get
            {
                if (_claimFieldValuesPerCall.HasValue && _claimFieldValuesPerCall.Value != 0)
                {
                    return _claimFieldValuesPerCall.Value;
                }
                _claimFieldValuesPerCall = 5000;
                if (ConfigurationManager.AppSettings["ClaimFieldValuesPerCall"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["ClaimFieldValuesPerCall"], out size))
                    {
                        _claimFieldValuesPerCall = size;
                    }
                }
                return _claimFieldValuesPerCall.Value;
            }
        }

        /// <summary>
        /// The _service line codes page size
        /// </summary>
        private static int? _serviceLineCodesPageSize;
        /// <summary>
        /// Gets the size of the service line codes page.
        /// </summary>
        /// <value>
        /// The size of the service line codes page.
        /// </value>
        public static int ServiceLineCodesPageSize
        {
            get
            {
                if (_serviceLineCodesPageSize.HasValue && _serviceLineCodesPageSize.Value != 0)
                {
                    return _serviceLineCodesPageSize.Value;
                }
                _serviceLineCodesPageSize = 500;
                if (ConfigurationManager.AppSettings["ServiceLineCodesPageSize"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["ServiceLineCodesPageSize"], out size))
                    {
                        _serviceLineCodesPageSize = size;
                    }
                }
                return _serviceLineCodesPageSize.Value;
            }
        }

        /// <summary>
        /// The _alert page size
        /// </summary>
        private static int? _alertPageSize;

        /// <summary>
        /// Gets the size of the alert page.
        /// </summary>
        /// <value>
        /// The size of the alert page.
        /// </value>
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

        /// <summary>
        /// The _open claim record
        /// </summary>
        private static int? _openClaimRecord;
        /// <summary>
        /// Gets the open claim record.
        /// </summary>
        /// <value>
        /// The open claim record.
        /// </value>
        public static int OpenClaimRecord
        {
            get
            {
                if (_openClaimRecord.HasValue && _openClaimRecord.Value != 0)
                {
                    return _openClaimRecord.Value;
                }
                _openClaimRecord = 100000;
                if (ConfigurationManager.AppSettings["OpenClaimRecord"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["OpenClaimRecord"], out size))
                    {
                        _openClaimRecord = size;
                    }
                }
                return _openClaimRecord.Value;
            }
        }

        /// <summary>
        /// The _time out background adjudication
        /// </summary>
        private static int? _timeOutBackgroundAdjudication;
        /// <summary>
        /// Gets the application service path.
        /// </summary>
        /// <value>
        /// The application service path.
        /// </value>
        public static int TimeOutBackgroundAdjudication
        {
            get
            {

                if (_timeOutBackgroundAdjudication.HasValue && _timeOutBackgroundAdjudication.Value != 0)
                {
                    return _timeOutBackgroundAdjudication.Value;
                }
                _timeOutBackgroundAdjudication = 5400;
                if (ConfigurationManager.AppSettings["TimeOutBackgroundAdjudication"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["TimeOutBackgroundAdjudication"], out size))
                    {
                        _timeOutBackgroundAdjudication = size;
                    }
                }
                return _timeOutBackgroundAdjudication.Value;
            }
        }

        /// <summary>
        /// The _is log event
        /// </summary>
        private static bool? _isLogEvent;


        /// <summary>
        /// Gets a value indicating whether this instance is log event.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is log event; otherwise, <c>false</c>.
        /// </value>
        public static bool IsLogEvent
        {
            get
            {
                if (_isLogEvent.HasValue)
                {
                    return _isLogEvent.Value;
                }
                _isLogEvent = false;
                if (ConfigurationManager.AppSettings["IsLogEvent"] != null)
                {
                    bool keyValueFound;
                    if (bool.TryParse(ConfigurationManager.AppSettings["IsLogEvent"], out keyValueFound))
                    {
                        _isLogEvent = keyValueFound;
                    }
                }
                return _isLogEvent.Value;
            }
        }

        /// <summary>
        /// The _payment table file size
        /// </summary>
        private static int? _paymentTableFileSize;

        /// <summary>
        /// Gets the adjustment option.
        /// </summary>
        /// <value>
        /// The adjustment option.
        /// </value>
        public static int PaymentTableFileSize
        {
            get
            {
                if (_paymentTableFileSize.HasValue && _paymentTableFileSize.Value != 0)
                {
                    return _paymentTableFileSize.Value;
                }
                _paymentTableFileSize = 0;
                if (ConfigurationManager.AppSettings["PaymentTableFileSize"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["PaymentTableFileSize"], out count))
                    {
                        _paymentTableFileSize = count;
                    }
                }
                return _paymentTableFileSize.Value;
            }
        }

        private static int? _contractDocumentFileSize;

        /// <summary>
        /// Gets the adjustment option.
        /// </summary>
        /// <value>
        /// The adjustment option.
        /// </value>
        public static int ContractDocumentFileSize
        {
            get
            {
                if (_contractDocumentFileSize.HasValue && _contractDocumentFileSize.Value != 0)
                {
                    return _contractDocumentFileSize.Value;
                }
                _contractDocumentFileSize = 0;
                if (ConfigurationManager.AppSettings["ContractDocumentFileSize"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["ContractDocumentFileSize"], out count))
                    {
                        _contractDocumentFileSize = count;
                    }
                }
                return _contractDocumentFileSize.Value;
            }
        }

        /// <summary>
        /// The _contractdocuments path
        /// </summary>
        private static string _contractdocumentsPath;

        /// <summary>
        /// Gets the contract documents path.
        /// </summary>
        /// <value>
        /// The contract documents path.
        /// </value>
        public static string ContractDocumentsPath
        {
            get
            {
                if (!(string.IsNullOrEmpty(_contractdocumentsPath) && string.IsNullOrWhiteSpace(_contractdocumentsPath)))
                {
                    return _contractdocumentsPath;
                }
                _contractdocumentsPath = "/";
                if (ConfigurationManager.AppSettings["ContractDocumentsPath"] != null)
                {
                    _contractdocumentsPath = ConfigurationManager.AppSettings["ContractDocumentsPath"];
                }
                return _contractdocumentsPath;
            }
        }

        /// <summary>
        /// The _claim field values custom length
        /// </summary>
        private static int? _claimFieldValuesCustomLength;

        /// <summary>
        /// Gets the length of the claim field values custom.
        /// </summary>
        /// <value>
        /// The length of the claim field values custom.
        /// </value>
        public static int ClaimFieldValuesCustomLength
        {
            get
            {
                if (_claimFieldValuesCustomLength.HasValue && _claimFieldValuesCustomLength.Value != 0)
                {
                    return _claimFieldValuesCustomLength.Value;
                }
                _claimFieldValuesCustomLength = 7000;
                if (ConfigurationManager.AppSettings["ClaimFieldValuesCustomLength"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["ClaimFieldValuesCustomLength"], out size))
                    {
                        _claimFieldValuesCustomLength = size;
                    }
                }
                return _claimFieldValuesCustomLength.Value;
            }
        }

        /// <summary>
        /// The _claim field values maximum records
        /// </summary>
        private static int? _claimFieldValuesMaxRecords;

        /// <summary>
        /// Gets the claim field values custom maximum records.
        /// </summary>
        /// <value>
        /// The claim field values custom maximum records.
        /// </value>
        public static int ClaimFieldValuesCustomMaxRecords
        {
            get
            {
                if (_claimFieldValuesMaxRecords.HasValue && _claimFieldValuesMaxRecords.Value != 0)
                {
                    return _claimFieldValuesMaxRecords.Value;
                }
                _claimFieldValuesMaxRecords = 7000;
                if (ConfigurationManager.AppSettings["ClaimFieldValuesCustomMaxRecords"] != null)
                {
                    int size;
                    if (int.TryParse(ConfigurationManager.AppSettings["ClaimFieldValuesCustomMaxRecords"], out size))
                    {
                        _claimFieldValuesMaxRecords = size;
                    }
                }
                return _claimFieldValuesMaxRecords.Value;
            }
        }


        /// <summary>
        /// The _reassign page size
        /// </summary>
        private static int? _reassignPageSize;

        /// <summary>
        /// Gets the size of the reassign page.
        /// </summary>
        /// <value>
        /// The size of the reassign page.
        /// </value>
        public static int ReassignPageSize
        {
            get
            {
                if (_reassignPageSize.HasValue && _reassignPageSize.Value != 0)
                {
                    return _reassignPageSize.Value;
                }
                _reassignPageSize = 10;
                if (ConfigurationManager.AppSettings["ReassignPerPage"] != null)
                {
                    int size;
                    if (Int32.TryParse(ConfigurationManager.AppSettings["ReassignPerPage"], out size))
                    {
                        _reassignPageSize = size;
                    }
                }
                return _reassignPageSize.Value;
            }
        }


        /// <summary>
        /// The _event source
        /// </summary>
        private static string _dbName;
        /// <summary>
        /// Gets the event source.
        /// </summary>
        /// <value>
        /// The event source.
        /// </value>
        //FIXED-2016-R3-S3 : Rename DBName to DbName
        public static string DbName
        {
            get
            {
                if (!(string.IsNullOrEmpty(_dbName) && string.IsNullOrWhiteSpace(_dbName)))
                {
                    return _dbName;
                }
                if (ConfigurationManager.AppSettings["DbName"] != null)
                {
                    _dbName = ConfigurationManager.AppSettings["DbName"];
                }
                return _dbName;
            }
        }

        /// <summary>
        /// The _cmMembershipConnectionString
        /// </summary>
        private static string _cmMembershipConnectionString;

        /// <summary>
        /// Gets the CM Membership ConnectionString.
        /// </summary>
        /// <value>
        /// The CM Membership ConnectionString.
        /// </value>
        public static string CmMembershipConnectionString
        {
            get
            {
                if (!(string.IsNullOrEmpty(_cmMembershipConnectionString) && string.IsNullOrWhiteSpace(_cmMembershipConnectionString)))
                {
                    return _cmMembershipConnectionString;
                }
                if (ConfigurationManager.ConnectionStrings["CMMembershipConnectionString"] != null)
                {
                    _cmMembershipConnectionString = ConfigurationManager.ConnectionStrings["CMMembershipConnectionString"].ConnectionString;
                }
                return _cmMembershipConnectionString;
            }
        }

        /// <summary>
        /// The no of Thread Count
        /// </summary>
        private static int? _threadCount;

        /// <summary>
        /// Gets the Thread Count.
        /// </summary>
        /// <value>
        /// The Thread Count.
        /// </value>
        public static int ThreadCount
        {
            get
            {
                if (_threadCount.HasValue && _threadCount.Value != 0)
                {
                    return _threadCount.Value;
                }
                _threadCount = 1;
                if (ConfigurationManager.AppSettings["ThreadCount"] != null)
                {

                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["ThreadCount"], out count))
                    {
                        _threadCount = count;
                    }
                }
                return _threadCount.Value;
            }
        }

        /// <summary>
        /// The number of Thread Count for Background
        /// </summary>
        private static int? _threadCountForBackground;

        /// <summary>
        /// Gets the number of Thread Count for Background
        /// </summary>
        /// <value>
        /// The Thread Count.
        /// </value>
        public static int ThreadCountForBackground
        {
            get
            {
                if (_threadCountForBackground.HasValue && _threadCountForBackground.Value != 0)
                {
                    return _threadCountForBackground.Value;
                }
                _threadCountForBackground = 5;
                if (ConfigurationManager.AppSettings["ThreadCountForBackground"] != null)
                {
                    int count;
                    if (int.TryParse(ConfigurationManager.AppSettings["ThreadCountForBackground"], out count))
                    {
                        _threadCountForBackground = count;
                    }
                }
                return _threadCountForBackground.Value;
            }
        }
    }
}