using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using System.Text;
using System.Windows.Forms;
using CFI.Utility.Logging;
using Interop.QBXMLRP2;

namespace RandREng.QBServer
{
	public partial class QBService : IQBService
	{
		static internal ILogger Logger { get;set;}

		class Session
		{
			public RequestProcessor2 rp;
			public string ticket;
			public string MaxVersion { get; set; }
			public string CompanyFile { get; set; }
			public string AppID { get; set; }
			public string AppName { get; set; }

			internal QBResponse ProcessRequest(QBRequest request)
			{
				string resp = rp.ProcessRequest(ticket, request.XML);
				QBResponse response = new QBResponse
				{
					Data = resp,
					Timestamp = DateTime.Now
				};

				return response;

			}
		}

		static readonly Dictionary<string, Session> sessions = new Dictionary<string, Session>();

		public string GetSessionKey()
		{
			string key = string.Empty;
			OperationContext context = OperationContext.Current;
			if (context != null)
			{
				MessageProperties prop = context.IncomingMessageProperties;

				if (prop != null &&
					!string.IsNullOrEmpty(RemoteEndpointMessageProperty.Name) &&
					prop.ContainsKey(RemoteEndpointMessageProperty.Name))
				{
					RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
					key = endpoint.Address; // + ":" + endpoint.Port;
				}
			}
			else
			{
				Logger.LogError("No Context");
			}
			return key;
		}
		public QBResponse Send(QBRequest request)
		{
			System.Console.WriteLine("Send Command");
			Session session = this.ConnectToQB(request);
			return session.ProcessRequest(request);
		}

		public QBResponse GetMaxVersion(QBRequest request)
		{
			Session session = this.ConnectToQB(request);
			return session.ProcessRequest(request);
		}

		public void Disconnect()
		{
			this.DisconnectFromQB();
		}


		// CONNECTION TO QB
		private Session ConnectToQB(QBRequest request)
		{
			string key = GetSessionKey();
			Session session;
			if (!sessions.Keys.Contains(key))
			{
				session = new Session
				{
					AppID = request.AppID,
					AppName = request.AppName,
					CompanyFile = request.CompanyFile
				};
				sessions.Add(key, session);

			}
			else
			{
				session = sessions[key];
			}
			if (session.rp == null)
			{
				try
				{
					session.rp = new RequestProcessor2Class();
					//			rp.OpenConnection(request.AppID, request.AppName);
					session.rp.OpenConnection2(request.AppID, request.AppName, QBXMLRPConnectionType.localQBD);
					if (request.CompanyFile == null)
					{
						request.CompanyFile = string.Empty;
					}
					session.ticket = session.rp.BeginSession(request.CompanyFile, QBFileMode.qbFileOpenDoNotCare);
					string[] versions = (string[])session.rp.get_QBXMLVersionsForSession(session.ticket);
					session.MaxVersion = versions[versions.Length - 1];
				}
				catch (Exception e)
				{
					Logger.LogCritical(e);
					DisconnectFromQB();
				}
			}

			return session;
		}

		public void DisconnectFromQB()
		{
			string key = GetSessionKey();
			Session session;
			if (sessions.Keys.Contains(key))
			{
				session = sessions[key];
				if (session.ticket != null)
				{
					try
					{
						session.rp.EndSession(session.ticket);
						session.ticket = null;
					}
					catch (Exception e)
					{
						System.Console.WriteLine(e.Message);
					}
				}
				if (session.rp != null)
				{
					try
					{
						session.rp.CloseConnection();
						session.rp = null;
					}
					catch (Exception e)
					{
						Logger.LogCritical(e);
					}
				}
			}
		}
	}

	//public partial class QBService2 : ServiceBase
	//{
	//	private ServiceHost QBServiceHost = null;

	//	public QBService()
	//	{
	//		InitializeComponent();
	//	}

	//	protected override void OnStart(string[] args)
	//	{
	//		base.OnStart(args);
	//		Console.WriteLine("OnStart called");

	//		// If our host is still in existence, close and nullify it
	//		//
	//		CloseAndNullifyHosts(QBServiceHost);
	//		QBServiceHost = new ServiceHost(typeof(QBServiceHost));
	//		QBServiceHost.Open();
	//		DisplayHostInfo(QBServiceHost);
	//	}

	//	protected override void OnStop()
	//	{
	//		Console.WriteLine("OnStop called");
	//		CloseAndNullifyHosts(QBServiceHost);
	//		base.OnStop();
	//	}

	//	public static void DisplayHostInfo(ServiceHost h)
	//	{
	//		foreach (System.ServiceModel.Description.ServiceEndpoint se in h.Description.Endpoints)
	//		{
	//			Console.WriteLine("Address:  {0}", se.Address);
	//			Console.WriteLine("Binding:  {0}", se.Binding.Name);
	//			Console.WriteLine("Contract: {0}", se.Contract.Name);
	//		}
	//	}

	//	private void CloseAndNullifyHosts(ServiceHost h)
	//	{
	//		if (h != null)
	//		{
	//			h.Close();
	//			h = null;
	//		}
	//	}
	//}
}
