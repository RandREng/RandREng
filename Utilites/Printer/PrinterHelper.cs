using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using CFI.Utility.Logging;
using System.Diagnostics;

namespace CFI.Utility.Printer
{
    public class PrinterHelper
    {
        public string RequestedPrinterName { get; set; }
        public string PrinterName { get; set; }
        public string PrinterDriver { get; set; }
        public string PrinterPort { get; set; }
        public string DeviceID { get; set; }

        private ILogger Logger;

        public PrinterHelper(ILogger logger)
        {
            Logger = logger;
        }

        public void SetDefaultPrinter(string PrinterID)
        {
            ManagementPath path = new ManagementPath();
            ManagementBaseObject inParams = null;
            ManagementBaseObject outParams = null;
            path.Server = ".";
            path.NamespacePath = @"root\CIMV2";
            string relPath = string.Format("Win32_Printer.DeviceID='{0}'", PrinterID);
            path.RelativePath = relPath;

            try
            {
                ManagementObject mo = new ManagementObject(path);
                outParams = mo.InvokeMethod("SetDefaultPrinter", inParams, null);
                Logger.Log(EnLogLevel.INFO, string.Format("Printer {0} is default now ", PrinterID));
            }
            catch (Exception e)
            {
                Logger.Log(EnLogLevel.INFO, "SetDefaultPrinter() - " + e.Message);
            }
        }

        public static string QueryPrinters(string printerName, ILogger logger)
        {
            logger.Log(EnLogLevel.INFO, string.Format("QueryPrinters - {0}", printerName));
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            string deviceID = null;
            // Get DeviceID from the Printer class using the name as search criteria
            string queryString = "SELECT DeviceID FROM Win32_Printer WHERE Name=\"" + printerName + "\"";
            query = new ManagementObjectSearcher(queryString);
            queryCollection = query.Get();
            // should only contain one entry
            foreach (ManagementObject mo in queryCollection)
            {
                deviceID = mo["DeviceID"] as string;
            }
            return deviceID;
        }

        public static void ListPrinters(ILogger logger)
        {
            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;
            string deviceID = null;
            // Get DeviceID from the Printer class using the name as search criteria
            string queryString = "SELECT DeviceID FROM Win32_Printer";
            query = new ManagementObjectSearcher(queryString);
            queryCollection = query.Get();
            // should only contain one entry
            logger.Log(EnLogLevel.INFO, "Available Printers");
            foreach (ManagementObject mo in queryCollection)
            {
                deviceID = mo["DeviceID"] as string;
                logger.Log(EnLogLevel.INFO, deviceID);
            }
            logger.Log(EnLogLevel.INFO, "");
        }

        public bool GetPrinterInfo(string RequestedPrinterName)
        {
            bool Success = false;
            PrinterDriver = "";
            PrinterName = "";
            PrinterPort = "";

            ManagementObjectSearcher query;
            ManagementObjectCollection queryCollection;

            string queryString = "SELECT DriverName, PortName, Name FROM Win32_Printer";
            query = new ManagementObjectSearcher(queryString);
            queryCollection = query.Get();

            foreach (ManagementObject mo in queryCollection)
            {
                string Name = ((string)mo["Name"]).ToUpper();
                if (RequestedPrinterName.ToUpper() == Name)
                {
                    PrinterName = mo["Name"] as String;
                    PrinterDriver = mo["DriverName"] as string;
                    PrinterPort = mo["PortName"] as String;
                    Success = true;
                }
                Logger.Log(String.Format("GetPrinterInfo: Name={0}, Driver={1}, Port={2}", mo["Name"].ToString(), mo["DriverName"].ToString(), mo["PortName"].ToString()));
            }

            return Success;
        }

