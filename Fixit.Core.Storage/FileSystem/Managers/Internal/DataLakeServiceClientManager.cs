using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.Storage.FileSystem.Adapters;
using Fixit.Core.Storage.Storage;
using Fixit.Core.Storage.Storage.Blob.Adapters;
using static Fixit.Core.Storage.FileSystem.Resolvers.FileSystemResolvers;

[assembly: InternalsVisibleTo("Fixit.Core.UnitTests")]
namespace Fixit.Core.Storage.FileSystem.Managers.Internal
{
  internal class DataLakeServiceClientManager: IFileSystemServiceClient
  {
    private readonly IMapper _mapper;

    private readonly IDataLakeServiceClientAdapter _dataLakeServiceClient;
    private readonly IBlobStorageServiceClientAdapter _cloudBlobClient;
    private readonly FileSystemClientResolver _fileSystemClientResolver;

    public DataLakeServiceClientManager(IMapper mapper,
                                        IStorageFactory storageFactory,
                                        IDataLakeServiceClientAdapter dataLakeServiceClient,
                                        FileSystemClientResolver fileSystemClientResolver)
    {
      if(storageFactory == null)
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(storageFactory)}... null argument was provided");
      }

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(mapper)}... null argument was provided");
      _dataLakeServiceClient = dataLakeServiceClient ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(dataLakeServiceClient)}... null argument was provided");
      _fileSystemClientResolver = fileSystemClientResolver;

      _cloudBlobClient = storageFactory.CreateBlobStorageClient();
    }

    #region Get File System

    public IFileSystemClient GetFileSystem(string fileSystemName)
    {
      if (String.IsNullOrWhiteSpace(fileSystemName))
      {
        throw new ArgumentNullException($"{nameof(GetFileSystem)} expects a value for {nameof(fileSystemName)}... null argument was provided");
      }

      var dataLakeFileSystemClient = default(IFileSystemClient);

      var fileSystem = _dataLakeServiceClient.GetFileSystemClient(fileSystemName.ToLowerInvariant());
      var cloudContainer = _cloudBlobClient.GetContainerReference(fileSystemName);

      if (fileSystem != null)
      {
        dataLakeFileSystemClient = _fileSystemClientResolver == null ? new DataLakeFileSystemManager(fileSystem, cloudContainer, _mapper) : _fileSystemClientResolver(fileSystem, cloudContainer, _mapper);
      }

      return dataLakeFileSystemClient;
    }
    #endregion

    #region Delete File System

    public async Task<HttpStatusCode> DeleteFileSystemAsync(string fileSystemName, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(fileSystemName))
      {
        throw new ArgumentNullException($"{nameof(DeleteFileSystemAsync)} expects a value for {nameof(fileSystemName)}... null argument was provided");
      }

      var response = await _dataLakeServiceClient.DeleteFileSystemAsync(fileSystemName.ToLowerInvariant(), cancellationToken);
      
      var result = (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), response.Status);

      return result;
    }

    public HttpStatusCode DeleteFileSystem(string fileSystemName)
    {
      if (String.IsNullOrWhiteSpace(fileSystemName))
      {
        throw new ArgumentNullException($"{nameof(DeleteFileSystem)} expects a value for {nameof(fileSystemName)}... null argument was provided");
      }

      var response = _dataLakeServiceClient.DeleteFileSystem(fileSystemName.ToLowerInvariant());

      var result = (HttpStatusCode)Enum.ToObject(typeof(HttpStatusCode), response.Status);

      return result;
    }

    #endregion

    #region Create File System

    public async Task<IFileSystemClient> CreateOrGetFileSystemAsync(string fileSystemName, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(fileSystemName))
      {
        throw new ArgumentNullException($"{nameof(CreateOrGetFileSystemAsync)} expects a value for {nameof(fileSystemName)}... null argument was provided");
      }

      var dataLakeFileSystemClient = _dataLakeServiceClient.GetFileSystemClient(fileSystemName.ToLowerInvariant());

      if (dataLakeFileSystemClient == null)
      {
        dataLakeFileSystemClient = await _dataLakeServiceClient.CreateFileSystemAsync(fileSystemName.ToLowerInvariant(),cancellationToken);
      }

      var cloudContainer = _cloudBlobClient.GetContainerReference(fileSystemName);

      return _fileSystemClientResolver == null ? new DataLakeFileSystemManager(dataLakeFileSystemClient, cloudContainer, _mapper) : _fileSystemClientResolver(dataLakeFileSystemClient, cloudContainer, _mapper);
    }

    public IFileSystemClient CreateOrGetFileSystem(string fileSystemName)
    {
      if (String.IsNullOrWhiteSpace(fileSystemName))
      {
        throw new ArgumentNullException($"{nameof(CreateOrGetFileSystem)} expects a value for {nameof(fileSystemName)}... null argument was provided");
      }

      var dataLakeFileSystemClient = _dataLakeServiceClient.GetFileSystemClient(fileSystemName.ToLowerInvariant());

      if (dataLakeFileSystemClient == null)
      {
        dataLakeFileSystemClient = _dataLakeServiceClient.CreateFileSystem(fileSystemName.ToLowerInvariant());
      }

      var cloudContainer = _cloudBlobClient.GetContainerReference(fileSystemName);

      return _fileSystemClientResolver == null ? new DataLakeFileSystemManager(dataLakeFileSystemClient, cloudContainer, _mapper) : _fileSystemClientResolver(dataLakeFileSystemClient, cloudContainer, _mapper);
    }
    #endregion

    #region IDisposable

    private void ReleaseUnmanagedResources()
    {
      // TODO release unmanaged resources here
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
    }


    #endregion
  }
}
