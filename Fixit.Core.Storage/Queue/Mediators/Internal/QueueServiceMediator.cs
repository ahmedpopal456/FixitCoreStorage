using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.Queue.Adapters;
using Fixit.Core.Storage.Queue.Helpers;

namespace Fixit.Core.Storage.Queue.Mediators.Internal
{
  internal class QueueServiceMediator : IQueueServiceMediator
  {
    private IQueueServiceAdapter _queueServiceAdapter;
    private IMapper _mapper;
    private OperationStatusExceptionDecorator _decorator;

    public QueueServiceMediator(IQueueServiceAdapter queueServiceAdapter, IMapper mapper)
    {
      _queueServiceAdapter = queueServiceAdapter ?? throw new ArgumentNullException($"{nameof(QueueServiceMediator)} expects a value for {nameof(queueServiceAdapter)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(QueueServiceMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _decorator = new OperationStatusExceptionDecorator();
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

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(DeleteQueueAsync)} expects a valid value for {nameof(queueName)}");
      }
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync(result, async () => {
        var statusCode = await _queueServiceAdapter.DeleteQueueAsync(queueName, cancellationToken);

        result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(statusCode);
        result.OperationMessage = statusCode.ToString();
      });
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
