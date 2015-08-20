﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Practices.Unity;
using ModelGeneratorTool.Interfaces;
using ModelGeneratorTool.Utilities;

namespace ModelGeneratorTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Variables
        private Dictionary<string, Property> _properties = new Dictionary<string, Property>();
        private IEnumerable<string> tableCollections;
        private int count = 0;
        //private IModelGenerator _modelGenerator;
        #endregion
        #region PropertyIocInjection
        // ModelGenerator object
        [Dependency]
        public IModelGenerator ModelGenerator { get; set; }
        // Logger Object
        [Dependency]
        public ILogger Logger { get; set; }
        #endregion
        #region Constructor
        // Default Constructor
        public MainWindow()
        {
            InitializeComponent();
            EventSubscribtion();
            btnPreview.IsEnabled = btnCopyCode.IsEnabled = false;
            cbProvider.ItemsSource = new string[] { Messages.SQLSERVER/*, Messages.Oracle*/ };
            cbProvider.SelectedIndex = 0;
            LoadConnectionCombos();
            txtBlkDisclaimer.Text = Messages.Disclaimer;
            this.Title = "ModelGenerator " + DBConnection.Version;
        }

        #endregion
        #region Events
        private void EventSubscribtion()
        {
            // datasource selection changed
            cbDataSource.SelectionChanged += (sender, e) =>
            {
                if (cbDataSource.SelectedIndex > 0)
                {
                    txtDataSource.Text = cbDataSource.SelectedValue.ToString();
                }
            };

            // database selection changed
            cbDatabase.SelectionChanged += (sender, e) =>
            {
                if (cbDatabase.SelectedIndex > 0)
                {
                    txtDatabase.Text = cbDatabase.SelectedValue.ToString();
                }
            };

            // schema selection changed
            cbSchema.SelectionChanged += (sender, e) =>
            {
                if (cbSchema.SelectedIndex > 0)
                {
                    txtSchema.Text = cbSchema.SelectedValue.ToString();
                }
            };

            // Create Model click event
            btnCreateModel.Click += async (sender, e) =>
            {
                try
                {
                    string modelName = txtModelName.Text.Trim();
                    string tableName = lbTables.SelectedValue != null ? lbTables.SelectedValue.ToString() : string.Empty;
                    string dataSource = txtDataSource.Text.Trim();
                    string dataBase = txtDatabase.Text.Trim();
                    string propertyNames = txtPropertyNames.Text.Trim();
                    string path = txtPath.Text.Trim();
                    bool? interfaceRequired = chkInterfaceRequired.IsChecked;
                    bool? ormRequired = chkOrmRequired.IsChecked;
                    bool? backingFieldsRequired = chkBackingField.IsChecked;
                    bool? validationRequired = chkValidation.IsChecked;
                    string dbType = cbProvider.Text.Trim();
                    var userInputs = new
                    {
                        TableName = tableName,
                        ModelName = modelName,
                        DataSource = dataSource,
                        DataBase = dataBase,
                        Path = path,
                        InterfaceRequired = interfaceRequired,
                        OrmRequired = ormRequired,
                        BackingFieldRequired = backingFieldsRequired,
                        ValidationRequired = validationRequired,
                        PropertyNames = propertyNames,
                        DbType = dbType,
                        Version = DBConnection.Version
                    };
                    var propertyList = new List<Property>();
                    for (int i = 0; i < _properties.Count; i++)
                    {
                        var sortedProperties = _properties.OrderBy(x => Convert.ToInt32(x.Key.Split(',')[0]));
                        propertyList.Add(sortedProperties.ElementAt(i).Value);
                    }
                    string message = await ModelGenerator.GenerateModelAsync(userInputs, propertyList);
                    if (message.Contains("Successful"))
                    {
                        this.ShowMessageAsync(Messages.Success, message);
                        ClearControlsDataSource();
                    }
                    else
                    {
                        this.ShowMessageAsync(Messages.Failed, message);
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogErrorIntoFile(ex.Message);
                }
            };

            // Connection Status click event
            btnLoadTables.Click += async (sender, e) =>
            {
                //var progressDialog = await this.ShowProgressAsync("Please wait...", Messages.TestingConnection);
                bool status = await LoadTablesAsync();
                //await progressDialog.CloseAsync();
                if (status)
                {
                    await this.ShowMessageAsync(Messages.Success, Messages.ConnectionSuccessful);
                }
                else
                {
                    await this.ShowMessageAsync(Messages.Failed, Messages.ConnectionFailed);
                }
            };

            // Browse for Location to store
            btnBrowse.Click += (sender, e) =>
            {
                var dialog = new System.Windows.Forms.FolderBrowserDialog();
                dialog.ShowDialog();
                txtPath.Text = dialog.SelectedPath;
            };

            // Save Connection event
            btnSaveConnection.Click += (sender, e) =>
            {
                string source = txtDataSource.Text.Trim();
                string database = txtDatabase.Text.Trim();
                string schema = txtSchema.Text.Trim();
                if (!(string.IsNullOrEmpty(source) || string.IsNullOrEmpty(database) || string.IsNullOrEmpty(schema)))
                {
                    if (!(cbDataSource.Items.Contains(source) && cbDatabase.Items.Contains(database) && cbSchema.Items.Contains(schema)))
                    {
                        string conString = source + "," + database + "," + schema;
                        var writer = new ConnectionWriterReader();
                        if (writer.WriteConnection(conString))
                        {
                            LoadConnectionCombos();
                            this.ShowMessageAsync(Messages.Success, Messages.ConnectionSaveSuccessful);
                        }
                        else
                        {
                            this.ShowMessageAsync(Messages.Failed, Messages.ConnectionSaveUnSuccessful);
                        }
                        return;
                    }
                    else
                    {
                        this.ShowMessageAsync(Messages.Failed, Messages.ConnectionAlreadyPresent);
                        return;
                    }
                }
                this.ShowMessageAsync(Messages.Failed, Messages.EnterMandatoryFields);
            };
            // Remove connection event
            btnRemoveConnection.Click += (sender, e) =>
            {
                string source = txtDataSource.Text.Trim();
                string database = txtDatabase.Text.Trim();
                string schema = txtSchema.Text.Trim();
                if (!(string.IsNullOrEmpty(source) && string.IsNullOrEmpty(database) && string.IsNullOrEmpty(schema)))
                {
                    var reader = new ConnectionWriterReader();
                    if (reader.DeleteConnection(source, database, schema))
                        this.ShowMessageAsync(Messages.Success, Messages.DeletionSuccessful);
                    else
                        this.ShowMessageAsync(Messages.Failed, string.Format(Messages.DeletionUnSuccessful, source, database, schema));
                    LoadConnectionCombos();
                }
                else
                {
                    this.ShowMessageAsync(Messages.Failed, Messages.EnterMandatoryFields);
                }
            };
            // Tables selection event
            lbTables.SelectionChanged += async (sender, e) =>
            {
                if (lbTables.SelectedItem != null)
                {
                    try
                    {
                        string source = txtDataSource.Text.Trim();
                        string database = txtDatabase.Text.Trim();
                        string table = txtModelName.Text = lbTables.SelectedItem.ToString();
                        string conString = string.Format(Messages.SourceDatabaseSchema, source, database);
                        string query = string.Format(Messages.SchemaOnColumns, table);
                        if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(database))
                        {
                            await this.ShowMessageAsync(Messages.Failed, Messages.ConnectionFailed);
                            return;
                        }

                        lbColumns.ItemsSource = null;
                        var helper = new DBHelper();
                        var columnCollection = await helper.ExecuteReaderOnColumnsAsync(conString, query);
                        if (columnCollection.Count() > 0)
                        {
                            lbColumns.ItemsSource = columnCollection;
                            lbColumns.DisplayMemberPath = "ColumnName";
                            btnPreview.IsEnabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogErrorIntoFile(ex.Message);
                    }
                }
            };
            // Preview button click event
            btnPreview.Click += (sender, e) =>
            {
                string selectedColumns = "";
                var selected = lbColumns.SelectedItems;
                if (selected.Count > 0)
                {
                    var obsoleteCols = _properties.Where(x => x.Key.Contains(lbTables.SelectedValue.ToString())).ToList();
                    for (int i = 0; i < obsoleteCols.Count; i++)
                    {
                        _properties.Remove(obsoleteCols[i].Key);
                    }
                    for (int i = 0; i < selected.Count; i++)
                    {
                        _properties.Add(count + "," + lbTables.SelectedValue.ToString(), selected[i] as Property);
                        count++;
                    }
                    for (int i = 0; i < _properties.Count; i++)
                    {
                        var sortedProperties = _properties.OrderBy(x => Convert.ToInt32(x.Key.Split(',')[0]));
                        selectedColumns += sortedProperties.ElementAt(i).Value.ColumnName + ",";
                    }
                    selectedColumns = selectedColumns.Substring(0, selectedColumns.Length - 1);
                    txtPreviewColumns.Text = selectedColumns;
                    return;
                }
                this.ShowMessageAsync(Messages.Failed, Messages.SelectAnyColumns);
            };
            // ClearAll button click event
            btnClearAll.Click += (sender, e) =>
            {
                _properties.Clear();
                count = 0;
                ClearControlsDataSource();
            };
            // SearchTables TextChanged event
            txtSearchTables.TextChanged += (sender, e) =>
            {
                if (tableCollections != null && tableCollections.Count() > 0)
                {
                    lbColumns.ItemsSource = null;
                    lbTables.ItemsSource = tableCollections.Where(x => x.IndexOf(txtSearchTables.Text, StringComparison.OrdinalIgnoreCase) >= 0).AsEnumerable();
                }
            };
            // SearchTables PreviewKeyDowm event
            txtSearchTables.PreviewKeyDown += (sender, e) =>
            {
                if (e.Key == System.Windows.Input.Key.Down && lbTables.ItemsSource != null)
                {
                    lbTables.Focus();
                    lbTables.SelectedIndex = 1;
                }
            };
            // CopyCode button click event
            btnCopyCode.Click += (sender, e) =>
            {
                string propInput = txtPropertyNames.Text;
                string copyString = "";
                if (chkCopyCode.IsChecked == true && !string.IsNullOrEmpty(propInput))
                {
                    var props = propInput.Split(',');
                    var properties = new List<Property>();
                    for (int i = 0; i < props.Length; i++)
                    {
                        properties.Add(new Property
                        {
                            PropertyName = props[i].Trim()
                        });
                    }
                    if (properties.Count > 0)
                    {
                        copyString = ModelGenerator.CopyPropertyStrings(properties, chkBackingField.IsChecked == true);
                    }
                }
                System.Windows.Clipboard.SetText(copyString);
            };
            // CopyCode checkbox click event
            chkCopyCode.Click += (sender, e) =>
            {
                btnCopyCode.IsEnabled = chkCopyCode.IsChecked == true ? true : false;
            };
        }

        #endregion
        #region Private Methods
        private async System.Threading.Tasks.Task<bool> LoadTablesAsync()
        {
            try
            {
                await System.Threading.Tasks.Task.Delay(50);
                string source = txtDataSource.Text.Trim();
                string database = txtDatabase.Text.Trim();
                string schema = txtSchema.Text.Trim();
                string conString = string.Format(Messages.SourceDatabaseSchema, source, database);
                string query = string.Format(Messages.SchemaOnTables, database, schema);
                if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(database)) return false;
                var helper = new DBHelper();
                tableCollections = await helper.ExecuteReaderAsync(conString, query);
                if (tableCollections.Count() > 0)
                {
                    lbTables.ItemsSource = tableCollections;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private void LoadConnectionCombos()
        {
            ClearComboControls();
            var connections = new ConnectionWriterReader().ReadConnections();
            if (connections != null)
            {
                foreach (var connection in connections)
                {
                    if (!cbDataSource.Items.Contains(connection.DataSource))
                        cbDataSource.Items.Add(connection.DataSource);
                    if (!cbDatabase.Items.Contains(connection.Database))
                        cbDatabase.Items.Add(connection.Database);
                    if (!cbSchema.Items.Contains(connection.Schema))
                        cbSchema.Items.Add(connection.Schema);
                }
            }
        }

        private void ClearComboControls()
        {
            cbDataSource.Items.Clear();
            cbDataSource.Items.Add("---Select---");
            cbDataSource.SelectedIndex = 0;
            cbDatabase.Items.Clear();
            cbDatabase.Items.Add("---Select---");
            cbDatabase.SelectedIndex = 0;
            cbSchema.Items.Clear();
            cbSchema.Items.Add("---Select---");
            cbSchema.SelectedIndex = 0;
        }

        private void ClearControlsDataSource()
        {
            lbColumns.ItemsSource = null;
            txtPreviewColumns.Text = string.Empty;
            txtPath.Text = string.Empty;
            cbProvider.SelectedIndex = 0;
            btnPreview.IsEnabled = false;
            txtModelName.Text = string.Empty;
            txtPropertyNames.Text = string.Empty;            
            txtSearchTables.Text = string.Empty;
            chkCopyCode.IsChecked = chkInterfaceRequired.IsChecked = chkOrmRequired.IsChecked = 
                chkBackingField.IsChecked = chkValidation.IsChecked = btnCopyCode.IsEnabled = false;
            lbTables.SelectedIndex = -1;            
        }
        #endregion
    }
}