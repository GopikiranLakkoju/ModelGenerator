using System.Configuration;

namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Database connection string, this class cannot be inherited
    /// </summary>
    public sealed class DbConnection
    {
        public DbConnection()
        {
            ConnnectionString = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }

        /// <summary>
        /// gets connection string, and its readonly property
        /// </summary>
        public string ConnnectionString { get; }

        public static string Version => ConfigurationManager.AppSettings["Version"].ToString();
    }
}