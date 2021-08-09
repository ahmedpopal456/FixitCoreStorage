using Microsoft.Azure.Cosmos.Table;

namespace Fixit.Core.Storage.DataContracts.TableEntities
{
	public class TableFileEntity : TableEntity
  {
    public string FolderPath { get; set; }

    public string FileName { get; set; }

    public string FileSize { get; set; }

    public string CreatedAtTimestampUtc { get; set; }
  }
}
