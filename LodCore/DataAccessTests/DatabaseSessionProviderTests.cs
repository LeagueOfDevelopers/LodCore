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
            using (var session = provider.OpenSession())
            {
            }
        }
    }
}
