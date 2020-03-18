using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk;

namespace Dynamics.Utils.Tests
{
    [TestClass]
    public class ProxyTimeout
    {
        [TestMethod]
        public void OrganizationServiceProxyTimeout()
        {
            string connectionString =
                "AuthType=Office365;" +
                "Url=https://mvp20200218.crm11.dynamics.com;" +
                "Username=ramon@mvp20200218.onmicrosoft.com;" +
                "Password=[PASSWORD];";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            crmServiceClient.OrganizationServiceProxy.Timeout = new TimeSpan(0, 0, 15);

            Guid taskid = crmServiceClient.Create(new Entity("task"));

            crmServiceClient.Retrieve("task",taskid, new Microsoft.Xrm.Sdk.Query.ColumnSet(false));
        }

        [TestMethod]
        public void OrganizationWebProxyClientTimeOut()
        {
            string connectionString =
                "authtype=ClientSecret;" +
                "url=https://mvp20200218.crm11.dynamics.com;" +
                "clientid=6a268676-1043-4b1e-b1d8-d89b7859eeb5;" +
                "clientsecret=[SECRET]";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            crmServiceClient.OrganizationWebProxyClient.InnerChannel.OperationTimeout = new TimeSpan(0, 0, 15);

            Guid taskid = crmServiceClient.Create(new Entity("task"));

            crmServiceClient.Retrieve("task", taskid, new Microsoft.Xrm.Sdk.Query.ColumnSet(false));
        }
    }
}
