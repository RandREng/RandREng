using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace CFI.Utility.Logging
{
    public sealed class Logger
    {

        //private static volatile ILogger instance;

        private Logger()
        {
        }

        //public static ILogger Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new NullLogger();
        //        }
        //        return instance;
        //    }
        //    set
        //    {
        //        Logger.instance = value;
        //    }
        //}

        public static void WriteEventLog(string Source, Exception ex)
        {
            WriteEventLog(Source, BaseLogger.GetException(ex));
        }

        public static void WriteEventLog(string Source, string Message)
        {
            try
            {
                if (!System.Diagnostics.EventLog.SourceExists(Source))
                {
                    System.Diagnostics.EventLog.CreateEventSource(Source, "Application");
                }

                EventLog log = new EventLog();
                log.Source = Source;
                log.WriteEntry(Message);
            }
            catch(Exception )
            {
            }
        }

        //public static string Log(EnLogLevel level, string message)
        //{
        //    return Logger.Instance.Log(level, message);
        //}

        //public static string Log(string Message)
        //{
        //    return Logger.Instance.Log(Message);
        //}

        //public static string LogError(string Message)
        //{
        //    return Logger.Instance.LogError(Message);
        //}

        //public static string LogException(Exception ex)
        //{
        //    return Logger.Instance.LogException(ex);
        //}

        //public static void LogSeparator()
        //{
        //    Logger.Instance.LogSeparator();
        //}

        //public static EnLogLevel LogLevel
        //{
        //    get
        //    {
        //        return Logger.Instance.LogLevel;
        //    }
        //    set
        //    {
        //        Logger.Instance.LogLevel = value;
        //    }
        //}
    }
}
