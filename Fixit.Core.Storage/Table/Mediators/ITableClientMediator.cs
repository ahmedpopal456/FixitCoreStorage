using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Table.Mediators
{
  public interface ITableClientMediator
  {
    Task<OperationStatus> DeleteEntityIfExistsAsync<T>(T tableEntity, CancellationToken cancellationToken) where T : ITableEntity;

    Task<IEnumerable<T>> GetEntitiesAsync<T>(string partitionKey, CancellationToken cancellationToken) where T : ITableEntity;

    Task<T> GetEntityAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken) where T : ITableEntity;

    Task<OperationStatus> InsertOrReplaceEntityAsync<T>(T tableEntity, CancellationToken cancellationToken) where T : ITableEntity;
  }
}
