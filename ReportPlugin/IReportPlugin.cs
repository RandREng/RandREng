using System;

namespace CFI.InstallationManager.ReportPlugin
{
    public interface IReportPlugin : IDisposable
    {
        ReportPluginInfo GetPluginInfo();
    }
}
