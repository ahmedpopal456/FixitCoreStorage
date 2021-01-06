using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.Queue.Adapters;

namespace Fixit.Core.Storage.Queue.Mediators.Internal
{
  internal class QueueServiceMediator : IQueueServiceMediator
  {
    private IQueueServiceAdapter _queueServiceAdapter;
    private IMapper _mapper;

    public QueueServiceMediator(IQueueServiceAdapter queueServiceAdapter, IMapper mapper)
    {
      _queueServiceAdapter = queueServiceAdapter ?? throw new ArgumentNullException($"{nameof(QueueServiceMediator)} expects a value for {nameof(queueServiceAdapter)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(QueueServiceMediator)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    public async Task<IQueueMediator> CreateQueueAsync(string queueName, IDictionary<string, string> metadata = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(CreateQueueAsync)} expects a valid value for {nameof(queueName)}");
      }

      return new QueueMediator(await _queueServiceAdapter.CreateQueueAsync(queueName, metadata, cancellationToken), _mapper);
    }

    public async Task<OperationStatus> DeleteQueueAsync(string queueName, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();
      OperationStatus result = new OperationStatus();

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(DeleteQueueAsync)} expects a valid value for {nameof(queueName)}");
      }

      try
      {
        HttpStatusCode statusCode = await _queueServiceAdapter.DeleteQueueAsync(queueName, cancellationToken);

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

    public IQueueMediator GetQueueClient(string queueName)
    {
      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(GetQueueClient)} expects a valid value for {nameof(queueName)}");
      }

      return new QueueMediator(_queueServiceAdapter.GetQueueClient(queueName), _mapper);
    }
  }
}
