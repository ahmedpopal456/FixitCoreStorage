using System;
using AutoMapper;
using Fixit.Core.Storage.Queue.Adapters.Internal;
using Fixit.Core.Storage.Queue.Mappers;
using Fixit.Core.Storage.Queue.Mediators;
using Fixit.Core.Storage.Queue.Mediators.Internal;
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

      QueueServiceClientAdapter queueServiceAdapter = new QueueServiceClientAdapter(_queueConnectionString);
      return new QueueServiceClientMediator(queueServiceAdapter, mapperConfig.CreateMapper());
    }
  }
}
