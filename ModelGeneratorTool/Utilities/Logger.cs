using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using ModelGeneratorTool.Interfaces;

namespace ModelGeneratorTool.Utilities
{
    class Logger : ILogger
    {
        /// <summary>
        /// <see cref="ILogger.LogErrorIntoFile"/>
        /// </summary>
        public void LogErrorIntoFile(Exception exception)
        {
            var directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (directoryName != null)
            {
                string filename = directoryName.Replace("bin", "BugReports").Replace("Debug", "BugReport.txt");

                lock (new Logger())
                {
                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    stringBuilder.AppendLine("#Exception Cause:");
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.AppendLine("Error is due to, " + exception.Message + " " + DateTime.Now.ToString("MMM dd yyyy, h:mm tt", System.Globalization.CultureInfo.InvariantCulture));
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.AppendLine("#Exception Trace:");
                    stringBuilder.Append(Environment.NewLine);
                    stringBuilder.Append(exception.StackTrace);
                    using (StreamWriter stream = new StreamWriter(filename, false))
                    {
                        stream.Write(stringBuilder.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="ILogger.LogErrorIntoEventLog"/>
        /// </summary>
        public void LogErrorIntoEventLog(string error)
        {
            EventLog log = new EventLog("Application") { Source = Assembly.GetExecutingAssembly().ToString() };
            log.WriteEntry(error, EventLogEntryType.Error);
        }
    }
}
