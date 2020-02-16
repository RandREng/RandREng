using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace RandREng.QBServer
{
	/// <summary>
	/// Defines an Test request message.
	/// </summary>
	[DataContract(Namespace = "http://RandREng.com/contracts/")]
	public class QBRequest
	{
		[DataMember]
		public string XML { get; set; }
		[DataMember]
		public string CompanyFile { get; set; }
		[DataMember]
		public string AppID { get; set; }
		[DataMember]
		public string AppName { get; set; }
	}

	/// <summary>
	/// Defines an Test response message.
	/// </summary>
	[DataContract(Namespace = "http://RandREng.com/contracts/")]
	public class QBResponse
	{
		[DataMember]
		public string Data { get; set; }
		[DataMember]
		public DateTime Timestamp { get; set; }
	}


	/// <summary>
	/// Defines an Test service.
	/// </summary>
	[ServiceContract(Namespace = "http://RandREng.com/contracts/")]
	public interface IQBService
	{
		[OperationContract]
		QBResponse Send(QBRequest request);

		[OperationContract]
		QBResponse GetMaxVersion(QBRequest request);

		[OperationContract]
		void Disconnect();
	}
}
