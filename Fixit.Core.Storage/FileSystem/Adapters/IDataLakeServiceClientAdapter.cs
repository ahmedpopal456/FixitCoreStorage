using System.Threading;
using System.Threading.Tasks;
using Azure;

namespace Fixit.Core.Storage.FileSystem.Adapters
{
  public interface IDataLakeServiceClientAdapter
  {
    IDataLakeFileSystemAdapter GetFileSystemClient(string fileSystemName);

    IDataLakeFileSystemAdapter CreateFileSystem(string fileSystemName);

    Task<IDataLakeFileSystemAdapter> CreateFileSystemAsync(string fileSystemName, CancellationToken cancellationToken);

    Response DeleteFileSystem(string fileSystemName);

    Task<Response> DeleteFileSystemAsync(string fileSystemName, CancellationToken cancellationToken);
  }
}