using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;

namespace CFI.Utility.Assemblies
{
    public class VersionInfo
    {
        static public string GetCopyright()
        {
            Assembly asm = Assembly.GetEntryAssembly();
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
            System.Version v = Assembly.GetEntryAssembly().GetName().Version;
            string s = string.Format(v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision);
            list += "Executing Assembly: " + Assembly.GetEntryAssembly().GetName().Name + ", version " + s + "\r\n";

            Assembly[] myAssemblies = Thread.GetDomain().GetAssemblies();
            foreach (Assembly assem in myAssemblies)
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
