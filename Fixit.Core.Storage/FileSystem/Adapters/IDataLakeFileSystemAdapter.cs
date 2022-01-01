using System.Threading;
using System.Threading.Tasks;
using Azure;

namespace Fixit.Core.Storage.FileSystem.Adapters
{
  public interface IDataLakeFileSystemAdapter
  {
    public IDataLakeDirectoryClientAdapter GetDirectoryClient(string directoryName);

    public IDataLakeFileClientAdapter GetFileClient(string fileName);

    public Response<bool> Exists();

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken);
  }
}