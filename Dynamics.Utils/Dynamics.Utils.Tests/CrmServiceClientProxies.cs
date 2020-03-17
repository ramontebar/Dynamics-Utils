using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Tooling.Connector;
using System.Configuration;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using System.Net;

namespace Dynamics.Utils.Tests
{
    [TestClass]
    public class CrmServiceClientProxies
    {
        [TestMethod]
        public void UsingOffice365Auth()
        {
            string connectionString =
                "AuthType=Office365;" +
                "Url=https://mvp20200218.crm11.dynamics.com;" +
                "Username=ramon@mvp20200218.onmicrosoft.com;" +
                "Password=SupCRM1001;";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            Assert.IsNotNull(crmServiceClient.OrganizationServiceProxy);
            Assert.IsNull(crmServiceClient.OrganizationWebProxyClient);

            WhoAmIResponse whoAmIResponse = crmServiceClient.Execute(new WhoAmIRequest()) as WhoAmIResponse;
            Assert.IsNotNull(whoAmIResponse);
        }

        [TestMethod]
        public void UsingClientSecretAuth()
        {
            string connectionString =
                "authtype=ClientSecret;" +
                "url=https://mvp20200218.crm11.dynamics.com;" +
                "clientid=6a268676-1043-4b1e-b1d8-d89b7859eeb5;" +
                "clientsecret=[iSCPxcTdf8ofh@C@eG-2X35PIT11vA1";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            Assert.IsNull(crmServiceClient.OrganizationServiceProxy);
            Assert.IsNotNull(crmServiceClient.OrganizationWebProxyClient);

            WhoAmIResponse whoAmIResponse = crmServiceClient.Execute(new WhoAmIRequest()) as WhoAmIResponse;
            Assert.IsNotNull(whoAmIResponse);
        }

        [TestMethod]
        public void UsingOAuthAuth()
        {
            string connectionString =
                "AuthType=OAuth;" +
                "Username=ramon@mvp20200218.onmicrosoft.com;" +
                "Password=SupCRM1001;" +
                "Url=https://mvp20200218.crm11.dynamics.com;" +
                "AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;" +
                "RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;" +
                "LoginPrompt=Never";

            CrmServiceClient crmServiceClient = new CrmServiceClient(connectionString);
            Assert.IsNull(crmServiceClient.OrganizationServiceProxy);
            Assert.IsNotNull(crmServiceClient.OrganizationWebProxyClient);

            WhoAmIResponse whoAmIResponse = crmServiceClient.Execute(new WhoAmIRequest()) as WhoAmIResponse;
            Assert.IsNotNull(whoAmIResponse);
        }

        
    }
}
