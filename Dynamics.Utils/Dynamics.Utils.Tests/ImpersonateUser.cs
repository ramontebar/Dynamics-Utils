using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;

namespace Dynamics.Utils.Tests
{
    
    [TestClass]
    public class ImpersonateUser
    {
        private const string UserID1 = "DF6CBBAE-0F51-4973-8975-34470B1239C5";
        private const string UserID2 = "CF132883-1222-E911-A987-00224800CDC2";

        [TestMethod]
        public void CreateContact()
        {
            string dynamicsConnectionString = ConfigurationManager.ConnectionStrings["DynamicsConnectionString"].ConnectionString;
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);

            crmServiceClient.OrganizationServiceProxy.CallerId = new Guid(UserID1);
            Entity contact = InitialiseContact();
            contact.Id = crmServiceClient.Create(contact);

            crmServiceClient.OrganizationServiceProxy.CallerId = new Guid(UserID2);
            contact["emailaddress1"] = string.Format("test.{0}@example.com", DateTime.Now.ToString("yyyyMMddHHmmss"));
            crmServiceClient.Update(contact);
        }

        private Entity InitialiseContact()
        {
            Entity contact = new Entity("contact");
            contact["firstname"] = "Test";
            contact["lastname"] = DateTime.Now.ToString("yyyyMMddHHmmss");

            return contact;
        }
    }
}
