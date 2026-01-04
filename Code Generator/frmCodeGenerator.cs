// File: frmCodeGenerator.cs
using CodeGenerator.BusinessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CodeGenerator 
{
    public partial class frmCodeGenerator : Form
    {
        private readonly CodeGeneratorService _codeGenService;
        private string _masterConnectionString;
        private string _currentConnectionString;
        private string _currentProcedureName = string.Empty;
        private string _currentTableName = string.Empty;
        private List<SimpleColumnInfo> _currentTableColumns = new List<SimpleColumnInfo>();
        public frmCodeGenerator()
        {
            InitializeComponent();
            _codeGenService = new CodeGeneratorService();
            // أضف الأعمدة للـ DataGridView لو مش موجودة
            if (dgvColumns.Columns.Count == 0)
            {
                dgvColumns.Columns.Add("colName", "Column Name");
                dgvColumns.Columns.Add("colDataType", "Data Type");

                dgvColumns.Columns[0].Width = 200;
                dgvColumns.Columns[1].Width = 150;
                dgvColumns.ReadOnly = true;
                dgvColumns.AllowUserToAddRows = false;
            }
            
        }

        private void frmCodeGenerator_Load(object sender, EventArgs e)
        {
            grpDatabase.Enabled = false;
            grpTable.Enabled = false;
            grpGenerate.Enabled = false;
            SetStatus("Ready");
        }

        private void SetStatus(string message, bool isError = false)
        {
            sslStatus.Text = message;
            sslStatus.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Black;
            statusStrip1.Refresh(); // تحديث فوري للرسالة
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text.Trim();

            if (string.IsNullOrEmpty(server))
            {
                SetStatus("Please enter a valid server name or IP address.", true);
                MessageBox.Show("Server name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtServer.Focus();
                return;
            }

            if (!chkWindowsAuth.Checked)
            {
                if (string.IsNullOrEmpty(txtUser.Text.Trim()))
                {
                    SetStatus("Username is required when using SQL Authentication.", true);
                    MessageBox.Show("Please enter username.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtUser.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    SetStatus("Password is required when using SQL Authentication.", true);
                    MessageBox.Show("Please enter password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPassword.Focus();
                    return;
                }
            }

            SetStatus("Connecting to server...");
            btnConnect.Enabled = false;
            Cursor = Cursors.WaitCursor;

            string masterConn = chkWindowsAuth.Checked
                ? $"Server={server};Database=master;Integrated Security=True;TrustServerCertificate=True;"
                : $"Server={server};Database=master;User Id={txtUser.Text.Trim()};Password={txtPassword.Text};TrustServerCertificate=True;";

            try
            {
                var databases = _codeGenService.GetAllDatabases(masterConn);

                cmbDatabases.Items.Clear();
                foreach (var db in databases)
                {
                    cmbDatabases.Items.Add(db);
                }

                _masterConnectionString = masterConn;

                if (databases.Count == 0)
                {
                    SetStatus("Connected, but no user databases found.", true);
                    MessageBox.Show("Connected successfully, but no user databases were found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cmbDatabases.SelectedIndex = 0;
                    grpDatabase.Enabled = true;
                    SetStatus($"Connected successfully. Found {databases.Count} databases.");
                    MessageBox.Show($"Connected successfully!\nFound {databases.Count} databases.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Connection failed: " + ex.Message, true);
                MessageBox.Show("Cannot connect to server:\n" + ex.Message, "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConnect.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbDatabases.SelectedItem == null) return;

            string selectedDb = cmbDatabases.SelectedItem.ToString();
            _currentConnectionString = _masterConnectionString.Replace("Database=master;", $"Database={selectedDb};");

            SetStatus($"Loading tables from database: {selectedDb}...");
            grpTable.Enabled = false;
            grpGenerate.Enabled = false;
            lstTables.Items.Clear();
            dgvColumns.Rows.Clear();
            txtGeneratedCode.Clear();
            lblSelectedTable.Text = "Selected Table: None";
            Cursor = Cursors.WaitCursor;

            try
            {
                var tables = _codeGenService.GetTables(_currentConnectionString);

                lstTables.Items.Clear();
                foreach (var table in tables)
                {
                    lstTables.Items.Add(table);
                }

                SetStatus(tables.Count > 0
                    ? $"Loaded {tables.Count} tables from '{selectedDb}'."
                    : $"Connected to '{selectedDb}', but no tables found.");

                grpTable.Enabled = true;
            }
            catch (Exception ex)
            {
                SetStatus("Failed to load tables: " + ex.Message, true);
                MessageBox.Show("Error loading tables:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void lstTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstTables.SelectedItem == null) return;

            _currentTableName = lstTables.SelectedItem.ToString();

            try
            {
                _currentTableColumns = _codeGenService.GetColumns(_currentConnectionString, _currentTableName);

                dgvColumns.Rows.Clear();
                foreach (var col in _currentTableColumns)
                {
                    dgvColumns.Rows.Add(col.Name, col.DataType);
                }

                // توليد Entity (التبويب الأول)
                var entity = _codeGenService.GenerateEntityClass(_currentTableName, _currentTableColumns);
                txtGeneratedCode.Text = entity.EntityCode;

                lblSelectedTable.Text = $"Selected Table: {_currentTableName}";
                grpGenerate.Enabled = true;

                // تفعيل التبويبات الجديدة للـ Table
                tabGeneratedCode.TabPages[2].Enabled = true; // Data Logic (Table)
                tabGeneratedCode.TabPages[3].Enabled = true; // Business Logic (Table)

                SetStatus($"Table '{_currentTableName}' selected. Switch to 'Data Logic (Table)' or 'Business Logic (Table)' tabs to generate CRUD code.");
            }
            catch (Exception ex)
            {
                SetStatus("Error loading table columns: " + ex.Message, true);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLoadProcedures_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentConnectionString))
            {
                SetStatus("Please connect to a database first.", true);
                return;
            }

            SetStatus("Loading stored procedures...");
            Cursor = Cursors.WaitCursor;

            try
            {
                var procedures = _codeGenService.GetStoredProcedures(_currentConnectionString);
                lstProcedures.Items.Clear();
                foreach (var proc in procedures)
                {
                    lstProcedures.Items.Add(proc);
                }

                SetStatus($"Loaded {procedures.Count} stored procedure(s).");
                if (procedures.Count > 0)
                {
                    MessageBox.Show($"Successfully loaded {procedures.Count} stored procedures.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SetStatus("Error loading procedures: " + ex.Message, true);
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private string ToPascalCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var parts = input.Split(new[] { '_', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Length > 0)
                {
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i].Substring(1).ToLower();
                }
            }
            return string.Join("", parts);
        }

        private void grpTable_Enter(object sender, EventArgs e)
        {

        }

        private void lstProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstProcedures.SelectedItem == null)
            {
                btnGenerateLogic.Enabled = false;
                _currentProcedureName = string.Empty;
                SetStatus("No procedure selected.");
                return;
            }

            _currentProcedureName = lstProcedures.SelectedItem.ToString();
            SetStatus($"Selected: '{_currentProcedureName}'. Click 'Generate Logic Code' to create layers.");
            btnGenerateLogic.Enabled = true;
        }

        private void btnGenerateLogic_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentProcedureName))
            {
                SetStatus("Select a stored procedure first.", true);
                return;
            }

            SetStatus($"Generating code for '{_currentProcedureName}'...");
            Cursor = Cursors.WaitCursor;
            txtLogicCode.Clear();

            try
            {
                // جلب الباراميترات
                var parameters = _codeGenService.GetStoredProcedureParameters(_currentConnectionString, _currentProcedureName);

                // جلب الأعمدة المرجعة من الـ Procedure (مهم جدًا للـ DTO والـ Mapping)
                var resultColumns = _codeGenService.GetProcedureResultColumns(_currentConnectionString, _currentProcedureName);

                // توليد Data Logic
                string dataLogicCode = _codeGenService.GenerateDataLogicCode(_currentProcedureName, parameters);

                // توليد Business Logic مع DTO تلقائي و Mapping كامل
                string businessLogicCode = _codeGenService.GenerateBusinessLogicCode(_currentProcedureName, parameters, resultColumns);

                // عرض الكود الكامل في التبويب الثاني
                txtLogicCode.Text =
                    "// ================================================\n" +
                    "// DATA LOGIC LAYER\n" +
                    "// ================================================\n\n" +
                    dataLogicCode +
                    "\n\n// ================================================\n" +
                    "// BUSINESS LOGIC LAYER (مع DTO تلقائي و Mapping كامل)\n" +
                    "// ================================================\n\n" +
                    businessLogicCode;

                // الانتقال للتبويب الثاني
                tabGeneratedCode.SelectedIndex = 1;

                SetStatus($"Generated successfully for '{_currentProcedureName}'. Ready to save.");
            }
            catch (Exception ex)
            {
                SetStatus("Generation failed: " + ex.Message, true);
                MessageBox.Show("Error:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        // تعديل btnSaveCode_Click عشان يحفظ حسب التبويب
        private void btnSaveCode_Click(object sender, EventArgs e)
        {
            string code = string.Empty;
            string fileName = "GeneratedCode.cs";

            if (tabGeneratedCode.SelectedIndex == 0) // Entity
            {
                code = txtGeneratedCode.Text;
                if (lstTables.SelectedItem != null)
                    fileName = ToPascalCase(lstTables.SelectedItem.ToString()) + ".cs";
            }
            else if (tabGeneratedCode.SelectedIndex == 1) // Logic
            {
                code = txtLogicCode.Text;
                fileName = (_currentProcedureName ?? "Procedure") + "_Logic.cs";
            }

            if (string.IsNullOrWhiteSpace(code))
            {
                SetStatus("No code to save.", true);
                return;
            }

            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.Description = "Select folder to save code";
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string path = Path.Combine(fbd.SelectedPath, fileName);
                    File.WriteAllText(path, code);
                    SetStatus($"Saved: {fileName}");
                    MessageBox.Show($"Saved successfully!\n{path}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void tabGeneratedCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_currentTableName)) return;

            try
            {
                // تحويل إلى SimpleColumnInfo
                var simpleColumns = _currentTableColumns.Select(c => new SimpleColumnInfo
                {
                    Name = c.Name,
                    DataType = c.DataType
                }).ToList();

                if (tabGeneratedCode.SelectedIndex == 2) // Data Logic (Table)
                {
                    string dataLogicCode = _codeGenService.GenerateTableDataLogic(_currentTableName, simpleColumns);
                    txtTableData.Text = dataLogicCode;
                    SetStatus($"Generated Data Logic for table '{_currentTableName}'.");
                }
                else if (tabGeneratedCode.SelectedIndex == 3) // Business Logic (Table)
                {
                    string businessLogicCode = _codeGenService.GenerateTableBusinessLogic(_currentTableName, simpleColumns);
                    txtTableBusiness.Text = businessLogicCode;
                    SetStatus($"Generated Business Logic with DTO for table '{_currentTableName}'.");
                }
            }
            catch (Exception ex)
            {
                SetStatus("Generation failed: " + ex.Message, true);
                MessageBox.Show("Error generating code:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}