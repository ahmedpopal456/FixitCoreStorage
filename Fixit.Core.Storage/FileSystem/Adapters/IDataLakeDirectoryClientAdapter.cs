using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace Fixit.Core.Storage.FileSystem.Adapters
{
  public interface IDataLakeDirectoryClientAdapter
  {
    public Response<PathInfo> CreateIfNotExists();

    public Task<Response<PathInfo>> CreateIfNotExistsAsync(CancellationToken cancellationToken);

    public Response<bool> DeleteIfExists();

    public Task<Response<bool>> DeleteIfExistsAsync(CancellationToken cancellationToken);

    public Response<PathInfo> Create();

    public Task<Response<PathInfo>> CreateAsync(CancellationToken cancellationToken);

    public Response Delete();

    public Task<Response> DeleteAsync(CancellationToken cancellationToken);

    public Response<DataLakeDirectoryClient> Rename(string destinationPath);

    public Task<Response<DataLakeDirectoryClient>> RenameAsync(string destinationPath, CancellationToken cancellationToken);

    public Response<bool> Exists();

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken);
  }
}