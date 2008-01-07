using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Resources;

namespace Org.Nikonfans.PhotoTracker
{
    public partial class frmPhotoTraker : Form
    {

        private ArrayList SelectedPhotoFiles = new ArrayList();
        private ArrayList SelectedLogFiles = new ArrayList();

        
        private int miniInterval = 15;

        BindingSource photoDataSource = new BindingSource();
        BindingSource logDataSource = new BindingSource();

        string SetStrPhotoFilter = string.Empty;
        string SetStrPhotoFileTypes = string.Empty;
        string SetStrLogFilter = string.Empty;
        string SetStrNMEA_GPRMC_Prefix = string.Empty;
        string SetStrEXIFToolPath = string.Empty;
        string SetStrExiftoolReadingArgs = string.Empty;

        private string strPhotosAdded = string.Empty;
        private string strLogsAdded = string.Empty;
        private string strMsgLoadingPhotos = string.Empty;
        private string strMsgLoadedPhotos = string.Empty;
        private string strMsgLoadingLogs = string.Empty;
        private string strMsgLoadedLogs = string.Empty;
        private string strMsgNoPhotoSelected = string.Empty;
        private string strMsgProcessingPhoto = string.Empty;
        private string strMsgProcessingPhotosFinished = string.Empty;
        
        private string strCHPhotoFilePath = string.Empty;
        private string strCHPhotoFilename = string.Empty;
        private string strCHPhotoFileType = string.Empty;
        private string strCHCamera = string.Empty;
        private string strCHTakenTime = string.Empty;
        private string strCHHNS = string.Empty;
        private string strCHLat = string.Empty;
        private string strCHHEW = string.Empty;
        private string strCHLong = string.Empty;
        private string strCHMatched = string.Empty;
        private string strCHLogFilename = string.Empty;
        private string strCHUTCSDT = string.Empty;
        private string strCHUTCEDT = string.Empty;
        private string strCHSDT = string.Empty;
        private string strCHEDT = string.Empty;

        private string pid = string.Empty;

        public frmPhotoTraker()
        {
            InitializeComponent();
        }

        private void btnAddPhotos_Click(object sender, EventArgs e)
        {
            // Open file dialog to select photo files
            ArrayList _tFilteredPhotoFiles = new ArrayList();
            OpenFileDialog ofdPhotos = new OpenFileDialog();
            ofdPhotos.Multiselect = true;
            ofdPhotos.Filter = SetStrPhotoFilter;
            ofdPhotos.Title = "Select photos";

            if (ofdPhotos.ShowDialog() == DialogResult.Cancel)
                return;

            foreach (string filename in ofdPhotos.FileNames)
            {
                _tFilteredPhotoFiles.Add(filename);
            }
 
            pgApp.Value = 0; 
            LoadPhotosDelegate loadPhotos = new LoadPhotosDelegate(LoadPhotos);
            loadPhotos.BeginInvoke(_tFilteredPhotoFiles, null, null);
        }

        // Select a directory, add all photography files under this directory, sub-dir is excluded
        private void btnSelectPhotoDir_Click(object sender, EventArgs e)
        {
            int i = 0;
            ArrayList _tFilteredPhotoFiles = new ArrayList();

            // Show up a directory selection dialog
            System.Windows.Forms.FolderBrowserDialog fbdPhotos = new System.Windows.Forms.FolderBrowserDialog();
            fbdPhotos.ShowNewFolderButton = true;
            fbdPhotos.Description = "Select a photo directory";
            if (fbdPhotos.ShowDialog() == DialogResult.Cancel)
                return;

            // Filter the files, return the Picture file names those with specific file extensions
            DirectoryInfo photoDir = new DirectoryInfo(fbdPhotos.SelectedPath);
            FileInfo[] photoList = photoDir.GetFiles();

            foreach (FileInfo pf in photoList)
            {
                if (SetStrPhotoFileTypes.IndexOf(pf.Extension.ToLower()) >= 0)
                {
                    _tFilteredPhotoFiles.Add(pf.FullName);
                    i++;
                }
            }

            // Load photos into view
            pgApp.Value = 0;
            LoadPhotosDelegate loadPhotos = new LoadPhotosDelegate(LoadPhotos);
            loadPhotos.BeginInvoke(_tFilteredPhotoFiles, null, null);
        }
        
        delegate void LoadPhotosDelegate(ArrayList _tFilteredPhotoFiles);

