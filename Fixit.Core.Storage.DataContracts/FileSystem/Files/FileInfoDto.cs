using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  [DataContract]
  public class FileInfoDto
  {
    [DataMember]
    public string EntityId { get; set; }

    [DataMember]
    public string EntityName { get; set; }
    
    [DataMember]
    public string MnemonicName { get; set; }
    
    [DataMember]
    public string MnemonicId { get; set; }

    [DataMember]
    public Guid LastUpdateByUserId { get; set; }

    [DataMember]
    public string UpdatedTimestampUtc { get; set; }

    [DataMember]
    public string CreatedTimestampUtc { get; set; }

    [DataMember]
    public Guid CreatedByUserId { get; set; }

    [DataMember]
    public string ContentType { get; set; }

    [DataMember]
    public string SizeInBytes { get; set; }

    [DataMember]
    public string ThumbnailUrl { get; set; }

    [DataMember]
    public ImageUrlDto ImageUrl { get; set; }

    [DataMember]
    public IList<FileTagDto> Tags { get; set; }

    [DataMember]
    public FileMetadataExtension MetadataExtension { get; set; }
  }
}