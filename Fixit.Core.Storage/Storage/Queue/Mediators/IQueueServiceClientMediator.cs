using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.Storage.Queue.Mediators
{
  public interface IQueueServiceClientMediator
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="metadata"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IQueueClientMediator> CreateQueueAsync(string queueName, IDictionary<string, string> metadata = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteQueueAsync(string queueName, CancellationToken cancellationToken = default);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName"></param>
    /// <returns></returns>
    IQueueClientMediator GetQueueClient(string queueName);
  }
}
