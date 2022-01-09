using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.Helpers;
using Fixit.Core.Storage.DataContracts.TableEntities;
using Fixit.Core.Storage.Storage.Table.Adapters;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Managers.Internal
{
  internal class TableStorageClientManager : ITableStorage
  {
    private readonly ITableStorageClientAdapter _cloudTable;

    public TableStorageClientManager(ITableStorageClientAdapter cloudTable)
    {
      _cloudTable = cloudTable ?? throw new ArgumentNullException($"{nameof(TableStorageClientManager)} expects a value for {nameof(cloudTable)}... null argument was provided");
    }

    #region Get Entity

    public async Task<T> GetEntityAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken) where T : ITableEntity
    {
      var result = default(T);

      try
      {
        TableOperation insert = TableOperation.Retrieve<T>(partitionKey, rowKey);
        var getResult = await _cloudTable.ExecuteAsync(insert);

        result = (T)getResult.Result;
      }
      catch
      {
        // Fall through
      }

      return result;
    }

    public T GetEntity<T>(string partitionKey, string rowKey) where T : ITableEntity
    {
      var result = default(T);

      try
      {
        TableOperation insert = TableOperation.Retrieve<T>(partitionKey, rowKey);
        var getResult = _cloudTable.ExecuteAsync(insert)
                      .GetAwaiter()
                      .GetResult();

        result = (T)getResult.Result;
      }
      catch
      {
        // Fall through
      }

      return result;
    }

    public async Task<IEnumerable<TableFileEntity>> GetEntitiesLikePathAsync(string partitionKey, string pathPrefix, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      List<TableFileEntity> results = new List<TableFileEntity>();

      try
      {
        var querySegment = default(TableQuerySegment<TableFileEntity>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {
          var directory = StringHelper.ToAzureDirectoryPath(Path.GetDirectoryName(pathPrefix));

          // Generate Filter for Partition Key and Folder Path
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
          var folderPathFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.GreaterThanOrEqual, directory);

          var combinedFilter = TableQuery.CombineFilters(filePartitionFilter, TableOperators.And, folderPathFilter);

          var lastCharacter = directory.Last();
          var lessThanComparisonString = directory.Remove(directory.Length - 1, 1);

          var nextAsciiValue = lastCharacter + 1;
          lessThanComparisonString = $"{lessThanComparisonString}{(char)nextAsciiValue}";

          var incrementedFolderFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.LessThan, lessThanComparisonString);
          combinedFilter = TableQuery.CombineFilters(incrementedFolderFilter, TableOperators.And, combinedFilter);

          // If file name was provided as well, then generate filter for file name 
          var fileName = Path.GetFileName(pathPrefix);

          if (!string.IsNullOrWhiteSpace(fileName))
          {
            var fileNameFilter = TableQuery.GenerateFilterCondition("FileName", QueryComparisons.Equal, fileName);

            combinedFilter = TableQuery.CombineFilters(combinedFilter, TableOperators.And, fileNameFilter);
          }


          TableQuery<TableFileEntity> query = new TableQuery<TableFileEntity>().Where(combinedFilter);

          querySegment = await _cloudTable.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public IEnumerable<TableFileEntity> GetEntitiesLikePath(string partitionKey, string pathPrefix)
    {
      List<TableFileEntity> results = new List<TableFileEntity>();

      try
      {
        var querySegment = default(TableQuerySegment<TableFileEntity>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {

          var directory = Path.GetDirectoryName(pathPrefix);

          // Generate Filter for Partition Key and Folder Path
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
          var folderPathFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.GreaterThanOrEqual, directory);

          var combinedFilter = TableQuery.CombineFilters(filePartitionFilter, TableOperators.And, folderPathFilter);

          var lastCharacter = directory.Last();
          var lessThanComparisonString = directory.Remove(directory.Length - 1, 1);

          var nextAsciiValue = lastCharacter + 1;
          lessThanComparisonString = $"{lessThanComparisonString}{(char)nextAsciiValue}";

          var wIncrementedFolderFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.LessThan, lessThanComparisonString);
          combinedFilter = TableQuery.CombineFilters(wIncrementedFolderFilter, TableOperators.And, combinedFilter);


          // If file name was provided as well, then generate filter for file name 
          var fileName = Path.GetFileName(pathPrefix);

          if (!string.IsNullOrWhiteSpace(fileName))
          {
            var fileNameFilter = TableQuery.GenerateFilterCondition("FileName", QueryComparisons.Equal, fileName);

            combinedFilter = TableQuery.CombineFilters(combinedFilter, TableOperators.And, fileNameFilter);
          }

          TableQuery<TableFileEntity> wQuery = new TableQuery<TableFileEntity>().Where(combinedFilter);

          querySegment = _cloudTable.ExecuteQuerySegmentedAsync(wQuery, querySegment?.ContinuationToken).Result;
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public IEnumerable<TableFileEntity> GetEntitiesByPath(string partitionKey, string pathPrefix)
    {
      List<TableFileEntity> results = new List<TableFileEntity>();

      try
      {
        var querySegment = default(TableQuerySegment<TableFileEntity>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {

          var directory = Path.GetDirectoryName(pathPrefix);

          // Generate Filter for Partition Key and Folder Path
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
          var folderPathFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.Equal, directory);

          var combinedFilter = TableQuery.CombineFilters(filePartitionFilter, TableOperators.And, folderPathFilter);

          // If file name was provided as well, then generate filter for file name 
          var fileName = Path.GetFileName(pathPrefix);

          if (!string.IsNullOrWhiteSpace(fileName))
          {
            var fileNameFilter = TableQuery.GenerateFilterCondition("FileName", QueryComparisons.Equal, fileName);

            combinedFilter = TableQuery.CombineFilters(combinedFilter, TableOperators.And, fileNameFilter);
          }

          TableQuery<TableFileEntity> wQuery = new TableQuery<TableFileEntity>().Where(combinedFilter);

          querySegment = _cloudTable.ExecuteQuerySegmentedAsync(wQuery, querySegment?.ContinuationToken).Result;
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public async Task<IEnumerable<TableFileEntity>> GetEntitiesByPathAsync(string partitionKey, string pathPrefix, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      List<TableFileEntity> results = new List<TableFileEntity>();

      try
      {
        var querySegment = default(TableQuerySegment<TableFileEntity>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {

          var directory = StringHelper.ToAzureDirectoryPath(Path.GetDirectoryName(pathPrefix));

          // Generate Filter for Partition Key and Folder Path
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
          var folderPathFilter = TableQuery.GenerateFilterCondition("FolderPath", QueryComparisons.Equal, directory);

          var combinedFilter = TableQuery.CombineFilters(filePartitionFilter, TableOperators.And, folderPathFilter);

          // If file name was provided as well, then generate filter for file name 
          var fileName = Path.GetFileName(pathPrefix);

          if (!string.IsNullOrWhiteSpace(fileName))
          {
            var fileNameFilter = TableQuery.GenerateFilterCondition("FileName", QueryComparisons.Equal, fileName);

            combinedFilter = TableQuery.CombineFilters(combinedFilter, TableOperators.And, fileNameFilter);
          }

          TableQuery<TableFileEntity> query = new TableQuery<TableFileEntity>().Where(combinedFilter);

          querySegment = await _cloudTable.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public async Task<IEnumerable<T>> GetEntitiesAsync<T>(string partitionKey, CancellationToken cancellationToken) where T : TableEntity, new()
    {
      List<T> results = new List<T>();

      try
      {
        var querySegment = default(TableQuerySegment<T>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

          TableQuery<T> wQuery = new TableQuery<T>().Where(filePartitionFilter);

          querySegment = await _cloudTable.ExecuteQuerySegmentedAsync(wQuery, querySegment?.ContinuationToken);
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public IEnumerable<T> GetEntities<T>(string partitionKey) where T : TableEntity, new()
    {
      List<T> results = new List<T>();

      try
      {
        var querySegment = default(TableQuerySegment<T>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {
          var filePartitionFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

          TableQuery<T> wQuery = new TableQuery<T>().Where(filePartitionFilter);

          querySegment = _cloudTable.ExecuteQuerySegmentedAsync(wQuery, querySegment?.ContinuationToken).Result;
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public IEnumerable<T> GetEntitiesByFilter<T>(Expression<Func<T, bool>> expression) where T : TableEntity, new()
    {
      List<T> results = new List<T>();

      try
      {
        var querySegment = default(TableQuerySegment<T>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {
          var query = _cloudTable.CreateQuery<T>().Where(expression) as TableQuery<T>;

          querySegment = _cloudTable.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken).Result;
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public async Task<IEnumerable<T>> GetEntitiesByFilterAsync<T>(Expression<Func<T, bool>> expression) where T : TableEntity, new()
    {
      List<T> results = new List<T>();

      try
      {
        var querySegment = default(TableQuerySegment<T>);

        while (querySegment == null || querySegment.ContinuationToken != null)
        {
          var query = _cloudTable.CreateQuery<T>().Where(expression) as TableQuery<T>;

          querySegment = await _cloudTable.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
          results.AddRange(querySegment);
        }
      }
      catch
      {
        // Fall through
      }

      return results;
    }

    public async Task<(IList<T> results, TableContinuationToken tableQuerySegment)> GetEntitiesByFilterAsync<T>(Expression<Func<T, bool>> expression, int count, TableContinuationToken? tableContinuationToken = null) where T : TableEntity, new()
    {
      List<T> results = new List<T>();
      var querySegment = default(TableQuerySegment<T>);

      try
      {
        var query = _cloudTable.CreateQuery<T>().Where(expression).Take(count) as TableQuery<T>;

        tableContinuationToken = querySegment != null ? querySegment.ContinuationToken : tableContinuationToken;
        querySegment = await _cloudTable.ExecuteQuerySegmentedAsync(query, tableContinuationToken);
        results.AddRange(querySegment.Results);
      }
      catch
      {
        // Fall through
      }

      return (results, querySegment?.ContinuationToken);
    }


    #endregion

    #region Insert Or Rename Entity

    public async Task<OperationStatus> InsertOrReplaceEntityAsync<T>(T tableEntity, CancellationToken cancellationToken) where T : ITableEntity
    {
      var result = new OperationStatus();

      try
      {
        TableOperation insert = TableOperation.InsertOrReplace(tableEntity);
        await _cloudTable.ExecuteAsync(insert);

        result.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        result.IsOperationSuccessful = false;
        result.OperationException = iException;
      }

      return result;
    }

    public OperationStatus InsertOrReplaceEntity<T>(T tableEntity) where T : ITableEntity
    {
      var result = new OperationStatus();

      try
      {
        TableOperation insert = TableOperation.InsertOrReplace(tableEntity);
        _cloudTable.ExecuteAsync(insert)
              .GetAwaiter()
              .GetResult();

        result.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        result.IsOperationSuccessful = false;
        result.OperationException = iException;
      }

      return result;
    }

    #endregion

    #region Delete Entity

    public async Task<OperationStatus> DeleteEntityIfExistsAsync<T>(string partitionKey, string rowKey, CancellationToken cancellationToken) where T : ITableEntity
    {
      var result = new OperationStatus()
      {
        IsOperationSuccessful = true
      };

      var entity = GetEntity<T>(partitionKey, rowKey);

      if (entity != null)
      {
        try
        {
          TableOperation insert = TableOperation.Delete(entity);
          await _cloudTable.ExecuteAsync(insert);

          result.IsOperationSuccessful = true;
        }
        catch (Exception iException)
        {
          result.IsOperationSuccessful = false;
          result.OperationException = iException;
        }
      }

      return result;
    }

    public OperationStatus DeleteEntityIfExists<T>(string partitionKey, string rowKey) where T : ITableEntity
    {
      var result = new OperationStatus()
      {
        IsOperationSuccessful = true
      };

      var entity = GetEntity<T>(partitionKey, rowKey);

      if (entity != null)
      {
        try
        {
          TableOperation insert = TableOperation.Delete(entity);
          _cloudTable.ExecuteAsync(insert);

          result.IsOperationSuccessful = true;
        }
        catch (Exception iException)
        {
          result.IsOperationSuccessful = false;
          result.OperationException = iException;
        }
      }

      return result;
    }

    #endregion
  }
}
