namespace CodeGenerator
{
    partial class frmCodeGenerator
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.chkWindowsAuth = new System.Windows.Forms.CheckBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.grpDatabase = new System.Windows.Forms.GroupBox();
            this.cmbDatabases = new System.Windows.Forms.ComboBox();
            this.grpTable = new System.Windows.Forms.GroupBox();
            this.lblSelectedTable = new System.Windows.Forms.Label();
            this.dgvColumns = new System.Windows.Forms.DataGridView();
            this.lstTables = new System.Windows.Forms.CheckedListBox();
            this.grpGenerate = new System.Windows.Forms.GroupBox();
            this.btnSaveCode = new System.Windows.Forms.Button();
            this.txtGeneratedCode = new System.Windows.Forms.TextBox();
            this.lstProcedures = new System.Windows.Forms.ListBox();
            this.btnLoadProcedures = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabGeneratedCode = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtLogicCode = new System.Windows.Forms.TextBox();
            this.btnGenerateLogic = new System.Windows.Forms.Button();
            this.txtTableDataLogic = new System.Windows.Forms.TabPage();
            this.txtTableBusinessLogic = new System.Windows.Forms.TabPage();
            this.txtTableData = new System.Windows.Forms.TextBox();
            this.txtTableBusiness = new System.Windows.Forms.TextBox();
            this.grpDatabase.SuspendLayout();
            this.grpTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).BeginInit();
            this.grpGenerate.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tabGeneratedCode.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.txtTableDataLogic.SuspendLayout();
            this.txtTableBusinessLogic.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server:";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(83, 33);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(110, 20);
            this.txtServer.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "User:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(83, 76);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(110, 20);
            this.txtUser.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(83, 116);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(110, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // chkWindowsAuth
            // 
            this.chkWindowsAuth.AutoSize = true;
            this.chkWindowsAuth.Location = new System.Drawing.Point(83, 153);
            this.chkWindowsAuth.Name = "chkWindowsAuth";
            this.chkWindowsAuth.Size = new System.Drawing.Size(142, 17);
            this.chkWindowsAuth.TabIndex = 6;
            this.chkWindowsAuth.Text = "Windows Authentication";
            this.chkWindowsAuth.UseVisualStyleBackColor = true;
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(69, 181);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(124, 49);
            this.btnConnect.TabIndex = 7;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // grpDatabase
            // 
            this.grpDatabase.Controls.Add(this.cmbDatabases);
            this.grpDatabase.Location = new System.Drawing.Point(223, 36);
            this.grpDatabase.Name = "grpDatabase";
            this.grpDatabase.Size = new System.Drawing.Size(200, 100);
            this.grpDatabase.TabIndex = 8;
            this.grpDatabase.TabStop = false;
            this.grpDatabase.Text = "Databases";
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(6, 19);
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(121, 21);
            this.cmbDatabases.TabIndex = 0;
            this.cmbDatabases.SelectedIndexChanged += new System.EventHandler(this.cmbDatabases_SelectedIndexChanged);
            // 
            // grpTable
            // 
            this.grpTable.Controls.Add(this.dgvColumns);
            this.grpTable.Controls.Add(this.lblSelectedTable);
            this.grpTable.Controls.Add(this.lstTables);
            this.grpTable.Location = new System.Drawing.Point(461, 53);
            this.grpTable.Name = "grpTable";
            this.grpTable.Size = new System.Drawing.Size(690, 500);
            this.grpTable.TabIndex = 9;
            this.grpTable.TabStop = false;
            this.grpTable.Text = "Tables & Columns";
            this.grpTable.Enter += new System.EventHandler(this.grpTable_Enter);
            // 
            // lblSelectedTable
            // 
            this.lblSelectedTable.AutoSize = true;
            this.lblSelectedTable.Location = new System.Drawing.Point(10, 127);
            this.lblSelectedTable.Name = "lblSelectedTable";
            this.lblSelectedTable.Size = new System.Drawing.Size(109, 13);
            this.lblSelectedTable.TabIndex = 2;
            this.lblSelectedTable.Text = "Selected Table: None";
            // 
            // dgvColumns
            // 
            this.dgvColumns.AllowUserToAddRows = false;
            this.dgvColumns.AllowUserToDeleteRows = false;
            this.dgvColumns.AllowUserToOrderColumns = true;
            this.dgvColumns.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvColumns.Location = new System.Drawing.Point(142, 19);
            this.dgvColumns.Name = "dgvColumns";
            this.dgvColumns.ReadOnly = true;
            this.dgvColumns.Size = new System.Drawing.Size(542, 475);
            this.dgvColumns.TabIndex = 1;
            // 
            // lstTables
            // 
            this.lstTables.FormattingEnabled = true;
            this.lstTables.Location = new System.Drawing.Point(16, 19);
            this.lstTables.Name = "lstTables";
            this.lstTables.Size = new System.Drawing.Size(120, 94);
            this.lstTables.TabIndex = 0;
            this.lstTables.SelectedIndexChanged += new System.EventHandler(this.lstTables_SelectedIndexChanged);
            // 
            // grpGenerate
            // 
            this.grpGenerate.Controls.Add(this.btnSaveCode);
            this.grpGenerate.Controls.Add(this.txtGeneratedCode);
            this.grpGenerate.Location = new System.Drawing.Point(3, 241);
            this.grpGenerate.Name = "grpGenerate";
            this.grpGenerate.Size = new System.Drawing.Size(455, 542);
            this.grpGenerate.TabIndex = 10;
            this.grpGenerate.TabStop = false;
            this.grpGenerate.Text = "Generated Code";
            // 
            // btnSaveCode
            // 
            this.btnSaveCode.Location = new System.Drawing.Point(0, 481);
            this.btnSaveCode.Name = "btnSaveCode";
            this.btnSaveCode.Size = new System.Drawing.Size(131, 40);
            this.btnSaveCode.TabIndex = 11;
            this.btnSaveCode.Text = "Save Code to File";
            this.btnSaveCode.UseVisualStyleBackColor = true;
            this.btnSaveCode.Click += new System.EventHandler(this.btnSaveCode_Click);
            // 
            // txtGeneratedCode
            // 
            this.txtGeneratedCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtGeneratedCode.Location = new System.Drawing.Point(3, 16);
            this.txtGeneratedCode.Multiline = true;
            this.txtGeneratedCode.Name = "txtGeneratedCode";
            this.txtGeneratedCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtGeneratedCode.Size = new System.Drawing.Size(449, 459);
            this.txtGeneratedCode.TabIndex = 0;
            // 
            // lstProcedures
            // 
            this.lstProcedures.FormattingEnabled = true;
            this.lstProcedures.Location = new System.Drawing.Point(477, 559);
            this.lstProcedures.Name = "lstProcedures";
            this.lstProcedures.Size = new System.Drawing.Size(186, 160);
            this.lstProcedures.TabIndex = 11;
            this.lstProcedures.SelectedIndexChanged += new System.EventHandler(this.lstProcedures_SelectedIndexChanged);
            // 
            // btnLoadProcedures
            // 
            this.btnLoadProcedures.Location = new System.Drawing.Point(477, 725);
            this.btnLoadProcedures.Name = "btnLoadProcedures";
            this.btnLoadProcedures.Size = new System.Drawing.Size(131, 40);
            this.btnLoadProcedures.TabIndex = 12;
            this.btnLoadProcedures.Text = "Load Procedures";
            this.btnLoadProcedures.UseVisualStyleBackColor = true;
            this.btnLoadProcedures.Click += new System.EventHandler(this.btnLoadProcedures_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sslStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 815);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1791, 22);
            this.statusStrip1.TabIndex = 13;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sslStatus
            // 
            this.sslStatus.Name = "sslStatus";
            this.sslStatus.Size = new System.Drawing.Size(1776, 17);
            this.sslStatus.Spring = true;
            this.sslStatus.Text = "Ready";
            // 
            // tabGeneratedCode
            // 
            this.tabGeneratedCode.Controls.Add(this.tabPage1);
            this.tabGeneratedCode.Controls.Add(this.tabPage2);
            this.tabGeneratedCode.Controls.Add(this.txtTableDataLogic);
            this.tabGeneratedCode.Controls.Add(this.txtTableBusinessLogic);
            this.tabGeneratedCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabGeneratedCode.Location = new System.Drawing.Point(0, 0);
            this.tabGeneratedCode.Name = "tabGeneratedCode";
            this.tabGeneratedCode.SelectedIndex = 0;
            this.tabGeneratedCode.Size = new System.Drawing.Size(1791, 815);
            this.tabGeneratedCode.TabIndex = 14;
            this.tabGeneratedCode.SelectedIndexChanged += new System.EventHandler(this.tabGeneratedCode_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnGenerateLogic);
            this.tabPage1.Controls.Add(this.grpTable);
            this.tabPage1.Controls.Add(this.btnConnect);
            this.tabPage1.Controls.Add(this.chkWindowsAuth);
            this.tabPage1.Controls.Add(this.grpDatabase);
            this.tabPage1.Controls.Add(this.txtPassword);
            this.tabPage1.Controls.Add(this.grpGenerate);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.lstProcedures);
            this.tabPage1.Controls.Add(this.txtUser);
            this.tabPage1.Controls.Add(this.btnLoadProcedures);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtServer);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1783, 789);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Entity Class";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtLogicCode);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1783, 789);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabGeneratedCode";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtLogicCode
            // 
            this.txtLogicCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLogicCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLogicCode.Location = new System.Drawing.Point(3, 3);
            this.txtLogicCode.Multiline = true;
            this.txtLogicCode.Name = "txtLogicCode";
            this.txtLogicCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLogicCode.Size = new System.Drawing.Size(1777, 783);
            this.txtLogicCode.TabIndex = 0;
            // 
            // btnGenerateLogic
            // 
            this.btnGenerateLogic.Location = new System.Drawing.Point(617, 725);
            this.btnGenerateLogic.Name = "btnGenerateLogic";
            this.btnGenerateLogic.Size = new System.Drawing.Size(131, 40);
            this.btnGenerateLogic.TabIndex = 12;
            this.btnGenerateLogic.Text = "Generate Logic Code";
            this.btnGenerateLogic.UseVisualStyleBackColor = true;
            this.btnGenerateLogic.Click += new System.EventHandler(this.btnGenerateLogic_Click);
            // 
            // txtTableDataLogic
            // 
            this.txtTableDataLogic.Controls.Add(this.txtTableData);
            this.txtTableDataLogic.Location = new System.Drawing.Point(4, 22);
            this.txtTableDataLogic.Name = "txtTableDataLogic";
            this.txtTableDataLogic.Padding = new System.Windows.Forms.Padding(3);
            this.txtTableDataLogic.Size = new System.Drawing.Size(1783, 789);
            this.txtTableDataLogic.TabIndex = 2;
            this.txtTableDataLogic.Text = "Data Logic";
            this.txtTableDataLogic.UseVisualStyleBackColor = true;
            // 
            // txtTableBusinessLogic
            // 
            this.txtTableBusinessLogic.Controls.Add(this.txtTableBusiness);
            this.txtTableBusinessLogic.Location = new System.Drawing.Point(4, 22);
            this.txtTableBusinessLogic.Name = "txtTableBusinessLogic";
            this.txtTableBusinessLogic.Size = new System.Drawing.Size(1783, 789);
            this.txtTableBusinessLogic.TabIndex = 3;
            this.txtTableBusinessLogic.Text = "Business Logic";
            this.txtTableBusinessLogic.UseVisualStyleBackColor = true;
            // 
            // txtTableData
            // 
            this.txtTableData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTableData.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTableData.Location = new System.Drawing.Point(3, 3);
            this.txtTableData.Multiline = true;
            this.txtTableData.Name = "txtTableData";
            this.txtTableData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTableData.Size = new System.Drawing.Size(1777, 783);
            this.txtTableData.TabIndex = 0;
            // 
            // txtTableBusiness
            // 
            this.txtTableBusiness.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTableBusiness.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTableBusiness.Location = new System.Drawing.Point(0, 0);
            this.txtTableBusiness.Multiline = true;
            this.txtTableBusiness.Name = "txtTableBusiness";
            this.txtTableBusiness.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtTableBusiness.Size = new System.Drawing.Size(1783, 789);
            this.txtTableBusiness.TabIndex = 1;
            // 
            // frmCodeGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1791, 837);
            this.Controls.Add(this.tabGeneratedCode);
            this.Controls.Add(this.statusStrip1);
            this.Name = "frmCodeGenerator";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmCodeGenerator_Load);
            this.grpDatabase.ResumeLayout(false);
            this.grpTable.ResumeLayout(false);
            this.grpTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvColumns)).EndInit();
            this.grpGenerate.ResumeLayout(false);
            this.grpGenerate.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabGeneratedCode.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.txtTableDataLogic.ResumeLayout(false);
            this.txtTableDataLogic.PerformLayout();
            this.txtTableBusinessLogic.ResumeLayout(false);
            this.txtTableBusinessLogic.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.CheckBox chkWindowsAuth;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox grpDatabase;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.GroupBox grpTable;
        private System.Windows.Forms.DataGridView dgvColumns;
        private System.Windows.Forms.CheckedListBox lstTables;
        private System.Windows.Forms.Label lblSelectedTable;
        private System.Windows.Forms.GroupBox grpGenerate;
        private System.Windows.Forms.TextBox txtGeneratedCode;
        private System.Windows.Forms.Button btnSaveCode;
        private System.Windows.Forms.ListBox lstProcedures;
        private System.Windows.Forms.Button btnLoadProcedures;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel sslStatus;
        private System.Windows.Forms.TabControl tabGeneratedCode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnGenerateLogic;
        private System.Windows.Forms.TextBox txtLogicCode;
        private System.Windows.Forms.TabPage txtTableDataLogic;
        private System.Windows.Forms.TabPage txtTableBusinessLogic;
        private System.Windows.Forms.TextBox txtTableData;
        private System.Windows.Forms.TextBox txtTableBusiness;
    }
}

