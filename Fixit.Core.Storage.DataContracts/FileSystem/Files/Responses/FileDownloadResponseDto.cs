using System;
using System.Runtime.Serialization;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Responses
{
  [DataContract]
  public class FileDownloadResponseDto : OperationStatus
  {
    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string FilePath { get; set; }

    [DataMember]
    public string DownloadUrl { get; set; }
  }
}
