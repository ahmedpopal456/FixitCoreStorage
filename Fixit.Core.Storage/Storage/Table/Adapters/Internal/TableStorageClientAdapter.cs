using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Adapters.Internal
{
  public class TableStorageClientAdapter : ITableStorageClientAdapter
  {
    private readonly CloudTable _cloudTable;

    public TableStorageClientAdapter(CloudTable cloudTable)
    {
      _cloudTable = cloudTable ?? throw new ArgumentNullException($"{nameof(TableStorageClientAdapter)} expects a value for {nameof(cloudTable)}... null argument was provided");
    }

    public Task CreateAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken())
    {
      return _cloudTable.CreateAsync(requestOptions, operationContext, cancellationToken: cancellationToken);
    }

    public TableQuery<T> CreateQuery<T>() where T : TableEntity, new()
    {
      return _cloudTable.CreateQuery<T>();
    }

    public Task<bool> DeleteIfExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken)
    {
      return _cloudTable.DeleteIfExistsAsync(requestOptions, operationContext, cancellationToken);
    }

    public Task<bool> ExistsAsync(TableRequestOptions requestOptions, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken())
    {
      return _cloudTable.ExistsAsync(requestOptions, operationContext, cancellationToken);
    }

    public Task<TableResult> ExecuteAsync(TableOperation operation)
    {
      return _cloudTable.ExecuteAsync(operation);
    }

    public Task<TableQuerySegment<T>> ExecuteQuerySegmentedAsync<T>(TableQuery<T> query, TableContinuationToken token) where T : TableEntity, new()
    {
      return _cloudTable.ExecuteQuerySegmentedAsync(query, token);
    }
  }
}
