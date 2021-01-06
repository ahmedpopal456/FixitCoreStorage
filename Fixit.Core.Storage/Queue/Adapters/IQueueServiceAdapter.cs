using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fixit.Core.Storage.Queue.Adapters
{
  public interface IQueueServiceAdapter
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <param name="metadata"></param>
    /// <returns></returns>
    Task<IQueueAdapter> CreateQueueAsync(string queueName, IDictionary<string, string> metadata, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HttpStatusCode> DeleteQueueAsync(string queueName, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="queueName"></param>
    /// <returns></returns>
    IQueueAdapter GetQueueClient(string queueName);
  }
}
