using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace Fixit.Core.Storage.FileSystem.Adapters.Internal
{
  internal class DataLakeDirectoryClientAdapter  : IDataLakeDirectoryClientAdapter
  {
    private readonly DataLakeDirectoryClient _dataLakeDirectoryClient;

    public DataLakeDirectoryClientAdapter(DataLakeDirectoryClient iDataLakeDirectoryClient)
    {
      _dataLakeDirectoryClient = iDataLakeDirectoryClient ?? throw new ArgumentNullException($"{nameof(DataLakeDirectoryClientAdapter)} expects a value for {nameof(iDataLakeDirectoryClient)}... null argument was provided");
    }

    public Response<PathInfo> CreateIfNotExists()
    {
      return _dataLakeDirectoryClient.CreateIfNotExists();
    }

    public Task<Response<PathInfo>> CreateIfNotExistsAsync(CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.CreateIfNotExistsAsync(cancellationToken:cancellationToken);
    }

    public Response<bool> DeleteIfExists()
    {
      return _dataLakeDirectoryClient.DeleteIfExists();
    }

    public Task<Response<bool>> DeleteIfExistsAsync(CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public Response<PathInfo> Create()
    {
      return _dataLakeDirectoryClient.Create();
    }

    public Task<Response<PathInfo>> CreateAsync(CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.CreateAsync(cancellationToken:cancellationToken);
    }

    public Response Delete()
    {
      return _dataLakeDirectoryClient.Delete();
    }

    public Task<Response> DeleteAsync(CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.DeleteAsync(cancellationToken:cancellationToken);
    }

    public Response<DataLakeDirectoryClient> Rename(string destinationPath)
    {
      return _dataLakeDirectoryClient.Rename(destinationPath);
    }

    public Task<Response<DataLakeDirectoryClient>> RenameAsync(string destinationPath, CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.RenameAsync(destinationPath, cancellationToken: cancellationToken);
    }

    public Response<bool> Exists()
    {
      return _dataLakeDirectoryClient.Exists();
    }

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken)
    {
      return _dataLakeDirectoryClient.ExistsAsync(cancellationToken);
    }
  }
}