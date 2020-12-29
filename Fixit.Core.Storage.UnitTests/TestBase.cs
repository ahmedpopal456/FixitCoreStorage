using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.UnitTests
{
  public class TestBase
  {
    public IFakeSeederFactory fakeDtoSeederFactory;

    //protected Mock<IDatabaseAdapter> _cosmosDatabaseAdapter;
    //protected Mock<IDatabaseTableAdapter> _cosmosDatabaseTableAdapter;
    //protected Mock<IDatabaseTableEntityAdapter> _cosmosDatabaseTableEntityAdapter;

    public TestBase()
    {
      //fakeDtoSeederFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext) { }

    [AssemblyCleanup]
    public static void AfterSuiteTests() { }
  }
}
