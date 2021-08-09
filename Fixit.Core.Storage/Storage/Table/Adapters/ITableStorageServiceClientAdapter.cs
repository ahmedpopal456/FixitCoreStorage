namespace Fixit.Core.Storage.Storage.Table.Adapters
{
	public interface ITableStorageServiceClientAdapter
  {
    ITableStorageClientAdapter GetTableReference(string tableName);
  }
}