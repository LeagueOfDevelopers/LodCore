using System;
using System.Configuration;
using System.IO;
using LodCore.Common;
using LodCore.Domain.UserManagement;
using Newtonsoft.Json;

namespace IntegrationControllerTests.Helpers
{
    public static class Settings
    {
        public static Uri EndpointUri => new Uri(ConfigurationManager.AppSettings["EndpointUri"]);

        public static Account ExistantAdminAccount
        {
            get
            {
                var serializedAccount = File.ReadAllText("existantAccount.json");
                var account = JsonConvert.DeserializeObject<Account>(serializedAccount,
                    new JsonEmailConverter(), new JsonPasswordConverter());
                return account;
            }
        }
    }
}