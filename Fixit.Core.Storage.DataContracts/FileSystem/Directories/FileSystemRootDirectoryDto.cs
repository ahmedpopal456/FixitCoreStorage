using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories
{
  [DataContract]
  public class FileSystemRootDirectoryDto
  {
    [DataMember]
    public FileSystemDirectoryDto DirectoryInfo { get; set; } = new FileSystemDirectoryDto();

    [DataMember]
    public IEnumerable<FileTagDto> DirectoryTags { get; set; }
  }
}
