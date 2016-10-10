using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Feathers
{
    public class FeatherLogger
    {
        public const string TAB_STRING = "  "; // 2-space tab

        public const int TRACE_LEVEL_EXTREME = 10;
        public const int TRACE_LEVEL_INFO = 9;
        public const int TRACE_LEVEL_SQL = 6;
        public const int TRACE_LEVEL_WARN = 3;
        public const int TRACE_LEVEL_ERROR = 1;
        public const int TRACE_LEVEL_NOTHING = int.MinValue;

        public int TraceLevel { get; set; }
        public string FullPath { get { return m_strFolderName + @"\" + m_strFilename; } }
        public string ErrorMessage { get; set; } // Contains text when the logger was created in some wrong state

        private int m_iTabLevel;
        private string m_strFolderName, m_strFilename;


        public FeatherLogger()
        {
            ErrorMessage = "";
            TraceLevel = TRACE_LEVEL_NOTHING;
        }

        public FeatherLogger(int p_iTraceLevel, string p_strFoldername, string p_strFilename, bool p_blnTimestampInFilename, string p_strExtension)
        {
            ErrorMessage = "";
            TraceLevel = TRACE_LEVEL_NOTHING;

            try
            {
                this.TraceLevel = p_iTraceLevel;

                m_strFolderName = !String.IsNullOrEmpty(p_strFoldername) ? p_strFoldername :
                    Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

                m_strFilename = p_strFilename + (p_blnTimestampInFilename ? DateTime.Now.ToString("yyyyMMddhhmmss") : "") +
                    (String.IsNullOrEmpty(p_strExtension) ? "" : "." + p_strExtension);

                m_iTabLevel = 0;

                if (!Directory.Exists(m_strFolderName)) Directory.CreateDirectory(m_strFolderName);

                WriteOneLine("<!-- Logger starting -->");
            }
            catch (Exception ex)
            {
                this.ErrorMessage = "EXCEPTION: " + ex.Message +
                    ((ex.InnerException == null) ? "" : " / INNER EXCEPTION: " + ex.InnerException.Message);
            }

        }

        public void Error(string s)
        {
            if (this.TraceLevel >= TRACE_LEVEL_ERROR) WriteOneLine("ERROR: " + s);
        }

        public void Error(Exception ex)
        {
            if (this.TraceLevel >= TRACE_LEVEL_ERROR)
            {
                WriteOneLine("ERROR: Got exception '" + ex.Message + "'");
                if (ex.InnerException != null)
                {
                    WriteOneLine("ERROR: Got inner exception '" +
                        ex.InnerException.Message + "'");
                }
            }
        }

        public void Warn(string s)
        {
            if (this.TraceLevel >= TRACE_LEVEL_WARN) WriteOneLine("WARN : " + s);
        }

        public void Sql(string s)
        {
            if (this.TraceLevel >= TRACE_LEVEL_SQL) WriteOneLine("S Q L: " + s);
        }

        public void Info(string s)
        {
            if (this.TraceLevel >= TRACE_LEVEL_INFO) WriteOneLine("INFO : " + s);
        }

        public void Extreme(string s)
        {
            if (this.TraceLevel >= TRACE_LEVEL_EXTREME) WriteOneLine("EXTRM: " + s);
        }

        public void OpenSection(string s)
        {
            WriteOneLine("<" + s + "> " + GenerateTimestamp());
            m_iTabLevel++;
        }

        public void CloseSection(string s)
        {
            m_iTabLevel--;
            WriteOneLine("</" + s + "> " + GenerateTimestamp());
        }

        private void WriteOneLine(string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m_iTabLevel; i++) sb.Append(TAB_STRING);
            sb.Append(s);
            using (StreamWriter sw = new StreamWriter(m_strFolderName + @"\" + m_strFilename, true, Encoding.UTF8))
            {
                sw.WriteLine(FeatherStrings.ReplaceWhitespaceSpecialCharacters(sb.ToString()));
            }
        }

        private string GenerateTimestamp()
        {
            return DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            // This way of calculating milliseconds doesn't work much, sometimes gives values higher than 1000
            //    (Convert.ToInt32(DateTime.Now.TimeOfDay.TotalMilliseconds) -
            //    (Convert.ToInt32(DateTime.Now.TimeOfDay.TotalSeconds) * 1000));
        }



        // This uses the app.config from the calling exe
        /*
        public static FeatherLogger CreateFeatherLoggerFromConfigFile()
        {
            FeatherLogger fl = new FeatherLogger(); // logs nothing
            string strTraceLevel, strLogFolder, strLogFilename, strTimestamp;

            try
            {
                strTraceLevel = ConfigurationManager.AppSettings.Get("TraceLevel");
                strLogFolder = ConfigurationManager.AppSettings.Get("LogFolder");
                strLogFilename = ConfigurationManager.AppSettings.Get("LogFilename");
                strTimestamp = ConfigurationManager.AppSettings.Get("TimestampInFilename");
                if (!String.IsNullOrEmpty(strTraceLevel) &&
                    !String.IsNullOrEmpty(strLogFilename) &&
                    !String.IsNullOrEmpty(strTimestamp))
                {
                    // Note that LogFolder can be null/empty, in that case we use the running exe path
                    fl = new FeatherLogger(
                        Convert.ToInt32(strTraceLevel),
                        strLogFolder,
                        strLogFilename,
                        strTimestamp.Equals("TRUE"),
                        "xml"
                    );
                }
            }
            catch (Exception ex)
            {
                fl.ErrorMessage = "EXCEPTION: " + ex.Message +
                    ((ex.InnerException == null) ? "" : " / INNER EXCEPTION: " + ex.InnerException.Message);
            }

            return fl;
        }
        */


    }
}