        private void LoadPhotos(ArrayList _tFilteredPhotoFiles)
        {
            bool isSingleFile = false;
            string _lineBuffer = string.Empty;
            string argFilenames = string.Empty;
            ArrayList alEXIFInfo = new ArrayList();

            Process expsReadingEXIF = new Process();
            expsReadingEXIF.StartInfo.FileName = SetStrEXIFToolPath;
            expsReadingEXIF.StartInfo.CreateNoWindow = true;
            expsReadingEXIF.StartInfo.UseShellExecute = false;
            expsReadingEXIF.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            expsReadingEXIF.StartInfo.RedirectStandardOutput = true;

            // Pick out photos those have been added into selected pool
            // build filenames string as exiftool's arguments
            if (_tFilteredPhotoFiles.Count == 1)
            {
                bool hasFileAdded = false;
                isSingleFile = true;
                string _tFilename = (string)_tFilteredPhotoFiles[0];
                if (SelectedPhotoFiles.Count != 0)
                {
                    foreach (PhotoFile _pf in SelectedPhotoFiles)
                    {
                        if (_pf.FileFullPath.Equals(_tFilename.ToLower()))
                        {
                            hasFileAdded = true;
                            break;
                        }
                    }
                }

                if (!hasFileAdded)
                {
                    argFilenames += '\"' + (string)_tFilteredPhotoFiles[0] + '\"';
                }
                else
                {
                    return;
                }

            }
            else
            {
                foreach (string filename in _tFilteredPhotoFiles)
                {
                    bool hasFileAdded = false;
                    // If the photo file not in array, add it into
                    if (SelectedPhotoFiles.Count != 0)
                    {
                        foreach (PhotoFile _pf in SelectedPhotoFiles)
                        {
                            if (_pf.FileFullPath.Equals(filename.ToLower()))
                            {
                                hasFileAdded = true;
                                break;
                            }
                        }
                    }

                    if (!hasFileAdded)
                    {
                        argFilenames += '\"' + filename + '\"' + " ";
                    }
                    else
                    {
                        continue;
                    }

                }
            }


            // Execute EXIFTOOL
            expsReadingEXIF.StartInfo.Arguments = SetStrExiftoolReadingArgs + argFilenames;
            expsReadingEXIF.StartInfo.
            expsReadingEXIF.Start();
            string strExiftoolOutput = expsReadingEXIF.StandardOutput.ReadToEnd();
            expsReadingEXIF.WaitForExit();
            TextReader tx = new StringReader(strExiftoolOutput);

            ShowProgress(0, 0, "exiftool.exe " + expsReadingEXIF.StartInfo.Arguments, false);

            // Add first line for signle photo file
            if (isSingleFile)
            {
                alEXIFInfo.Add("======== " + (string)_tFilteredPhotoFiles[0]);

            }
            // Build a arraylist for every line in the exif output result
            while (tx.Peek() != -1)
            {
                alEXIFInfo.Add(tx.ReadLine());
            }

            // Add last line for signle photo file
            if (isSingleFile)
            {
                alEXIFInfo.Add("    1 image files read");  // Do not modify it manually
            }

            for (int lineNo = 0; lineNo < alEXIFInfo.Count; )
            {
                string _EXIFFileNameLine = (string)alEXIFInfo[lineNo];
                string _EXIFModelLine = string.Empty;
                string _EXIFCreateDateLine = string.Empty;

                PhotoFile newPF = new PhotoFile();

                if (_EXIFFileNameLine.IndexOf("image files read") != -1)
                {
                    break;
                }
                else if (!_EXIFFileNameLine.Equals(string.Empty))
                {
                    if (_EXIFFileNameLine.IndexOf("========") != -1 && !_EXIFFileNameLine.Substring(9).Trim().Equals(string.Empty))
                    {
                        _EXIFModelLine = (string)alEXIFInfo[lineNo + 1];
                        _EXIFCreateDateLine = (string)alEXIFInfo[lineNo + 2];
                        newPF.FileFullPath = _EXIFFileNameLine.Substring(9).Trim();
                    }
                    
                    if (_EXIFModelLine.IndexOf("Model") != -1 && !_EXIFModelLine.Substring(34).Trim().Equals(string.Empty) && !_EXIFModelLine.Substring(34).Equals("-"))
                    {
                        newPF.Camera = _EXIFModelLine.Substring(34);
                    }
                    
                    // Grab taken time string and convert to DateTime Data Obj, only the taken time can be
                    // fetched, the m_HasEXIF will be set as true
                    if (_EXIFCreateDateLine.IndexOf("CreateDate") != -1 && !_EXIFCreateDateLine.Substring(34).Equals("-"))
                    {
                        newPF.TakenTime = DateTime.ParseExact(_EXIFCreateDateLine.Substring(34, 19), "yyyy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                        newPF.HasEXIF = true;
                    }
                }

                SelectedPhotoFiles.Add(newPF);
                lineNo = lineNo + 3;
                
                ShowProgress(alEXIFInfo.Count - 1, lineNo, strMsgLoadingPhotos, false);
            }

            ShowProgress(0, 0, strMsgLoadedPhotos, true);
        }

        // Add new GPS/GPX files
        private void btnNewLog_Click(object sender, EventArgs e)
        {
            ArrayList _tFilteredLogFiles = new ArrayList();

            // Open file dialog to select log files
            OpenFileDialog ofdLogs = new OpenFileDialog();
            ofdLogs.Multiselect = true;
            ofdLogs.Filter = SetStrLogFilter;
            ofdLogs.Title = "Select Log Files";
            if (ofdLogs.ShowDialog() == DialogResult.Cancel)
                return;

            foreach (string filename in ofdLogs.FileNames)
            {
                _tFilteredLogFiles.Add(filename);
            }

            pgApp.Value = 0;

            LoadLogsDelegate loadLogs = new LoadLogsDelegate(LoadLogs);
            loadLogs.BeginInvoke(_tFilteredLogFiles, null, null);

        }

        delegate void LoadLogsDelegate(ArrayList _tFilteredLogFiles);

        // Load log files
        private void LoadLogs(ArrayList _tFilteredLogFiles)
        {
            bool hasFileAdded = false;

            for(int i = 0; i < _tFilteredLogFiles.Count;)
            {
                LogFile tLogFile = new LogFile();
                
                // If the log file not in array, add it into
                if (SelectedLogFiles.Count != 0)
                {
                    foreach (LogFile _lf in SelectedLogFiles)
                    {
                        if (_lf.Filename.Equals((string)_tFilteredLogFiles[i]))
                        {
                            hasFileAdded = true;
                            break;
                        }
                    }
                }
                tLogFile.Filename = (string)_tFilteredLogFiles[i];
                tLogFile.RetrieveGPSData();
                if (!hasFileAdded)
                { 
                    SelectedLogFiles.Add(tLogFile); 
                    i++;
                    ShowProgress(_tFilteredLogFiles.Count, i, strMsgLoadingLogs, false);
                }
            }

            ShowProgress(0, 0, strMsgLoadedLogs, true);
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            pgApp.Value = 0;
            SaveExifAllPhotosDelegate saveExifAllPhotos = new SaveExifAllPhotosDelegate(SaveExifAllPhotos);
            saveExifAllPhotos.BeginInvoke(null, null);
        }

        delegate void SaveExifAllPhotosDelegate();

        private void SaveExifAllPhotos()
        {
            // 异步显示进度
            string strOptions = string.Empty;
            for (int i = 0; i < SelectedPhotoFiles.Count; )
            {
                PhotoFile _pf = (PhotoFile)SelectedPhotoFiles[i];
                System.Diagnostics.Process expsEXIFWriting = new Process();
                if (_pf.Matched)
                {
                    try
                    {
                        // exiftool running opitions
                        if (chbOverwrite.Checked)
                        {
                            strOptions = "-overwrite_original " + '\"' + _pf.FileFullPath + '\"';
                        }
                        else
                        {

                            string opFn = System.IO.Path.GetDirectoryName(_pf.FileFullPath)
                                        + "\\output\\"
                                        + System.IO.Path.GetFileName(_pf.FileFullPath);

                            strOptions = '\"' + _pf.FileFullPath + '\"' + " -o "
                                        + "\"" + System.IO.Path.GetDirectoryName(_pf.FileFullPath) + "\\output\\"
                                        + System.IO.Path.GetFileName(_pf.FileFullPath) + "\"";

                            if (System.IO.File.Exists(opFn))
                            {
                                try
                                {
                                    System.IO.File.Delete(opFn);
                                }
                                catch (Exception ex)
                                {
                                    // applicationLog("Error : Can't elete existing output file, please remove it manually. " + opFn);
                                }
                            }
                        }

                        expsEXIFWriting.StartInfo.FileName = SetStrEXIFToolPath;
                        expsEXIFWriting.StartInfo.Arguments = " -m -GPSLatitudeRef=" + _pf.HemisphereNS
                                            + " -GPSLatitude=" + _pf.LatDecDegree
                                            + " -GPSLongitudeRef=" + _pf.HemisphereEW
                                            + " -GPSLongitude=" + _pf.LongDecDegree + " "
                                            + strOptions;

                        // Startinfo options
                        expsEXIFWriting.StartInfo.CreateNoWindow = true;
                        expsEXIFWriting.StartInfo.UseShellExecute = false;
                        expsEXIFWriting.StartInfo.CreateNoWindow = true;
                        expsEXIFWriting.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        expsEXIFWriting.StartInfo.RedirectStandardOutput = true;
                        expsEXIFWriting.Start();
                        expsEXIFWriting.WaitForExit();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                    }
                }
                i++;
                ShowProgress(SelectedPhotoFiles.Count, i, strMsgProcessingPhoto + expsEXIFWriting.StartInfo.Arguments, false);
            }

            ShowProgress(0, 0, strMsgProcessingPhotosFinished, true);
        }

        delegate void ShowProgressDelegate(int pgAppMax, int pgAppValue, string strLblMsg, bool isRefreshView);

        void ShowProgress(int pgAppMax, int pgAppValue, string strLblMsg, bool isRefreshView)
        {
            if (this.InvokeRequired == false)
            {
                if (isRefreshView)
                {
                    photoDataSource.ResetBindings(true);
                    dgvPhotos.Refresh();
                    logDataSource.ResetBindings(true);
                    dgvLogs.Refresh();
                }
                else
                {
                    this.pgApp.Maximum = pgAppMax;
                    this.pgApp.Value = pgAppValue;
                }

                if (this.chbDebug.Checked)
                {
                    this.rtDebug.AppendText(strLblMsg + "\r\n");
                }

                lblPhotoNumber.Text = string.Format(strPhotosAdded, SelectedPhotoFiles.Count);
                lblLogNumber.Text = string.Format(strLogsAdded, SelectedLogFiles.Count);
                this.lblMsg.Text = strLblMsg;
            }
            else
            {
                // Show progress asynchronously
                ShowProgressDelegate showProgress = new ShowProgressDelegate(ShowProgress);
                //Invoke(showProgress, new object[] { pi, totalDigits, digitsSoFar});
                BeginInvoke(showProgress, new object[] { pgAppMax, pgAppValue, strLblMsg, isRefreshView });
            }
        }

        private void ChangeLanguage(string cultureString)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture(cultureString);
            ResourceManager rm = new ResourceManager("PhotoTracker.PT", typeof(frmPhotoTraker).Assembly);


            this.Text = rm.GetString("frmPhotoTracker.Text", culture);
            btnAddPhotos.Text = rm.GetString("btnAddPhotos.Text", culture);
            btnClearLogs.Text = rm.GetString("btnClearLogs.Text", culture);
            btnClearPhotos.Text = rm.GetString("btnClearPhotos.Text", culture);
            btnExit.Text = rm.GetString("btnExit.Text", culture);
            btnNewLog.Text = rm.GetString("btnNewLog.Text", culture);
            btnSaveAll.Text = rm.GetString("btnSaveAll.Text", culture);
            btnSelectPhotoDir.Text = rm.GetString("btnSelectPhotoDir.Text", culture);
            btnMatch.Text = rm.GetString("btnMatch.Text", culture);
            chbOverwrite.Text = rm.GetString("chbOverwrite.Text", culture);
            ssBar.Text = rm.GetString("ssBar.Text", culture);
            tabLog.Text = rm.GetString("tabLog.Text", culture);
            tabOption.Text = rm.GetString("tabOption.Text", culture);
            tabPhoto.Text = rm.GetString("tabPhoto.Text", culture);
            btnLanguages.Text = rm.GetString("btnLanguages.Text", culture);
            btnLanSChinese.Text = rm.GetString("btnLanSChinese.Text", culture);
            btnLanEnglish.Text = rm.GetString("btnLanEnglish.Text", culture);
            chbOverwrite.Text = rm.GetString("chbOverwrite.Text", culture);
            lblTimOffset.Text = rm.GetString("lblTimOffset.Text", culture);
            chbDebug.Text = rm.GetString("chbDebug.Text", culture);

            strCHPhotoFilePath = rm.GetString("columnPhotoFilePath.HeaderText", culture);
            strCHPhotoFilename = rm.GetString("columnPhotoFilename.HeaderText", culture);
            strCHPhotoFileType = rm.GetString("columnPhotoFileType.HeaderText", culture);
            strCHCamera = rm.GetString("columnCamera.HeaderText", culture);
            strCHTakenTime = rm.GetString("columnTakenTime.HeaderText", culture);
            strCHHNS = rm.GetString("columnHemisphereNS.HeaderText", culture);
            strCHLat = rm.GetString("columnLatDecDegree.HeaderText", culture);
            strCHHEW = rm.GetString("columnHemisphereEW.HeaderText", culture);
            strCHLong = rm.GetString("columnLongDecDegree.HeaderText", culture);
            strCHMatched = rm.GetString("columnMatched.HeaderText", culture);
            strCHLogFilename = rm.GetString("columnLogFilename.HeaderText", culture);
            strCHUTCSDT = rm.GetString("columnUTCStartDT.HeaderText", culture);
            strCHUTCEDT = rm.GetString("columnUTCEndDT.HeaderText", culture);
            strCHSDT = rm.GetString("columnStartDT.HeaderText", culture);
            strCHEDT = rm.GetString("columnEndDT.HeaderText", culture);

            strMsgLoadingPhotos = rm.GetString("strMsgLoadingPhotos", culture);
            strMsgLoadedPhotos = rm.GetString("strMsgLoadedPhotos", culture);
            strMsgLoadingLogs = rm.GetString("strMsgLoadingLogs", culture);
            strMsgLoadedLogs = rm.GetString("strMsgLoadedLogs", culture);
            strPhotosAdded = rm.GetString("strPhotosAdded", culture);
            strLogsAdded = rm.GetString("strLogsAdded", culture);
            strMsgNoPhotoSelected = rm.GetString("strMsgNoPhotoSelected", culture);
            strMsgProcessingPhoto = rm.GetString("strMsgProcessingPhoto", culture);
            strMsgProcessingPhotosFinished = rm.GetString("strMsgProcessingPhotosFinished", culture);


            AppSettingsRW AS = new AppSettingsRW(Application.StartupPath + "\\PhotoTracker.exe.config");
            AS.LoadFromFile();
            AS.SetValue("Language", cultureString);
            AS.SaveToFile();
        }

