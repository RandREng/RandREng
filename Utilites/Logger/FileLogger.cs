using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace CFI.Utility.Logging
{
    public class FileLogger : BaseLogger
    {
        private string m_LogPathName = "";
        private string m_LogBaseFileName = "";
        private const int LOG_TYPE_COL_WIDTH = 13;
        private bool m_LogFileSet = false;

        public FileLogger(string LogPathName, string LogBaseFileName)
        {
            LoggingEnabled = true;
            LogLevel = EnLogLevel.EXCEPTION;

            lock (syncRoot)
            {
                if (false == LogPathName.EndsWith("\\"))
                {
                    LogPathName += @"\";
                }

                m_LogPathName = LogPathName;
                m_LogBaseFileName = LogBaseFileName;

                // if path does not exist, create it.
                if (!Directory.Exists(m_LogPathName))
                {
                    Directory.CreateDirectory(m_LogPathName);
                }

                m_LogFileSet = true;
            }
        }

        override protected bool LogReady
        {
            get
            {
                return (m_LogFileSet);
            }
        }        

        override public void LogSeparator(char LineChar, int Num)
        {

                if (LogReady)
                {
                    if (Num > 80)
                    {
                        Num = 80;
                    }

                    string Line = "";
                    for (int Count = 0; Count < Num; Count++)
                    {
                        Line += LineChar;
                    }

                    writeLine(Line);
                }
        }

        private string lastname = "";
        private string mutexname = "";

        private string getMutexName(string logname)
        {
            if (lastname != logname)
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
                Byte[] bytes = encoding.GetBytes(logname);
                mutexname = Convert.ToBase64String(bytes);
            }
            return mutexname;
        }

        public string GetFileName()
        {
            string DateString = DateTime.Now.ToShortDateString();
            DateString = DateString.Replace("/", "_");
            string LogFile = m_LogPathName + m_LogBaseFileName + "_" + DateString + ".log";
            return LogFile;
        }

        private void writeLine(string line)
        {
            string LogFile = GetFileName();

            using (Mutex mutex = new Mutex(false, getMutexName(LogFile)))
            {
                mutex.WaitOne();
                StreamWriter sw = File.AppendText(LogFile);

                sw.WriteLine(line);
                sw.Close();
                mutex.ReleaseMutex();
            }

        }

        override protected void WriteLog(EnLogLevel Level, string Message)
        {
            // format our data
            string LevelString = "[" + Level.ToString() + "]";
            LevelString = LevelString.PadRight(LOG_TYPE_COL_WIDTH, ' ');
            string Line = String.Format("{0}  {1}{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fffzzz"), LevelString, Message);

            // if this is a warning or worse, add it to our errors list so we can email it later
            if (BufferErrors && (Level == EnLogLevel.ERROR || Level == EnLogLevel.EXCEPTION))
            {
                Errors.Add(Line);
            }
            this.writeLine(Line);
        }
    }
}
