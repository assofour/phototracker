using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;
using System.Xml;

namespace Org.Nikonfans.PhotoTracker
{
    // Photo file class
    public class PhotoFile
    {
        // File (EXIF) original information
        private string m_FileFullPath = string.Empty;   // File full path
        private string m_Filename = string.Empty;   // File full path
        private string m_FileType = string.Empty;       // File type like NEF, JPG,CR2
        private string m_Camera = "No Camera";         // Camera model
        private DateTime m_TakenTime = new DateTime();

        // GPS information
        private double m_LatDecDegree = 0;              // 纬度Latitude 	DDMM.MMM
        private string m_HemisphereNS = string.Empty;   // 南北Hemisphere. North/South 	N/S
        private double m_LongDecDegree = 0;             // 经度Longitude 	DDDMM.MMM
        private string m_HemisphereEW = string.Empty;   // 东西Hemisphere. East/West 	E/W

        // Application information
        private bool m_Matched = false;
        private bool m_HasEXIF = false;

        public DateTime TakenTime
        {
            set
            {
                this.m_TakenTime = value;
            }
            get
            {
                return m_TakenTime;
            }
        }

        public string FileFullPath
        {
            set
            {
                this.m_FileFullPath = value;
                this.m_FileFullPath = this.m_FileFullPath.ToLower();
                this.m_Filename = System.IO.Path.GetFileName(this.m_FileFullPath);
                this.m_FileType = this.m_FileFullPath.Substring(this.m_FileFullPath.Length - 4).ToLower();

            }
            get
            {
                return m_FileFullPath;
            }
        }

        public string Filename
        {
            get
            {
                return m_Filename;
            }
        }

        public string Camera
        {
            set
            {
                this.m_Camera = value;
            }
            get
            {
                return m_Camera;
            }
        }

        public string FileType
        {
            get
            {
                return m_FileType;
            }
        }

        public double LatDecDegree
        {
            set
            {
                this.m_LatDecDegree = value;
            }
            get
            {
                return m_LatDecDegree;
            }
        }

        public string HemisphereNS
        {
            set
            {
                this.m_HemisphereNS = value;
            }
            get
            {
                return m_HemisphereNS;
            }
        }

        public double LongDecDegree
        {
            set
            {
                this.m_LongDecDegree = value;
            }
            get
            {
                return m_LongDecDegree;
            }
        }

        public string HemisphereEW
        {
            set
            {
                this.m_HemisphereEW = value;
            }
            get
            {
                return m_HemisphereEW;
            }
        }

        public bool Matched
        {
            set
            {
                this.m_Matched = value;
            }
            get
            {
                return m_Matched;
            }
        }

        public bool HasEXIF
        {
            set
            {
                this.m_HasEXIF = value;
            }
            get
            {
                return m_HasEXIF;
            }
        }

        public PhotoFile()
        {

        }

    }

    // NMEA Log file class
    public class LogFile
    {
        private string m_Filename = string.Empty;
        private string m_FileType = string.Empty;

        private TimeZoneInformation m_TimeZone = TimeZoneInformation.CurrentTimeZone;
        private DateTime m_StartDateTime;
        private DateTime m_EndDateTime;
        private DateTime m_UTCStartDateTime;
        private DateTime m_UTCEndDateTime;

        private ArrayList m_RoutePoints = new ArrayList();

        public string Filename
        {
            set
            {
                this.m_Filename = value;
                this.m_FileType = System.IO.Path.GetExtension(this.m_Filename);
            }
            get
            {
                return m_Filename;
            }
        }

        public string FileType
        {
            get
            {
                return m_FileType;
            }
        }

        public TimeZoneInformation TimeZone
        {
            set
            {
                this.m_TimeZone = value;
                this.m_StartDateTime = TimeZoneInformation.FromUniversalTime(this.m_TimeZone.Index, this.m_UTCStartDateTime);
                this.m_EndDateTime = TimeZoneInformation.FromUniversalTime(this.m_TimeZone.Index, this.m_UTCEndDateTime);
            }
            get
            {
                return m_TimeZone;
            }
        }

        public DateTime StartDateTime
        {
            set
            {
                this.m_StartDateTime = value;
            }
            get
            {
                return m_StartDateTime;
            }
        }

