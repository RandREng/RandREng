using RandREng.ReportServer.ReportExecution;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static RandREng.ReportServer.ReportExecution.ReportExecutionServiceSoapClient;

namespace RandREng.ReportServer
{
    public class Renderer
    {
        public async static Task<byte[]> Render(string serverName, string reportPath, ParameterValue[] parameters, string userName, string passWord, string format = "PDF")
        {
            BasicHttpsBinding rsBinding = new BasicHttpsBinding(BasicHttpsSecurityMode.Transport)
            {
                Security =
                    {
                        Transport = new HttpTransportSecurity {ClientCredentialType = HttpClientCredentialType.Ntlm}
                    },
                MaxBufferSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                MaxReceivedMessageSize = int.MaxValue,
                AllowCookies = true
            };

            EndpointAddress rsEndpointAddress = new EndpointAddress($"https://{serverName}/ReportServer/ReportExecution2005.asmx");
            

            ReportExecutionServiceSoapClient rsExec = new ReportExecutionServiceSoapClient(rsBinding, rsEndpointAddress);
            rsExec.ClientCredentials.UserName.UserName = userName;
            rsExec.ClientCredentials.UserName.Password = passWord;

            string historyID = null;
            TrustedUserHeader trustedUserHeader = null;

            // Here we call the async LoadReport() method using the "await" keyword, which means any code below this method
            // will not execute until the result from the LoadReportAsync task is returned

            LoadReportResponse loadReport = await rsExec.LoadReportAsync(trustedUserHeader, reportPath, historyID);
            // By the time the LoadReportAsync task is returned successfully, its "executionInfo" property
            // would have already been populated. Now the remaining code in this main thread will resume executing

            SetExecutionParametersResponse setParamsResponse = await rsExec.SetExecutionParametersAsync(loadReport.ExecutionHeader, trustedUserHeader, parameters, "en-US");


            string deviceInfo = "<DeviceInfo><Toolbar>False</Toolbar></DeviceInfo>";
            RenderRequest renderReq = new RenderRequest(loadReport.ExecutionHeader, trustedUserHeader, format, deviceInfo);
            // Now, similar to the above task, we will call the RenderAsync() method and await its result
            RenderResponse taskRender = await rsExec.RenderAsync(renderReq);


            return taskRender.Result;
        }
 


    }
}
