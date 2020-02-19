using Microsoft.Extensions.Logging;
using System;

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
