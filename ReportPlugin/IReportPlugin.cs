using System;

namespace IM.ReportPlugin
{
    public interface IReportPlugin : IDisposable
    {
        ReportPluginInfo GetPluginInfo();
    }
}
