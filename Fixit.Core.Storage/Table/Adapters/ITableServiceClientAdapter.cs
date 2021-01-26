namespace Fixit.Core.Storage.Table.Adapters
{
  public interface ITableServiceClientAdapter
  {
    ITableClientAdapter GetTableReference(string tableName);
  }
}
