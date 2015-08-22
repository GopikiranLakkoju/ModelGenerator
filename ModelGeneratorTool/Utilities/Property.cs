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
        public const string Tablename = "TableName";
        /// <summary>
        /// TableCatalog
        /// </summary>
        public const string Tablecatalog = "TableCatalog";
        /// <summary>
        /// ColumnName
        /// </summary>
        public const string Columnname = "ColumnName";
        /// <summary>
        /// PropertyName
        /// </summary>
        public const string Propertyname = "PropertyName";
        /// <summary>
        /// DataType
        /// </summary>
        public const string Datatype = "DataType";
        /// <summary>
        /// MaxLength
        /// </summary>
        public const string Maxlength = "MaxLength";
        /// <summary>
        /// IsNullable
        /// </summary>
        public const string Isnullable = "IsNullable";
        /// <summary>
        /// Schema
        /// </summary>
        public const string Schema = "Schema";
        /// <summary>
        /// uniqueidentifier
        /// </summary>
        public const string Uniqueidentifier = "uniqueidentifier";
        /// <summary>
        /// Guid
        /// </summary>
        public const string Guid = "Guid";
        /// <summary>
        /// UUID
        /// </summary>
        public const string Uuid = "UUID";
        /// <summary>
        /// Char
        /// </summary>
        public const string Char = "Char";
        /// <summary>
        /// text
        /// </summary>
        public const string Text = "text";
        /// <summary>
        /// varchar
        /// </summary>
        public const string Varchar = "varchar";
        /// <summary>
        /// nchar
        /// </summary>
        public const string Nchar = "nchar";
        /// <summary>
        /// nvarchar
        /// </summary>
        public const string Nvarchar = "nvarchar";
        /// <summary>
        /// ntext
        /// </summary>
        public const string Ntext = "ntext";
        /// <summary>
        /// string
        /// </summary>
        public const string String = "string";
        /// <summary>
        /// bit
        /// </summary>
        public const string Bit = "bit";
        /// <summary>
        /// bool
        /// </summary>
        public const string Bool = "bool";
        /// <summary>
        /// int
        /// </summary>
        public const string Int = "int";
        /// <summary>
        /// bigint
        /// </summary>
        public const string Bigint = "bigint";
        /// <summary>
        /// smallint
        /// </summary>
        public const string Smallint = "smallint";
        /// <summary>
        /// tinyint
        /// </summary>
        public const string Tinyint = "tinyint";
        /// <summary>
        /// decimal
        /// </summary>
        public const string Decimal = "decimal";
        /// <summary>
        /// float
        /// </summary>
        public const string Float = "float";
        /// <summary>
        /// date
        /// </summary>
        public const string Date = "date";
        /// <summary>
        /// datetime
        /// </summary>
        public const string Datetime = "datetime";
        /// <summary>
        /// datetime2
        /// </summary>
        public const string Datetime2 = "datetime2";
        /// <summary>
        /// time
        /// </summary>
        public const string Time = "time";
    }
}
