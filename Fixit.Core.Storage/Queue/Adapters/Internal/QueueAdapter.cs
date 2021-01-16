using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace Fixit.Core.Storage.Queue.Adapters.Internal
{
  internal class QueueAdapter : IQueueAdapter
  {
    private QueueClient _queueClient;

    public QueueAdapter(QueueClient queueClient)
    {
      _queueClient = queueClient ?? throw new ArgumentNullException($"{nameof(QueueAdapter)} expects a value for {nameof(queueClient)}... null argument was provided");
    }

    public async Task<int> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken)
    {
      return (await _queueClient.DeleteMessageAsync(messageId, popReceipt, cancellationToken)).Status;
    }

    public async Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
    {
      return await _queueClient.ReceiveMessageAsync(visibilityTimeout, cancellationToken);
    }

    public async Task<QueueMessage[]> ReceiveMessagesAsync(int? maxMessages, TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
    {
      return await _queueClient.ReceiveMessagesAsync(maxMessages, visibilityTimeout, cancellationToken);
    }

    public async Task<Response<SendReceipt>> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await _queueClient.SendMessageAsync(messageText, visibilityTimeout, timeToLive, cancellationToken);
    }

    public async Task<Response<SendReceipt>> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await _queueClient.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);
    }

    public async Task<Response<UpdateReceipt>> UpdateMessageAsync(string messageId, string popReceipt, string messageText, TimeSpan visibilityTimeout, CancellationToken cancellationToken)
    {
      return await _queueClient.UpdateMessageAsync(messageId, popReceipt, messageText, visibilityTimeout, cancellationToken);
    }

    public async Task<Response<UpdateReceipt>> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan visibilityTimeout, CancellationToken cancellationToken)
    {
      return await _queueClient.UpdateMessageAsync(messageId, popReceipt, message, visibilityTimeout, cancellationToken);
    }
  }
}
