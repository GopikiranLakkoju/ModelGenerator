using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Database reader
    /// </summary>
    internal sealed class DBHelper
    {
        /// <summary>
        /// Returns list of columns based on the input table name provided
        /// </summary>
        /// <param name="conString">User input Connection String</param>
        /// <param name="query">Query based on table</param>
        /// <returns>Returns list of columns, reference to a table</returns>
        public async Task<IEnumerable<string>> ExecuteReaderAsync(string conString, string query)
        {
            var resultList = new List<string>();
            using (var connection = new SqlConnection(conString))
            {                
                connection.Open();
                string commandText = query;
                SqlCommand command = new SqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultList.Add(reader.GetString(0));
                    }
                }                
            }
            return resultList;
        }

        /// <summary>
        /// Returns List of Property objects based on the table
        /// </summary>
        /// <param name="conString">User input Connection String</param>
        /// <param name="query">Query based on table</param>
        /// <returns>Returns list of columns, reference to a table</returns>
        public async Task<IEnumerable<Property>> ExecuteReaderOnColumnsAsync(string conString, string query)
        {
            var resultList = new List<Property>();
            using (var connection = new SqlConnection(conString))
            {
                connection.Open();
                string commandText = query;
                SqlCommand command = new SqlCommand();
                command.CommandText = commandText;
                command.Connection = connection;                
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        resultList.Add(new Property
                        {
                            TableName = reader.GetString(0),
                            ColumnName = reader.GetString(1),
                            DataType = reader.GetString(2),
                            MaxLength = reader.GetValue(3).ToString(),
                            IsNullable = reader.GetString(4) == "YES" ? true : false,
                            Schema = reader.GetString(5),
                            TableCatalog = reader.GetString(6)
                        });
                    }
                }                
            }
            return resultList;
        }
    }
}