using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories.Requests
{
  [DataContract]
  public class DirectoryRenameRequestDto
  {
    [DataMember]
    public string CurrentDirectoryPath { get; set; }

    [DataMember]
    public string RenamedDirectoryPath { get; set; }
  }
}