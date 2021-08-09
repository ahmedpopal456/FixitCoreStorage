using System.Runtime.Serialization;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  [DataContract]
  public class FileMetadataSummary
  {
    [DataMember]
    public string MnemonicName { get; set; }

    [DataMember]
    public string ContentType { get; set; }

    [DataMember]
    public string TagNames { get; set; }

    [IgnoreDataMember]
    public string ThumbnailUrl { get; set; }

    [IgnoreDataMember]
    public ImageUrlDto ImageUrl { get; set; }

    [DataMember]
    public FileMetadataExtension MetadataExtension { get; set; }
  }
}
