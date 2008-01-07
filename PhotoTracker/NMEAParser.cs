using System;
using System.Collections.Generic;
using System.Text;

namespace Org.Nikonfans.PhotoTracker
{
    class NMEAParser
    {

        // Parameters for the GWS84 ellipsoid
        const double WGS84_E2 = 0.006694379990197;
        const double WGS84_E4 = WGS84_E2 * WGS84_E2;
        const double WGS84_E6 = WGS84_E4 * WGS84_E2;
        const double WGS84_SEMI_MAJOR_AXIS = 6378137.0;
        const double WGS84_SEMI_MINOR_AXIS = 6356752.314245;

        // Parameters for UTM projection
        const double UTM_LONGITUDE_OF_ORIGIN = 3.0 / 180 * Math.PI;
        const double UTM_LATITUDE_OF_ORIGIN = 0;
        const double UTM_FALSE_EASTING = 500000;
        const double UTM_FALSE_NORTHING_N = 0; // Northern hemisphere
        const double UTM_FALSE_NORTHING_S = 10000000; // Southern hemisphere
        const double UTM_SCALE_FACTOR = 0.9996;

        public struct NMEA_GPRMC_DATA
        {
            public DateTime UTCDateTime;        // Universal Time Coordinated (UTC) (Greenwich time). HHMMSS.SSS
            public bool IsValid;                // Valid/Not valid 	A/V
            public double LatDecDegree;         // Latitude 	DDMM.MMM
            public string HemisphereNS;         // Hemisphere. North/South 	N/S
            public double LongDecDegree;        // Longitude 	DDDMM.MMM
            public string HemisphereEW;         // Hemisphere. East/West 	E/W
            // public double SpeedOG;           // Speed over ground D.DDD
            // public double Direction;         // Direction of movement (degrees) DDD.D
        }

