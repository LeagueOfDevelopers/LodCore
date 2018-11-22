using LodCore.Infrastructure.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessTests
{
    [TestClass]
    public class DatabaseSessionProviderTests
    {
        [TestMethod]
        [TestCategory("Integration")]
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