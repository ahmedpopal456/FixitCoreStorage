using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
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

    public async Task<HttpStatusCode> CreateIfNotExistsAsync(CancellationToken cancellationToken, IDictionary<string, string> metadata = null)
    {
      return (HttpStatusCode) (await _queueClient.CreateIfNotExistsAsync(metadata, cancellationToken)).Status;
    }

    public async Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
    {
      return (await _queueClient.DeleteIfExistsAsync(cancellationToken)).Value;
    }

    public async Task<HttpStatusCode> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken)
    {
      return (HttpStatusCode) (await _queueClient.DeleteMessageAsync(messageId, popReceipt, cancellationToken)).Status;
    }

    public async Task<PeekedMessage> PeekMessageAsync(CancellationToken cancellationToken)
    {
      return await _queueClient.PeekMessageAsync(cancellationToken);
    }

    public async Task<PeekedMessage[]> PeekMessagesAsync(int? maxMessages, CancellationToken cancellationToken)
    {
      return await PeekMessagesAsync(maxMessages, cancellationToken);
    }

    public async Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
    {
      return await ReceiveMessageAsync(visibilityTimeout, cancellationToken);
    }

    public async Task<QueueMessage[]> ReceiveMessagesAsync(int? maxMessages, TimeSpan? visibilityTimeout, CancellationToken cancellationToken)
    {
      return await ReceiveMessagesAsync(maxMessages, visibilityTimeout, cancellationToken);
    }

    public async Task<SendReceipt> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await SendMessageAsync(messageText, visibilityTimeout, timeToLive, cancellationToken);
    }

    public async Task<SendReceipt> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);
    }

    public async Task<UpdateReceipt> UpdateMessageAsync(string messageId, string popReceipt, string messageText, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await UpdateMessageAsync(messageId, popReceipt, messageText, visibilityTimeout, timeToLive, cancellationToken);
    }

    public async Task<UpdateReceipt> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken)
    {
      return await UpdateMessageAsync(messageId, popReceipt, message, visibilityTimeout, timeToLive, cancellationToken);
    }
  }
}
