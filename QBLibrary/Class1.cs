using System;
using Interop.QBXMLRP2;
//using RandREng.QBLibrary.QBServer;
using Microsoft.Extensions.Logging;
using CFI.Utility.Logging;


namespace RandREng.QBLibrary
{
    public interface IQBProcessor : IDisposable
	{
		string Transmit(string request);
		string MaxVersion { get; }
		string CompanyFile { get; set; }
		string AppID { get; set; }
		string AppName { get; set; }
		bool CanConnect { get; }
	}

	public class QBProcessor : IQBProcessor
	{
		readonly IQBProcessor _processor;
		public QBProcessor(ILogger logger)
		{
//			if (System.Environment.Is64BitProcess)
			//{
			//	this._processor = new QBProcessor64(logger);
			//}
			//else
			//{
				this._processor = new QBProcessor32(logger);
			//}
		}

		#region IDisposable
		~QBProcessor()
		{
			Dispose(false);
		}
		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!_disposed)
			{
				if (disposing)
				{
				}
				this._processor.Dispose();
				// Indicate that the instance has been disposed.
				_disposed = true;
			}

		}
		#endregion

		public string Transmit(string request)
		{
			return this._processor.Transmit(request);
		}

		public string MaxVersion
		{
			get { throw new NotImplementedException(); }
		}

		public string CompanyFile
		{
			get
			{
				return this._processor.CompanyFile;
			}
			set
			{
				this._processor.CompanyFile = value;
			}
		}

		public string AppID
		{
			get
			{
				return this._processor.AppID;
			}
			set
			{
				this._processor.AppID = value;
			}
		}

		public string AppName
		{
			get
			{
				return this._processor.AppName;
			}
			set
			{
				this._processor.AppName = value;
			}
		}

		public bool CanConnect
		{
			get { return this._processor.CanConnect; }
		}
	}

	//internal class QBProcessor64 : IQBProcessor
	//{
	//	private ILogger Logger { get; set; }
	//	internal QBProcessor64(ILogger logger)
	//	{
	//		this.Logger = logger;
	//	}
	//	#region IDisposable
	//	~QBProcessor64()
	//	{
	//		Dispose(false);
	//	}
	//	private bool _disposed = false;

	//	public void Dispose()
	//	{
	//		Dispose(true);

	//		// Use SupressFinalize in case a subclass
	//		// of this type implements a finalizer.
	//		GC.SuppressFinalize(this);
	//	}

	//	protected virtual void Dispose(bool disposing)
	//	{
	//		// If you need thread safety, use a lock around these 
	//		// operations, as well as in your methods that use the resource.
	//		if (!_disposed)
	//		{

	//			if (this._client != null)
	//			{
	//				this._client.Disconnect();
	//				this._client.Close();
	//				this._client = null;
	//			}
	//			if (disposing)
	//			{
	//			}
	//			// Indicate that the instance has been disposed.
	//			_disposed = true;
	//		}

	//	}
	//	#endregion

	//	public bool CanConnect
	//	{
	//		get { return false; }
	//	}

	//	QBServer.QBServiceClient _client = null;
		
	//	QBServer.QBServiceClient Client
	//	{
	//		get 
	//		{
	//			if (this._client == null)
	//			{
	//				this._client = new QBServer.QBServiceClient("BasicHttpBinding_IQBServices");
	//				this._client.InnerChannel.OperationTimeout = TimeSpan.FromMinutes(10);
	//			}
	//			return this._client;
	//		}
	//	}

	//	public string Transmit(string req)
	//	{
	//		string temp = null;
	//		try
	//		{
				
	//			QBRequest request = new QBRequest { XML = req };
	//			request.CompanyFile = this.CompanyFile;
	//			request.AppID = this.AppID;
	//			request.AppName = this.AppName;

	//			QBResponse response = Client.Send(request);
	//			temp = response.Data;
	//			//			Console.WriteLine("Server response: " + response.Data);

	//		}
	//		catch (Exception e)
	//		{
	//			this.Logger.LogCritical(e);
	//		}
	//		return temp;
	//	}

	//	readonly string _maxVersion = null;
	//	public string MaxVersion
	//	{
	//		get
	//		{
	//			if (_maxVersion == null)
	//			{
	//				try
	//				{
	//					QBRequest request = new QBRequest { XML = string.Empty };
	//					request.CompanyFile = this.CompanyFile;
	//					request.AppID = this.AppID;
	//					request.AppName = this.AppName;
	//				}
	//				catch (Exception e)
	//				{
	//					this.Logger.LogCritical(e);
	//				}
	//			}
	//			return _maxVersion;
	//		}
	//	}

	//	public string CompanyFile { get; set; }
	//	public string AppID { get; set; }
	//	public string AppName { get; set; }
	//}

	internal class QBProcessor32 : IQBProcessor, IDisposable
	{
		private ILogger Logger { get; set; }
		internal QBProcessor32(ILogger logger)
		{
			this.Logger = logger;
		}

		#region IDisposable
		~QBProcessor32()
		{
			Dispose(false);
		}

		private bool _disposed = false;

		public void Dispose()
		{
			Dispose(true);

			// Use SupressFinalize in case a subclass
			// of this type implements a finalizer.
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			// If you need thread safety, use a lock around these 
			// operations, as well as in your methods that use the resource.
			if (!_disposed)
			{
				if (disposing)
				{
				}
				DisconnectFromQB();
				// Indicate that the instance has been disposed.
				_disposed = true;
			}

		}
		#endregion

		private RequestProcessor2 rp;
		private string ticket;
		public string MaxVersion { get; set; }
		public string CompanyFile { get; set; }
		public string AppID { get; set; }
		public string AppName { get; set; }
		private readonly Interop.QBXMLRP2.QBFileMode mode = Interop.QBXMLRP2.QBFileMode.qbFileOpenDoNotCare;

		public bool CanConnect
		{
			get 
			{
				ConnectToQB();
				return this.rp != null;
			}
		}
		// CONNECTION TO QB
		private void ConnectToQB()
		{
			if (rp == null)
			{
				try
				{
					rp = new RequestProcessor2Class();
					rp.OpenConnection2(AppID, AppName, QBXMLRPConnectionType.localQBD);
					if (CompanyFile == null)
					{
						CompanyFile = string.Empty;
					}
					ticket = rp.BeginSession(CompanyFile, mode);
					string[] versions = (string[])rp.get_QBXMLVersionsForSession(ticket);
					MaxVersion = versions[versions.Length - 1];
				}
				catch (Exception e)
				{
					Logger.LogCritical(e);
					DisconnectFromQB();
				}
			}
		}

		public void DisconnectFromQB()
		{
			if (ticket != null)
			{
				try
				{
					rp.EndSession(ticket);
					ticket = null;
				}
				catch (Exception e)
				{
					Logger.LogCritical(e);
				}
			}
			if (rp != null)
			{
				try
				{
					rp.CloseConnection();
					rp = null;
				}
				catch (Exception e)
				{
					Logger.LogCritical(e);
				}
			}
		}


		public string Transmit(string request)
		{
			try
			{
				this.ConnectToQB();
				string resp = rp.ProcessRequest(ticket, request);
				//				this.disconnectFromQB();
				return resp;
			}
			catch (Exception e)
			{
				Logger.LogCritical(e);
				return null;
			}
		}
	}

	//public class Class1
	//{
	////	string homeCurrency = "";
	////	decimal exchangeRate = 1.0m;
	//	private IQBProcessor Processor { get; set; }
	//	public Class1(IQBProcessor proc)
	//	{
	//		this.Processor = proc;
	//	}

	////	public int getCount(string request)
	////	{
	////		string req = buildDataCountQuery(request);
	////		string response = this.Processor.processRequestFromQB(req);
	////		int count = parseRsForCount(response, request);
	////		return count;
	////	}

	////	public bool multiCurrencyOn()
	////	{
	////		bool ret = false;
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildPreferencesQueryRqXML(new string[] { "MultiCurrencyPreferences" }, null));
	////		string[] prefs = parsePreferencesQueryRs(response, 2);
	////		ret = Convert.ToBoolean(prefs[0]);
	////		homeCurrency = prefs[1];
	////		Processor.disconnectFromQB();
	////		return ret;
	////	}

	////public List<string> loadCustomers()
	////{
	////	Processor.connectToQB();
	////	string response = Processor.processRequestFromQB(buildCustomerQueryRqXML(new string[] { "FullName" /*, "BillAddress", "ShipAddress" */ }, null));
	////	List<string> customerList = parseCustomerQueryRs(response);
	////	Processor.disconnectFromQB();
	////	return customerList;
	////}

	//public string Transmit(string xml)
	//{
	//	return Processor.Transmit(xml);
	//}

	////	public string testCustomers()
	////	{
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildCustomerQueryRqXML(new string[] { "FullName", "BillAddress", "ShipAddress" }, null));
	////		Processor.disconnectFromQB();
	////		return response;
	////	}

	////	public string[] loadItems()
	////	{
	////		string request = "ItemQueryRq";
	////		Processor.connectToQB();
	////		int count = getCount(request);
	////		string response = Processor.processRequestFromQB(buildItemQueryRqXML(new string[] { "FullName" }, null));
	////		string[] itemList = parseItemQueryRs(response, count);
	////		Processor.disconnectFromQB();
	////		return itemList;
	////	}

	////	public string[] loadTerms(IQBProcessor Processor)
	////	{
	////		string request = "TermsQueryRq";
	////		Processor.connectToQB();
	////		int count = getCount(request);
	////		string response = Processor.processRequestFromQB(buildTermsQueryRqXML());
	////		string[] termsList = parseTermsQueryRs(response, count);
	////		Processor.disconnectFromQB();
	////		return termsList;
	////	}

	////	public string[] loadSalesTaxCodes()
	////	{
	////		string request = "SalesTaxCodeQueryRq";
	////		Processor.connectToQB();
	////		int count = getCount(request);
	////		string response = Processor.processRequestFromQB(buildSalesTaxCodeQueryRqXML());
	////		string[] salesTaxCodeList = parseSalesTaxCodeQueryRs(response, count);
	////		Processor.disconnectFromQB();
	////		return salesTaxCodeList;
	////	}

	////	public string getBillShipTo(string customerName, string billOrShip)
	////	{
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildCustomerQueryRqXML(new string[] { billOrShip }, customerName));
	////		List<string> billShipTo = parseCustomerQueryRs(response);
	////		if (billShipTo == null || billShipTo.Count == 0) return string.Empty;
	////		Processor.disconnectFromQB();
	////		return billShipTo[0];
	////	}

	////	public string getCurrencyCode(string customerName)
	////	{
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildCustomerQueryRqXML(new string[] { "CurrencyRef" }, customerName));
	////		List<string> currencyCode = parseCustomerQueryRs(response);
	////		Processor.disconnectFromQB();
	////		return currencyCode[0];
	////	}

	////	public string getExchangeRate(string currencyName)
	////	{
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildCurrencyQueryRqXML(currencyName));
	////		string[] exrate = parseCurrencyQueryRs(response, 1);
	////		Processor.disconnectFromQB();
	////		if (exrate[0] == null || exrate[0] == "") exrate[0] = "1.0";
	////		return exrate[0];
	////	}

	////	public string[] getItemInfo(string itemName)
	////	{
	////		Processor.connectToQB();
	////		string response = Processor.processRequestFromQB(buildItemQueryRqXML(new string[] { "SalesOrPurchase" }, itemName));
	////		string[] itemInfo = parseItemQueryRs(response, 2);
	////		Processor.disconnectFromQB();
	////		return itemInfo;
	////	}

	////	public string[] loadCustomerMsg()
	////	{
	////		string request = "CustomerMsgQueryRq";
	////		Processor.connectToQB();
	////		int count = getCount(request);
	////		string response = Processor.processRequestFromQB(buildCustomerMsgQueryRqXML(new string[] { "Name" }, null));
	////		string[] customerMsgList = parseCustomerMsgQueryRs(response, count);
	////		Processor.disconnectFromQB();
	////		return customerMsgList;
	////	}



	////	// RESPONSE PARSING
	////	private string[] parseInvoiceAddRs(string xml)
	////	{
	////		string[] retVal = new string[3];
	////		try
	////		{
	////			XmlNodeList RsNodeList = null;
	////			XmlDocument Doc = new XmlDocument();
	////			Doc.LoadXml(xml);
	////			RsNodeList = Doc.GetElementsByTagName("InvoiceAddRs");
	////			XmlAttributeCollection rsAttributes = RsNodeList.Item(0).Attributes;
	////			XmlNode statusCode = rsAttributes.GetNamedItem("statusCode");
	////			retVal[0] = Convert.ToString(statusCode.Value);
	////			XmlNode statusSeverity = rsAttributes.GetNamedItem("statusSeverity");
	////			retVal[1] = Convert.ToString(statusSeverity.Value);
	////			XmlNode statusMessage = rsAttributes.GetNamedItem("statusMessage");
	////			retVal[2] = Convert.ToString(statusMessage.Value);
	////		}
	////		catch (Exception e)
	////		{
	////			System.Console.WriteLine("Error encountered when parsing Invoice info returned from QuickBooks: " + e.Message);
	////			retVal = null;
	////		}
	////		return retVal;
	////	}

	////	private string[] parseCurrencyQueryRs(string xml, int count)
	////	{
	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CurrencyQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CurrencyRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "ExchangeRate":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private string[] parseCustomerMsgQueryRs(string xml, int count)
	////	{
	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CustomerMsgQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CustomerMsgRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "Name":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private string[] parseSalesTaxCodeQueryRs(string xml, int count)
	////	{
	////		/*
	////		<?xml version="1.0" ?> 
	////		<QBXML>
	////		<QBXMLMsgsRs>
	////		<SalesTaxCodeQueryRs requestID="3" statusCode="0" statusSeverity="Info" statusMessage="Status OK">
	////			<SalesTaxCodeRet>
	////				<FullName>Tax</FullName> 
	////			</SalesTaxCodeRet>
	////		</SalesTaxCodeQueryRs>
	////		</QBXMLMsgsRs>
	////		</QBXML>            
	////		*/

	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "SalesTaxCodeQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "SalesTaxCodeRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "Name":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private string[] parseTermsQueryRs(string xml, int count)
	////	{
	////		/*
	////		<?xml version="1.0" ?> 
	////		<QBXML>
	////		<QBXMLMsgsRs>
	////		<TermsQueryRs requestID="3" statusCode="0" statusSeverity="Info" statusMessage="Status OK">
	////			<StandardTermsRet>
	////				<Name>1% 10 Net 30</Name> 
	////			</StandardTermsRet>
	////		</TermsQueryRs>
	////		</QBXMLMsgsRs>
	////		</QBXML>            
	////		*/

	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "TermsQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "StandardTermsRet":
	////				case "DateDrivenTermsRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "Name":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private string[] parsePreferencesQueryRs(string xml, int count)
	////	{
	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "PreferencesQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "PreferencesRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "MultiCurrencyPreferences":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "HomeCurrencyRef":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "FullName":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				case "IsMultiCurrencyOn":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					//more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private string[] parseItemQueryRs(string xml, int count)
	////	{
	////		/*
	////		  <?xml version="1.0" ?> 
	////		- <QBXML>
	////		- <QBXMLMsgsRs>
	////		- <ItemQueryRs requestID="2" statusCode="0" statusSeverity="Info" statusMessage="Status OK">
	////		- <ItemServiceRet>
	////			<ListID>20000-933272655</ListID> 
	////			<TimeCreated>1999-07-29T11:24:15-08:00</TimeCreated> 
	////			<TimeModified>2007-12-15T11:32:53-08:00</TimeModified> 
	////			<EditSequence>1197747173</EditSequence> 
	////			<Name>Installation</Name> 
	////			<FullName>Installation</FullName> 
	////			<IsActive>true</IsActive> 
	////			<Sublevel>0</Sublevel> 
	////		- 	<SalesTaxCodeRef>
	////				<ListID>20000-999022286</ListID> 
	////				<FullName>Non</FullName> 
	////			</SalesTaxCodeRef>
	////		- 	<SalesOrPurchase>
	////				<Desc>Installation labor</Desc> 
	////				<Price>35.00</Price> 
	////		- 		<AccountRef>
	////					<ListID>190000-933270541</ListID> 
	////					<FullName>Construction Income:Labor Income</FullName> 
	////				</AccountRef>
	////			</SalesOrPurchase>
	////		  </ItemServiceRet>
	////		  </ItemQueryRs>
	////		  </QBXMLMsgsRs>
	////		  </QBXML>
	////		*/

	////		string[] retVal = new string[count];
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		int x = 0;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "ItemQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "ItemServiceRet":
	////				case "ItemNonInventoryRet":
	////				case "ItemOtherChargeRet":
	////				case "ItemInventoryRet":
	////				case "ItemInventoryAssemblyRet":
	////				case "ItemFixedAssetRet":
	////				case "ItemSubtotalRet":
	////				case "ItemDiscountRet":
	////				case "ItemPaymentRet":
	////				case "ItemSalesTaxRet":
	////				case "ItemSalesTaxGroupRet":
	////				case "ItemGroupRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "FullName":
	////					retVal[x] = nav.Value.Trim();
	////					x++;
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				case "SalesOrPurchase":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "Desc":
	////				case "Price":
	////					string val = nav.Value.Trim();
	////					decimal price = 0.0m;
	////					if (IsDecimal(val))
	////					{
	////						price = Convert.ToDecimal(val);
	////						if (exchangeRate != 1.0m) { price = price / exchangeRate; }
	////						retVal[1] = price.ToString("N2");
	////					}
	////					else
	////					{
	////						retVal[0] = val;
	////					}

	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	private List<string> parseCustomerQueryRs(string xml)
	////	{
	////		/*
	////		 <?xml version="1.0" ?> 
	////		 <QBXML>
	////		 <QBXMLMsgsRs>
	////		 <CustomerQueryRs requestID="1" statusCode="0" statusSeverity="Info" statusMessage="Status OK">
	////			 <CustomerRet>
	////				 <FullName>Abercrombie, Kristy</FullName> 
	////			 </CustomerRet>
	////		 </CustomerQueryRs>
	////		 </QBXMLMsgsRs>
	////		 </QBXML>    
	////		*/

	////		List<string> retVal = new List<string>();
	////		System.IO.StringReader rdr = new System.IO.StringReader(xml);
	////		System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument(rdr);
	////		System.Xml.XPath.XPathNavigator nav = doc.CreateNavigator();

	////		if (nav != null)
	////		{
	////			nav.MoveToFirstChild();
	////		}
	////		bool more = true;
	////		while (more)
	////		{
	////			switch (nav.LocalName)
	////			{
	////				case "QBXML":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "QBXMLMsgsRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CustomerQueryRs":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "CustomerRet":
	////					more = nav.MoveToFirstChild();
	////					continue;
	////				case "FullName":
	////					retVal.Add(nav.Value.Trim());
	////					more = nav.MoveToParent();
	////					more = nav.MoveToNext();
	////					continue;
	////				case "BillAddress":
	////				case "ShipAddress":
	////				case "CurrencyRef":
	////					more = nav.MoveToFirstChild();
	////					retVal.Add("");
	////					continue;
	////				case "Addr1":
	////				case "Addr2":
	////				case "Addr3":
	////				case "Addr4":
	////				case "Addr5":
	////				case "City":
	////				case "State":
	////				case "PostalCode":
	////					retVal[retVal.Count - 1] = retVal[retVal.Count - 1] + "\r\n" + nav.Value.Trim();
	////					more = nav.MoveToNext();
	////					continue;
	////				default:
	////					more = nav.MoveToNext();
	////					continue;
	////			}
	////		}
	////		return retVal;
	////	}

	////	public virtual int parseRsForCount(string xml, string request)
	////	{
	////		int ret = -1;
	////		try
	////		{
	////			XmlNodeList RsNodeList = null;
	////			XmlDocument Doc = new XmlDocument();
	////			Doc.LoadXml(xml);
	////			string tagname = request.Replace("Rq", "Rs");
	////			RsNodeList = Doc.GetElementsByTagName(tagname);
	////			System.Text.StringBuilder popupMessage = new System.Text.StringBuilder();
	////			XmlAttributeCollection rsAttributes = RsNodeList.Item(0).Attributes;
	////			XmlNode retCount = rsAttributes.GetNamedItem("retCount");
	////			ret = Convert.ToInt32(retCount.Value);
	////		}
	////		catch (Exception e)
	////		{
	////			System.Console.WriteLine("Error encountered: " + e.Message);
	////			ret = -1;
	////		}
	////		return ret;
	////	}


	////	// REQUEST BUILDING

	////	//TODO: Build InvoiceAdd request xml.
	////	private string buildInvoiceAddRqXML(string Customer, DateTime trnsDate, string refNum, string BillTO, string Terms, string CustomerMessage, string ExchangeRate)
	////	{
	////		string requestXML = "";

	////		//if (!validateInput()) return null;

	////		//GET ALL INPUT INTO XML
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement InvoiceAddRq = xmlDoc.CreateElement("InvoiceAddRq");
	////		qbXMLMsgsRq.AppendChild(InvoiceAddRq);
	////		XmlElement InvoiceAdd = xmlDoc.CreateElement("InvoiceAdd");
	////		InvoiceAddRq.AppendChild(InvoiceAdd);

	////		// CustomerRef -> FullName
	////		if (Customer != "")
	////		{
	////			XmlElement Element_CustomerRef = xmlDoc.CreateElement("CustomerRef");
	////			InvoiceAdd.AppendChild(Element_CustomerRef);
	////			XmlElement Element_CustomerRef_FullName = xmlDoc.CreateElement("FullName");
	////			Element_CustomerRef.AppendChild(Element_CustomerRef_FullName).InnerText = Customer;
	////		}

	////		// TxnDate 
	////		DateTime DT_TxnDate = System.DateTime.Today;
	////		if (trnsDate != null)
	////		{
	////			string TxnDate = getDateString(trnsDate);
	////			XmlElement Element_TxnDate = xmlDoc.CreateElement("TxnDate");
	////			InvoiceAdd.AppendChild(Element_TxnDate).InnerText = TxnDate;
	////		}

	////		// RefNumber 
	////		if (refNum != "")
	////		{
	////			XmlElement Element_RefNumber = xmlDoc.CreateElement("RefNumber");
	////			InvoiceAdd.AppendChild(Element_RefNumber).InnerText = refNum;
	////		}

	////		// BillAddress
	////		if (BillTO != "")
	////		{
	////			string[] BillAddress = BillTO.Split('\n');
	////			XmlElement Element_BillAddress = xmlDoc.CreateElement("BillAddress");
	////			InvoiceAdd.AppendChild(Element_BillAddress);
	////			for (int i = 0; i < BillAddress.Length; i++)
	////			{
	////				if (BillAddress[i] != "" || BillAddress[i] != null)
	////				{
	////					XmlElement Element_Addr = xmlDoc.CreateElement("Addr" + (i + 1));
	////					Element_BillAddress.AppendChild(Element_Addr).InnerText = BillAddress[i];
	////				}
	////			}
	////		}

	////		// TermsRef -> FullName 
	////		bool termsAvailable = false;
	////		if (Terms != "")
	////		{
	////			termsAvailable = true;
	////			XmlElement Element_TermsRef = xmlDoc.CreateElement("TermsRef");
	////			InvoiceAdd.AppendChild(Element_TermsRef);
	////			XmlElement Element_TermsRef_FullName = xmlDoc.CreateElement("FullName");
	////			Element_TermsRef.AppendChild(Element_TermsRef_FullName).InnerText = Terms;
	////		}

	////		// DueDate 
	////		if (termsAvailable)
	////		{
	////			DateTime DT_DueDate = System.DateTime.Today;
	////			double dueInDays = getDueInDays(Terms);
	////			DT_DueDate = DT_TxnDate.AddDays(dueInDays);
	////			string DueDate = getDateString(DT_DueDate);
	////			XmlElement Element_DueDate = xmlDoc.CreateElement("DueDate");
	////			InvoiceAdd.AppendChild(Element_DueDate).InnerText = DueDate;
	////		}

	////		// CustomerMsgRef -> FullName 
	////		if (CustomerMessage != "")
	////		{
	////			XmlElement Element_CustomerMsgRef = xmlDoc.CreateElement("CustomerMsgRef");
	////			InvoiceAdd.AppendChild(Element_CustomerMsgRef);
	////			XmlElement Element_CustomerMsgRef_FullName = xmlDoc.CreateElement("FullName");
	////			Element_CustomerMsgRef.AppendChild(Element_CustomerMsgRef_FullName).InnerText = CustomerMessage;
	////		}

	////		// ExchangeRate 
	////		if (ExchangeRate != "")
	////		{
	////			XmlElement Element_ExchangeRate = xmlDoc.CreateElement("ExchangeRate");
	////			InvoiceAdd.AppendChild(Element_ExchangeRate).InnerText = ExchangeRate;
	////		}

	////		//Line Items
	////		XmlElement Element_InvoiceLineAdd;
	////		for (int x = 1; x < 6; x++)
	////		{
	////			Element_InvoiceLineAdd = xmlDoc.CreateElement("InvoiceLineAdd");
	////			InvoiceAdd.AppendChild(Element_InvoiceLineAdd);

	////			string[] lineItem = getLineItem(x);
	////			if (lineItem[0] != "")
	////			{
	////				XmlElement Element_InvoiceLineAdd_ItemRef = xmlDoc.CreateElement("ItemRef");
	////				Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_ItemRef);
	////				XmlElement Element_InvoiceLineAdd_ItemRef_FullName = xmlDoc.CreateElement("FullName");
	////				Element_InvoiceLineAdd_ItemRef.AppendChild(Element_InvoiceLineAdd_ItemRef_FullName).InnerText = lineItem[0];
	////			}
	////			if (lineItem[1] != "")
	////			{
	////				XmlElement Element_InvoiceLineAdd_Desc = xmlDoc.CreateElement("Desc");
	////				Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Desc).InnerText = lineItem[1];
	////			}
	////			if (lineItem[2] != "")
	////			{
	////				XmlElement Element_InvoiceLineAdd_Quantity = xmlDoc.CreateElement("Quantity");
	////				Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Quantity).InnerText = lineItem[2];
	////			}
	////			if (lineItem[3] != "")
	////			{
	////				XmlElement Element_InvoiceLineAdd_Rate = xmlDoc.CreateElement("Rate");
	////				Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Rate).InnerText = lineItem[3];
	////			}
	////			if (lineItem[4] != "")
	////			{
	////				XmlElement Element_InvoiceLineAdd_Amount = xmlDoc.CreateElement("Amount");
	////				Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Amount).InnerText = lineItem[4];
	////			}
	////		}


	////		InvoiceAddRq.SetAttribute("requestID", "99");
	////		requestXML = xmlDoc.OuterXml;

	////		return requestXML;
	////	}

	////	private string[] getLineItem(int index)
	////	{
	////		string[] lineItem = new string[5];
	////		switch (index)
	////		{
	////			//case 1:
	////			//	lineItem[0] = (comboBox_Item1.Text==""      || comboBox_Item1.Text==null)   ? "" : comboBox_Item1.Text;
	////			//	lineItem[1] = (textBox_Desc1.Text == ""     || textBox_Desc1.Text == null)  ? "" : textBox_Desc1.Text;
	////			//	lineItem[2] = (textBox_Qty1.Text == ""      || textBox_Qty1.Text == null)   ? "" : textBox_Qty1.Text;
	////			//	lineItem[3] = (textBox_Price1.Text == ""    || textBox_Price1.Text == null) ? "" : textBox_Price1.Text;
	////			//	lineItem[4] = (textBox_Amount1.Text == ""   || textBox_Amount1.Text == null)? "" : textBox_Amount1.Text;
	////			//	break;
	////			//case 2:
	////			//	lineItem[0] = (comboBox_Item2.Text == "" || comboBox_Item2.Text == null) ? "" : comboBox_Item2.Text;
	////			//	lineItem[1] = (textBox_Desc2.Text == "" || textBox_Desc2.Text == null) ? "" : textBox_Desc2.Text;
	////			//	lineItem[2] = (textBox_Qty2.Text == "" || textBox_Qty2.Text == null) ? "" : textBox_Qty2.Text;
	////			//	lineItem[3] = (textBox_Price2.Text == "" || textBox_Price2.Text == null) ? "" : textBox_Price2.Text;
	////			//	lineItem[4] = (textBox_Amount2.Text == "" || textBox_Amount2.Text == null) ? "" : textBox_Amount2.Text;
	////			//	break;
	////			//case 3:
	////			//	lineItem[0] = (comboBox_Item3.Text == "" || comboBox_Item3.Text == null) ? "" : comboBox_Item3.Text;
	////			//	lineItem[1] = (textBox_Desc3.Text == "" || textBox_Desc3.Text == null) ? "" : textBox_Desc3.Text;
	////			//	lineItem[2] = (textBox_Qty3.Text == "" || textBox_Qty3.Text == null) ? "" : textBox_Qty3.Text;
	////			//	lineItem[3] = (textBox_Price3.Text == "" || textBox_Price3.Text == null) ? "" : textBox_Price3.Text;
	////			//	lineItem[4] = (textBox_Amount3.Text == "" || textBox_Amount3.Text == null) ? "" : textBox_Amount3.Text;
	////			//	break;
	////			//case 4:
	////			//	lineItem[0] = (comboBox_Item4.Text == "" || comboBox_Item4.Text == null) ? "" : comboBox_Item4.Text;
	////			//	lineItem[1] = (textBox_Desc4.Text == "" || textBox_Desc4.Text == null) ? "" : textBox_Desc4.Text;
	////			//	lineItem[2] = (textBox_Qty4.Text == "" || textBox_Qty4.Text == null) ? "" : textBox_Qty4.Text;
	////			//	lineItem[3] = (textBox_Price4.Text == "" || textBox_Price4.Text == null) ? "" : textBox_Price4.Text;
	////			//	lineItem[4] = (textBox_Amount4.Text == "" || textBox_Amount4.Text == null) ? "" : textBox_Amount4.Text;
	////			//	break;
	////			//case 5:
	////			//	lineItem[0] = (comboBox_Item5.Text == "" || comboBox_Item5.Text == null) ? "" : comboBox_Item5.Text;
	////			//	lineItem[1] = (textBox_Desc5.Text == "" || textBox_Desc5.Text == null) ? "" : textBox_Desc5.Text;
	////			//	lineItem[2] = (textBox_Qty5.Text == "" || textBox_Qty5.Text == null) ? "" : textBox_Qty5.Text;
	////			//	lineItem[3] = (textBox_Price5.Text == "" || textBox_Price5.Text == null) ? "" : textBox_Price5.Text;
	////			//	lineItem[4] = (textBox_Amount5.Text == "" || textBox_Amount5.Text == null) ? "" : textBox_Amount5.Text;
	////			//	break;
	////		}
	////		return lineItem;
	////	}

	////	private double getDueInDays(string Terms)
	////	{
	////		double dueInDays = 0;
	////		switch (Terms)
	////		{
	////			case "Due on receipt":
	////				dueInDays = 0;
	////				break;
	////			case "Net 15":
	////				dueInDays = 15;
	////				break;
	////			case "Net 30":
	////				dueInDays = 30;
	////				break;
	////			case "Net 60":
	////				dueInDays = 60;
	////				break;
	////			default:
	////				dueInDays = 0;
	////				break;
	////		}
	////		return dueInDays;
	////	}

	////	private string getDateString(DateTime dt)
	////	{
	////		string year = dt.Year.ToString();
	////		string month = dt.Month.ToString();
	////		if (month.Length < 2) month = "0" + month;
	////		string day = dt.Day.ToString();
	////		if (day.Length < 2) day = "0" + day;
	////		return year + "-" + month + "-" + day;
	////	}

	////	private string buildCurrencyQueryRqXML(string fullName)
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement CurrencyQueryRq = xmlDoc.CreateElement("CurrencyQueryRq");
	////		qbXMLMsgsRq.AppendChild(CurrencyQueryRq);
	////		if (fullName != null)
	////		{
	////			XmlElement fullNameElement = xmlDoc.CreateElement("FullName");
	////			CurrencyQueryRq.AppendChild(fullNameElement).InnerText = fullName;
	////		}
	////		CurrencyQueryRq.SetAttribute("requestID", "6");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildCustomerMsgQueryRqXML(string[] includeRetElement, string fullName)
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement CustomerMsgQueryRq = xmlDoc.CreateElement("CustomerMsgQueryRq");
	////		qbXMLMsgsRq.AppendChild(CustomerMsgQueryRq);
	////		if (fullName != null)
	////		{
	////			XmlElement fullNameElement = xmlDoc.CreateElement("FullName");
	////			CustomerMsgQueryRq.AppendChild(fullNameElement).InnerText = fullName;
	////		}
	////		for (int x = 0; x < includeRetElement.Length; x++)
	////		{
	////			XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////			CustomerMsgQueryRq.AppendChild(includeRet).InnerText = includeRetElement[x];
	////		}
	////		CustomerMsgQueryRq.SetAttribute("requestID", "5");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildSalesTaxCodeQueryRqXML()
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement salesTaxCodeQueryRq = xmlDoc.CreateElement("SalesTaxCodeQueryRq");
	////		qbXMLMsgsRq.AppendChild(salesTaxCodeQueryRq);
	////		XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////		salesTaxCodeQueryRq.AppendChild(includeRet).InnerText = "Name";
	////		salesTaxCodeQueryRq.SetAttribute("requestID", "4");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildTermsQueryRqXML()
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement termsQueryRq = xmlDoc.CreateElement("TermsQueryRq");
	////		qbXMLMsgsRq.AppendChild(termsQueryRq);
	////		XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////		termsQueryRq.AppendChild(includeRet).InnerText = "Name";
	////		termsQueryRq.SetAttribute("requestID", "3");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildItemQueryRqXML(string[] includeRetElement, string fullName)
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement ItemQueryRq = xmlDoc.CreateElement("ItemQueryRq");
	////		qbXMLMsgsRq.AppendChild(ItemQueryRq);
	////		if (fullName != null)
	////		{
	////			XmlElement fullNameElement = xmlDoc.CreateElement("FullName");
	////			ItemQueryRq.AppendChild(fullNameElement).InnerText = fullName;
	////		}
	////		for (int x = 0; x < includeRetElement.Length; x++)
	////		{
	////			XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////			ItemQueryRq.AppendChild(includeRet).InnerText = includeRetElement[x];
	////		}
	////		ItemQueryRq.SetAttribute("requestID", "2");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildPreferencesQueryRqXML(string[] includeRetElement, string fullName)
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement PrefsQueryRq = xmlDoc.CreateElement("PreferencesQueryRq");
	////		qbXMLMsgsRq.AppendChild(PrefsQueryRq);
	////		for (int x = 0; x < includeRetElement.Length; x++)
	////		{
	////			XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////			PrefsQueryRq.AppendChild(includeRet).InnerText = includeRetElement[x];
	////		}
	////		PrefsQueryRq.SetAttribute("requestID", "1");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	private string buildCustomerQueryRqXML(string[] includeRetElement, string fullName)
	////	{
	////		string xml = "";
	////		XmlDocument xmlDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, this.Processor.maxVersion);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		XmlElement CustomerQueryRq = xmlDoc.CreateElement("CustomerQueryRq");
	////		qbXMLMsgsRq.AppendChild(CustomerQueryRq);
	////		if (fullName != null)
	////		{
	////			XmlElement fullNameElement = xmlDoc.CreateElement("FullName");
	////			CustomerQueryRq.AppendChild(fullNameElement).InnerText = fullName;
	////		}
	////		for (int x = 0; x < includeRetElement.Length; x++)
	////		{
	////			XmlElement includeRet = xmlDoc.CreateElement("IncludeRetElement");
	////			CustomerQueryRq.AppendChild(includeRet).InnerText = includeRetElement[x];
	////		}
	////		CustomerQueryRq.SetAttribute("requestID", "1");
	////		xml = xmlDoc.OuterXml;
	////		return xml;
	////	}

	////	public virtual string buildDataCountQuery(string request)
	////	{
	////		string input = "";
	////		XmlDocument inputXMLDoc = new XmlDocument();
	////		XmlElement qbXMLMsgsRq = buildRqEnvelope(inputXMLDoc, this.Processor.maxVersion);
	////		XmlElement queryRq = inputXMLDoc.CreateElement(request);
	////		queryRq.SetAttribute("metaData", "MetaDataOnly");
	////		qbXMLMsgsRq.AppendChild(queryRq);
	////		input = inputXMLDoc.OuterXml;
	////		return input;
	////	}

	////	private XmlElement buildRqEnvelope(XmlDocument doc, string maxVer)
	////	{
	////		doc.AppendChild(doc.CreateXmlDeclaration("1.0", null, null));
	////		doc.AppendChild(doc.CreateProcessingInstruction("qbxml", "version=\"" + maxVer + "\""));
	////		XmlElement qbXML = doc.CreateElement("QBXML");
	////		doc.AppendChild(qbXML);
	////		XmlElement qbXMLMsgsRq = doc.CreateElement("QBXMLMsgsRq");
	////		qbXML.AppendChild(qbXMLMsgsRq);
	////		qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
	////		return qbXMLMsgsRq;
	////	}


	////	// Utilities
	////	private static bool IsDecimal(string theValue)
	////	{
	////		bool returnVal = false;
	////		try
	////		{
	////			Convert.ToDouble(theValue, System.Globalization.CultureInfo.CurrentCulture);
	////			returnVal = true;
	////		}
	////		catch
	////		{
	////			returnVal = false;
	////		}
	////		finally
	////		{
	////		}

	////		return returnVal;

	////	}

	////	private string calculateAmount(string quantity, string price)
	////	{
	////		if (quantity == null || quantity == "") quantity = "0";
	////		if (price == null || price == "") price = "0";
	////		decimal qty = Convert.ToDecimal(quantity);
	////		decimal prc = Convert.ToDecimal(price);
	////		decimal amount = qty * prc;
	////		return amount.ToString();
	////	}
	//}
}
