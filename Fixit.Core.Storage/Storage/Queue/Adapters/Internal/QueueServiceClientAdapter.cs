using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;

namespace Fixit.Core.Storage.Storage.Queue.Adapters.Internal
{
  [ExcludeFromCodeCoverage]
  internal class QueueServiceClientAdapter : IQueueServiceClientAdapter
  {
    private readonly QueueServiceClient _queueServiceClient;

    public QueueServiceClientAdapter(QueueServiceClient queueServiceClient)
    {
      _queueServiceClient = queueServiceClient ?? throw new ArgumentNullException($"{nameof(QueueServiceClientAdapter)} expects a value for {nameof(queueServiceClient)}... null argument was provided");
    }

    public async Task<IQueueClientAdapter> CreateQueueAsync(string queueName, IDictionary<string, string> metadata, CancellationToken cancellationToken)
    {
      return new QueueClientAdapter(await _queueServiceClient.CreateQueueAsync(queueName, metadata, cancellationToken));
    }

    public async Task<Response> DeleteQueueAsync(string queueName, CancellationToken cancellationToken)
    {
      return await _queueServiceClient.DeleteQueueAsync(queueName, cancellationToken);
    }

    public IQueueClientAdapter GetQueueClient(string queueName)
    {
      return new QueueClientAdapter(_queueServiceClient.GetQueueClient(queueName));
    }
  }
}
