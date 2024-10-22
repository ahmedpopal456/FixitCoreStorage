﻿using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fixit.Core.Storage.UnitTests.FakeDataProviders;
using Fixit.Core.DataContracts.Seeders;
using Fixit.Core.Storage.Storage.Queue.Adapters;
using Fixit.Core.Storage.Storage.Queue.Mappers;

namespace Fixit.Core.Storage.UnitTests
{
  public class TestBase
  {
    public IFakeSeederFactory _fakeDtoSeederFactory;

    // Storage System Mocks
    protected Mock<IQueueServiceClientAdapter> _queueServiceAdapter;
    protected Mock<IQueueClientAdapter> _queueAdapter;

    // Mapper
    protected MapperConfiguration _mapperConfiguration = new MapperConfiguration(config =>
    {
      config.AddProfile(new QueueMapper());
    });

    public TestBase()
    {
      _fakeDtoSeederFactory = new FakeDtoSeederFactory();
    }

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext) { }

    [AssemblyCleanup]
    public static void AfterSuiteTests() { }
  }
}
