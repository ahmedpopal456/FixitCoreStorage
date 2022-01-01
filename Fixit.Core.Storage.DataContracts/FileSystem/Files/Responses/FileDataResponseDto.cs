using System;
using System.IO;
using System.Runtime.Serialization;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Responses
{
  [DataContract]
  public class FileDataResponseDto : OperationStatus
  {
    [DataMember]
    public Stream FileStream { get; set; }

    [DataMember]
    public Guid FileId { get; set; }
  }
}