        public DateTime EndDateTime
        {
            set
            {
                this.m_EndDateTime = value;
            }
            get
            {
                return m_EndDateTime;
            }
        }

        public DateTime UTCStartDateTime
        {
            set
            {
                this.m_UTCStartDateTime = value;
            }
            get
            {
                return m_UTCStartDateTime;
            }
        }

        public DateTime UTCEndDateTime
        {
            set
            {
                this.m_UTCEndDateTime = value;
            }
            get
            {
                return m_UTCEndDateTime;
            }
        }


        public ArrayList RoutePoinits
        {
            get
            {
                return m_RoutePoints;
            }
        }

        public void RetrieveGPSData()
        {
            if (this.FileType.ToLower().Equals(".gpx"))
            {
                this.ParseGPXLogData();
            }
            else
            {
                this.ParseNMEALogData();
            }
        }

        private void ParseNMEALogData()
        {   
            if (!this.m_Filename.Equals(string.Empty))
            {
                try
                {
                    StreamReader srLogFile = new StreamReader(this.m_Filename, Encoding.ASCII);
                    NMEAParser parser = new NMEAParser();

                    while (srLogFile.Peek() > -1)
                    {
                        string _sentence = srLogFile.ReadLine() + "\r\n";
                        if (_sentence.Substring(0, 6).Equals("$GPRMC"))
                        {
                            NMEAParser.NMEA_GPRMC_DATA _Point = parser.proccessNMEAGPRMC(parser.Parse(_sentence));
                            if (this.m_RoutePoints.Count == 0)
                            {
                                this.m_UTCStartDateTime = _Point.UTCDateTime;
                                this.m_UTCEndDateTime = _Point.UTCDateTime;
                            }
                            else
                            {
                                if (DateTime.Compare(this.m_StartDateTime, _Point.UTCDateTime) > 0)
                                {
                                    this.m_UTCStartDateTime = _Point.UTCDateTime;
                                }

                                if (DateTime.Compare(this.m_EndDateTime, _Point.UTCDateTime) < 0)
                                {
                                    this.m_UTCEndDateTime = _Point.UTCDateTime;
                                }
                            }

                            // Transform to timezoned time

                            this.m_RoutePoints.Add(_Point);
                        }
                    }

                    this.m_StartDateTime = TimeZoneInformation.FromUniversalTime(TimeZoneInformation.CurrentTimeZone.Index, this.m_UTCStartDateTime);
                    this.m_EndDateTime = TimeZoneInformation.FromUniversalTime(TimeZoneInformation.CurrentTimeZone.Index, this.m_UTCEndDateTime);

                }
                catch (Exception ex)
                {

                }
            }
        }

