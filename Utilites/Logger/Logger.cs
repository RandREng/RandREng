using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using System.Diagnostics;

namespace CFI.Utility.Logging
{
    public static class Extensions
    {
        public static void LogCritical(this ILogger logger, Exception e)
        {
            logger.LogCritical(e, "");
        }

        public static void Log(this ILogger logger, string message)
        {
            logger.Log(LogLevel.Information, message);
        }
    }

}
