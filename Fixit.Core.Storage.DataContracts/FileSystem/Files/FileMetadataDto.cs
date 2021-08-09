using System;
using System.Collections.Generic;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  public class FileMetadataDto
  {
    public string EntityId { get; set; }

    public string MnemonicName { get; set; }

    public string EntityName { get; set; }

    public string ContentType { get; set; }

    public string ThumbnailUrl { get; set; }

    public string MnemonicId { get; set; }

    public ImageUrlDto ImageUrl { get; set; }

    public IList<FileTagDto> Tags { get; set; }

    public Guid LastUpdatedByUserId { get; set; }

    public FileMetadataExtension MetadataExtension { get; set; }
  }
}