        private void ParseGPXLogData()
        {
            if (!this.m_Filename.Equals(string.Empty))
            {
                try
                {
                    string convertedLogPath = System.IO.Path.GetDirectoryName(this.m_Filename) + "\\convert\\";
                    string convertedFilename = System.IO.Path.GetFileName(this.m_Filename) + ".nmea.log";
                    // gpsbabel.exe -i gpx -f mystic_basin_trail.gpx -o "nmea,snlen" -F t.txt
                    System.Diagnostics.Process expsConvert = new System.Diagnostics.Process();
                    expsConvert.StartInfo.FileName = "gpsbabel.exe";

                    if (!System.IO.Directory.Exists(convertedLogPath))
                    {
                        System.IO.Directory.CreateDirectory(convertedLogPath);
                    }
                    expsConvert.StartInfo.Arguments = " -i gpx -f " + '\"' + this.m_Filename + '\"' + " -o nmea -F \"" +
                            convertedLogPath + convertedFilename + '\"';
                    expsConvert.StartInfo.CreateNoWindow = true;
                    expsConvert.StartInfo.UseShellExecute = false;
                    expsConvert.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    expsConvert.StartInfo.RedirectStandardOutput = true;
                    expsConvert.Start();
                    expsConvert.WaitForExit();
                    if (expsConvert.HasExited && System.IO.File.Exists(convertedLogPath + convertedFilename))
                    {
                        this.m_Filename = convertedLogPath + convertedFilename;
                        this.ParseNMEALogData();
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

    }

    // Timezone Comparer Class
    public class TimeZoneComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            TimeZoneInformation tzx, tzy;

            tzx = x as TimeZoneInformation;
            tzy = y as TimeZoneInformation;

            if (tzx == null || tzy == null)
            {
                throw new ArgumentException("Parameter null or wrong type");
            }

            int biasDifference = tzx.Bias - tzy.Bias;

            if (biasDifference == 0)
            {
                return tzx.DisplayName.CompareTo(tzy.DisplayName);
            }
            else
            {
                return biasDifference;
            }
        }
    }

    /// <summary>
    /// AppSettingsReader and Writer.
    /// </summary>
    public class AppSettingsRW
    {
        #region Fields
        /// <summary>
        /// Path to the XML configuration file.
        /// </summary>
        private string configFilePath = null;
        /// <summary>
        /// Loaded when <code>LoadFromFile()</code> method is called.
        /// </summary>
        private NameValueCollection appSettings = null;
        #endregion

        #region Properties
        /// <summary>
        /// Check if you have called <code>LoadFromFile()</code> first.
        /// </summary>
        public NameValueCollection AppSettings
        {
            get
            {
                return appSettings;
            }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// It initializes the <code>configFilePath</code>.
        /// </summary>
        public AppSettingsRW(string configFilePath)
        {
            if (configFilePath == null)
                throw new ArgumentNullException("configFilePath", "Config file cannot be null.");
            this.configFilePath = configFilePath;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Loads the &lt;appSettings&gt; config section data from the configuration file
        /// into the <code>appSettings</code> member of type <code>NameValueCollection</code>.
        /// </summary>
        public void LoadFromFile()
        {
            if (appSettings == null)
            {
                try
                {
                    XmlDocument configXml = new XmlDocument();
                    configXml.Load(this.configFilePath);
                    XmlNode appSettingsNode = configXml.SelectSingleNode("configuration/appSettings");
                    if (appSettingsNode.LocalName == "appSettings")
                    {
                        NameValueSectionHandler handler = new NameValueSectionHandler();

                        // The NameValueCollection instance returned by the 'Create' method is readonly.
                        // Assigning this instance to 'appSettings' member would not allow modifications.
                        // It seems that by passing this instance to the NameValueCollection constructor
                        // there is a similar instance created that is NOT readonly. Hmmm...
                        appSettings = new NameValueCollection((NameValueCollection)handler.Create(null, null, appSettingsNode));
                    }
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("Error while loading appSettings. Message: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// Loops through the <code>appSettings</code> NameValueCollection 
        /// and recreates the XML nodes of the &lt;appSettings&gt; config 
        /// section accordingly. It saves the configuration file afterwards.
        /// </summary>
        public void SaveToFile()
        {
            if (appSettings != null)
            {
                try
                {
                    XmlDocument configXml = new XmlDocument();
                    configXml.Load(this.configFilePath);
                    XmlNode appSettingsNode = configXml.SelectSingleNode("configuration/appSettings");
                    appSettingsNode.RemoveAll();
                    for (int i = 0; i < appSettings.Count; i++)
                    {
                        string key = appSettings.GetKey(i);
                        string val = appSettings.Get(i);

                        XmlNode node = configXml.CreateNode(XmlNodeType.Element, "add", "");

                        XmlAttribute attr = configXml.CreateAttribute("key");
                        attr.Value = key;
                        node.Attributes.Append(attr);

                        attr = configXml.CreateAttribute("value");
                        attr.Value = val;
                        node.Attributes.Append(attr);

                        appSettingsNode.AppendChild(node);
                    }
                    configXml.Save(this.configFilePath);
                }
                catch (Exception ex)
                {
                    throw new ConfigurationException("Error while saving appSettings.", ex);
                }
            }
        }

        /// <summary>
        /// Gets the 'value' of the specified 'key'.
        /// The <code>LoadFromFile()</code> method has to be called first.
        /// </summary>
        /// <returns></returns>
        public string GetValue(string key)
        {
            return appSettings[key];
        }

        /// <summary>
        /// Sets the specified 'value' to the specified 'key'.
        /// The <code>LoadFromFile()</code> method has to be called first.
        /// The configuration file is not modified.
        /// </summary>
        public void SetValue(string key, string val)
        {
            appSettings[key] = val;
        }
        #endregion
    }
}
