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
        public void CreateTaskWithTimeout()
        {
            string dynamicsConnectionString = ConfigurationManager.ConnectionStrings["DynamicsConnectionString"].ConnectionString;
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);
            
            crmServiceClient.OrganizationServiceProxy.Timeout = new TimeSpan(0, 0, 10);

            Guid taskid = crmServiceClient.Create(new Entity("task"));

            crmServiceClient.Retrieve("task",taskid, new Microsoft.Xrm.Sdk.Query.ColumnSet(false));
        }
    }
}
