using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Adapters
{
  public interface ITableStorageClientAdapter
  {
    Task CreateAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken());

    TableQuery<T> CreateQuery<T>() where T : TableEntity, new();

    Task<bool> DeleteIfExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken);
   
    Task<bool> ExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken());

    Task<TableResult> ExecuteAsync(TableOperation operation);

    Task<TableQuerySegment<T>> ExecuteQuerySegmentedAsync<T>(TableQuery<T> query, TableContinuationToken token) where T : TableEntity, new();
  }
}
