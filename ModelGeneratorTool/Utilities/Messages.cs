namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Static Messages
    /// </summary>
    internal class Messages
    {
        /// <summary>
        /// Tables loaded Successful
        /// </summary>
        public const string ConnectionSuccessful = "Tables loaded Successful";
        /// <summary>
        /// Failed to load tables, please check the inputs and proceed..
        /// </summary>
        public const string ConnectionFailed = "Failed to load tables, please check the inputs and proceed..";
        /// <summary>
        /// Entered DataSource {0} already exists, please check and proceed..
        /// </summary>
        public const string RedundantDataSource = "Entered DataSource {0} already exists, please check and proceed..";
        /// <summary>
        /// Entered Database {0} already exists, please check and proceed..
        /// </summary>
        public const string RedundantDatabase = "Entered Database {0} already exists, please check and proceed..";
        /// <summary>
        /// Entered Schema {0} already exists, please check and proceed..
        /// </summary>
        public const string RedundantSchema = "Entered Schema {0} already exists, please check and proceed..";
        /// <summary>
        /// Successfully Saved the connection
        /// </summary>
        public const string ConnectionSaveSuccessful = "Successfully Saved the connection";
        /// <summary>
        /// Failed to save, please check in BugReports!
        /// </summary>
        public const string ConnectionSaveUnSuccessful = "Failed to save, please check in BugReports!";
        /// <summary>
        /// Please Enter DataSource, Database and Schema to save connection..
        /// </summary>
        public const string EnterMandatoryFields = "Please Enter DataSource, Database and Schema to proceed..";
        /// <summary>
        /// Successfully deleted the connection
        /// </summary>
        public const string DeletionSuccessful = "Successfully deleted the connection";
        /// <summary>
        /// Input {0}, {1} and {2} provided are not found
        /// </summary>
        public const string DeletionUnSuccessful = "Input {0}, {1} and {2} provided are not found";
        /// <summary>
        /// Please enter the Database, Table\Columns and Provider!
        /// </summary>
        public const string DatabaseTableProviderMandate = "Please enter the Database, Table\\Columns and Provider!";
        /// <summary>
        /// Entered table name is not found!
        /// </summary>
        public const string TableNotFound = "Please check the table selected..";
        /// <summary>
        /// ORACLE
        /// </summary>
        public const string Oracle = "ORACLE";
        /// <summary>
        /// SQL SERVER
        /// </summary>
        public const string SQLSERVER = "SQL SERVER";
        /// <summary>
        /// Support for Oracle coming soon..
        /// </summary>
        public const string OracleSupport = "Support for Oracle coming soon..";
        /// <summary>
        /// Successfully created '{0}' model..
        /// </summary>
        public const string ModelCreationSuccessful = "Successfully created model file(s)..";
        /// <summary>
        /// Input provided are already present..
        /// </summary>
        public const string ConnectionAlreadyPresent = "Input provided already exist..";
        /// <summary>
        /// Data Source={0};Database={1};Integrated Security=True
        /// </summary>
        public const string SourceDatabaseSchema = "Data Source={0};Database={1};Integrated Security=True";
        /// <summary>
        /// SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = '{0}' AND TABLE_SCHEMA = '{1}' AND TABLE_TYPE <> 'VIEW'
        /// </summary>
        public const string SchemaOnTables = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_CATALOG = '{0}' AND TABLE_SCHEMA = '{1}' AND TABLE_TYPE <> 'VIEW'";
        /// <summary>
        /// SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, TABLE_SCHEMA, TABLE_CATALOG from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'
        /// </summary>
        public const string SchemaOnColumns = "SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE, TABLE_SCHEMA, TABLE_CATALOG from INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{0}'";
        /// <summary>
        /// Testing connectivity...
        /// </summary>
        public const string TestingConnection = "Testing connectivity...";
        /// <summary>
        /// Saving connectivity...
        /// </summary>
        public const string SavingConnection = "Saving connectivity...";
        /// <summary>
        /// Deleting connectivity...
        /// </summary>
        public const string DeletingConnection = "Deleting connectivity...";
        /// <summary>
        /// Select any value from columns list...
        /// </summary>
        public const string SelectAnyColumns = "Select any value from columns list...";
        /// <summary>
        /// Please Enter Model Name
        /// </summary>
        public const string ModelNameMissing = "Please Enter Model Name";
        /// <summary>
        /// Custom Properties entered are invalid, format need to be followed..
        /// </summary>
        public const string CustomPropertiesInvalid = "Custom Properties entered are invalid, format need to be followed..";
        /// <summary>
        /// Success!
        /// </summary>
        public const string Success = "Success!";
        /// <summary>
        /// Failed!
        /// </summary>
        public const string Failed = "Failed!";
        /// <summary>
        /// *Note: Please consider Database, Datasource and Schema as mandatory, these inputs to 'Test Connection' and populate tables in table dropdown, any exceptions can be traced in BugReports folder
        /// </summary>
        public const string Disclaimer = "*Note: Please consider Database, Datasource and Schema as mandatory, these input to 'Load Tables' and populate tables in table's ListBox, any exceptions can be traced in BugReports folder";
    }
}