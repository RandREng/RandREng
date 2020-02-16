using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CFI.Utility.Logging
{
    public class BaseLogger : ILogger
    {
        public EnLogLevel LogLevel { get; set; }
        public bool LoggingEnabled { get; set; }
        protected object syncRoot = new Object();
        
        public BaseLogger()
        {
            this.BufferErrors = false;
        }

        static public string GetException(Exception ex)
        {
            // wrap in try catch so we don't let logging error bring it all down
            StringBuilder sb = new StringBuilder();
            Exception innerException = ex;
            string prefix = "";
            while (innerException != null)
            {
                sb.AppendFormat("{0}{1}\r\n\r\n{2}\r\n\r\n", prefix, innerException.Message, innerException.StackTrace);
                innerException = innerException.InnerException;
                prefix = "[INNER EXCEPTION] ";
            }

            return sb.ToString();
        }

        public string LogException(Exception ex)
        {
            try
            {
                // wrap in try catch so we don't let logging error bring it all down
                StringBuilder sb = new StringBuilder();
                Exception innerException = ex;
                string prefix = "";
                while (innerException != null)
                {
                    sb.AppendFormat("{0}{1}\r\n\r\n{2}\r\n\r\n", prefix, innerException.Message, innerException.StackTrace);
                    innerException = innerException.InnerException;
                    prefix = "[INNER EXCEPTION] ";
                }

                return Log(EnLogLevel.EXCEPTION, sb.ToString());
            }
            catch
            {
                return Log(EnLogLevel.ERROR, "Detailed exception logging failed");
            }
        }

        protected List<string> Errors
        {
            get
            {
                if (m_Errors == null)
                {
                    m_Errors = new List<string>();
                }
                return m_Errors;
            }
        }

        virtual protected bool LogReady
        {
            get
            {
                return false;
            }
        }

        public void LogSeparator()
        {
            LogSeparator('=');
        }

        public void LogSeparator(char LineChar)
        {
            LogSeparator(LineChar, 65);
        }

        virtual public void LogSeparator(char LineChar, int Num)
        {
        }

        public string LogError(string Message)
        {
            return Log(EnLogLevel.ERROR, Message);
        }

        public string Log(string Message)
        {
            return Log(EnLogLevel.INFO, Message);
        }

        public string Log(EnLogLevel Level, string Message)
        {
            if (!LoggingEnabled)
            {
                return Message;
            }

            lock (syncRoot)
            {
                if (LogReady)
                {
                    if (Level >= LogLevel)
                    {
                        WriteLog(Level, Message);
                    }
                }
            }

            return Message;
        }

        protected List<string> m_Errors;

        public bool BufferErrors { get; set; }

        public List<string> GetAndClearErrors()
        {
            List<string> TempErrors = new List<string>();
            lock (syncRoot)
            {
                TempErrors.AddRange(this.Errors);
                this.Errors.Clear();
            }
            return TempErrors;
        }


        virtual protected void WriteLog(EnLogLevel Level, string Line)
        { 
        }
    }
}
