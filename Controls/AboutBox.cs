using System;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;
using RandREng.Utility.Assembly;
using System.Runtime.Versioning;

namespace RandREng.Controls
{
	[SupportedOSPlatform("windows")]
	public partial class AboutBox : System.Windows.Forms.Form
	{

		public AboutBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			base.Text = "About " + Application.ProductName;
            Version v = Assembly.GetEntryAssembly().GetName().Version;
            string s = string.Format(v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision);
            Version = String.Format("{0} (Built: {1})", s, VersionInfo.GetBuildDateTime(s));
            ApplicationName = Application.ProductName;

            this.Copyright = VersionInfo.GetCopyright();
        
        }

		public string Version
		{
			set { this.labelVersion.Text = value; }
		}

		public string ApplicationName
		{
			set 
			{ 
				this.labelAppName.Text = value;
				this.Text = "About " + value;
			}
		}

		public string ExtraInfo
		{
			set { this.labelExtraInfo.Text = value; }
		}

		public string Warning
		{
			set { this.labelWarning.Text = value; }
		}

		public string Copyright
		{
			set { this.labelCopyright.Text = value; }
		}

		private void m_btSystemInfo_Click( object sender, EventArgs e )
		{
			LoadMSINFO();
		}

		private void LoadMSINFO()
		{
			string msinfoPath = string.Empty;

			if ( ( msinfoPath = GetMSINFOPath() ) == null )
				MessageBox.Show( this, "Unable to find msinfo.exe on system.", "System information error", MessageBoxButtons.OK, MessageBoxIcon.Error );
			else
				Process.Start( msinfoPath );
		}

		private string GetMSINFOPath()
		{
			string path = string.Empty;

			if ( ( path = GetRegistryValue( @"Software\Microsoft\Shared Tools\MSInfo", "Path" ) ) == null )
			{
				if ( ( path = GetRegistryValue( @"Software\Microsoft\Shared Tools Location", "MSInfo" ) ) == null )
					return null;
			}

			return path;
		}

		private string GetRegistryValue( string rootKey, string keyName )
		{
			using ( RegistryKey regKey = Registry.LocalMachine.OpenSubKey( rootKey ) )
			{
				if ( regKey == null )
					return null;

				return (string) regKey.GetValue( keyName );
			}
		}

		private void buttonCopyToClipBoard_Click(object sender, EventArgs e)
		{
			ClipboardUtilities.SetText( "<root>" +
								this.assembliesUserControl1.GetPasteData() +
								"</root>");
		}

		private void link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start("http://www.fusioncomputing.com/");
		}
	}
}