        private void frmPhotoTraker_Load(object sender, EventArgs e)
        {
            // Load application settings
            SetStrPhotoFilter = System.Configuration.ConfigurationSettings.AppSettings["PhotoFilter"];
            SetStrPhotoFileTypes = System.Configuration.ConfigurationSettings.AppSettings["PhotoFileTypes"].ToLower();
            SetStrLogFilter = System.Configuration.ConfigurationSettings.AppSettings["LogFilter"];
            SetStrNMEA_GPRMC_Prefix = System.Configuration.ConfigurationSettings.AppSettings["NMEA_GPRMC_Prefix"];
            SetStrEXIFToolPath = System.Configuration.ConfigurationSettings.AppSettings["ExiftoolPath"];
            SetStrExiftoolReadingArgs = System.Configuration.ConfigurationSettings.AppSettings["ExiftoolReadingArgs"];
            // Change default language
            ChangeLanguage(System.Configuration.ConfigurationSettings.AppSettings.Get("Language"));
            // Build Timezone list
            cmbTimeZone.SelectedIndexChanged += new EventHandler(cmbTimeZone_SelectedIndexChanged);
            TimeZoneInformation[] m_zones = TimeZoneInformation.EnumZones(); 
            Array.Sort(m_zones, new TimeZoneComparer());
            cmbTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTimeZone.Items.AddRange(m_zones);
            cmbTimeZone.Text = TimeZoneInformation.CurrentTimeZone.DisplayName;
            initialDGVs();
        }

