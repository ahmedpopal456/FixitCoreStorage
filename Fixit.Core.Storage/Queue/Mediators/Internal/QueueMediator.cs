using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Queues.Models;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.Queue;
using Fixit.Core.Storage.Queue.Adapters;

[assembly: InternalsVisibleTo("Fixit.Core.Storage.UnitTests")]
namespace Fixit.Core.Storage.Queue.Mediators.Internal
{
  internal class QueueMediator : IQueueMediator
  {
    private IQueueAdapter _queueAdapter;
    private IMapper _mapper;

    public QueueMediator(IQueueAdapter queueAdapter, IMapper mapper)
    {
      _queueAdapter = queueAdapter ?? throw new ArgumentNullException($"{nameof(QueueMediator)} expects a value for {nameof(queueAdapter)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(QueueMediator)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    public async Task<OperationStatus> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (string.IsNullOrWhiteSpace(messageId))
      {
        throw new ArgumentNullException($"{nameof(DeleteMessageAsync)} expects a valid value for {nameof(messageId)}");
      }
      if (string.IsNullOrWhiteSpace(popReceipt))
      {
        throw new ArgumentNullException($"{nameof(DeleteMessageAsync)} expects a valid value for {nameof(popReceipt)}");
      }

      try
      {
        HttpStatusCode statusCode = await _queueAdapter.DeleteMessageAsync(messageId, popReceipt, cancellationToken);

        if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent)
        {
          result.IsOperationSuccessful = true;
        }
        result.OperationMessage = statusCode.ToString();
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<ReceivedMessageDto> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ReceivedMessageDto result = new ReceivedMessageDto() { IsOperationSuccessful = true };

      try
      {
        var message = await _queueAdapter.ReceiveMessageAsync(visibilityTimeout, cancellationToken);
        if (message != null)
        {
          result.Message = _mapper.Map<QueueMessage, MessageDto>(message);
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<ReceivedMessagesDto> ReceiveMessagesAsync(int? maxMessages = default, TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      ReceivedMessagesDto result = new ReceivedMessagesDto() { IsOperationSuccessful = true };

      try
      {
        var messages = await _queueAdapter.ReceiveMessagesAsync(maxMessages, visibilityTimeout, cancellationToken);
        if (messages.Length != default(int))
        {
          foreach (var message in messages)
          {
            result.Messages.Add(_mapper.Map<QueueMessage, MessageDto>(message));
          }
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<OperationStatus> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (string.IsNullOrEmpty(messageText))
      {
        throw new ArgumentNullException($"{nameof(SendMessageAsync)} expects a valid value for {nameof(messageText)}");
      }

      try
      {
        var response = await _queueAdapter.SendMessageAsync(messageText, visibilityTimeout, timeToLive, cancellationToken);

        if (response != null)
        {
          HttpStatusCode statusCode = (HttpStatusCode) response.GetRawResponse().Status;
          if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent)
          {
            result.IsOperationSuccessful = true;
          }
          result.OperationMessage = statusCode.ToString();
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<OperationStatus> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (message == null)
      {
        throw new ArgumentNullException($"{nameof(SendMessageAsync)} expects a valid value for {nameof(message)}");
      }

      try
      {
        var response = await _queueAdapter.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);

        if (response != null)
        {
          HttpStatusCode statusCode = (HttpStatusCode)response.GetRawResponse().Status;
          if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent)
          {
            result.IsOperationSuccessful = true;
          }
          result.OperationMessage = statusCode.ToString();
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, string messageText = default, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (string.IsNullOrWhiteSpace(messageId))
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(messageId)}");
      }
      if (string.IsNullOrWhiteSpace(popReceipt))
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(popReceipt)}");
      }

      try
      {
        var response = await _queueAdapter.UpdateMessageAsync(messageId, popReceipt, messageText, visibilityTimeout, cancellationToken);

        if (response != null)
        {
          HttpStatusCode statusCode = (HttpStatusCode)response.GetRawResponse().Status;
          if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent)
          {
            result.IsOperationSuccessful = true;
          }
          result.OperationMessage = statusCode.ToString();
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }

    public async Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (string.IsNullOrWhiteSpace(messageId))
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(messageId)}");
      }
      if (string.IsNullOrWhiteSpace(popReceipt))
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(popReceipt)}");
      }
      if (message == null)
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(message)}");
      }

      try
      {
        var response = await _queueAdapter.UpdateMessageAsync(messageId, popReceipt, message, visibilityTimeout, cancellationToken);

        if (response != null)
        {
          HttpStatusCode statusCode = (HttpStatusCode)response.GetRawResponse().Status;
          if (statusCode == HttpStatusCode.OK || statusCode == HttpStatusCode.NoContent)
          {
            result.IsOperationSuccessful = true;
          }
          result.OperationMessage = statusCode.ToString();
        }
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }

      return result;
    }
  }
}