        public static bool PrintTiff(string filename, string printer, ILogger logger)
        {
            bool bOK = false;
            using (Process myProcess = new Process())
            {
                myProcess.StartInfo.FileName = "rundll32.exe";
                myProcess.StartInfo.Arguments = String.Format("shimgvw.dll,ImageView_PrintTo /pt \"{0}\" \"{1}\"", filename, printer);
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.WaitForExit();
                bOK = myProcess.ExitCode == 0;
                if (myProcess.ExitCode != 0)
                {
                    logger.Log(EnLogLevel.ERROR, string.Format("PrintTiff - {0} - {1} - {2}", myProcess.StartInfo.FileName, myProcess.StartInfo.Arguments, myProcess.ExitCode));
                }
                else
                {
                    logger.Log(EnLogLevel.DEBUG, string.Format("PrintTiff - {0} - {1} - {2}", myProcess.StartInfo.FileName, myProcess.StartInfo.Arguments, myProcess.ExitCode));
                }
            }
            return bOK;
        }

        public static bool Print(string filename)
        {
            bool bOk = false;
            using (Process myProcess = new Process())
            {
                ProcessStartInfo startInfo = new ProcessStartInfo(filename);
                startInfo.Verb = "print";
                myProcess.StartInfo = startInfo;
                myProcess.Start();
//                myProcess.WaitForExit();
                bOk = true;
            }
            return bOk;
        }

        public static bool PrintPDF(string filename, string printer, ILogger logger)
        {
            bool bOK = false;
            using (Process myProcess = new Process())
            {
                string progfiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
                myProcess.StartInfo.FileName = progfiles + @"\Foxit Software\Foxit Reader\Foxit Reader.exe";
                myProcess.StartInfo.Arguments = String.Format("/t \"{0}\" \"{1}\"", filename, printer);
                myProcess.StartInfo.CreateNoWindow = true;
                myProcess.Start();
                myProcess.WaitForExit();
                bOK = myProcess.ExitCode == 0;
                if (myProcess.ExitCode != 0)
                {
                    logger.Log(EnLogLevel.ERROR, string.Format("PrintPDF - {0} - {1} - {2}", myProcess.StartInfo.FileName, myProcess.StartInfo.Arguments, myProcess.ExitCode));
                }
                else
                {
                    logger.Log(EnLogLevel.DEBUG, string.Format("PrintPDF - {0} - {1} - {2}", myProcess.StartInfo.FileName, myProcess.StartInfo.Arguments, myProcess.ExitCode));
                }
            }
            return bOK;
        }

        public void SetPrinter(string RequestedPrinterName)
        {
            RequestedPrinterName = "";
            if (GetPrinterInfo(RequestedPrinterName))
            {
                this.RequestedPrinterName = RequestedPrinterName;
                Logger.Log(EnLogLevel.INFO, String.Format("PO Printer set to: Name={0}, Driver={1}, Port={2}", PrinterName, PrinterDriver, PrinterPort));
            }
            else
            {
                Logger.Log(EnLogLevel.INFO, "PO Printer will use the default printer.");
            }
        }


        public struct SuEvent
        {
            public System.DateTime TimeGenerated;
            public string Message;
            public SuEvent(System.DateTime time, string strMessage)
            {
                this.TimeGenerated = time;
                this.Message = strMessage;
            }
        } ;

        static public List<SuEvent> GetPrintedFilesFromEventLog(ILogger Logger)
        {
            List<SuEvent> alEntry = new List<SuEvent>();
            try
            {
                string source = "PrintService";
                string logname = @"Microsoft-Windows-PrintService/Operational";
                string machine = ".";

                // Create an EventLog instance and assign its source.
                using (EventLog myLog = new EventLog(logname, machine, source))
                {
                    alEntry = (from e in myLog.Entries.Cast<EventLogEntry>()
                               where
                                 (e.Message.ToLower().Contains(".pdf") ||
                                 e.Message.ToLower().Contains(".tif")) &&
                                 !e.Message.ToLower().Contains("deleted")
                               select e).ToList().Select(w => new SuEvent(w.TimeGenerated, w.Message)).ToList();
                }
            }
            catch (Exception e)
            {
                Logger.LogException(e);
            }

            return alEntry;
        }

    }
}
