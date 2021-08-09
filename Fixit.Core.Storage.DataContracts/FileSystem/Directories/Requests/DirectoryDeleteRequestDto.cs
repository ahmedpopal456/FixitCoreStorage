using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories.Requests
{
  [DataContract]
  public class DirectoryDeleteRequestDto
  {
    [DataMember]
    public string DirectoryPathToDelete { get; set; }
  }
}