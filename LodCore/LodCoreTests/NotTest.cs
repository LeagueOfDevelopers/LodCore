using System.Threading.Tasks;
using IntegrationControllerTests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LodCore.QueryService.Handlers;
using LodCore.QueryService.Queries.NotificationQuery;
using MySql.Data.MySqlClient;
using System.Text;
using Dapper;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace IntegrationControllerTests
{
    [TestClass]
    public class NotTest
    {
        [TestMethod]
        public void MyTestMethod()
        {
        string connect = "Server=localhost;Database=test;Uid=root;Pwd=ybrjkfq86";

        NotificationHandler notificationHandler = new NotificationHandler(connect);
           var res = notificationHandler.Handle(new PageNotificationForDeveloperQuery(1,0,2));

        }

        [TestMethod]
        public void s()
        {
            List<Account> ar;
            string connect = "Server=localhost;Database=test;Uid=root;Pwd=ybrjkfq86";
            string query = "SELECT UserId, Firstname, RegistrationTime FROM accounts";

            //Dictionary<string, string> columMaps = new Dictionary<string, string>
            //{
            //    {"UserId", "UserID" },
            //    {"Firstname", "FirstName" }
            //};
            //var mapper = new Func<Type, string, PropertyInfo>
            //var map = new CustomPropertyTypeMap(typeof(Account), (type, columName) => mapper())
            //SqlMapper.SetTypeMap(typeof(Account), map);

            using (var connection = new MySqlConnection(connect))
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
               ar =connection.Query<dynamic>(query).Select(p => 
                    new Account(p.UserId, p.Firstname, p.RegistrationTime))
                    .AsList();
                
            }
int a = 2;
        }

        public class Account
        {
            public Account(int userID, string firstName, DateTime registrationTime)
            {
                UserID = userID;
                FirstName = firstName;
                RegistrationTime = registrationTime;
            }

            public DateTime RegistrationTime { get; set; }
            public int UserID { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
        }
    }

}