        public NMEA_GPRMC_DATA proccessNMEAGPRMC(string[] NMEASentenceData)
        {
            NMEA_GPRMC_DATA tData = new NMEA_GPRMC_DATA();
            string tUTCTime, tUTCDate;

            try
            {
                tData.LatDecDegree = Math.Round(Nmea2DecDeg(NMEASentenceData[3], NMEASentenceData[4]), 6);
                tData.LongDecDegree = Math.Round(Nmea2DecDeg(NMEASentenceData[5], NMEASentenceData[6]),6);
                tData.HemisphereNS = NMEASentenceData[4];
                tData.HemisphereEW = NMEASentenceData[6];

                if (NMEASentenceData[2].Equals("A"))
                {
                    tData.IsValid = true;
                }

                // Add : into time string to make it looks like HH:MM:SS
                tUTCTime = NMEASentenceData[1].Insert(2, ":");
                tUTCTime = tUTCTime.Insert(5, ":");

                // Reform date string to be YY/MM/DD
                tUTCDate = NMEASentenceData[9].Substring(4, 2) + ":" +
                           NMEASentenceData[9].Substring(2, 2) + ":" +
                           NMEASentenceData[9].Substring(0, 2);

                tData.UTCDateTime = DateTime.ParseExact(tUTCDate + " " + tUTCTime, "yy:MM:dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);

                return tData;
            }
            catch (Exception e)
            {
                // System.Diagnostics.Debug.Print(e.Message);
            }

            return tData;
        }

        // Returns true if checksum of NMEA sentence is valid
        public bool ValidateChecksum(string Sentence)
        {
            int sum = 0, inx;
            char[] sentence_chars = Sentence.ToCharArray();
            char tmp;
            // All character xor:ed results in the trailing hex checksum
            // The checksum calc starts after '$' and ends before '*'
            for (inx = 1; ; inx++)
            {
                if (inx >= sentence_chars.Length) // No checksum found
                    return false;
                tmp = sentence_chars[inx];
                // Indicates end of data and start of checksum
                if (tmp == '*')
                    break;
                sum = sum ^ tmp; // Build checksum
            }
            // Calculated checksum converted to a 2 digit hex string
            string sum_str = String.Format("{0:X2}", sum);
            // Compare to checksum in sentence
            return sum_str.Equals(Sentence.Substring(inx + 1, 2));
        }

        private string m_raw_buffer;
        // Returns an array of all fields in the next valid NMEA sentence RawData 
        // adds new raw data to be parsed. If RawData is null parsing of old data 
        // continues. Returns null if no more valid sentences are found
        public string[] Parse(string RawData)
        {
            string sentence;
            int start, end;

            if (RawData != null)
            {
                m_raw_buffer += RawData; // Add new data
            }
            do
            {
                // Find start of next sentence
                start = m_raw_buffer.IndexOf("$");
                if (start == -1)
                {
                    // No start found
                    m_raw_buffer = null;
                    return null;
                }

                m_raw_buffer = m_raw_buffer.Substring(start);
                // Find end of sentence
                end = m_raw_buffer.IndexOf("\r\n");

                if (end == -1)
                {
                    // No end found, wait for more data
                    return null;
                }

                sentence = m_raw_buffer.Substring(0, end + 2);
                m_raw_buffer = m_raw_buffer.Substring(end + 2);
            }
            // while (true);
            while (!ValidateChecksum(sentence));

            // Valid sentence found!
            // Remove trailing checksum and \r\n
            sentence = sentence.Substring(0, sentence.IndexOf("*"));
            // Split into fields and return array
            return sentence.Split(",".ToCharArray());

        }

        // Converts NMEA formatted (DDMM.MMMMM) position (latitude or longitude)
        // to decimal degrees
        public double Nmea2DecDeg(string NmeaLonLat, string Hemisphere)
        {
            int inx = NmeaLonLat.IndexOf(".");
            if (inx == -1)
            {
                return 0; // Invalid syntax
            }
            string minutes_str = NmeaLonLat.Substring(inx - 2);
            double minutes = Double.Parse(minutes_str);
            string degrees_str = NmeaLonLat.Substring(0, inx - 2);
            double degrees = Convert.ToDouble(degrees_str) + minutes / 60.0;
            if (Hemisphere.Equals("W") || Hemisphere.Equals("S"))
            {
                degrees = -degrees;
            }
            return degrees;
        }

        // Takes a position in latitude / longitude (WGS84) as input
        // Returns position in UTM easting/northing/zone (in meters)
        public void DecDeg2UTM(double latitude, double longitude, out double easting, out double northing, out int zone)
        {
            // Normalize longitude into Zone, 6 degrees
            int int_zone = (int)(longitude / 6.0);
            if (longitude < 0)
                int_zone--;
            longitude -= (double)int_zone * 6.0;
            zone = int_zone + 31; // UTM zone
            // Convert from decimal degrees to radians
            longitude *= Math.PI / 180.0;
            latitude *= Math.PI / 180.0;
            // Projection
            double M = WGS84_SEMI_MAJOR_AXIS * m_calc(latitude);
            double M_origin = WGS84_SEMI_MAJOR_AXIS * m_calc(UTM_LATITUDE_OF_ORIGIN);
            double A = (longitude - UTM_LONGITUDE_OF_ORIGIN) * Math.Cos(latitude);
            double A2 = A * A;
            double e2_prim = WGS84_E2 / (1 - WGS84_E2);
            double C = e2_prim * Math.Pow(Math.Cos(latitude), 2);
            double T = Math.Tan(latitude);
            T *= T;
            double v = WGS84_SEMI_MAJOR_AXIS / Math.Sqrt(1 - WGS84_E2 *
            Math.Pow(Math.Sin(latitude), 2));
            northing = UTM_SCALE_FACTOR * (M - M_origin + v * Math.Tan(latitude) * (
            A2 / 2 + (5 - T + 9 * C + 4 * C * C) * A2 * A2 / 24 +
            (61 - 58 * T + T * T + 600 * C - 330 * e2_prim) *
            A2 * A2 * A2 / 720));
            if (latitude < 0)
                northing += UTM_FALSE_NORTHING_S;
            easting = UTM_FALSE_EASTING + UTM_SCALE_FACTOR * v * (
            A + (1 - T + C) * A2 * A / 6 +
            (5 - 18 * T + T * T + 72 * C - 58 * e2_prim) * A2 * A2 * A / 120);
        }

        private double m_calc(double lat)
        {
            return (1 - WGS84_E2 / 4 - 3 * WGS84_E4 / 64 - 5 * WGS84_E6 / 256) * lat -
            (3 * WGS84_E2 / 8 + 3 * WGS84_E4 / 32 + 45 * WGS84_E6 / 1024) *
            Math.Sin(2 * lat) + (15 * WGS84_E4 / 256 + 45 * WGS84_E6 / 1024) *
            Math.Sin(4 * lat) - (35 * WGS84_E6 / 3072) * Math.Sin(6 * lat);
        }

    }
}
