using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage;
using Azure.Storage.Files.DataLake;

namespace Fixit.Core.Storage.FileSystem.Adapters.Internal
{
  internal class DataLakeServiceClientAdapter : IDataLakeServiceClientAdapter
  {
    private Azure.Storage.Files.DataLake.DataLakeServiceClient _dataLakeServiceClient;

    public DataLakeServiceClientAdapter(Uri uri, StorageSharedKeyCredential storageSharedKeyCredential)
    {
      if (uri == null)
      {
        throw new ArgumentNullException($"{nameof(DataLakeServiceClientAdapter)} expects a value for {nameof(uri)}... null argument was provided");
      }

      if (storageSharedKeyCredential == null)
      {
        throw new ArgumentNullException($"{nameof(DataLakeServiceClientAdapter)} expects a value for {nameof(storageSharedKeyCredential)}... null argument was provided");
      }

      _dataLakeServiceClient = new DataLakeServiceClient(uri,storageSharedKeyCredential);
    }

    public IDataLakeFileSystemAdapter GetFileSystemClient(string fileSystemName)
    {
      var wResult = default(IDataLakeFileSystemAdapter);

      var wFileSystem = _dataLakeServiceClient.GetFileSystemClient(fileSystemName);

      if (wFileSystem.Exists())
      {
        wResult = new DataLakeFileSystemAdapter(_dataLakeServiceClient.GetFileSystemClient(fileSystemName)) as IDataLakeFileSystemAdapter;
      }

      return wResult;
    }

    public IDataLakeFileSystemAdapter CreateFileSystem(string fileSystemName)
    {
      return new DataLakeFileSystemAdapter(_dataLakeServiceClient.CreateFileSystem(fileSystemName).Value) as IDataLakeFileSystemAdapter;
    }

    public async Task<IDataLakeFileSystemAdapter> CreateFileSystemAsync(string fileSystemName, CancellationToken cancellationToken)
    {
      return new DataLakeFileSystemAdapter(await _dataLakeServiceClient.CreateFileSystemAsync(fileSystemName, cancellationToken:cancellationToken)) as IDataLakeFileSystemAdapter;
    }

    public Response DeleteFileSystem(string fileSystemName)
    {
      return _dataLakeServiceClient.DeleteFileSystem(fileSystemName);
    }

    public Task<Response> DeleteFileSystemAsync(string fileSystemName, CancellationToken cancellationToken)
    {
      return _dataLakeServiceClient.DeleteFileSystemAsync(fileSystemName, cancellationToken:cancellationToken);
    }
  }
}