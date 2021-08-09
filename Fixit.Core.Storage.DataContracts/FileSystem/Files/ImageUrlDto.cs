using System;
using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  [DataContract]
  public class ImageUrlDto
  {
    [DataMember]
    public string Url { get; set; }

    [DataMember]
    public DateTime ExpiryDate { get;set; }
  }
}
