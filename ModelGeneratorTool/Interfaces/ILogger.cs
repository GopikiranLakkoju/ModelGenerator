
namespace ModelGeneratorTool.Interfaces
{
    public interface ILogger
    {
        /// <param name="exception">The exception to extract and log.</param>
        void LogErrorIntoFile(System.Exception exception);
        /// <summary>Logs the specified error.</summary>
        /// <param name="error">The error to log.</param>
        void LogErrorIntoEventLog(string error);
    }
}
