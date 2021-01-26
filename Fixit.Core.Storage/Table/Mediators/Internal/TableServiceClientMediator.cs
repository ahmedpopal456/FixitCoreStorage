using System;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Decorators.Exceptions;
using Fixit.Core.Storage.Table.Adapters;

namespace Fixit.Core.Storage.Table.Mediators.Internal
{
  internal class TableServiceClientMediator : ITableServiceClientMediator
  {
    private ITableServiceClientAdapter _tableServiceClientAdapter;
    private OperationStatusExceptionDecorator _decorator;

    public TableServiceClientMediator(ITableServiceClientAdapter tableServiceClientAdapter)
    {
      _tableServiceClientAdapter = tableServiceClientAdapter ?? throw new ArgumentNullException($"{nameof(TableServiceClientMediator)} expects a value for {nameof(tableServiceClientAdapter)}... null argument was provided");
      _decorator = new OperationStatusExceptionDecorator();
    }

    public async Task<ITableClientMediator> CreateTableIfNotExistsAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException($"{nameof(CreateTableIfNotExistsAsync)} expects a valid value for {nameof(tableName)}");
      }
      TableClientMediator tableClientMediator = default(TableClientMediator);

      var tableClientAdapter = _tableServiceClientAdapter.GetTableReference(tableName);
      await tableClientAdapter.CreateIfNotExistsAsync(cancellationToken);
      tableClientMediator = new TableClientMediator(tableClientAdapter);

      return tableClientMediator;
    }

    public async Task<OperationStatus> DeleteTableIfExistsAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException($"{nameof(DeleteTableIfExistsAsync)} expects a valid value for {nameof(tableName)}");
      }
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync(result, async () =>
      {
        var tableClientAdapter = _tableServiceClientAdapter.GetTableReference(tableName);
        bool isDeleteSuccessful = await tableClientAdapter.DeleteIfExistsAsync(cancellationToken);
        result.IsOperationSuccessful = isDeleteSuccessful;
      });
      return result;
    }

    public async Task<ITableClientMediator> GetTableAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException($"{nameof(GetTableAsync)} expects a valid value for {nameof(tableName)}");
      }
      TableClientMediator tableClientMediator = default(TableClientMediator);

      var tableClientAdapter = _tableServiceClientAdapter.GetTableReference(tableName);

      if (await tableClientAdapter.ExistsAsync(cancellationToken))
      {
        tableClientMediator = new TableClientMediator(tableClientAdapter);
      }
      return tableClientMediator;
    }
  }
}
