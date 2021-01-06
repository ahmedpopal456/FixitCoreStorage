using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.Queue.Adapters;
using Fixit.Core.Storage.UnitTests.FakeDataProviders;
using AutoMapper;
using Fixit.Core.Storage.Queue.Mappers;

namespace Fixit.Core.Storage.UnitTests
{
  public class TestBase
  {
    public IFakeSeederFactory fakeDtoSeederFactory;

    // Storage System Mocks
    protected Mock<IQueueServiceAdapter> _queueServiceAdapter;
    protected Mock<IQueueAdapter> _queueAdapter;

    // Mapper
    protected MapperConfiguration _mapperConfiguration = new MapperConfiguration(config =>
    {
      config.AddProfile(new QueueMapper());
    });

    public TestBase()
    {
      fakeDtoSeederFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext) { }

    [AssemblyCleanup]
    public static void AfterSuiteTests() { }
  }
}
