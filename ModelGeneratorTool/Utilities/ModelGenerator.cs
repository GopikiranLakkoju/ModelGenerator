using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using ModelGeneratorTool.Interfaces;

namespace ModelGeneratorTool.Utilities
{
    /// <summary>
    /// Generates model for the specified tables
    /// </summary>
    public sealed class ModelGenerator : IModelGenerator
    {
        #region Variables & Constants
        private string _modelName = "";
        private string _filePath = "";
        private string _version = "";
        private const string PUBLIC = "public";
        private const string CLASS = "class";
        private const string ONLYCLASS = "OnlyClass";
        private const string INTERFACE = "interface";
        private const string PRIVATE = "private";

        #endregion
        #region GenerateModel public method
        /// <summary>
        /// <see cref="IModelGenerator.GenerateModel"/>
        /// </summary>
        public async System.Threading.Tasks.Task<string> GenerateModelAsync(dynamic userInputs, List<Property> properties)
        {
            _version = userInputs.Version;
            if (userInputs.DbType == Messages.Oracle) return Messages.OracleSupport;
            if (!string.IsNullOrEmpty(userInputs.TableName) && properties.Count == 0)
            {
                string inputConString = string.Format(Messages.SourceDatabaseSchema, userInputs.DataSource, userInputs.DataBase);
                var query = string.Format(Messages.SchemaOnColumns, userInputs.TableName);
                List<Property> propList = await new DBHelper().ExecuteReaderOnColumnsAsync(inputConString, query);
                int status = 0;
                UpdatePropertyNames(userInputs.PropertyNames, ref propList, ref status);
                return CreateFile(propList, userInputs);
            }
            else if (!string.IsNullOrEmpty(userInputs.DataSource) && !string.IsNullOrEmpty(userInputs.DataBase))
            {
                int status = 0;
                UpdatePropertyNames(userInputs.PropertyNames, ref properties, ref status);
                if (status == 1)
                    return Messages.CustomPropertiesInvalid;
                return CreateFile(properties, userInputs);

            }
            else
            {
                return Messages.DatabaseTableProviderMandate;
            }
        }

        /// <summary>
        /// Creates the file with specified properties
        /// </summary>
        /// <param name="properties">Properties to write</param>
        /// <param name="userInputs">anonymous user definitions</param>
        /// <returns></returns>
        private string CreateFile(IEnumerable<Property> properties, dynamic userInputs)
        {
            if (properties.Count() > 0)
            {
                int iterate = 0;
                string[] classInterface = new[] { "class", "interface" };
                var modelSplits = userInputs.ModelName.Split('_');
                _modelName = "";
                foreach (var split in modelSplits)
                {
                    if (!string.IsNullOrEmpty(split))
                    {
                        _modelName += char.ToUpper(split[0]) + split.Substring(1);
                    }
                }
                if (string.IsNullOrEmpty(_modelName)) return Messages.ModelNameMissing;
                iterate = userInputs.InterfaceRequired ? 1 : iterate;
                for (int i = 0; i <= iterate; i++)
                {
                    _modelName = classInterface[i] == "class" ? _modelName : "I" + _modelName;
                    _filePath = !string.IsNullOrEmpty(userInputs.Path) ? userInputs.Path + "\\" + _modelName + ".g.cs" : Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace("bin", "Models").Replace("Debug", _modelName + ".g.cs");
                    using (StreamWriter stream = new StreamWriter(_filePath, false))
                    {
                        var type = iterate == 0 ? "OnlyClass" : classInterface[i];
                        var codeProp = WritePropertyStreamInCSharp(properties, type, userInputs.InterfaceRequired, userInputs.BackingFieldRequired, userInputs.ValidationRequired);
                        stream.Write(codeProp.ToString());
                    }
                }
                if (userInputs.OrmRequired)
                {
                    CreateOrmAndWriteXml(properties);
                }

                return Messages.ModelCreationSuccessful;
            }
            else
            {
                return Messages.TableNotFound;
            }
        }