        private void initialDGVs()
        {
            // Initialize the photo files datagridview component
            photoDataSource.DataSource = SelectedPhotoFiles;
            dgvPhotos.DataSource = photoDataSource;
            dgvPhotos.AutoGenerateColumns = false;

            logDataSource.DataSource = SelectedLogFiles;
            dgvLogs.DataSource = logDataSource;
            dgvLogs.AutoGenerateColumns = false;

            DataGridViewColumn columnPhotoFilePath = new DataGridViewTextBoxColumn();
            columnPhotoFilePath.DataPropertyName = "FileFullPath";
            columnPhotoFilePath.Name = "clnFileFullPath";
            columnPhotoFilePath.HeaderText = strCHPhotoFilePath;
            columnPhotoFilePath.Visible = false;
            this.dgvPhotos.Columns.Add(columnPhotoFilePath);

            DataGridViewColumn columnPhotoFilename = new DataGridViewTextBoxColumn();
            columnPhotoFilename.DataPropertyName = "Filename";
            columnPhotoFilename.Name = "clnFilename";
            columnPhotoFilename.HeaderText = strCHPhotoFilename;
            this.dgvPhotos.Columns.Add(columnPhotoFilename);

            DataGridViewColumn columnPhotoFileType = new DataGridViewTextBoxColumn();
            columnPhotoFileType.DataPropertyName = "FileType";
            columnPhotoFileType.Name = "clnFileType";
            columnPhotoFileType.HeaderText = strCHPhotoFileType;
            columnPhotoFileType.Visible = false;
            this.dgvPhotos.Columns.Add(columnPhotoFileType);

            DataGridViewColumn columnCamera = new DataGridViewTextBoxColumn();
            columnCamera.DataPropertyName = "Camera";
            columnCamera.Name = "clnCamera";
            columnCamera.HeaderText = strCHCamera;
            this.dgvPhotos.Columns.Add(columnCamera);

            DataGridViewColumn columnTakenTime = new DataGridViewTextBoxColumn();
            columnTakenTime.DataPropertyName = "TakenTime";
            columnTakenTime.Name = "clnTakenTime";
            columnTakenTime.HeaderText = strCHTakenTime;
            this.dgvPhotos.Columns.Add(columnTakenTime);

            DataGridViewColumn columnHemisphereNS = new DataGridViewTextBoxColumn();
            columnHemisphereNS.DataPropertyName = "HemisphereNS";
            columnHemisphereNS.Name = "clnHemisphereNS";
            columnHemisphereNS.HeaderText = strCHHNS;
            columnHemisphereNS.Width = 35;
            this.dgvPhotos.Columns.Add(columnHemisphereNS);

            DataGridViewColumn columnLatDecDegree = new DataGridViewTextBoxColumn();
            columnLatDecDegree.DataPropertyName = "LatDecDegree";
            columnLatDecDegree.Name = "clnLatDecDegree";
            columnLatDecDegree.HeaderText = strCHLat;
            columnLatDecDegree.Width = 100;
            this.dgvPhotos.Columns.Add(columnLatDecDegree);

            DataGridViewColumn columnHemisphereEW = new DataGridViewTextBoxColumn();
            columnHemisphereEW.DataPropertyName = "HemisphereEW";
            columnHemisphereEW.Name = "clnHemisphereEW";
            columnHemisphereEW.HeaderText = strCHHEW;
            columnHemisphereEW.Width = 35;
            this.dgvPhotos.Columns.Add(columnHemisphereEW);

            DataGridViewColumn columnLongDecDegree = new DataGridViewTextBoxColumn();
            columnLongDecDegree.DataPropertyName = "LongDecDegree";
            columnLongDecDegree.Name = "clnLongDecDegree";
            columnLongDecDegree.HeaderText = strCHLong;
            this.dgvPhotos.Columns.Add(columnLongDecDegree);

            DataGridViewColumn columnHasEXIF = new DataGridViewTextBoxColumn();
            columnHasEXIF.DataPropertyName = "hasEXIF";
            columnHasEXIF.Name = "clnHasEXIF";
            columnHasEXIF.HeaderText = "Has EXIF";
            columnHasEXIF.Visible = false;
            this.dgvPhotos.Columns.Add(columnHasEXIF);

            DataGridViewColumn columnMatched = new DataGridViewTextBoxColumn();
            columnMatched.DataPropertyName = "Matched";
            columnMatched.Name = "clnMatched";
            columnMatched.HeaderText = strCHMatched;
            columnMatched.Visible = false;
            this.dgvPhotos.Columns.Add(columnMatched);

            // Initialize the GPS/GPX log files datagridview component
            DataGridViewColumn columnLogFilename = new DataGridViewTextBoxColumn();
            columnLogFilename.DataPropertyName = "Filename";
            columnLogFilename.Name = "clnLogFilename";
            columnLogFilename.HeaderText = strCHLogFilename;
            columnLogFilename.ReadOnly = true;
            this.dgvLogs.Columns.Add(columnLogFilename);

            DataGridViewColumn columnUTCStartDT = new DataGridViewTextBoxColumn();
            columnUTCStartDT.DataPropertyName = "UTCStartDateTime";
            columnUTCStartDT.Name = "clnLogUTCStartDT";
            columnUTCStartDT.HeaderText = strCHUTCSDT;
            columnUTCStartDT.ReadOnly = true;
            this.dgvLogs.Columns.Add(columnUTCStartDT);

            DataGridViewColumn columnUTCEndDT = new DataGridViewTextBoxColumn();
            columnUTCEndDT.DataPropertyName = "UTCEndDateTime";
            columnUTCEndDT.Name = "clnLogUTCEndDT";
            columnUTCEndDT.HeaderText = strCHUTCEDT;
            columnUTCEndDT.ReadOnly = true;
            this.dgvLogs.Columns.Add(columnUTCEndDT);

            DataGridViewColumn columnStartDT = new DataGridViewTextBoxColumn();
            columnStartDT.DataPropertyName = "StartDateTime";
            columnStartDT.Name = "clnLogStartDT";
            columnStartDT.HeaderText = strCHSDT;
            columnStartDT.ReadOnly = true;
            this.dgvLogs.Columns.Add(columnStartDT);

            DataGridViewColumn columnEndDT = new DataGridViewTextBoxColumn();
            columnEndDT.DataPropertyName = "EndDateTime";
            columnEndDT.Name = "clnLogEndDT";
            columnEndDT.HeaderText = strCHEDT;
            columnEndDT.ReadOnly = true;
            this.dgvLogs.Columns.Add(columnEndDT);
        }

