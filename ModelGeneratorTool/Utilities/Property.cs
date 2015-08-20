namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Contains basic elements of a property
    /// </summary>
    public sealed class Property
    {
        /// <summary>
        /// Gets TableName of the column
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// Gets ColumnName
        /// </summary>
        public string ColumnName { get; set; }
        /// <summary>
        /// Gets DataType of the column
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// Gets MaxLength of the column
        /// </summary>
        public string MaxLength { get; set; }
        /// <summary>
        /// Gets IsNullable
        /// </summary>
        public bool IsNullable { get; set; }
        /// <summary>
        /// Gets Schema of the Table
        /// </summary>
        public string Schema { get; set; }
        /// <summary>
        /// Gets TableCatalog
        /// </summary>
        public string TableCatalog { get; set; }
        /// <summary>
        /// Gets PropertyName
        /// </summary>
        public string PropertyName { get; set; }
    }

    /// <summary>
    /// property constants
    /// </summary>
    internal class PropertyNames
    {
        /// <summary>
        /// TableName
        /// </summary>
        public const string TABLENAME = "TableName";
        /// <summary>
        /// TableCatalog
        /// </summary>
        public const string TABLECATALOG = "TableCatalog";
        /// <summary>
        /// ColumnName
        /// </summary>
        public const string COLUMNNAME = "ColumnName";
        /// <summary>
        /// PropertyName
        /// </summary>
        public const string PROPERTYNAME = "PropertyName";
        /// <summary>
        /// DataType
        /// </summary>
        public const string DATATYPE = "DataType";
        /// <summary>
        /// MaxLength
        /// </summary>
        public const string MAXLENGTH = "MaxLength";
        /// <summary>
        /// IsNullable
        /// </summary>
        public const string ISNULLABLE = "IsNullable";
        /// <summary>
        /// Schema
        /// </summary>
        public const string SCHEMA = "Schema";
        /// <summary>
        /// uniqueidentifier
        /// </summary>
        public const string UNIQUEIDENTIFIER = "uniqueidentifier";
        /// <summary>
        /// Guid
        /// </summary>
        public const string GUID = "Guid";
        /// <summary>
        /// UUID
        /// </summary>
        public const string UUID = "UUID";
        /// <summary>
        /// Char
        /// </summary>
        public const string CHAR = "Char";
        /// <summary>
        /// text
        /// </summary>
        public const string TEXT = "text";
        /// <summary>
        /// varchar
        /// </summary>
        public const string VARCHAR = "varchar";
        /// <summary>
        /// nchar
        /// </summary>
        public const string NCHAR = "nchar";
        /// <summary>
        /// nvarchar
        /// </summary>
        public const string NVARCHAR = "nvarchar";
        /// <summary>
        /// ntext
        /// </summary>
        public const string NTEXT = "ntext";
        /// <summary>
        /// string
        /// </summary>
        public const string STRING = "string";
        /// <summary>
        /// bit
        /// </summary>
        public const string BIT = "bit";
        /// <summary>
        /// bool
        /// </summary>
        public const string BOOL = "bool";
        /// <summary>
        /// int
        /// </summary>
        public const string INT = "int";
        /// <summary>
        /// bigint
        /// </summary>
        public const string BIGINT = "bigint";
        /// <summary>
        /// smallint
        /// </summary>
        public const string SMALLINT = "smallint";
        /// <summary>
        /// tinyint
        /// </summary>
        public const string TINYINT = "tinyint";
        /// <summary>
        /// decimal
        /// </summary>
        public const string DECIMAL = "decimal";
        /// <summary>
        /// float
        /// </summary>
        public const string FLOAT = "float";
        /// <summary>
        /// date
        /// </summary>
        public const string DATE = "date";
        /// <summary>
        /// datetime
        /// </summary>
        public const string DATETIME = "datetime";
        /// <summary>
        /// datetime2
        /// </summary>
        public const string DATETIME2 = "datetime2";
        /// <summary>
        /// time
        /// </summary>
        public const string TIME = "time";
    }
}
