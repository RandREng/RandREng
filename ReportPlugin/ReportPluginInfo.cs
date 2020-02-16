using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Linq;
using CFI.Utility.Logging;

namespace CFI.InstallationManager.ReportPlugin
{
    public class ReportPluginInfo : IDisposable
    {
        private ILogger Logger;

        public ReportPluginInfo(ILogger logger)
        {
            Init();
            Logger = logger;
        }

        public ReportPluginInfo(string fileName, ILogger logger)
            :this(logger)
        {
            FileName = fileName;
        }
        
        public ReportPluginInfo(string _guid, string _reportname, string _description, string _toplevelmenukey, string _secondlevelmenukey, ILogger logger)
            :this(logger)
        {
            GUID = _guid;
            ReportName = _reportname;
            Description = _description;
            TopLevelMenuKey = _toplevelmenukey;
            SecondLevelMenuKey = _secondlevelmenukey;
        }

        private bool _IsNew = true;
        private bool _IsDisposed;
        private static string _Folder = System.AppDomain.CurrentDomain.BaseDirectory + @"plugins\";

        [XmlElement("GUID")]
        public string GUID { get; set; }

        [XmlElement("Description")]        
        public string Description { get; set; }

        [XmlElement("Filename")]
        public string FileName { get; set; }

        [XmlElement("ReportName")]
        public string ReportName { get; set; }

        [XmlElement("TopLevelMenuKey")]
        public string TopLevelMenuKey { get; set; }

        [XmlElement("SecondLevelMenuKey")]
        public string SecondLevelMenuKey { get; set; }

        private void Init()
        {
            GUID = String.Empty;
            ReportName = String.Empty;
            Description = String.Empty;
            TopLevelMenuKey = String.Empty;
            SecondLevelMenuKey = String.Empty;
        }

        public bool Save()
        {
            bool SaveOK = false;

            if (_IsNew)
            {
                IReportPlugin plugin = CreateInstance();
                if (plugin != null)
                {
                    ReportPluginInfo plugininfo = plugin.GetPluginInfo();
                    GUID = plugininfo.GUID;
                    Description = plugininfo.Description;
                    ReportName = plugininfo.ReportName;
                    TopLevelMenuKey = plugininfo.TopLevelMenuKey;
                    SecondLevelMenuKey = plugininfo.SecondLevelMenuKey;
                    plugin.Dispose();
                }
            }

            if (!Directory.Exists(_Folder))
            {
                Directory.CreateDirectory(_Folder);
            }

            if (GUID.Length > 0)
            {
                Serializer<ReportPluginInfo>.Save(this, _Folder + GUID + ".xml");
                OnChanged(this);
                _IsNew = false;
                SaveOK = true;
            }

            return SaveOK;
        }

        public static void DiscoverAllPlugins(ILogger logger)
        {
            if (!Directory.Exists(_Folder))
            {
                Directory.CreateDirectory(_Folder);
            }
            else
            {
                foreach (string filename in Directory.GetFiles(_Folder, "*.dll"))
                {
                    List<ReportPluginInfo> plugins = DiscoverPlugins(filename, logger);                    
                }
            }
        }

        /// <summary>
        /// Discover Plugins implemented in an assembly and creates corresponding XML file definitions for faster loading in the future.
        /// </summary>
        public static List<ReportPluginInfo> DiscoverPlugins(string fileName, ILogger logger)
        {
            if (!File.Exists(fileName))
            {
                return null;
            }

            try
            {
                List<ReportPluginInfo> plugins = new List<ReportPluginInfo>();

                if (!Directory.Exists(_Folder))
                {
                    Directory.CreateDirectory(_Folder);
                }

                Assembly _assembly = Assembly.LoadFile(fileName);
                foreach (Type t in _assembly.GetTypes())
                {
                    if (t.GetInterfaces().Contains(typeof(IReportPlugin)))
                    {
                        IReportPlugin plugin = Activator.CreateInstance(t) as IReportPlugin;                        
                        ReportPluginInfo _plugininfo = plugin.GetPluginInfo();

                        _plugininfo.FileName = fileName;

                        if ((_plugininfo.GUID.Length > 0) && (_plugininfo.ReportName.Length > 0))
                        {
                            plugins.Add(_plugininfo);
                            
                            string XMLFileName = _Folder + _plugininfo.GUID + ".xml";
                            if (File.Exists(XMLFileName))
                            {
                                File.Delete(XMLFileName);
                            }

                            Serializer<ReportPluginInfo>.Save(_plugininfo, XMLFileName);
                            _plugininfo._IsNew = false;
                        }                        
                    }
                }                    

                if (plugins.Count > 0)
                {
                    return plugins;
                }
                else
                {
                    return null;
                }
            }
            catch (InvalidOperationException)
            {
                return null;
            }
            catch (Exception ex)
            {
                logger.LogException(ex);
                return null;
            }
        }

        /// <summary>
        /// Loads a Plug-in based on the specified file name.
        /// </summary>
        public static ReportPluginInfo Load(string fileName)
        {
            if (!File.Exists(fileName))
                return null;

            try
            {
                ReportPluginInfo info = Serializer<ReportPluginInfo>.Load(fileName);
                info.GUID = new FileInfo(fileName).Name.Replace(".xml", string.Empty);
                info._IsNew = false;
                return info;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Removes the plug-in from the list.
        /// </summary>
        public void Delete()
        {
            string filename = _Folder + GUID + ".xml";
            if (File.Exists(filename))
            {
                File.Delete(filename);
                OnChanged(this);
            }
        }

        public static Collection<ReportPluginInfo> LoadAll()
        {
            Collection<ReportPluginInfo> infos = new Collection<ReportPluginInfo>();

            if (!Directory.Exists(_Folder))
            {
                Directory.CreateDirectory(_Folder);
            }
            else
            {
                foreach (string filename in Directory.GetFiles(_Folder, "*.xml"))
                {
                    ReportPluginInfo info = Load(filename);
                    if (info != null)
                    {
                        infos.Add(info);
                    }
                }
            }

            return infos;
        }

        /// <summary>
        /// Creates an instance of the plugin corresponding to the GUID
        /// </summary>
        public IReportPlugin CreateInstance()
        {
            if (!File.Exists(FileName))
            {
                return null;
            }

            Assembly ass = Assembly.LoadFile(FileName);

            TypeFilter myFilter = new TypeFilter(MyInterfaceFilter);
            
            foreach (Type type in ass.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(IReportPlugin)))
                {
                    IReportPlugin _plugin = Activator.CreateInstance(type) as IReportPlugin;
                    if (_plugin.GetPluginInfo().GUID == GUID)
                    {
                        return _plugin;
                    }
                    _plugin.Dispose();
                }
            }

            return null;
        }

        public static bool MyInterfaceFilter(Type typeObj, Object criteriaObj)
        {
            if (typeObj.ToString() == criteriaObj.ToString())
            {
                return true;
            }

            return false;
        }

        public static event EventHandler<EventArgs> Changed;
        /// <summary>
        /// Occurs when the class is Changed
        /// </summary>
        protected static void OnChanged(ReportPluginInfo plugin)
        {
            if (Changed != null)
            {
                Changed(plugin, new EventArgs());
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing && !_IsDisposed)
            {                
                _IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }    
}
