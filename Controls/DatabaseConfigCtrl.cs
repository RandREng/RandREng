using System;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Configuration;

namespace RandREng.Controls
{
    public partial class DatabaseConfigCtrl : UserControl
    {
        protected bool _useappconfig = false;

        public bool UseAppConfig
        {
            get
            {
                return _useappconfig;
            }

            set
            {
                _useappconfig = value;
            }
        }

        public DatabaseConfigCtrl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (false == DesignMode)
            {
                if (_useappconfig)
                {
                    InitComboBoxAppConfig();
                }
                else
                {
                    InitComboBoxXml();
                }
            }
        }

        public void InitComboBoxXml()
        {
            String ActiveDB = "";
            String ActiveName = "";

            XmlDocument doc = new XmlDocument();
            if (File.Exists("database.xml"))
            {
                doc.Load("database.xml");
                XmlElement xelem = doc.DocumentElement;
                XmlNodeList nodes = xelem.GetElementsByTagName("add");

                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["key"].Value.Equals("ActiveDB"))
                    {
                        ActiveDB = node.Attributes["value"].Value.ToString();
                        break;
                    }
                }

                foreach (XmlNode node in nodes)
                {
                    if (node.Attributes["key"].Value.StartsWith("Database"))
                    {
                        if (node.Attributes["key"].Value.Equals(ActiveDB))
                        {
                            ActiveName = node.Attributes["name"].Value.ToString();
                        }

                        comboDatabases.Items.Add(node.Attributes["name"].Value.ToString());

                    }
                }

                comboDatabases.SelectedIndex = comboDatabases.FindStringExact(ActiveName);
            }
        }

        protected void InitComboBoxAppConfig()
        {
            String ActiveName = "";

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            ConfigurationSection csData = config.GetSection("dataConfiguration");
            ActiveName = csData.ElementInformation.Properties["defaultDatabase"].Value.ToString();
            
            ConnectionStringsSection csSection = config.ConnectionStrings;
            for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
            {
                ConnectionStringSettings cs = csSection.ConnectionStrings[i];
                if (false == cs.Name.Equals("LocalSqlServer"))
                {
                    comboDatabases.Items.Add(cs.Name);
                }
            }

            comboDatabases.SelectedIndex = comboDatabases.FindStringExact(ActiveName);

        }

        public bool SaveDatabase()
        {
            bool SetOK = true;

            if (_useappconfig)
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                ConfigurationSection csData = config.GetSection("dataConfiguration");
                csData.ElementInformation.Properties["defaultDatabase"].Value = comboDatabases.SelectedItem;
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("dataConfiguration");
            }
            else
            {

            }

            return SetOK;
        }
    }
}
