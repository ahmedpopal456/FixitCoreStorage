using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Table.Adapters
{
  public interface ITableClientAdapter
  {
    public Task CreateIfNotExistsAsync(CancellationToken cancellationToken);

    public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken);

    public Task<TableResult> ExecuteAsync(TableOperation operation, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(CancellationToken cancellationToken);
  }
}
