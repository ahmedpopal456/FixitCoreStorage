using System;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.Storage.Table.Adapters;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Managers.Internal
{
	internal class TableStorageServiceClientManager : ITableServiceClient
  {

    private readonly ITableStorageServiceClientAdapter _cloudTableClient;

    private readonly OperationContext _operationContext;
    private readonly TableRequestOptions _tableRequestOptions;

    public TableStorageServiceClientManager(ITableStorageServiceClientAdapter cloudTableClient)
    {
      _cloudTableClient = cloudTableClient ?? throw new ArgumentNullException($"{nameof(TableStorageServiceClientManager)} expects a value for {nameof(cloudTableClient)}... null argument was provided");
      
      _tableRequestOptions = new TableRequestOptions();
      _operationContext = new OperationContext();
    }

    #region Create or Get Table 

    public async Task<ITableStorage> CreateOrGetTableAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var tableStorage = await GetTableAsync(tableName, cancellationToken);

      if (tableStorage == null)
      {
        var cloudTable = _cloudTableClient.GetTableReference(tableName);

        await cloudTable.CreateAsync(_tableRequestOptions, _operationContext, cancellationToken);

        tableStorage = new TableStorageClientManager(cloudTable);
      }

      return tableStorage;
    }

    public ITableStorage CreateOrGetTable(string tableName)
    {
      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var tableStorage = GetTable(tableName);

      if (tableStorage == null)
      {
        var cloudTable = _cloudTableClient.GetTableReference(tableName);

        cloudTable.CreateAsync(_tableRequestOptions, _operationContext).GetAwaiter().GetResult();

        tableStorage = new TableStorageClientManager(cloudTable);
      }

      return tableStorage;
    }

    #endregion

    #region Get Table 

    public async Task<ITableStorage> GetTableAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var tableStorage = default(ITableStorage);

      var table = _cloudTableClient.GetTableReference(tableName);

      if (await table.ExistsAsync(_tableRequestOptions, _operationContext, cancellationToken))
      {
        tableStorage = new TableStorageClientManager(table);
      }

      return tableStorage;
    }

    public ITableStorage GetTable(string tableName)
    {
      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var tableStorage = default(ITableStorage);

      var table = _cloudTableClient.GetTableReference(tableName);

      if (table.ExistsAsync(_tableRequestOptions, _operationContext,CancellationToken.None).Result)
      {
        tableStorage = new TableStorageClientManager(table);
      }

      return tableStorage;
    }

    #endregion

    #region Delete Table 

    public async Task<OperationStatus> DeleteTableIfExistsAsync(string tableName, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var result = new OperationStatus();

      try
      {
        var table = _cloudTableClient.GetTableReference(tableName);

        result.IsOperationSuccessful = await table.DeleteIfExistsAsync(_tableRequestOptions, _operationContext, cancellationToken);
      }
      catch (Exception iException)
      {
        result.IsOperationSuccessful = false;
        result.OperationException = iException;
      }

      return result;
    }

    public OperationStatus DeleteTableIfExists(string tableName)
    {
      if (string.IsNullOrWhiteSpace(tableName))
      {
        throw new ArgumentNullException();
      }

      var result = new OperationStatus();

      try
      {
        var table = _cloudTableClient.GetTableReference(tableName);
        result.IsOperationSuccessful = table.DeleteIfExistsAsync(_tableRequestOptions, _operationContext, CancellationToken.None).Result;
      }
      catch (Exception iException)
      {
        result.IsOperationSuccessful = false;
        result.OperationException = iException;
      }

      return result;
    }
    
    #endregion

    #region IDisposable

    private void ReleaseUnmanagedResources()
    {
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
    }


    #endregion
  }
}
