using CFI.Utility.Logging;
using RandREng.QBLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace RandREng.QBLibrary
{
	public class QBCom : IDisposable
	{
		private RandREng.QBLibrary.IQBProcessor process ;
		private ILogger Logger { get; set; }
//		RandREng.QBLibrary.Class1 qb ;
		XmlSerializerNamespaces ns;
		XmlSerializer ser;

		public QBCom(string appID, string appName, string companyFile, ILogger logger)
		{
			process = new RandREng.QBLibrary.QBProcessor(logger);
			process.AppID = appID;
			process.AppName = appName;
			process.CompanyFile = companyFile;

			//Create our own namespaces for the output
			ns = new XmlSerializerNamespaces();

			//Add an empty namespace and empty value
			ns.Add("", "");
			ser = new XmlSerializer(typeof(QBXML));
		}

		public bool CanConnect { get { return this.process.CanConnect; } }

		public QBXMLMsgsRs Transmit<T>(object txx, out T resp)
		{
			QBXML req = new QBXML();
			req.ItemsElementName = new ItemsChoiceType103[] { ItemsChoiceType103.QBXMLMsgsRq };
			req.Items = new object[] { new QBXMLMsgsRq() };
			QBXMLMsgsRq r = req.Items[0] as QBXMLMsgsRq;
			r.onError = QBXMLMsgsRqOnError.stopOnError;
			r.Items = new object[] { txx };

			MemoryStream ms = new MemoryStream();
			XmlTextWriter w = new XmlTextWriter(ms, null);
			w.WriteProcessingInstruction("xml", "version=\"1.0\"");
			w.WriteProcessingInstruction("qbxml", "version=\"11.0\"");
			ser.Serialize(w, req, ns);
			ms.Seek(0, SeekOrigin.Begin);

			StreamReader sr = new StreamReader(ms);
			string t = sr.ReadToEnd();
			t = this.process.Transmit(t);

			QBXML ret = null;
			StringReader tr = new StringReader(t);
			ret = (QBXML)ser.Deserialize(tr);
			QBXMLMsgsRs rs = (ret.Items[0] as QBXMLMsgsRs);
			resp = (T) rs.Items[0];
			return rs;
		}


		public QBXMLMsgsRs InvoiceQuery(string ItemRefNumber, string refNumber)
		{
			InvoiceQueryRqType req = new InvoiceQueryRqType();
			if (!string.IsNullOrEmpty(ItemRefNumber))
			{
				req.TxnID = ItemRefNumber;
			}
			if (!string.IsNullOrEmpty(refNumber))
			{
				req.RefNumber = refNumber;
			}
			req.IncludeLineItems = "true";
			InvoiceQueryRsType resp;
			QBXMLMsgsRs r = this.Transmit(req, out resp);

			return r;

		}

		public QBXMLMsgsRs InvoiceMod(string ItemRefNumber, string refNumber)
		{
			InvoiceModRqType req = new InvoiceModRqType();
			if (!string.IsNullOrEmpty(ItemRefNumber))
			{
				req.InvoiceMod.TxnID = ItemRefNumber;
			}
			if (!string.IsNullOrEmpty(refNumber))
			{
				req.InvoiceMod.RefNumber = refNumber;
			}
			InvoiceQueryRsType resp;
			QBXMLMsgsRs r = this.Transmit(req, out resp);

			return r;
		}

		public QBXMLMsgsRs buildInvoiceAddRqXML(string customer, DateTime txnDate, string refNumber)
		{
			InvoiceAddRqType req = new InvoiceAddRqType();
			req.InvoiceAdd = new InvoiceAdd();
			req.InvoiceAdd.TemplateRef = new TemplateRef();
			req.InvoiceAdd.TemplateRef.FullName = customer;
			req.InvoiceAdd.CustomerRef = new CustomerRef();
			req.InvoiceAdd.CustomerRef.FullName = customer;
			req.InvoiceAdd.Other = "tttt";

			if (txnDate != null)
			{
				req.InvoiceAdd.TxnDate = txnDate.ToString("yyyy-MM-dd");
			}
			if (!string.IsNullOrEmpty(refNumber))
			{
				req.InvoiceAdd.RefNumber = refNumber;
			}
			//string requestXML = "";

			////if (!validateInput()) return null;

			////GET ALL INPUT INTO XML
			//XmlDocument xmlDoc = new XmlDocument();
			//XmlElement qbXMLMsgsRq = buildRqEnvelope(xmlDoc, maxVersion);
			//qbXMLMsgsRq.SetAttribute("onError", "stopOnError");
			//XmlElement InvoiceAddRq = xmlDoc.CreateElement("InvoiceAddRq");
			//qbXMLMsgsRq.AppendChild(InvoiceAddRq);
			//XmlElement InvoiceAdd = xmlDoc.CreateElement("InvoiceAdd");
			//InvoiceAddRq.AppendChild(InvoiceAdd);

			//// CustomerRef -> FullName
			//if (comboBox_Customer.Text != "")
			//{
			//	XmlElement Element_CustomerRef = xmlDoc.CreateElement("CustomerRef");
			//	InvoiceAdd.AppendChild(Element_CustomerRef);
			//	XmlElement Element_CustomerRef_FullName = xmlDoc.CreateElement("FullName");
			//	Element_CustomerRef.AppendChild(Element_CustomerRef_FullName).InnerText = comboBox_Customer.Text;
			//}


			// RefNumber 
			//if (textBox_RefNumber.Text != "")
			//{
			//	XmlElement Element_RefNumber = xmlDoc.CreateElement("RefNumber");
			//	InvoiceAdd.AppendChild(Element_RefNumber).InnerText = textBox_RefNumber.Text;
			//}

			// BillAddress
/*
			if (label_BillTo.Text != "")
			{
				string[] BillAddress = label_BillTo.Text.Split('\n');
				XmlElement Element_BillAddress = xmlDoc.CreateElement("BillAddress");
				InvoiceAdd.AppendChild(Element_BillAddress);
				for (int i = 0; i < BillAddress.Length; i++)
				{
					if (BillAddress[i] != "" || BillAddress[i] != null)
					{
						XmlElement Element_Addr = xmlDoc.CreateElement("Addr" + (i + 1));
						Element_BillAddress.AppendChild(Element_Addr).InnerText = BillAddress[i];
					}
				}
			}

			// TermsRef -> FullName 
			bool termsAvailable = false;
			if (comboBox_Terms.Text != "")
			{
				termsAvailable = true;
				XmlElement Element_TermsRef = xmlDoc.CreateElement("TermsRef");
				InvoiceAdd.AppendChild(Element_TermsRef);
				XmlElement Element_TermsRef_FullName = xmlDoc.CreateElement("FullName");
				Element_TermsRef.AppendChild(Element_TermsRef_FullName).InnerText = comboBox_Terms.Text;
			}

			// DueDate 
			if (termsAvailable)
			{
				DateTime DT_DueDate = System.DateTime.Today;
				double dueInDays = getDueInDays();
				DT_DueDate = DT_TxnDate.AddDays(dueInDays);
				string DueDate = getDateString(DT_DueDate);
				XmlElement Element_DueDate = xmlDoc.CreateElement("DueDate");
				InvoiceAdd.AppendChild(Element_DueDate).InnerText = DueDate;
			}

			// CustomerMsgRef -> FullName 
			if (comboBox_CustomerMessage.Text != "")
			{
				XmlElement Element_CustomerMsgRef = xmlDoc.CreateElement("CustomerMsgRef");
				InvoiceAdd.AppendChild(Element_CustomerMsgRef);
				XmlElement Element_CustomerMsgRef_FullName = xmlDoc.CreateElement("FullName");
				Element_CustomerMsgRef.AppendChild(Element_CustomerMsgRef_FullName).InnerText = comboBox_CustomerMessage.Text;
			}

			// ExchangeRate 
			if (textBox_ExchangeRate.Text != "")
			{
				XmlElement Element_ExchangeRate = xmlDoc.CreateElement("ExchangeRate");
				InvoiceAdd.AppendChild(Element_ExchangeRate).InnerText = textBox_ExchangeRate.Text;
			}
*/
			//Line Items

			List<InvoiceLineAdd> lineItems = new List<InvoiceLineAdd>();

			InvoiceLineAdd line = new InvoiceLineAdd();
//			ItemRef 
			line.ItemRef = new ItemRef();
			line.ItemRef.FullName = "Labor";
			line.Desc = "Test";
			line.ClassRef = new ClassRef();
			line.ClassRef.FullName = "Home Depot:Atlanta:Hard Surface";
			line.Amount = "123456.78";
			line.IsTaxable = "false";
			lineItems.Add(line);
			line = new InvoiceLineAdd();
			line.ItemRef = new ItemRef();
			line.ItemRef.FullName = "Back Charge";
			line.Desc = "Smith James";
			line.ClassRef = new ClassRef();
			line.ClassRef.FullName = "Home Depot:Atlanta:Hard Surface";
			line.Amount = "-543.21";
			line.IsTaxable = "false";
			lineItems.Add(line);
			req.InvoiceAdd.Items = lineItems.ToArray(); ;



			//XmlElement Element_InvoiceLineAdd;
			//for (int x = 1; x < 6; x++)
			//{
			//	Element_InvoiceLineAdd = xmlDoc.CreateElement("InvoiceLineAdd");
			//	InvoiceAdd.AppendChild(Element_InvoiceLineAdd);

			//	string[] lineItem = getLineItem(x);
			//	if (lineItem[0] != "")
			//	{
			//		XmlElement Element_InvoiceLineAdd_ItemRef = xmlDoc.CreateElement("ItemRef");
			//		Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_ItemRef);
			//		XmlElement Element_InvoiceLineAdd_ItemRef_FullName = xmlDoc.CreateElement("FullName");
			//		Element_InvoiceLineAdd_ItemRef.AppendChild(Element_InvoiceLineAdd_ItemRef_FullName).InnerText = lineItem[0];
			//	}
			//	if (lineItem[1] != "")
			//	{
			//		XmlElement Element_InvoiceLineAdd_Desc = xmlDoc.CreateElement("Desc");
			//		Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Desc).InnerText = lineItem[1];
			//	}
			//	if (lineItem[2] != "")
			//	{
			//		XmlElement Element_InvoiceLineAdd_Quantity = xmlDoc.CreateElement("Quantity");
			//		Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Quantity).InnerText = lineItem[2];
			//	}
			//	if (lineItem[3] != "")
			//	{
			//		XmlElement Element_InvoiceLineAdd_Rate = xmlDoc.CreateElement("Rate");
			//		Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Rate).InnerText = lineItem[3];
			//	}
			//	if (lineItem[4] != "")
			//	{
			//		XmlElement Element_InvoiceLineAdd_Amount = xmlDoc.CreateElement("Amount");
			//		Element_InvoiceLineAdd.AppendChild(Element_InvoiceLineAdd_Amount).InnerText = lineItem[4];
			//	}
			//}


			//InvoiceAddRq.SetAttribute("requestID", "99");
			//requestXML = xmlDoc.OuterXml;

			//return requestXML;



			InvoiceAddRsType resp;
			QBXMLMsgsRs r = this.Transmit(req, out resp);

			return r;
		}



		public void Dispose()
		{
			this.process.Dispose();
		}
	}
}
