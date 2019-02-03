using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using Microsoft.Xrm.Sdk.Query;

namespace Dynamics.Utils.Tests
{
    
    [TestClass]
    public class ImpersonationByCallerId
    {
        private Guid ServiceUser = new Guid("7C9E817B-E026-E911-A985-00224800C3E8");
        private Guid UserID1 = new Guid("DF6CBBAE-0F51-4973-8975-34470B1239C5");
        private Guid UserID2 = new Guid("CF132883-1222-E911-A987-00224800CDC2");
        private string dynamicsConnectionString;

        [TestInitialize()]
        public void InitialiseTests()
        {
            dynamicsConnectionString = ConfigurationManager.ConnectionStrings["DynamicsConnectionString"].ConnectionString;
        }

        [TestMethod]
        public void CreateContactUsingProxyCallerId()
        {
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);

            crmServiceClient.OrganizationServiceProxy.CallerId = UserID1;
            Entity newContact = InitialiseContact();
            newContact.Id = crmServiceClient.Create(newContact);

            crmServiceClient.OrganizationServiceProxy.CallerId = UserID2;
            newContact["emailaddress1"] = string.Format("test.{0}@example.com", DateTime.Now.ToString("yyyyMMddHHmmss"));
            crmServiceClient.Update(newContact);

            Entity createdContact = crmServiceClient.Retrieve("contact", newContact.Id, new ColumnSet(new string[] { "createdby", "createdonbehalfby","modifiedby","modifiedonbehalfby" }));

            Guid actualCreatedBy = ((EntityReference)createdContact["createdby"]).Id;
            Guid createdOnBehalfBy = ((EntityReference)createdContact["createdonbehalfby"]).Id;
            Guid actualModifiedBy = ((EntityReference)createdContact["modifiedby"]).Id;
            Guid modifiedOnBehalfBy = ((EntityReference)createdContact["modifiedonbehalfby"]).Id;

            Assert.AreEqual(UserID1, actualCreatedBy);
            Assert.AreEqual(ServiceUser, createdOnBehalfBy);
            Assert.AreEqual(UserID2, actualModifiedBy);
            Assert.AreEqual(ServiceUser, modifiedOnBehalfBy);
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
