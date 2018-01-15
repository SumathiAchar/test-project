using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SSI.ContractManagement.Shared.Helpers.ErrorLog
{
    /// <summary>
    /// Provides client-side logging services
    /// </summary>
    /// <remarks></remarks>
    public static class Log
    {
        //Log levels are hierarchical.
        //If Fatal is the current level, than all ALERTS and FATAL logs are allowed.  ERROR, WARN, INFO, and DEBUG messages are ignored.
        //If INFO is the current level, than all ALERT, FATAL, ERROR, WARN, and INFO messages are logged. DEBUG messages are ignored.
        /// <summary>
        /// Lists options for the severity of the log entry.
        /// </summary>
        /// <remarks></remarks>
        private enum Level
        {

            /// <summary>
            /// Logging is turned off.
            /// </summary>
            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once UnusedMember.Local
            Off,


            /// <summary>
            /// Log that alerts the development team.
            /// </summary>
            Alert,

            /// <summary>
            /// Log for a fatal error.
            /// </summary>
            // ReSharper disable once UnusedMember.Global
            // ReSharper disable once UnusedMember.Local
            Fatal,

            /// <summary>
            /// Log for a handled exception.
            /// </summary>
            Error,

            /// <summary>
            /// Log for an unusual condition.
            /// </summary>
            Warn,

            /// <summary>
            /// Log for tracking performance, queries, PHI, etc.
            /// </summary>
            Info,

            /// <summary>
            /// Log for debug use only.
            /// </summary>
            Debug
        };

        /// <summary>
        /// Backing field for CurrentLogLevel
        /// </summary>
        private static Level _pLogLevel = Level.Debug;

        /// <summary>
        /// Gets or sets the current threshold for submitting logs.
        /// </summary>
        /// <value>The current log level.</value>
        /// <remarks></remarks>
        // ReSharper disable once UnusedMember.Local
        private static Level CurrentLogLevel { get { return _pLogLevel; } set { _pLogLevel = value; } }

        /// <summary>
        /// Submits a log entry at the Error level.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userName"></param>
        /// <param name="ex"></param>
        /// <remarks></remarks>
        public static void LogError(string message, string userName, Exception ex = null)
        {
            try
            {
                //{ OFF, ALERT, FATAL, ERROR, WARN, INFO, DEBUG}
                if (CurrentLogLevel >= Level.Error)
                {
                    StringBuilder exceptionMessage = new StringBuilder();

                    if (ex != null)
                    {
                        Exception exTemp = ex;
                        exceptionMessage.Append(exTemp.Message);

                        while (exTemp.InnerException != null)
                        {
                            //store the message
                            exceptionMessage.Append(" << " + exTemp.InnerException.Message);

                            //Move the exception to the inner exception;
                            exTemp = exTemp.InnerException;
                        }
                    }

                    StackTrace stackTrace = new StackTrace();           // get call stack
                    StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

                    // write call stack method names
                    if (stackFrames != null)
                    {
                        string stackTraceText = string.Join(" << ", stackFrames.Skip(1).Select(a =>
                        {
                            Type declaringType = a.GetMethod().DeclaringType;
                            return declaringType != null ? string.Format("{0}.{1}", declaringType.ToString(), a.GetMethod().Name) : null;
                        }));
                        string errorMessage =
                            string.Format("{0} :: {1} :: {2} :: {3}", message, exceptionMessage,
                                ex != null ? ex.StackTrace : string.Empty,
                                stackTraceText);
                        CommitLog(errorMessage, userName);
                    }
                }
            }
            catch (Exception exception)
            {
                //What are we going to do, log it?
                string eventsource = GlobalConfigVariable.EventSource;
                EventLog.WriteEntry(string.IsNullOrEmpty(eventsource) ? string.Empty : eventsource, "LogError" + exception.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Submits a log entry at the Info level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="userName"></param>
        /// <remarks></remarks>
        public static void LogInfo(string message, string userName)
        {
            try
            {
                //{ OFF, ALERT, FATAL, ERROR, WARN, INFO, DEBUG}

                if (CurrentLogLevel >= Level.Info)
                {
                    CommitLog(message, userName);
                }
            }
            catch (Exception exception)
            {
                //What are we going to do, log it?
                EventLog.WriteEntry(GlobalConfigVariable.EventSource, exception.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Submits a log entry at the Warn level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="userName"></param>
        /// <remarks></remarks>
        public static void LogWarn(string message, string userName)
        {
            try
            {
                //{ OFF, ALERT, FATAL, ERROR, WARN, INFO, DEBUG}

                if (CurrentLogLevel >= Level.Warn)
                {
                    CommitLog(message, userName);
                }
            }
            catch (Exception exception)
            {
                //What are we going to do, log it?
                EventLog.WriteEntry(GlobalConfigVariable.EventSource, exception.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Submits a log entry at the Alert level.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="userName"></param>
        /// <remarks></remarks>
        // ReSharper disable once UnusedMember.Global
        public static void LogAlert(string message, string userName)
        {
            try
            {
                //{ OFF, ALERT, FATAL, ERROR, WARN, INFO, DEBUG}
                if (CurrentLogLevel >= Level.Alert)
                {
                    CommitLog(message, userName);
                }
            }
            catch (Exception exception)
            {
                //What are we going to do, log it?
                EventLog.WriteEntry(GlobalConfigVariable.EventSource, "LogAlert" + exception.Message, EventLogEntryType.Error);
            }
        }

        /// <summary>
        /// Commit Log
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="userName"></param>
        private static void CommitLog(string errorMessage, string userName)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(GlobalConfigVariable.CmMembershipConnectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand(Constants.SaveErrorLog)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    sqlCommand.Parameters.AddWithValue("@LogLevel", Convert.ToString(CurrentLogLevel));
                    sqlCommand.Parameters.AddWithValue("@UserName", userName);
                    sqlCommand.Parameters.AddWithValue("@ErrorMessage", errorMessage);
                    
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlCommand.ExecuteNonQuery();
                    sqlConnection.Close();
                }
            }
            catch (Exception exception)
            {
                //What are we going to do, log it?
                EventLog.WriteEntry(GlobalConfigVariable.EventSource, exception.Message, EventLogEntryType.Error);
            }
        }
    }
}