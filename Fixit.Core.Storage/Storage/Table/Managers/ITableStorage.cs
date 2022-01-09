using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.TableEntities;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Managers
{
  public interface ITableStorage
  {
    IEnumerable<T> GetEntitiesByFilter<T>(Expression<Func<T, bool>> expression) where T : TableEntity, new();

    Task<IEnumerable<T>> GetEntitiesByFilterAsync<T>(Expression<Func<T, bool>> expression) where T : TableEntity, new();

    Task<(IList<T> results, TableContinuationToken tableQuerySegment)> GetEntitiesByFilterAsync<T>(Expression<Func<T, bool>> expression, int count, TableContinuationToken? tableContinuationToken = null) where T : TableEntity, new();

    Task<IEnumerable<TableFileEntity>> GetEntitiesByPathAsync(string iPartitionKey, string iPathPrefix, CancellationToken iCancellationToken);

    IEnumerable<TableFileEntity> GetEntitiesByPath(string iPartitionKey, string iPathPrefix);

    Task<IEnumerable<TableFileEntity>> GetEntitiesLikePathAsync(string iPartitionKey, string iPathPrefix, CancellationToken iCancellationToken);

    IEnumerable<TableFileEntity> GetEntitiesLikePath(string iPartitionKey, string iPathPrefix);

    Task<IEnumerable<T>> GetEntitiesAsync<T>(string iPartitionKey, CancellationToken iCancellationToken) where T : TableEntity, new();

    IEnumerable<T> GetEntities<T>(string iPartitionKey) where T : TableEntity, new();

    Task<T> GetEntityAsync<T>(string iPartitionKey, string iRowKey, CancellationToken iCancellationToken) where T : ITableEntity;

    T GetEntity<T>(string iPartitionKey, string iRowKey) where T : ITableEntity;

    Task<OperationStatus> InsertOrReplaceEntityAsync<T>(T iTableEntity, CancellationToken iCancellationToken) where T : ITableEntity;

    OperationStatus InsertOrReplaceEntity<T>(T iTableEntity) where T : ITableEntity;

    Task<OperationStatus> DeleteEntityIfExistsAsync<T>(string partitionKey, string rowKey, CancellationToken iCancellationToken) where T : ITableEntity;

    OperationStatus DeleteEntityIfExists<T>(string partitionKey, string rowKey) where T : ITableEntity;
  }
}
