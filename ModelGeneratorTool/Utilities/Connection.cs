namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Contains basic elements of ConnectionString
    /// </summary>
    internal sealed class Connection
    {
        /// <summary>
        /// Gets or sets DataSource
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// Gets or sets Database
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// Gets or sets Schema
        /// </summary>
        public string Schema { get; set; }
    }
}
