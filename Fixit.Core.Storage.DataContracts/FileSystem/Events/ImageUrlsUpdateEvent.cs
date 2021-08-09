using System;
using System.Runtime.Serialization;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Events
{
  [DataContract]
  public class ImageUrlsUpdateEvent
  {
    [DataMember]
    public Guid FileId { get; set; }

    [DataMember]
    public string ThumbnailUrl { get; set; }

    [DataMember]
    public ImageUrlDto ImageUrl { get; set; }
  }
}
