using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Decorators.Exceptions.Internals;
using Fixit.Core.Storage.Storage.Queue.Adapters;
using Fixit.Core.Storage.Storage.Queue.Helpers;

namespace Fixit.Core.Storage.Storage.Queue.Mediators.Internal
{
  internal class QueueServiceClientMediator : IQueueServiceClientMediator
  {
    private IQueueServiceClientAdapter _queueServiceAdapter;
    private IMapper _mapper;
    private ExceptionDecorator _decorator;

    public QueueServiceClientMediator(IQueueServiceClientAdapter queueServiceAdapter, IMapper mapper)
    {
      _queueServiceAdapter = queueServiceAdapter ?? throw new ArgumentNullException($"{nameof(QueueServiceClientMediator)} expects a value for {nameof(queueServiceAdapter)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(QueueServiceClientMediator)} expects a value for {nameof(mapper)}... null argument was provided");
      _decorator = new ExceptionDecorator();
    }

    public async Task<IQueueClientMediator> CreateQueueAsync(string queueName, IDictionary<string, string> metadata = default, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(CreateQueueAsync)} expects a valid value for {nameof(queueName)}");
      }

      return new QueueClientMediator(await _queueServiceAdapter.CreateQueueAsync(queueName, metadata, cancellationToken), _mapper);
    }

    public async Task<OperationStatus> DeleteQueueAsync(string queueName, CancellationToken cancellationToken = default)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(DeleteQueueAsync)} expects a valid value for {nameof(queueName)}");
      }
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync(true, async () => {
        var response = await _queueServiceAdapter.DeleteQueueAsync(queueName, cancellationToken);
        result.IsOperationSuccessful = QueueValidators.IsSuccessStatusCode(response.Status);
        result.OperationMessage = response.ReasonPhrase;
      });
      return result;
    }

    public IQueueClientMediator GetQueueClient(string queueName)
    {
      if (string.IsNullOrWhiteSpace(queueName))
      {
        throw new ArgumentNullException($"{nameof(GetQueueClient)} expects a valid value for {nameof(queueName)}");
      }

      return new QueueClientMediator(_queueServiceAdapter.GetQueueClient(queueName), _mapper);
    }
  }
}
