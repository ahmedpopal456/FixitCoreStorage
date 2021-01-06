using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;

namespace Fixit.Core.Storage.Queue.Adapters.Internal
{
  internal class QueueServiceAdapter : IQueueServiceAdapter
  {
    private QueueServiceClient _queueServiceClient;

    public QueueServiceAdapter(QueueServiceClient queueServiceClient)
    {
      _queueServiceClient = queueServiceClient ?? throw new ArgumentNullException($"{nameof(QueueServiceAdapter)} expects a value for {nameof(queueServiceClient)}... null argument was provided");
    }

    public async Task<IQueueAdapter> CreateQueueAsync(string queueName, IDictionary<string, string> metadata, CancellationToken cancellationToken)
    {
      return new QueueAdapter(await _queueServiceClient.CreateQueueAsync(queueName, metadata, cancellationToken));
    }

    public async Task<HttpStatusCode> DeleteQueueAsync(string queueName, CancellationToken cancellationToken)
    {
      return (HttpStatusCode) (await _queueServiceClient.DeleteQueueAsync(queueName, cancellationToken)).Status;
    }

    public IQueueAdapter GetQueueClient(string queueName)
    {
      return new QueueAdapter(_queueServiceClient.GetQueueClient(queueName));
    }
  }
}