        #endregion
        #region Private Methods
        /// <summary>
        /// Update's the property names with custom names
        /// </summary>
        /// <param name="propertyNames">Custom Property Names</param>
        /// <param name="properties">Properties of class</param>
        private void UpdatePropertyNames(string propertyNames, ref List<Property> properties, ref int status)
        {
            try
            {
                if (!string.IsNullOrEmpty(propertyNames))
                {
                    string[] customProperties = propertyNames.Split(',');
                    for (int i = 0; i < customProperties.Length; i++)
                    {
                        string custCol = customProperties[i].ToString().Split(':')[0].Trim();
                        string custProp = customProperties[i].ToString().Split(':')[1].Trim();
                        for (int p = 0; p < properties.Count; p++)
                        {
                            string colName = properties[p].ColumnName;
                            if (colName == custCol)
                            {
                                properties[p].PropertyName = custProp;
                            }
                            else if (string.IsNullOrEmpty(properties[p].PropertyName) && colName != custCol)
                            {
                                properties[p].PropertyName = colName;
                            }
                        }
                    }
                }
                else
                {
                    for (int p = 0; p < properties.Count; p++)
                    {
                        string colName = properties[p].ColumnName;
                        properties[p].PropertyName = colName;
                    }
                }
            }
            catch (Exception)
            {
                status = 1;
            }
        }

        /// <summary>
        /// <see cref="IModelGenerator.CopyPropertyStrings"/>
        /// </summary>
        public string CopyPropertyStrings(List<Property> properties, bool backingFieldRequired)
        {
            var copyPropWiter = new StringBuilder();
            for (int i = 0; i < properties.Count; i++)
            {
                string propertyName = properties[i].PropertyName;
                if (backingFieldRequired)
                {
                    string backingField = Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                    copyPropWiter.AppendLine("private string " + backingField + ";");
                    copyPropWiter.AppendLine("public string " + propertyName);
                    copyPropWiter.AppendLine("    {");
                    copyPropWiter.AppendLine("        get { return " + backingField + "; }");
                    copyPropWiter.AppendLine("        set { " + backingField + " = value; }");
                    copyPropWiter.AppendLine("    }");
                }
                else
                {
                    copyPropWiter.AppendLine("public string " + propertyName + " { get; set; }");
                }
            }
            return copyPropWiter.ToString();
        }

        /// <summary>
        /// Writes the properties into StringBuilder in C#
        /// </summary>
        /// <param name="properties">Properties from database</param>        
        /// /// <param name="type">type as class or interface</param>
        /// <returns>StringBuilder with all properties appended</returns>
        private StringBuilder WritePropertyStreamInCSharp(IEnumerable<Property> properties, string type, bool interfaceRequired, bool backingFieldRequired, bool validationRequired)
        {
            string modifier = type == CLASS || type == ONLYCLASS ? "    " + PUBLIC + " " : "    ";
            string realType = type == CLASS || type == ONLYCLASS ? CLASS : type;
            const string summary = "    /// <summary>";
            StringBuilder codeWriter = new StringBuilder();
            codeWriter.AppendLine("using System;");
            if (validationRequired)
            {
                codeWriter.AppendLine("using System.ComponentModel.DataAnnotations;");
            }
            codeWriter.Append(Environment.NewLine);
            codeWriter.AppendLine("// This is a generated file from Model Generator " + _version);
            codeWriter.AppendLine("// Modifying this " + realType + " would incur problems as re-generating model would over-right the file");
            codeWriter.AppendLine("// However to add custom properties, its recommended to create a partial " + realType + " on top of this");
            // class upper body
            if (realType == CLASS && interfaceRequired)
            {
                codeWriter.AppendLine(PUBLIC + " " + realType + " " + _modelName + " : " + "I" + _modelName);
            }
            else
            {
                codeWriter.AppendLine(PUBLIC + " " + realType + " " + _modelName);
            }
            codeWriter.AppendLine("{");
            // property section
            foreach (var property in properties)
            {
                string add = "";
                string returnType = GetType(property);
                string backingField = "";
                string propertyName = !string.IsNullOrEmpty(property.PropertyName) && property.ColumnName != property.PropertyName ? property.PropertyName : property.ColumnName;
                // backing fields structure
                if (realType == CLASS && backingFieldRequired)
                {
                    backingField = Char.ToLowerInvariant(propertyName[0]) + propertyName.Substring(1);
                    codeWriter.AppendLine("    " + PRIVATE + " " + returnType + " " + backingField + ";");
                }
                // only summary section
                codeWriter.AppendLine(summary);
                if (type == CLASS)
                {
                    codeWriter.AppendLine("    /// <see cref=\"" + "I" + _modelName + "." + propertyName + "\"/>");
                }
                else if (property.DataType == PropertyNames.UNIQUEIDENTIFIER)
                {
                    codeWriter.AppendLine("    /// " + property.TableName + "." + property.ColumnName + " refers " + property.ColumnName.Replace("GUID", string.Empty) + " table in " + property.Schema + "." + property.TableCatalog + " with type " + property.DataType);

                }
                else
                {
                    codeWriter.AppendLine("    /// " + property.TableName + "." + property.ColumnName + " in " + property.Schema + "." + property.TableCatalog + " with type " + property.DataType);
                }
                codeWriter.AppendLine(summary);
                // validation attributes
                if (realType == CLASS && validationRequired)
                {
                    if (!property.IsNullable)
                    {
                        codeWriter.AppendLine("    [Required()]");
                    }
                    if (returnType == "string" || returnType == "string?")
                    {
                        codeWriter.AppendLine("    [StringLength(" + property.MaxLength + ")]");
                    }
                }
                // property section wrt. backingfields otherwise
                if (type != INTERFACE && backingFieldRequired)
                {
                    codeWriter.AppendLine(modifier + returnType + " " + propertyName);
                    codeWriter.AppendLine("    {");
                    codeWriter.AppendLine("        get { return " + backingField + "; }");
                    codeWriter.AppendLine("        set { " + backingField + " = value; }");
                    codeWriter.AppendLine("    }");
                }
                else
                {
                    add = modifier + returnType + " " + propertyName + " { get; set; }";
                    codeWriter.AppendLine(add);
                }
            }
            codeWriter.AppendLine("}");
            return codeWriter;
        }

