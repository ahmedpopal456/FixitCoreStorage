using System;
using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.Table.Adapters.Internal
{
  internal class TableServiceClientAdapter : ITableServiceClientAdapter
  {
    private CloudTableClient _cloudTableClient;

    public TableServiceClientAdapter(Uri uri, string accountName, string key)
    {
      if (uri == null)
      {
        throw new ArgumentNullException($"{nameof(TableServiceClientAdapter)} expects a valid value for {nameof(uri)}");
      }
      if (string.IsNullOrWhiteSpace(accountName))
      {
        throw new ArgumentNullException($"{nameof(TableServiceClientAdapter)} expects a valid value for {nameof(accountName)}");
      }
      if (string.IsNullOrWhiteSpace(key))
      {
        throw new ArgumentNullException($"{nameof(TableServiceClientAdapter)} expects a valid value for {nameof(key)}");
      }

      _cloudTableClient = new CloudTableClient(uri, new StorageCredentials(accountName, key));
    }

    public ITableClientAdapter GetTableReference(string tableName)
    {
      return new TableClientAdapter(_cloudTableClient.GetTableReference(tableName));
    }
  }
}
