using System;
using System.Collections.Generic;
namespace CFI.Utility.Logging
{
    public enum EnLogLevel { DEBUG = 0, INFO = 1, WARNING = 2, ERROR = 3, EXCEPTION = 4, HEARTBEAT = 5 };

    public interface ILogger
    {
        string Log(EnLogLevel Level, string Message);
        string Log(string Message);
        string LogError(string Message);
        string LogException(Exception ex);
        void LogSeparator();
        void LogSeparator(char LineChar);
        void LogSeparator(char LineChar, int Num);
        bool LoggingEnabled { get; set; }
        EnLogLevel LogLevel { get; set; }

        List<string> GetAndClearErrors();
        bool BufferErrors { get; set; }
    }
}
