using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;

namespace Fixit.Core.Storage.Storage.Queue.Adapters
{
  public interface IQueueServiceClientAdapter
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    Task<IQueueClientAdapter> CreateQueueAsync(string queueName, IDictionary<string, string> metadata, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Response> DeleteQueueAsync(string queueName, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName"></param>
    /// <returns></returns>
    IQueueClientAdapter GetQueueClient(string queueName);
  }
}
