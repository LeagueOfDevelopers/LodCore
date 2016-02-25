using DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTests
{
    [TestClass]
    public class DatabaseSessionProviderTests
    {
        [TestMethod]
        public void CreateSchemaTest()
        {
            var provider = new DatabaseSessionProvider();
            provider.OpenSession();
            using (var session = provider.GetCurrentSession())
            {
            }
        }
    }
}