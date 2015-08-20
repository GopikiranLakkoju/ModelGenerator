using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Writes the connections in a file
    /// </summary>
    internal sealed class ConnectionWriterReader
    {
        /// <summary>
        /// Writes the connections into a file, with string provided
        /// </summary>
        /// <param name="connectionString">connection string</param>
        /// <returns>returns true if success, else false</returns>
        public bool WriteConnection(string connectionString)
        {
            try
            {
                string filePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin", "Connection").Replace("Debug", "Connections.txt");
                using (StreamWriter conWriter = new StreamWriter(filePath, true))
                {
                    conWriter.Write(connectionString + "|");
                }
                return true;
            }
            catch (Exception e)
            {
                new Logger().LogErrorIntoFile(e.Message);
                return false;
            }
        }

        /// <summary>
        /// Reads connections written into file
        /// </summary>
        /// <returns>Returns List of connections available</returns>
        public List<Connection> ReadConnections()
        {
            var filePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin", "Connection").Replace("Debug", "Connections.txt");
            if (System.IO.File.Exists(filePath))
            {
                var connections = new List<Connection>();
                Connection connection;
                using (StreamReader stream = new StreamReader(filePath))
                {
                    var connectionStr = stream.ReadToEnd().Split('|');
                    for (int i = 0; i < connectionStr.Length - 1; i++)
                    {
                        connection = new Connection { DataSource = connectionStr[i].Split(',')[0], Database = connectionStr[i].Split(',')[1], Schema = connectionStr[i].Split(',')[2] };
                        connections.Add(connection);
                    }
                }
                return connections;
            }
            return null;
        }

        /// <summary>
        /// Deletes the existing connection
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="database">database</param>
        /// <param name="schema">schema</param>
        /// <returns>Returns true if deleted, else false</returns>
        public bool DeleteConnection(string source, string database, string schema)
        {
            try
            {
                bool status = false;
                int removeIndex = 0;
                var filePath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin", "Connection").Replace("Debug", "Connections.txt");
                if (System.IO.File.Exists(filePath))
                {
                    var connections = new string[] { };
                    using (StreamReader stream = new StreamReader(filePath))
                    {
                        connections = stream.ReadToEnd().Split('|');
                        for (int i = 0; i < connections.Length - 1; i++)
                        {
                            if (connections[i].Split(',')[0].Equals(source) && connections[i].Split(',')[1].Equals(database) && connections[i].Split(',')[2].Equals(schema))
                            {
                                removeIndex = i;
                                status = true;
                            }
                        }
                    }
                    if (status)
                    {
                        File.Delete(filePath);
                        for (int i = 0; i < connections.Length - 1; i++)
                        {                            
                            using (StreamWriter writer = new StreamWriter(filePath, true))
                            {
                                if (i != removeIndex)
                                {
                                    writer.Write(connections[i] + "|");
                                }
                            }
                        }
                        return status;
                    }
                    return status;
                }
                else
                {
                    new Logger().LogErrorIntoFile("Connections.txt file does not exist..");
                    throw new Exception("Connections.txt file does not exist..");
                }
            }
            catch (Exception e)
            {
                new Logger().LogErrorIntoFile(e.Message);
                return false;
            }
        }
    }    
}