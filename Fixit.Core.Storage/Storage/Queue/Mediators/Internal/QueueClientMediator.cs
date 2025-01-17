﻿using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Azure.Storage.Queues.Models;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Decorators.Exceptions;
using Fixit.Core.DataContracts.Decorators.Exceptions.Internals;
using Fixit.Core.Storage.DataContracts.Queue;
using Fixit.Core.Storage.Storage.Queue.Adapters;
using Fixit.Core.Storage.Storage.Queue.Helpers;

[assembly: InternalsVisibleTo("Fixit.Core.Storage.UnitTests")]
namespace Fixit.Core.Storage.Storage.Queue.Mediators.Internal
{
  internal class QueueClientMediator : IQueueClientMediator
  {
    private IQueueClientAdapter _queueAdapter;
    private IMapper _mapper;
    private IExceptionDecorator _decorator;

    public QueueClientMediator(IQueueClientAdapter queueAdapter, IMapper mapper)
    {
      _queueAdapter = queueAdapter ?? throw new ArgumentNullException($"{nameof(QueueClientMediator)} expects a value for {nameof(queueAdapter)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(QueueClientMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _decorator = new ExceptionDecorator();
    }

    public async Task<OperationStatus> DeleteMessageAsync(string messageId, string popReceipt, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      QueueValidators.ValidateMessageIdAndPopReceipt(messageId, popReceipt);
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync<OperationStatus>(true, async() => {
        var response = await _queueAdapter.DeleteMessageAsync(messageId, popReceipt, cancellationToken);
        result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(response.Status);
        result.OperationMessage = response.ReasonPhrase;
      }, result);
      return result;
    }

    public async Task<QueueMessageResponseDto> ReceiveMessageAsync(TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      QueueMessageResponseDto result = new QueueMessageResponseDto(true);

      result = await _decorator.ExecuteOperationAsync<QueueMessageResponseDto>(true, async () => {
        var message = await _queueAdapter.ReceiveMessageAsync(visibilityTimeout, cancellationToken);
        if (message != null)
        {
          result.Message = _mapper.Map<QueueMessage, QueueMessageDto>(message);
        }
      }, result);
      return result;
    }

    public async Task<QueueMessagesResponseDto> ReceiveMessagesAsync(int? maxMessages = default, TimeSpan? visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      QueueMessagesResponseDto result = new QueueMessagesResponseDto(true);

      result = await _decorator.ExecuteOperationAsync<QueueMessagesResponseDto>(true, async () => {
        var messages = await _queueAdapter.ReceiveMessagesAsync(maxMessages, visibilityTimeout, cancellationToken);
        if (messages.Length != default(int))
        {
          foreach (var message in messages)
          {
            result.Messages.Add(_mapper.Map<QueueMessage, QueueMessageDto>(message));
          }
        }
      }, result);
      return result;
    }

    public async Task<OperationStatus> SendMessageAsync(string messageText, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrEmpty(messageText))
      {
        throw new ArgumentNullException($"{nameof(SendMessageAsync)} expects a valid value for {nameof(messageText)}");
      }
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync<OperationStatus>(true, async () => {
        var response = await _queueAdapter.SendMessageAsync(messageText, visibilityTimeout, timeToLive, cancellationToken);

        if (response != null)
        {
          var statusCode = response.GetRawResponse().Status;
          result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(statusCode);
          result.OperationMessage = ((HttpStatusCode)statusCode).ToString();
        }
      }, result);
      return result;
    }

    public async Task<OperationStatus> SendMessageAsync(BinaryData message, TimeSpan? visibilityTimeout = default, TimeSpan? timeToLive = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (message == null)
      {
        throw new ArgumentNullException($"{nameof(SendMessageAsync)} expects a valid value for {nameof(message)}");
      }
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync<OperationStatus>(true, async () => {
        var response = await _queueAdapter.SendMessageAsync(message, visibilityTimeout, timeToLive, cancellationToken);

        if (response != null)
        {
          var statusCode = response.GetRawResponse().Status;
          result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(statusCode);
          result.OperationMessage = ((HttpStatusCode)statusCode).ToString();
        }
      }, result);
      return result;
    }

    public async Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, string messageText = default, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      QueueValidators.ValidateMessageIdAndPopReceipt(messageId, popReceipt);
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync<OperationStatus>(true, async () => {
        var response = await _queueAdapter.UpdateMessageAsync(messageId, popReceipt, messageText, visibilityTimeout, cancellationToken);

        if (response != null)
        {
          var statusCode = response.GetRawResponse().Status;
          result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(statusCode);
          result.OperationMessage = ((HttpStatusCode)statusCode).ToString();
        }
      }, result);
      return result;
    }

    public async Task<OperationStatus> UpdateMessageAsync(string messageId, string popReceipt, BinaryData message, TimeSpan visibilityTimeout = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      QueueValidators.ValidateMessageIdAndPopReceipt(messageId, popReceipt);
      if (message == null)
      {
        throw new ArgumentNullException($"{nameof(UpdateMessageAsync)} expects a valid value for {nameof(message)}");
      }

      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync<OperationStatus>(true, async () => {
        var response = await _queueAdapter.UpdateMessageAsync(messageId, popReceipt, message, visibilityTimeout, cancellationToken);

        if (response != null)
        {
          var statusCode = response.GetRawResponse().Status;
          result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(statusCode);
          result.OperationMessage = ((HttpStatusCode)statusCode).ToString();
        }
      }, result);
      return result;
    }
  }
}
