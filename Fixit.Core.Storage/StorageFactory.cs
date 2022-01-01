using System;
using AutoMapper;
using Azure.Storage.Queues;
using Fixit.Core.Storage.Storage.Queue.Adapters.Internal;
using Fixit.Core.Storage.Storage.Queue.Mappers;
using Fixit.Core.Storage.Storage.Queue.Mediators;
using Fixit.Core.Storage.Storage.Queue.Mediators.Internal;
using Microsoft.Extensions.Configuration;

namespace Fixit.Core.Storage
{
  public class StorageFactory
  {
    private readonly string _queueConnectionString;

    public StorageFactory(IConfiguration configuration)
    {
      string queueConnectionString = configuration["FIXIT-SA-QUEUE-CS"];

      if (string.IsNullOrWhiteSpace(queueConnectionString))
      {
        throw new ArgumentNullException($"{nameof(StorageFactory)} expects a valid value for {nameof(queueConnectionString)} within the configuration file");
      }

      _queueConnectionString = queueConnectionString;
    }

    public StorageFactory(string queueConnectionString)
    {
      if (string.IsNullOrWhiteSpace(queueConnectionString))
      {
        throw new ArgumentNullException($"{nameof(StorageFactory)} expects a valid value for {nameof(queueConnectionString)}");
      }

      _queueConnectionString = queueConnectionString;
    }

    public IQueueServiceClientMediator CreateQueueServiceClientMediator()
    {
      var mapperConfig = new MapperConfiguration(mc =>
      {
        mc.AddProfile(new QueueMapper());
      });

      QueueClientOptions queueClientOptions = new QueueClientOptions()
      {
        MessageEncoding = QueueMessageEncoding.Base64
      };

      QueueServiceClient queueServiceClient = new QueueServiceClient(_queueConnectionString, queueClientOptions);
      QueueServiceClientAdapter queueServiceAdapter = new QueueServiceClientAdapter(queueServiceClient);
      return new QueueServiceClientMediator(queueServiceAdapter, mapperConfig.CreateMapper());
    }
  }
}
