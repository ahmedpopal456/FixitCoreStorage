using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.DataContracts.TableEntities
{
  public class TableFileSystemInformationEntity : TableEntity
  {
    public long FileCount { get; set; }
  }
}
