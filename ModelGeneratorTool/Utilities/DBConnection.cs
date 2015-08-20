using System.Configuration;

namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Database connection string, this class cannot be inherited
    /// </summary>
    public sealed class DBConnection
    {
        private string sqlconnection = "";        

        public DBConnection()
        {
            sqlconnection = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
        }

        /// <summary>
        /// gets connection string, and its readonly property
        /// </summary>
        public string ConnnectionString
        {
            get { return sqlconnection; }
        }

        public static string Version
        {
            get { return ConfigurationManager.AppSettings["Version"].ToString(); }
        }
    }
}