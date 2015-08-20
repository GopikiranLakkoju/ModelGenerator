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
        public void LogErrorIntoFile(string error)
        {
            string filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin", "BugReports").Replace("Debug", "BugReport.txt");          
                        
            lock (new Logger())
            {               
                using (StreamWriter stream = new StreamWriter(filename, false))
                {                    
                    stream.Write(error + " " + DateTime.Now.ToString("dd-MM-yyyy"));
                }
            }
        }

        /// <summary>
        /// <see cref="ILogger.LogErrorIntoEventLog"/>
        /// </summary>
        public void LogErrorIntoEventLog(string error)
        {
            EventLog log = new EventLog("Application");
            log.Source = Assembly.GetExecutingAssembly().ToString();
            log.WriteEntry(error, EventLogEntryType.Error);
        }
    }
}