        /// <summary>
        /// Creates and Writes XML
        /// </summary>
        /// <param name="properties">Columns data</param>
        private void CreateOrmAndWriteXml(IEnumerable<Property> properties)
        {
            string modelName = _modelName.IndexOf("I") == 0 ? _modelName.Substring(1) : _modelName;
            var xml = new XElement(modelName,
                            new XElement("Properties",
                                properties.Select(property =>
                                new XElement("Property",
                                  new XElement(PropertyNames.COLUMNNAME, property.ColumnName),
                                  new XElement(PropertyNames.PROPERTYNAME, property.PropertyName),
                                  new XElement(PropertyNames.DATATYPE, property.DataType),
                                  new XElement(PropertyNames.MAXLENGTH, property.MaxLength),
                                  new XElement(PropertyNames.ISNULLABLE, property.IsNullable),
                                  new XElement(PropertyNames.SCHEMA, property.Schema),
                                  new XElement(PropertyNames.TABLECATALOG, property.TableCatalog),
                                  new XElement(PropertyNames.TABLENAME, property.TableName)))));
            xml.Save(_filePath.Replace(".g.cs", "").Replace("I", "") + ".orm.g.xml");
        }

        /// <summary>
        /// Gets the DataType in available comparison b/w sql and .net datatypes
        /// </summary>
        /// <param name="property">Property</param>
        /// <returns>DataType as string</returns>
        private string GetType(Property property)
        {
            string type = "";
            switch (property.DataType)
            {
                case PropertyNames.UNIQUEIDENTIFIER:
                    type = PropertyNames.GUID;
                    break;
                case PropertyNames.CHAR:
                case PropertyNames.TEXT:
                case PropertyNames.VARCHAR:
                case PropertyNames.NCHAR:
                case PropertyNames.NVARCHAR:
                case PropertyNames.NTEXT:
                    type = PropertyNames.STRING;
                    break;
                case PropertyNames.BIT:
                    type = PropertyNames.BOOL;
                    break;
                case PropertyNames.INT:
                case PropertyNames.BIGINT:
                case PropertyNames.SMALLINT:
                case PropertyNames.TINYINT:
                    type = PropertyNames.INT;
                    break;
                case PropertyNames.DECIMAL:
                    type = PropertyNames.DECIMAL;
                    break;
                case PropertyNames.FLOAT:
                    type = PropertyNames.FLOAT;
                    break;
                case PropertyNames.DATE:
                case PropertyNames.DATETIME:
                case PropertyNames.DATETIME2:
                case PropertyNames.TIME:
                    type = PropertyNames.DATETIME;
                    break;
            }
            type = property.IsNullable ? type + "?" : type;
            return type;
        }

        #endregion
    }
}