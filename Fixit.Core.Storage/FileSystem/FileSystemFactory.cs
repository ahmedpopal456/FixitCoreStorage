using System;
using AutoMapper;
using Azure.Storage;
using Fixit.Core.Storage.FileSystem.Adapters;
using Fixit.Core.Storage.FileSystem.Adapters.Internal;
using Fixit.Core.Storage.FileSystem.Managers;
using Fixit.Core.Storage.Storage;
using Microsoft.Extensions.Configuration;
using static Fixit.Core.Storage.FileSystem.Resolvers.FileSystemResolvers;
using DataLakeServiceClientManager = Fixit.Core.Storage.FileSystem.Managers.Internal.DataLakeServiceClientManager;

namespace Fixit.Core.Storage.FileSystem
{
  public class FileSystemFactory : IFileSystemFactory
  {
    private readonly string _accountName;
    private readonly string _accountKey;
    private readonly string _endpoint;

    private readonly IMapper _mapper;
    private readonly IStorageFactory _storageFactory;

    private IFileSystemServiceClient _fileServiceClient { get; set; }

    public FileSystemFactory(IMapper mapper,
                             IStorageFactory storageFactory,
                             string accountName,
                             string accountKey,
                             string endpoint)
    {
      if(String.IsNullOrWhiteSpace(accountName))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(accountName)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(accountKey))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(accountKey)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(endpoint))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(endpoint)}... null argument was provided");
      }

      _accountKey = accountKey;
      _accountName = accountName;
      _endpoint = endpoint;

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(mapper)}... null argument was provided");
      _storageFactory = storageFactory ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(storageFactory)}... null argument was provided");
    }

    public FileSystemFactory(IConfiguration configurationProvider,
                             IMapper mapper,
                             IStorageFactory storageFactory)
    {
      var accountName = configurationProvider["FIXIT-SA-AN"];
      var accountKey = configurationProvider["FIXIT-SA-AK"];
      var endpoint = configurationProvider["FIXIT-SA-EP"];

      if (string.IsNullOrWhiteSpace(accountName))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects the {nameof(configurationProvider)} to have defined the DataLake Account Name as {{FIXIT-SA-AN}} ");
      }

      if (string.IsNullOrWhiteSpace(accountKey))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects the {nameof(configurationProvider)} to have defined the DataLake Account Key as {{FIXIT-SA-AK}} ");
      }

      if (string.IsNullOrWhiteSpace(endpoint))
      {
        throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects the {nameof(configurationProvider)} to have defined the Storage Endpoint as {{FIXIT-SA-EP}} ");
      }

      _accountName = accountName;
      _accountKey = accountKey;
      _endpoint = endpoint;

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(mapper)}... null argument was provided");
      _storageFactory = storageFactory ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(_storageFactory)}... null argument was provided");
    }

    #region Client Creation

    public IFileSystemServiceClient CreateDataLakeFileSystemServiceClient(FileSystemClientResolver fileSystemClientResolver = null)
    {
      if (_fileServiceClient == null)
      {
        var dataLakeUri = new Uri($"https://{_accountName}.dfs.{_endpoint}");
        var dataLakeSharedKeyCredential = new StorageSharedKeyCredential(_accountName, _accountKey);

        var dataLakeServiceClient = (IDataLakeServiceClientAdapter) new DataLakeServiceClientAdapter(dataLakeUri, dataLakeSharedKeyCredential);

        _fileServiceClient = new DataLakeServiceClientManager(_mapper, _storageFactory, dataLakeServiceClient, fileSystemClientResolver);
      }

      return _fileServiceClient; 
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      _storageFactory?.Dispose();
      _fileServiceClient?.Dispose();
    }


    #endregion
  }
}
