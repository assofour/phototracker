namespace Org.Nikonfans.PhotoTracker
{
    partial class frmPhotoTraker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPhotoTraker));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ssBar = new System.Windows.Forms.StatusStrip();
            this.lblPhotoNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLogNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlabPhotoNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlabLogNumber = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnMatch = new System.Windows.Forms.ToolStripButton();
            this.btnSaveAll = new System.Windows.Forms.ToolStripButton();
            this.btnLanguages = new System.Windows.Forms.ToolStripDropDownButton();
            this.btnLanEnglish = new System.Windows.Forms.ToolStripMenuItem();
            this.btnLanSChinese = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.pgApp = new System.Windows.Forms.ProgressBar();
            this.lblMsg = new System.Windows.Forms.Label();
            this.tabOption = new System.Windows.Forms.TabPage();
            this.rtDebug = new System.Windows.Forms.RichTextBox();
            this.chbDebug = new System.Windows.Forms.CheckBox();
            this.chbOverwrite = new System.Windows.Forms.CheckBox();
            this.tabLog = new System.Windows.Forms.TabPage();
            this.lblTimOffset = new System.Windows.Forms.Label();
            this.cmbTimeZone = new System.Windows.Forms.ComboBox();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnNewLog = new System.Windows.Forms.Button();
            this.dgvLogs = new System.Windows.Forms.DataGridView();
            this.tabPhoto = new System.Windows.Forms.TabPage();
            this.PreviewBox = new System.Windows.Forms.PictureBox();
            this.wb = new System.Windows.Forms.WebBrowser();
            this.btnClearPhotos = new System.Windows.Forms.Button();
            this.btnSelectPhotoDir = new System.Windows.Forms.Button();
            this.btnAddPhotos = new System.Windows.Forms.Button();
            this.dgvPhotos = new System.Windows.Forms.DataGridView();
            this.tabWizard = new System.Windows.Forms.TabControl();
            this.ssBar.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabOption.SuspendLayout();
            this.tabLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).BeginInit();
            this.tabPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhotos)).BeginInit();
            this.tabWizard.SuspendLayout();
            this.SuspendLayout();
            // 
            // ssBar
            // 
            this.ssBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblPhotoNumber,
            this.lblLogNumber});
            this.ssBar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            resources.ApplyResources(this.ssBar, "ssBar");
            this.ssBar.Name = "ssBar";
            // 
            // lblPhotoNumber
            // 
            this.lblPhotoNumber.Name = "lblPhotoNumber";
            resources.ApplyResources(this.lblPhotoNumber, "lblPhotoNumber");
            // 
            // lblLogNumber
            // 
            this.lblLogNumber.Name = "lblLogNumber";
            resources.ApplyResources(this.lblLogNumber, "lblLogNumber");
            // 
            // tlabPhotoNumber
            // 
            this.tlabPhotoNumber.Name = "tlabPhotoNumber";
            resources.ApplyResources(this.tlabPhotoNumber, "tlabPhotoNumber");
            // 
            // tlabLogNumber
            // 
            this.tlabLogNumber.Name = "tlabLogNumber";
            resources.ApplyResources(this.tlabLogNumber, "tlabLogNumber");
            this.tlabLogNumber.Spring = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnMatch,
            this.btnSaveAll,
            this.btnLanguages,
            this.btnExit});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            // 
            // btnMatch
            // 
            this.btnMatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMatch.Name = "btnMatch";
            resources.ApplyResources(this.btnMatch, "btnMatch");
            this.btnMatch.Click += new System.EventHandler(this.btnStartMatch_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.btnSaveAll, "btnSaveAll");
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Click += new System.EventHandler(this.btnSaveAll_Click);
            // 
            // btnLanguages
            // 
            this.btnLanguages.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLanguages.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnLanEnglish,
            this.btnLanSChinese});
            this.btnLanguages.Name = "btnLanguages";
            resources.ApplyResources(this.btnLanguages, "btnLanguages");
            // 
            // btnLanEnglish
            // 
            this.btnLanEnglish.Name = "btnLanEnglish";
            resources.ApplyResources(this.btnLanEnglish, "btnLanEnglish");
            this.btnLanEnglish.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // btnLanSChinese
            // 
            this.btnLanSChinese.Name = "btnLanSChinese";
            resources.ApplyResources(this.btnLanSChinese, "btnLanSChinese");
            this.btnLanSChinese.Click += new System.EventHandler(this.btnLanSChinese_Click);
            // 
            // btnExit
            // 
            this.btnExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.btnExit, "btnExit");
            this.btnExit.Name = "btnExit";
            this.btnExit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // pgApp
            // 
            this.pgApp.ForeColor = System.Drawing.SystemColors.Desktop;
            resources.ApplyResources(this.pgApp, "pgApp");
            this.pgApp.Name = "pgApp";
            // 
            // lblMsg
            // 
            resources.ApplyResources(this.lblMsg, "lblMsg");
            this.lblMsg.Name = "lblMsg";
            // 
            // tabOption
            // 
            this.tabOption.Controls.Add(this.rtDebug);
            this.tabOption.Controls.Add(this.chbDebug);
            this.tabOption.Controls.Add(this.chbOverwrite);
            resources.ApplyResources(this.tabOption, "tabOption");
            this.tabOption.Name = "tabOption";
            this.tabOption.UseVisualStyleBackColor = true;
            // 
            // rtDebug
            // 
            this.rtDebug.BackColor = System.Drawing.SystemColors.Info;
            this.rtDebug.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.rtDebug, "rtDebug");
            this.rtDebug.Name = "rtDebug";
            this.rtDebug.ReadOnly = true;
            // 
            // chbDebug
            // 
            resources.ApplyResources(this.chbDebug, "chbDebug");
            this.chbDebug.Name = "chbDebug";
            this.chbDebug.UseVisualStyleBackColor = true;
            // 
            // chbOverwrite
            // 
            resources.ApplyResources(this.chbOverwrite, "chbOverwrite");
            this.chbOverwrite.Name = "chbOverwrite";
            this.chbOverwrite.UseCompatibleTextRendering = true;
            this.chbOverwrite.UseVisualStyleBackColor = true;
            // 
            // tabLog
            // 
            this.tabLog.Controls.Add(this.lblTimOffset);
            this.tabLog.Controls.Add(this.cmbTimeZone);
            this.tabLog.Controls.Add(this.btnClearLogs);
            this.tabLog.Controls.Add(this.btnNewLog);
            this.tabLog.Controls.Add(this.dgvLogs);
            resources.ApplyResources(this.tabLog, "tabLog");
            this.tabLog.Name = "tabLog";
            this.tabLog.UseVisualStyleBackColor = true;
            this.tabLog.Click += new System.EventHandler(this.tabLog_Click);
            // 
            // lblTimOffset
            // 
            resources.ApplyResources(this.lblTimOffset, "lblTimOffset");
            this.lblTimOffset.Name = "lblTimOffset";
            // 
            // cmbTimeZone
            // 
            resources.ApplyResources(this.cmbTimeZone, "cmbTimeZone");
            this.cmbTimeZone.FormattingEnabled = true;
            this.cmbTimeZone.Name = "cmbTimeZone";
            // 
            // btnClearLogs
            // 
            resources.ApplyResources(this.btnClearLogs, "btnClearLogs");
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            this.btnClearLogs.Click += new System.EventHandler(this.btnClearLogs_Click);
            // 
            // btnNewLog
            // 
            resources.ApplyResources(this.btnNewLog, "btnNewLog");
            this.btnNewLog.Name = "btnNewLog";
            this.btnNewLog.UseVisualStyleBackColor = true;
            this.btnNewLog.Click += new System.EventHandler(this.btnNewLog_Click);
            // 
            // dgvLogs
            // 
            this.dgvLogs.AllowUserToAddRows = false;
            this.dgvLogs.AllowUserToDeleteRows = false;
            this.dgvLogs.AllowUserToResizeRows = false;
            this.dgvLogs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLogs.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvLogs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLogs.CausesValidation = false;
            this.dgvLogs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvLogs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogs.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.dgvLogs, "dgvLogs");
            this.dgvLogs.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvLogs.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvLogs.MultiSelect = false;
            this.dgvLogs.Name = "dgvLogs";
            this.dgvLogs.RowHeadersVisible = false;
            this.dgvLogs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvLogs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogs.ShowEditingIcon = false;
            // 
            // tabPhoto
            // 
            this.tabPhoto.Controls.Add(this.PreviewBox);
            this.tabPhoto.Controls.Add(this.wb);
            this.tabPhoto.Controls.Add(this.btnClearPhotos);
            this.tabPhoto.Controls.Add(this.btnSelectPhotoDir);
            this.tabPhoto.Controls.Add(this.btnAddPhotos);
            this.tabPhoto.Controls.Add(this.dgvPhotos);
            resources.ApplyResources(this.tabPhoto, "tabPhoto");
            this.tabPhoto.Name = "tabPhoto";
            this.tabPhoto.UseVisualStyleBackColor = true;
            // 
            // PreviewBox
            // 
            resources.ApplyResources(this.PreviewBox, "PreviewBox");
            this.PreviewBox.Name = "PreviewBox";
            this.PreviewBox.TabStop = false;
            // 
            // wb
            // 
            resources.ApplyResources(this.wb, "wb");
            this.wb.MinimumSize = new System.Drawing.Size(20, 20);
            this.wb.Name = "wb";
            this.wb.Url = new System.Uri("http://www.nikonfans.org/PhotoTracker/PositionVerifymin.html", System.UriKind.Absolute);
            // 
            // btnClearPhotos
            // 
            resources.ApplyResources(this.btnClearPhotos, "btnClearPhotos");
            this.btnClearPhotos.Name = "btnClearPhotos";
            this.btnClearPhotos.UseVisualStyleBackColor = true;
            this.btnClearPhotos.Click += new System.EventHandler(this.btnClearPhotos_Click);
            // 
            // btnSelectPhotoDir
            // 
            resources.ApplyResources(this.btnSelectPhotoDir, "btnSelectPhotoDir");
            this.btnSelectPhotoDir.Name = "btnSelectPhotoDir";
            this.btnSelectPhotoDir.UseVisualStyleBackColor = true;
            this.btnSelectPhotoDir.Click += new System.EventHandler(this.btnSelectPhotoDir_Click);
            // 
            // btnAddPhotos
            // 
            resources.ApplyResources(this.btnAddPhotos, "btnAddPhotos");
            this.btnAddPhotos.Name = "btnAddPhotos";
            this.btnAddPhotos.UseVisualStyleBackColor = true;
            this.btnAddPhotos.Click += new System.EventHandler(this.btnAddPhotos_Click);
            // 
            // dgvPhotos
            // 
            this.dgvPhotos.AllowUserToAddRows = false;
            this.dgvPhotos.AllowUserToDeleteRows = false;
            this.dgvPhotos.AllowUserToOrderColumns = true;
            this.dgvPhotos.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPhotos.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPhotos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPhotos.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvPhotos.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvPhotos.CausesValidation = false;
            this.dgvPhotos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Tahoma", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Info;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPhotos.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvPhotos.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvPhotos.GridColor = System.Drawing.SystemColors.ControlLight;
            resources.ApplyResources(this.dgvPhotos, "dgvPhotos");
            this.dgvPhotos.MultiSelect = false;
            this.dgvPhotos.Name = "dgvPhotos";
            this.dgvPhotos.ReadOnly = true;
            this.dgvPhotos.RowHeadersVisible = false;
            this.dgvPhotos.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToDisplayedHeaders;
            this.dgvPhotos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPhotos.ShowEditingIcon = false;
            this.dgvPhotos.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPhotos_DataBindingComplete);
            this.dgvPhotos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPhotos_CellContentClick);
            // 
            // tabWizard
            // 
            this.tabWizard.Controls.Add(this.tabPhoto);
            this.tabWizard.Controls.Add(this.tabLog);
            this.tabWizard.Controls.Add(this.tabOption);
            resources.ApplyResources(this.tabWizard, "tabWizard");
            this.tabWizard.Name = "tabWizard";
            this.tabWizard.SelectedIndex = 0;
            // 
            // frmPhotoTraker
            // 
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.pgApp);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.tabWizard);
            this.Controls.Add(this.ssBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmPhotoTraker";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Load += new System.EventHandler(this.frmPhotoTraker_Load);
            this.ssBar.ResumeLayout(false);
            this.ssBar.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabOption.ResumeLayout(false);
            this.tabOption.PerformLayout();
            this.tabLog.ResumeLayout(false);
            this.tabLog.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogs)).EndInit();
            this.tabPhoto.ResumeLayout(false);
            this.tabPhoto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PreviewBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPhotos)).EndInit();
            this.tabWizard.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip ssBar;
        private System.Windows.Forms.ToolStripStatusLabel tlabPhotoNumber;
        private System.Windows.Forms.ToolStripStatusLabel tlabLogNumber;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnStartMatch;
        private System.Windows.Forms.ProgressBar pgApp;
        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ToolStripButton btnMatch;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton btnSaveAll;
        private System.Windows.Forms.ToolStripDropDownButton btnLanguages;
        private System.Windows.Forms.ToolStripMenuItem btnLanEnglish;
        private System.Windows.Forms.ToolStripMenuItem btnLanSChinese;
        private System.Windows.Forms.ToolStripStatusLabel lblPhotoNumber;
        private System.Windows.Forms.ToolStripStatusLabel lblLogNumber;
        private System.Windows.Forms.TabPage tabOption;
        private System.Windows.Forms.CheckBox chbOverwrite;
        private System.Windows.Forms.TabPage tabLog;
        private System.Windows.Forms.Label lblTimOffset;
        private System.Windows.Forms.ComboBox cmbTimeZone;
        private System.Windows.Forms.Button btnClearLogs;
        private System.Windows.Forms.Button btnNewLog;
        private System.Windows.Forms.DataGridView dgvLogs;
        private System.Windows.Forms.TabPage tabPhoto;
        private System.Windows.Forms.WebBrowser wb;
        private System.Windows.Forms.Button btnClearPhotos;
        private System.Windows.Forms.Button btnSelectPhotoDir;
        private System.Windows.Forms.Button btnAddPhotos;
        private System.Windows.Forms.DataGridView dgvPhotos;
        private System.Windows.Forms.TabControl tabWizard;
        private System.Windows.Forms.CheckBox chbDebug;
        private System.Windows.Forms.RichTextBox rtDebug;
        private System.Windows.Forms.PictureBox PreviewBox;
    }
}

