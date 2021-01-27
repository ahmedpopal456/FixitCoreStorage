using System;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.Queue.Mediators
{
  public interface IQueueClientMediator
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueueMessageResponseDto> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxMessages"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<QueueMessagesResponseDto> ReceiveMessagesAsync(int? maxMessages = default, TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageText"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="timeToLive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="timeToLive"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="messageText"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, string messageText = default, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="message"></param>
    /// <param name="visibilityTimeout"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default);
  }
}
