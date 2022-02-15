using System;
using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  [DataContract]
  public class FileToRegenerateUrlDto
  {
    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string EntityName { get; set; }

    [DataMember]
    public string EntityId { get; set; }

    [DataMember]
    public string ThumbnailUrl { get; set; }

    [DataMember]
    public ImageUrlDto ImageUrl { get; set; }
  }
}
