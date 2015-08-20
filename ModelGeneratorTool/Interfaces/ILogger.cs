
namespace ModelGeneratorTool.Interfaces
{
    public interface ILogger
    {
        /// <summary>Logs the specified error into file</summary>
        /// <param name="error">The error to log.</param>
        void LogErrorIntoFile(string error);
        /// <summary>Logs the specified error.</summary>
        /// <param name="error">The error to log.</param>
        void LogErrorIntoEventLog(string error);        
    }
}