        // Change row's color, format the dategridview display
        private void dgvPhotos_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < this.dgvPhotos.Rows.Count; )
            {
                //if (i%2 != 1)
                //{
                //    this.dgvPhotos.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                //}

                if (!(bool)this.dgvPhotos.Rows[i].Cells["clnHasEXIF"].Value)
                {
                    this.dgvPhotos.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                }
                if ((bool)this.dgvPhotos.Rows[i].Cells["clnMatched"].Value)
                {
                    this.dgvPhotos.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
                }
                this.dgvPhotos.Rows[i].Cells[1].ToolTipText = (string)this.dgvPhotos.Rows[i].Cells[0].Value;
                i++;
            }
        }

        // Select a timezone for per log file
        private void cmbTimeZone_SelectedIndexChanged(object sender, EventArgs e)
        {
            TimeZoneInformation tziLog = cmbTimeZone.SelectedItem as TimeZoneInformation;

            if (tziLog == null)
                return;

            // Change starttime and endtime view in log view
            // dgvLogs.CurrentCell.Value = tziLog;
            foreach (LogFile _lf in SelectedLogFiles)
            {
                _lf.TimeZone = tziLog;
            }

            logDataSource.ResetBindings(true);
            dgvLogs.Refresh();
        }

        // Clear selected photos
        private void btnClearPhotos_Click(object sender, EventArgs e)
        {
            SelectedPhotoFiles.Clear();
            photoDataSource.ResetBindings(true);
            tlabPhotoNumber.Text = SelectedPhotoFiles.Count.ToString() + " Photos Added";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Clear selected log files
        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            SelectedLogFiles.Clear();
            logDataSource.ResetBindings(true);
            tlabLogNumber.Text = SelectedPhotoFiles.Count.ToString() + " Log Files Added";
        }

        private void btnStartMatch_Click(object sender, EventArgs e)
        {
            if (SelectedLogFiles.Count <= 0)
            {
                return;
            }
            else if (SelectedPhotoFiles.Count <= 0)
            {
                tabPhoto.Show();
                return;
            }

            pgApp.Maximum = SelectedPhotoFiles.Count;
            pgApp.Value = 0;
            pgApp.Step = 1;
            foreach (PhotoFile _pf in SelectedPhotoFiles)
            {
                foreach (LogFile _lf in SelectedLogFiles)
                {
                    foreach (NMEAParser.NMEA_GPRMC_DATA _Point in _lf.RoutePoinits)
                    {
                        TimeSpan _ts = _pf.TakenTime.Subtract(TimeZoneInformation.FromUniversalTime(_lf.TimeZone.Index, _Point.UTCDateTime));
                        if (_pf.HasEXIF && Math.Abs(_ts.TotalSeconds) < miniInterval)
                        {
                            _pf.HemisphereEW = _Point.HemisphereEW;
                            _pf.HemisphereNS = _Point.HemisphereNS;
                            _pf.LatDecDegree = _Point.LatDecDegree;
                            _pf.LongDecDegree = _Point.LongDecDegree;
                            _pf.Matched = true;
                            debugApp(_lf.Filename + " matched a GPS point", 0);
                            break;
                        }
                    }
                }
                pgApp.Value++;
            }
            photoDataSource.ResetBindings(true);
            dgvPhotos.Refresh();
            tabWizard.SelectedTab = tabPhoto;
        }

        private void debugApp(string message, int msgType)
        {
            if (chbDebug.Checked)
            {
                rtDebug.AppendText(message + "\r\n");
            }
        }

        private void englishToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en-US");

        }

        private void btnLanSChinese_Click(object sender, EventArgs e)
        {
            ChangeLanguage("zh-CN");
        }

        private void dgvPhotos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                System.Diagnostics.Process expsPreview = new Process();
                expsPreview.StartInfo.FileName = "cmd.exe";

                // expsPreview.StartInfo.FileName = Application.StartupPath + "\\" + SetStrEXIFToolPath;
                // expsPreview.StartInfo.Arguments = "-PreviewImage " + '\"' + dgvPhotos.CurrentRow.Cells["clnFileFullPath"].Value.ToString() + '\"'
                //         + " > " + '\"' + Application.StartupPath + "\\tmpPreview.jpg" + '\"';

                // exiftool -b -PreviewImage 118_1834.JPG > preview.jpg
                // Startinfo options
                if (File.Exists(Application.StartupPath + "\\" + pid + ".jpg"))
                {
                    File.Delete(Application.StartupPath + "\\" + pid + ".jpg");
                }

                pid = System.Guid.NewGuid().ToString();
                expsPreview.StartInfo.UseShellExecute = false;
                expsPreview.StartInfo.CreateNoWindow = true;
                expsPreview.StartInfo.RedirectStandardInput = true;
                expsPreview.StartInfo.RedirectStandardOutput = true;
                expsPreview.Start();
                expsPreview.StandardInput.WriteLine(Application.StartupPath + "\\" + SetStrEXIFToolPath + " -b -PreviewImage " + '\"' + dgvPhotos.CurrentRow.Cells["clnFileFullPath"].Value.ToString() + "\" > \"" + Application.StartupPath + "\\" + pid + ".jpg\"");
                expsPreview.StandardInput.WriteLine("exit");
                expsPreview.WaitForExit();

                PreviewBox.SizeMode = PictureBoxSizeMode.Zoom;
                PreviewBox.ImageLocation = Application.StartupPath + "\\" + pid + ".jpg";
                PreviewBox.Refresh();

                debugApp(Application.StartupPath + "\\" + SetStrEXIFToolPath + " -b -PreviewImage " + '\"' + dgvPhotos.CurrentRow.Cells["clnFileFullPath"].Value.ToString() + "\" > \"" + Application.StartupPath + "\\" + pid + ".jpg\"", 0);
 
                if (!dgvPhotos.CurrentRow.Cells["clnLatDecDegree"].Value.ToString().Equals(string.Empty) && !dgvPhotos.CurrentRow.Cells["clnLongDecDegree"].Value.ToString().Equals(string.Empty))
                {

                    string httpParas = @"http://www.nikonfans.org/PhotoTracker/PositionVerifyMin.html?NewPoint=1&"
                            + "Ver=1.0.0.0&"
                            + "LatRef=" + dgvPhotos.CurrentRow.Cells["clnHemisphereNS"].Value.ToString() + "&"
                            + "Latitude=" + dgvPhotos.CurrentRow.Cells["clnLatDecDegree"].Value.ToString() + "&"
                            + "LongRef=" + dgvPhotos.CurrentRow.Cells["clnHemisphereEW"].Value.ToString() + "&"
                            + "Longitude=" + dgvPhotos.CurrentRow.Cells["clnLongDecDegree"].Value.ToString();
                    wb.Navigate(httpParas);
                
                }
                else
                {
                    MessageBox.Show(strMsgNoPhotoSelected);
                }
            }
            catch(Exception ex)
            {

            }
        }

        private void tabLog_Click(object sender, EventArgs e)
        {

        }
    }

}