using Fixit.Core.DataContracts.Seeders;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.UnitTests.FakeDataProviders
{
  public class FakeDtoSeederFactory : IFakeSeederFactory
  {
    public IFakeSeederAdapter<T> CreateFakeSeeder<T>() where T : class
    {
      string type = typeof(T).Name;

      switch (type)
      {
        case nameof(QueueMessageDto):
          return (IFakeSeederAdapter<T>)new FakeMessageDtoSeeder();
        default:
          return null;
      }
    }
  }
}
