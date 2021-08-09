using System;
using AutoMapper;
using Azure.Storage;
using Azure.Storage.Queues;
using Fixit.Core.Storage.Storage.Blob.Adapters;
using Fixit.Core.Storage.Storage.Blob.Adapters.Internal;
using Fixit.Core.Storage.Storage.Queue.Adapters.Internal;
using Fixit.Core.Storage.Storage.Queue.Mappers;
using Fixit.Core.Storage.Storage.Queue.Mediators;
using Fixit.Core.Storage.Storage.Queue.Mediators.Internal;
using Fixit.Core.Storage.Storage.Table.Adapters;
using Fixit.Core.Storage.Storage.Table.Adapters.Internal;
using Fixit.Core.Storage.Storage.Table.Managers;
using Fixit.Core.Storage.Storage.Table.Managers.Internal;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Extensions.Configuration;

namespace Fixit.Core.Storage.Storage
{
	public class AzureStorageFactory : IStorageFactory
  {
    private readonly string _accountName;
    private readonly string _accountKey;
    private readonly string _endpoint;


    private readonly IMapper _mapper;

    private ITableServiceClient TableStorage { get; set; }

    public AzureStorageFactory(IMapper mapper,
                               string accountName,
                               string accountKey,
                               string storageEndpoint)
    {
      if (String.IsNullOrWhiteSpace(accountName))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects a value for {nameof(accountName)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(accountKey))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects a value for {nameof(accountKey)}... null argument was provided");
      }


      if (String.IsNullOrWhiteSpace(storageEndpoint))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects a value for {nameof(storageEndpoint)}... null argument was provided");
      }

      _accountKey = accountKey;
      _accountName = accountName;
      _endpoint = storageEndpoint;

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    public AzureStorageFactory(IConfiguration configurationProvider,
                               IMapper mapper)
    {
      var accountName = configurationProvider["FIXIT-SA-AN"];
      var accountKey = configurationProvider["FIXIT-SA-AK"];
      var endpoint = configurationProvider["FIXIT-SA-EP"];


      if (string.IsNullOrWhiteSpace(accountName))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects the {nameof(configurationProvider)} to have defined the Storage Account Name as {{FIXIT-SA-AN}} ");
      }

      if (string.IsNullOrWhiteSpace(accountKey))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects the {nameof(configurationProvider)} to have defined the Storage Account Key as {{FIXIT-SA-AK}} ");
      }

      if (string.IsNullOrWhiteSpace(endpoint))
      {
        throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects the {nameof(configurationProvider)} to have defined the Storage Endpoint as {{FIXIT-SA-EP}} ");
      }

      _accountName = accountName;
      _accountKey = accountKey;
      _endpoint = endpoint;

      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(AzureStorageFactory)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    #region Client Creation

    public ITableServiceClient CreateTableStorageClient()
    {
      if (TableStorage == null)
      {
        var tableStorageUri = new Uri($"https://{_accountName}.table.{_endpoint}");

        var storageCredentials = new Microsoft.Azure.Cosmos.Table.StorageCredentials(_accountName, _accountKey);
        var cloudTableClientAdapter = (ITableStorageServiceClientAdapter) new TableStorageServiceClientAdapter(tableStorageUri, storageCredentials);

        TableStorage = new TableStorageServiceClientManager(cloudTableClientAdapter);
      }

      return TableStorage;
    }

    public IBlobStorageServiceClientAdapter CreateBlobStorageClient()
    {
      var blobStorageUri = new Uri($"https://{_accountName}.blob.{_endpoint}");
      var storageCredentials = new StorageCredentials(_accountName, _accountKey);

      return new BlobStorageServiceClientAdapter(blobStorageUri, storageCredentials);
    }


    public IQueueServiceClientMediator CreateQueueServiceClient()
    {
      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new QueueMapper());
      });

      var blobStorageUri = new Uri($"https://{_accountName}.queue.{_endpoint}");
      var storageCredentials = new StorageSharedKeyCredential(_accountName, _accountKey);

      QueueServiceClient queueServiceClient = new QueueServiceClient(blobStorageUri, storageCredentials, null);
      QueueServiceClientAdapter queueServiceAdapter = new QueueServiceClientAdapter(queueServiceClient);
      return new QueueServiceClientMediator(queueServiceAdapter, mapperConfig.CreateMapper());
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      TableStorage?.Dispose();
    }

    #endregion

  }
}
