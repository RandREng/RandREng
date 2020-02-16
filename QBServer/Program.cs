using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;
using CFI.Utility.Logging;

namespace RandREng.QBServer
{
	class Program : ServiceBase
	{
		private ServiceHost QBServerHost = null;

		public Program()
		{
			this.ServiceName = "QuickBookServer";
		}
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			this.ServiceName = "QBService";
		}

		#endregion

		static ILogger _logger = null;

		static ILogger Logger
		{
			get
			{
				if (_logger == null)
				{
					string appDataPath = Application.LocalUserAppDataPath;
					_logger = new FileLogger(@"c:\temp\", "QBServer");
					_logger.LogLevel = EnLogLevel.DEBUG;
					QBService.Logger = _logger;
				}
				return _logger;

			}

		}

		static public void WriteLine(string line)
		{
			Console.WriteLine(line);
			Logger.Log(EnLogLevel.INFO, line);
		}

		public static void DisplayHostInfo(ServiceHost h)
		{
			foreach (System.ServiceModel.Description.ServiceEndpoint se in h.Description.Endpoints)
			{
				WriteLine(string.Format("Address:  {0}", se.Address));
				WriteLine(string.Format("Binding:  {0}", se.Binding.Name));
				WriteLine(string.Format("Contract: {0}", se.Contract.Name));
			}
		}

		private void CloseAndNullifyHosts(ServiceHost h)
		{
			if (h != null)
			{
				h.Close();
				h = null;
			}
		}

		/// <summary>
		/// Start/restart the server application
		/// </summary>
		/// <param name="args"></param>
		protected override void OnStart(string[] args)
		{
			base.OnStart(args);
			ConfigureThreadExceptionhandling();
			WriteLine("OnStart called");
			try { 
			// If our host is still in existence, close and nullify it
			//
			CloseAndNullifyHosts(QBServerHost);

			QBServerHost = new ServiceHost(typeof(QBService));

			QBServerHost.Open();

			DisplayHostInfo(QBServerHost);
			}
			catch (AddressAccessDeniedException )
			{
				WriteLine("netsh http add urlacl url=http://+:8000/QBServer user=\"NT AUTHORITY\\Authenticated Users\"");
			}
		}


		public static void ConfigureThreadExceptionhandling()
		{
			// setup handler for worker threads
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs ex)
		{
			Exception exception = (Exception)ex.ExceptionObject;
			handleThreadException(sender, exception);
		}

		private static void handleThreadException(object sender, Exception ex)
		{
			CFI.Utility.Logging.Logger.WriteEventLog("QBService", ex);
			throw ex;
		}


		/// <summary>
		/// Stop the server application
		/// </summary>
		protected override void OnStop()
		{
			WriteLine("OnStop called");

			CloseAndNullifyHosts(QBServerHost);

			base.OnStop();
		}

		/// <summary>
		/// Displays the help message.
		/// </summary>
		/// <param name="args"></param>
		private void ProcessHelpCmd(string[] args)
		{
			WriteLine("Valid commands:");
			WriteLine("\thelp         - Brings up this menu");
			WriteLine("\texit         - Quit the server application");
		}

		/// <summary>
		/// Runs the debug prompt until the exit command is entered
		/// </summary>
		public void UserInteractiveMode()
		{
			string input = "";
			string[] inputArr = null;
			bool stopServer = false;

			while (!stopServer)
			{
				input = "";
				inputArr = null;

				Console.Write("Quick Book Server> ");
				input = Console.ReadLine().Trim().ToLower();
				inputArr = input.Split();

				// Parse commands
				switch (inputArr[0])
				{
					case "help":
						ProcessHelpCmd(inputArr);
						break;
					case "exit":
						stopServer = true;
						break;
					default:
						if (inputArr[0] != "")
							Console.WriteLine("Unknown command: {0}", inputArr[0]);
						break;
				}
			}
		}

		/// <summary>
		/// Server entry point.
		/// </summary>
		/// <param name="args"></param>
		static void Main(string[] args)
		{
			Program p = new Program();

			if (Environment.UserInteractive)
			{
				p.OnStart(args);

				WriteLine("Quick Book WCF Server");
				WriteLine("------------------");

				p.UserInteractiveMode();

				WriteLine("Goodbye!");
				Console.ReadKey();

				p.OnStop();
			}
			else
			{
				ServiceBase.Run(p);
			}
		}
	}
}
