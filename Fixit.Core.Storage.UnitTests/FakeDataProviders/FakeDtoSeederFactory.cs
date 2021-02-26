using System.Collections.Generic;
using Fixit.Core.DataContracts.Seeders;

namespace Fixit.Core.Storage.UnitTests.FakeDataProviders
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IList<T> CreateSeederFactory<T>(IFakeSeederAdapter<T> fakeSeederAdapter) where T : class
    {
      return fakeSeederAdapter.SeedFakeDtos();
    }
  }
}
