using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.DataLake;

namespace Fixit.Core.Storage.FileSystem.Adapters.Internal
{
  internal class DataLakeFileSystemAdapter : IDataLakeFileSystemAdapter
  {
    private readonly DataLakeFileSystemClient _dataLakeFileSystemClient;

    public DataLakeFileSystemAdapter(DataLakeFileSystemClient dataLakeFileSystemClient)
    {
      _dataLakeFileSystemClient = dataLakeFileSystemClient ?? throw new ArgumentNullException($"{nameof(DataLakeDirectoryClientAdapter)} expects a value for {nameof(dataLakeFileSystemClient)}... null argument was provided");
    }

    public IDataLakeDirectoryClientAdapter GetDirectoryClient(string directoryName)
    {
      return new DataLakeDirectoryClientAdapter(_dataLakeFileSystemClient.GetDirectoryClient(directoryName)) as IDataLakeDirectoryClientAdapter;
    }

    public IDataLakeFileClientAdapter GetFileClient(string fileName)
    {
      return new DataLakeFileClientAdapter(_dataLakeFileSystemClient.GetFileClient(fileName)) as IDataLakeFileClientAdapter; 
    }

    public Response<bool> Exists()
    {
      return _dataLakeFileSystemClient.Exists();
    }

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken)
    {
      return _dataLakeFileSystemClient.ExistsAsync(cancellationToken);
    }
  }
}