using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories
{
  [DataContract]
  public class FileSystemDirectoryItemsDto
  {
    [DataMember]
    public string Path { get; set; }

    [DataMember]
    public IList<FileSystemFileDto> DirectoryItems { get; set; } = new List<FileSystemFileDto>();
  }
}
