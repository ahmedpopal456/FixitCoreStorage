using System;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Storage.Table.Adapters.Internal
{
	public class TableStorageServiceClientAdapter : ITableStorageServiceClientAdapter
  {
    private readonly CloudTableClient _cloudTableClient;

    public TableStorageServiceClientAdapter(Uri uri, StorageCredentials storageCredentials)
    {
      if (uri == null)
      {
        throw new ArgumentNullException($"{nameof(TableStorageServiceClientAdapter)} expects a value for {nameof(uri)}... null argument was provided");
      }

      if (storageCredentials == null)
      {
        throw new ArgumentNullException($"{nameof(TableStorageServiceClientAdapter)} expects a value for {nameof(storageCredentials)}... null argument was provided");
      }

      _cloudTableClient = new CloudTableClient(uri, storageCredentials, new TableClientConfiguration());
    }

    public ITableStorageClientAdapter GetTableReference(string tableName)
    {
      var table = _cloudTableClient.GetTableReference(tableName);

      return new TableStorageClientAdapter(table) as ITableStorageClientAdapter;
    }
  }
}
