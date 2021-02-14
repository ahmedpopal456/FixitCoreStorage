using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.DataContracts.Decorators.Exceptions;
using Fixit.Core.Storage.Table.Adapters;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Table.Mediators.Internal
{
  internal class TableClientMediator : ITableClientMediator
  {
    private ITableClientAdapter _tableClientAdapter;
    private OperationStatusExceptionDecorator _decorator;

    public TableClientMediator(ITableClientAdapter tableClientAdapter)
    {
      _tableClientAdapter = tableClientAdapter ?? throw new ArgumentNullException($"{nameof(TableClientMediator)} expects a value for {nameof(tableClientAdapter)}... null argument was provided");
      _decorator = new OperationStatusExceptionDecorator();
    }

    public async Task<OperationStatus> DeleteEntityIfExistsAsync<T>(T tableEntity, CancellationToken cancellationToken) where T : ITableEntity
    {
      OperationStatus result = new OperationStatus();

      var entity = await GetEntityAsync<T>(tableEntity.PartitionKey, tableEntity.RowKey, cancellationToken);
      if (entity != null)
      {
        result = await _decorator.ExecuteOperationAsync(result, async () => {
          await _tableClientAdapter.ExecuteAsync(TableOperation.Delete(entity), cancellationToken);
          result.IsOperationSuccessful = true;
        });
      }
      return result;
    }

    public Task<IEnumerable<T>> GetEntitiesAsync<T>(string partitionKey, CancellationToken cancellationToken) where T : ITableEntity
    {
      throw new NotImplementedException();
    }

    public async Task<T> GetEntityAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken) where T : ITableEntity
    {
      T result = default(T);
      TableOperation operation = TableOperation.Retrieve(partitionKey, rowKey);
      result = (T) (await _tableClientAdapter.ExecuteAsync(operation, cancellationToken)).Result;
      return result;
    }

    public async Task<OperationStatus> InsertOrReplaceEntityAsync<T>(T tableEntity, CancellationToken cancellationToken) where T : ITableEntity
    {
      OperationStatus result = new OperationStatus();

      result = await _decorator.ExecuteOperationAsync(result, async () => {
        await _tableClientAdapter.ExecuteAsync(TableOperation.InsertOrReplace(tableEntity), cancellationToken);
        result.IsOperationSuccessful = true;
      });
      return result;
    }
  }
}
