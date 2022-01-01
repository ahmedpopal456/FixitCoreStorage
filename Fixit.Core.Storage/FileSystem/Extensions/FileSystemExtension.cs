using Microsoft.Extensions.DependencyInjection;

namespace Fixit.Core.Storage.FileSystem.Extensions
{
  public static class FileSystemExtension
  {
    public static void AddFixitCoreFileSystemServices(this IServiceCollection services)
    {
      services.AddTransient<IFileSystemFactory, FileSystemFactory>();
    }
  }
}
