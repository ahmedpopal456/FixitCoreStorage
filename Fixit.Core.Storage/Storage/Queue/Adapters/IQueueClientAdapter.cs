using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Queues.Models;

namespace Fixit.Core.Storage.Storage.Queue.Adapters
{
  public interface IQueueClientAdapter
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueueMessage> ReceiveMessageAsync(TimeSpan? visibilityTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxMessages"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueueMessage[]> ReceiveMessagesAsync(int? maxMessages, TimeSpan? visibilityTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageText"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="timeToLive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response<SendReceipt>> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="timeToLive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response<SendReceipt>> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout, TimeSpan? timeToLive, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="messageText"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response<UpdateReceipt>> UpdateMessageAsync(string messageId, string popReceipt, string messageText, TimeSpan visibilityTimeout, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="message"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response<UpdateReceipt>> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan visibilityTimeout, CancellationToken cancellationToken);
  }
}
