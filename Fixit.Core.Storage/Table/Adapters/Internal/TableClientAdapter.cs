using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Table.Adapters.Internal
{
  internal class TableClientAdapter : ITableClientAdapter
  {
    private CloudTable _cloudTable;

    public TableClientAdapter(CloudTable cloudTable)
    {
      _cloudTable = cloudTable ?? throw new ArgumentNullException($"{nameof(TableClientAdapter)} expects a value for {nameof(cloudTable)}... null argument was provided");
    }

    public Task CreateIfNotExistsAsync(CancellationToken cancellationToken)
    {
      return _cloudTable.CreateIfNotExistsAsync(cancellationToken);
    }

    public Task<bool> DeleteIfExistsAsync(CancellationToken cancellationToken)
    {
      return _cloudTable.DeleteIfExistsAsync(cancellationToken);
    }

    public Task<TableResult> ExecuteAsync(TableOperation operation, CancellationToken cancellationToken)
    {
      return _cloudTable.ExecuteAsync(operation, cancellationToken);
    }

    public Task<bool> ExistsAsync(CancellationToken cancellationToken)
    {
      return _cloudTable.ExistsAsync(cancellationToken);
    }
  }
}
