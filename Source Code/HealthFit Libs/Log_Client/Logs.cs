
using System.Diagnostics;

namespace HealthFit_LogClient
{
    /// <summary>
    /// This class implements writing messages and errors into log file.
    /// </summary>
    public class Logs
    {
        private static EventLog eventLog;                                   // System event log.
        private static string LogPath = "";                                 // Log file path.
        private static string LogsFolderName = "Logs";                      // Logs folder name.
        private static string _errorLogsFilePrefix = "HealthFitError";      // Error log files prefix.
        private static string _systemLogsFilePrefix = "HealthFitSystem";    // System log files prefix.

        // Class constructors.
        public Logs()
        {
            // Create event log journal.
            try
            {
                if (!EventLog.SourceExists("HealthFit")) EventLog.CreateEventSource("HealthFit", "HealthFit");

                eventLog = new EventLog();
                eventLog.Log = "HealthFit";
                eventLog.Source = "HealthFit";
            }
            catch (Exception ex)
            {
                eventLog = new EventLog();
                eventLog.Log = "Application";
                eventLog.Source = "HealthFit";

                // Write error message to the system event log.
                WriteSystemError(ex.Message);
            }
        }

        public Logs(string LogsPath)
            : this()
        {
            // Set logs path.
            ApplicationPath = LogsPath;
        }

        /// <summary>
        /// Gets or sets the error logs file prefix.
        /// </summary>
        public static string ErrorLogsFilePrefix
        {
            get { return _errorLogsFilePrefix; }
            set { _errorLogsFilePrefix = value; }
        }

        /// <summary>
        /// Gets or sets the system logs file prefix.
        /// </summary>
        public static string SystemLogsFilePrefix
        {
            get { return _systemLogsFilePrefix; }
            set { _systemLogsFilePrefix = value; }
        }

        /// <summary>
        /// Write error to the log file.
        /// </summary>
        /// <param name="ErrorDesc">the error message</param>
        /// <param name="ErrorSource">the error source</param>
        public static void WriteError(string ErrorSource, string ErrorDesc)
        {
            StreamWriter sw = null;
            string ErrorLogFileName = _errorLogsFilePrefix + DateTime.Now.ToString("dd_MM_yyyy") + ".log";   // Error log file name.

            // Write message to the log file.
            try
            {
                // Check log folder and create if one not exists.
                CreateLogFolder();

                if (LogPath != "")
                {
                    sw = new StreamWriter(File.Open(LogPath + "\\" + ErrorLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + ErrorSource + " -> " + ErrorDesc);
                }
                else
                {
                    // Write error message to the system event log.
                    //WriteSystemError(ErrorDesc);
                }
            }
            catch (Exception ex )
            {
                // Write error message to the system event log.
                //WriteSystemError(ErrorDesc);
                //WriteSystemError(ex.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        /// <summary>
        /// Write error to the log file.
        /// </summary>
        /// <param name="ErrorDesc">the error message</param>
        /// <param name="ErrorSource">the error source</param>
        public static void WriteError(string ErrorDesc)
        {
            StreamWriter sw = null;
            string ErrorLogFileName = _errorLogsFilePrefix + DateTime.Now.ToString("dd_MM_yyyy") + ".log";   // Error log file name.

            // Write message to the log file.
            try
            {
                // Check log folder and create if one not exists.
                CreateLogFolder();

                if (LogPath != "")
                {
                    sw = new StreamWriter(File.Open(LogPath + "\\" + ErrorLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + ErrorDesc);
                }
                else
                {
                    // Write error message to the system event log.
                    WriteSystemError(ErrorDesc);
                }
            }
            catch (Exception ex)
            {
                // Write error message to the system event log.
                WriteSystemError(ErrorDesc);
                WriteSystemError(ex.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        /// <summary>
        /// Write message to the log file.
        /// </summary>
        /// <param name="Message">the message</param>
        public static void WriteMessage(string Message)
        {
            StreamWriter sw = null;
            string SystemLogFileName = _systemLogsFilePrefix + DateTime.Now.ToString("dd_MM_yyyy") + ".log"; // System log file name.

            // Write message into the log file.
            try
            {
                // Check log folder and create if one not exists.
                CreateLogFolder();

                if (LogPath != "")
                {
                    sw = new StreamWriter(File.Open(LogPath + "\\" + SystemLogFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
                    sw.WriteLine(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " " + Message);
                }
                else
                {
                    // Write message to the system event log.
                    WriteSystemMessage(Message);
                }
            }
            catch (Exception ex)
            {
                // Write error message to the system event log.
                WriteSystemMessage(Message);
                WriteSystemError(ex.Message);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }

        /// <summary>
        /// Create log folder.
        /// </summary>
        private static void CreateLogFolder()
        {
            if (string.IsNullOrEmpty(LogPath) == true)
            {
                LogPath = GetLogPath(AppDomain.CurrentDomain.BaseDirectory);
            }
            if (!Directory.Exists(LogPath))
                Directory.CreateDirectory(LogPath);
        }

        /// <summary>
        /// Get log file path.
        /// </summary>
        /// <param name="appPath">the application path</param>
        /// <returns>the log file path</returns>
        private static string GetLogPath(string appPath)
        {
            string LogFilePath;

            LogFilePath = appPath + "\\" + LogsFolderName;

            return LogFilePath;
        }

        /// <summary>
        /// Set application path.
        /// </summary>
        public static string ApplicationPath
        { set { LogPath = GetLogPath(value); } }

        /// <summary>
        /// Write system error to the system event log journal.
        /// </summary>
        /// <param name="SystemError">the system error message</param>
        private static void WriteSystemError(string SystemError)
        {
            try
            {
                eventLog.WriteEntry(SystemError, EventLogEntryType.Error);
            }
            catch { }
        }

        /// <summary>
        /// Write system message to the system event log journal.
        /// </summary>
        /// <param name="SystemError">the system message</param>
        private static void WriteSystemMessage(string SystemMessage)
        {
            try
            {
                eventLog.WriteEntry(SystemMessage, EventLogEntryType.Information);
            }
            catch { }
        }
    }
}