using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CFI.InstallationManager.ReportPlugin
{
    public interface IReportPlugin : IDisposable
    {
        ReportPluginInfo GetPluginInfo();
    }
}
