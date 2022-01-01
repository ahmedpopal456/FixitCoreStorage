using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Requests
{
  [DataContract]
  public class FileRenameRequestDto
  {
    [DataMember]
    public string RenamedFilePath { get; set; }
  }
}