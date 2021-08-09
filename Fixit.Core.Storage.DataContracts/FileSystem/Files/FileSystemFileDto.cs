using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{

  [DataContract]
  public class FileSystemFileDto
  {
    [DataMember]
    public string Id { get; set; }
    
    [DataMember]
    public string EntityId { get; set; }

    [DataMember]
    public string EntityName { get; set; }

    [DataMember]
    public string Name { get; set; }

    [DataMember]
    public string ThumbnailUrl { get; set; }

    [DataMember]
    public ImageUrlDto ImageUrl { get; set; }

    [DataMember]
    public string MnemonicId { get; set; }

    [DataMember]
    public string MnemonicName { get; set; }

    [DataMember]
    public string ContentType { get; set; }

    [DataMember]
    public IEnumerable<FileTagDto> FileTags { get; set; }    

    [IgnoreDataMember]
    public string Uri { get; set; }

    [IgnoreDataMember]
    public string ParentUri { get; set; }    
  }
}
