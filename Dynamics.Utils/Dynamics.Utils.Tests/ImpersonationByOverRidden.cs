using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;

namespace Dynamics.Utils.Tests
{
    [TestClass]
    public class ImpersonationByOverRidden
    {
        private Guid UserID1 = new Guid("DF6CBBAE-0F51-4973-8975-34470B1239C5");
        private Guid UserID2 = new Guid("CF132883-1222-E911-A987-00224800CDC2");
        private string dynamicsConnectionString;

        [TestInitialize()]
        public void InitialiseTests()
        {
            dynamicsConnectionString = ConfigurationManager.ConnectionStrings["DynamicsConnectionString"].ConnectionString;
        }


        [TestMethod]
        public void CreateContactOverriddingCreatedOn()
        {
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);

            DateTime newCreatedOn = DateTime.Now.AddDays(-2);

            Entity newContact = InitialiseContact();
            newContact["overriddencreatedon"] = newCreatedOn;
            newContact.Id = crmServiceClient.Create(newContact);

            Entity createdContact = crmServiceClient.Retrieve("contact", newContact.Id, new ColumnSet(new string[] { "createdon" }));

            DateTime actualCreatedOn = ((DateTime)createdContact["createdon"]);

            Assert.AreEqual(newCreatedOn.ToString("yyyyMMddHHmmss"), actualCreatedOn.ToString("yyyyMMddHHmmss"));
        }

        [TestMethod]
        public void CreateContactTryingToOverrideCreatedAndModified()
        {
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);

            Entity newContact = InitialiseContact();

            DateTime newCreatedOn = DateTime.Now.AddDays(-1);
            DateTime newModifiedOn = DateTime.Now.AddHours(-2);

            newContact["createdon"] = newCreatedOn;
            newContact["createdby"] = new EntityReference("systemuser", UserID1);
            newContact["modifiedon"] = newModifiedOn;
            newContact["modifiedby"] = new EntityReference("systemuser", UserID2);
            newContact.Id = crmServiceClient.Create(newContact);

            Entity createdContact = crmServiceClient.Retrieve("contact", newContact.Id, new ColumnSet(new string[] { "createdon", "createdby","modifiedon","modifiedby"}));

            DateTime actualCreatedOn = ((DateTime)createdContact["createdon"]);
            Guid actualCreatedBy = ((EntityReference)createdContact["createdby"]).Id;
            DateTime actualModifiedOn = ((DateTime)createdContact["modifiedon"]);
            Guid actualModifiedBy = ((EntityReference)createdContact["modifiedby"]).Id;

            Assert.AreNotEqual(newCreatedOn.ToString("yyyyMMddHHmmss"), actualCreatedOn.ToString("yyyyMMddHHmmss"));
            Assert.AreNotEqual(UserID1, actualCreatedBy);
            Assert.AreNotEqual(newModifiedOn.ToString("yyyyMMddHHmmss"), actualModifiedOn.ToString("yyyyMMddHHmmss"));
            Assert.AreNotEqual(UserID2, actualModifiedBy);
        }

        /// <summary>
        /// This test requires:
        /// 1) the plugin "ImpersonateRecord" enabled in the pre-create contact operation.
        /// 2) custom contact attributes rtb_overriddencreatedby, rtb_overriddenmodifiedon and rtb_overriddenmodifiedby
        /// </summary>
        [TestMethod]
        public void CreateContactUsingCustomOverridden()
        {
            CrmServiceClient crmServiceClient = new CrmServiceClient(dynamicsConnectionString);

            DateTime newCreatedOn = DateTime.Now.AddDays(-2);
            EntityReference newCreatedBy = new EntityReference("systemuser",UserID1);
            DateTime newModifiedOn = DateTime.Now.AddHours(-5);
            EntityReference newModifiedBy = new EntityReference("systemuser", UserID2);

            Entity newContact = InitialiseContact();
            newContact["overriddencreatedon"] = newCreatedOn;
            newContact["rtb_overriddencreatedby"] = newCreatedBy;
            newContact["rtb_overriddenmodifiedon"] = newModifiedOn;
            newContact["rtb_overriddenmodifiedby"] = newModifiedBy;
            newContact.Id = crmServiceClient.Create(newContact);

            Entity createdContact = crmServiceClient.Retrieve("contact", newContact.Id, new ColumnSet(new string[] { "createdon", "createdby", "modifiedby", "modifiedon" }));

            Guid actualCreatedBy = ((EntityReference)createdContact["createdby"]).Id;
            DateTime actualCreatedOn = (DateTime)createdContact["createdon"];
            Guid actualModifiedBy = ((EntityReference)createdContact["modifiedby"]).Id;
            DateTime actualModifiedOn = (DateTime)createdContact["modifiedon"];

            Assert.AreEqual(UserID1, actualCreatedBy);
            Assert.AreEqual(newCreatedOn.ToString("yyyyMMddHHmmss"), actualCreatedOn.ToString("yyyyMMddHHmmss"));
            Assert.AreEqual(UserID2, actualModifiedBy);
            Assert.AreEqual(newModifiedOn.ToString("yyyyMMddHHmmss"), actualModifiedOn.ToString("yyyyMMddHHmmss"));

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
