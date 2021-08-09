using Microsoft.Extensions.DependencyInjection;

namespace Fixit.Core.Storage.Storage.Extensions
{
  public static class StorageExtension
  {
    public static void AddFixitCoreStorageServices(this IServiceCollection services)
    {
      services.AddTransient<IStorageFactory, AzureStorageFactory>();
    }
  }
}
