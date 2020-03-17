using System;
using System.Reflection;
using System.Threading;

namespace RandREng.Utility.Assembly
{
    public class VersionInfo
    {

        public static string GetBuildDateTime(string VersionNumber)
        {
            // VersionNumber is Major.Minor.Build.Revision
            // Build is equal to the number of days since January 1, 2000, local time
            // Revision is equal to the number of seconds since midnight, January 1, 2000, local time, divided by 2.

            string buildDateTime = string.Empty;

            string[] versionInfo = VersionNumber.Split('.');
            if (versionInfo.Length == 4)
            {
                DateTime buildDT = new DateTime(1980, 1, 1);
                double days = Convert.ToDouble(versionInfo[2]);
                //                System.Double seconds = Convert.ToDouble(versionInfo[3]) * 2;
                //                buildDT = buildDT.AddSeconds(seconds);
                buildDT = buildDT.AddDays(days);
                //              if (DateTime.Now.IsDaylightSavingTime())
                //                buildDT = buildDT.ToUniversalTime().ToLocalTime();
                buildDateTime = buildDT.ToShortDateString();
            }

            return buildDateTime;
        }

        static public string GetCopyright()
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetEntryAssembly();
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                AssemblyCopyrightAttribute aca = o as AssemblyCopyrightAttribute;

                if (aca != null)
                {
                    return aca.Copyright;
                }
            }
            return string.Empty;
        }

        static public string GetInfo()
        {
            string list = "";
            System.Version v = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string s = string.Format(v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision);
            list += "Executing Assembly: " + System.Reflection.Assembly.GetEntryAssembly().GetName().Name + ", version " + s + "\r\n";

            System.Reflection.Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();
            foreach (System.Reflection.Assembly assem in myAssemblies)
            {
                if (assem.GetName().Name.IndexOf("System.") == -1)
                {
                    AssemblyName[] ref_assem2 = assem.GetReferencedAssemblies();
                    if (ref_assem2.Length > 0)
                    {
                        list += assem.GetName().Name + " references the following:\r\n";
                    }
                    foreach (AssemblyName assem_name in ref_assem2)
                    {
                        string strTemp = "   Assembly: " + assem_name.Name;

                        list += strTemp.PadRight(50, ' ') + " ver. " + assem_name.Version.ToString() + "\r\n";
                    }
                }
            }

            return list;
        }
    }
}
