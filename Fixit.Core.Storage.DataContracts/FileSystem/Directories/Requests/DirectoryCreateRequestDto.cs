using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories.Requests
{
  [DataContract]
  public class DirectoryCreateRequestDto
  {
    [DataMember]
    public string DirectoryPathToCreate { get; set; }
  }
}