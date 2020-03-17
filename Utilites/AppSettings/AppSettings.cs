using System.Collections.Generic;
using System.Configuration;
using System.Collections.Specialized;

namespace RandREng.Utility.AppSettings
{
    public class AppSettings
    {
        public static int GetAppSetting(string strSettingName, int iDefault)
        {
            int value = iDefault; ;
            string temp = ConfigurationManager.AppSettings[strSettingName];

            if (temp != null)
            {
                int.TryParse(temp, out value);
            }
            return value;
        }

        public static bool GetAppSetting(string strSettingName, bool bDefault)
        {
            bool value = bDefault; ;
            string temp = ConfigurationManager.AppSettings[strSettingName];

            if (temp != null)
            {
                bool.TryParse(temp, out value);
            }
            return value;
        }

        public static string GetAppSetting(string strSettingName, string strDefault)
        {
            string temp = ConfigurationManager.AppSettings[strSettingName];

            if (temp != null)
                return temp;
            else
                return strDefault;
        }

        public static List<string> GetAppSettings(string strSettingName, string strDefault)
        {
            int Index = 1;
            string temp = "";

            List<string> Values = new List<string>();

            for (; temp != null;)
            {
                temp = ConfigurationManager.AppSettings[strSettingName + Index.ToString()];
                if (temp != null)
                {
                    Values.Add(temp);
                }
                Index++;
            }

            if (Values.Count == 0)
            {
                Values.Add(strDefault);
            }

            return Values;
        }

        public static List<string> GetAppSettingsLike(string Name)
        {
            List<string> Values = new List<string>();

            NameValueCollection appSettings = ConfigurationManager.AppSettings;

            foreach (string key in appSettings.Keys)
            {
                if (key.Contains(Name))
                {
                    Values.Add(appSettings[key]);
                }
            }

            return Values;
        }
    }
}
