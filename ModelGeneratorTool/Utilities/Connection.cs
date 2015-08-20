namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Contains basic elements of ConnectionString
    /// </summary>
    internal sealed class Connection
    {
        public string DataSource { get; set; }
        public string Database { get; set; }
        public string Schema { get; set; }
    }
}
