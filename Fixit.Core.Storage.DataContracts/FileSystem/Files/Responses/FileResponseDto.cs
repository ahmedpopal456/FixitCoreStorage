using System;
using System.Runtime.Serialization;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Responses
{
  [DataContract]
  public class FileResponseDto : OperationStatus
  {
    [DataMember]
    public FileInfoDto FileInfo { get; set; }

    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string FileName { get; set; }
  }
}