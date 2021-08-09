using System;
using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Requests
{
  [DataContract]
  public class FileRequestDto
  {
    [DataMember]
    public Guid FileId { get; set; }
  }
}