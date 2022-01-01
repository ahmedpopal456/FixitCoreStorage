using System;
using Fixit.Core.Storage.Storage.Blob.Adapters;
using Fixit.Core.Storage.Storage.Queue.Mediators;
using Fixit.Core.Storage.Storage.Table.Managers;

namespace Fixit.Core.Storage.Storage
{
  public interface IStorageFactory : IDisposable
  {
    /// <summary>
    /// A TableStorageClient allows you to manipulate a given table storage's entities
    /// </summary>
    /// <returns></returns>
    ITableServiceClient CreateTableStorageClient();

    /// <summary>
    /// A BlobStorageClient allows you to manipulate a given blob storage's entities
    /// </summary>
    /// <returns></returns>
    IBlobStorageServiceClientAdapter CreateBlobStorageClient();

    /// <summary>
    /// A QueueStorageClient allows you to manipulate a given queue storage's entities
    /// </summary>
    /// <returns></returns>
    IQueueServiceClientMediator CreateQueueServiceClient();
  }
}
