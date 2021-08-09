using System;
using System.Collections.Generic;
using System.Web;
using Fixit.Core.Storage.DataContracts.FileSystem;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Fixit.Core.Storage.FileSystem.Models
{
  public class FileMetadata
  {
    public Guid FileId { get; set; }

    public string MnemonicId { get; set; }

    public string MnemonicName { get; set; }

    public string EntityId { get; set; }

    public string EntityName { get; set; }

    public string ThumbnailUrl { get; set; }

    public ImageUrlDto ImageUrl { get; set; }

    public Guid LastUpdateByUserId { get; set; }

    public string UpdatedTimestampUtc { get; set; }

    public string CreatedTimestampUtc { get; set; }

    public Guid CreatedByUserId { get; set; }

    public string ContentType { get; set; }

    public string SizeInBytes { get; set; }

    public IList<FileTagDto> Tags { get; set; }

    public FileMetadataExtension MetadataExtension { get; set; }

    #nullable enable

    public IDictionary<string, string> ToDictionary()
    {
      var result = default(IDictionary<string, string>);

      try
      {
        var serializedObject = JsonSerializer.Serialize(this);

        result = new Dictionary<string, string>();

        result.Add(nameof(FileId), FileId.ToString());
        result.Add(nameof(MnemonicId), MnemonicId);
        result.Add(nameof(MnemonicName), HttpUtility.UrlEncode(MnemonicName));
        result.Add(nameof(EntityId), EntityId);
        result.Add(nameof(EntityName), EntityName);
        result.Add(nameof(ThumbnailUrl), ThumbnailUrl);
        result.Add(nameof(LastUpdateByUserId), LastUpdateByUserId.ToString());
        result.Add(nameof(UpdatedTimestampUtc), UpdatedTimestampUtc);
        result.Add(nameof(CreatedTimestampUtc), CreatedTimestampUtc);
        result.Add(nameof(CreatedByUserId), CreatedByUserId.ToString());
        result.Add(nameof(ContentType), ContentType);
        result.Add(nameof(Tags), Tags != null ? HttpUtility.UrlEncode(JsonConvert.SerializeObject(Tags)) : string.Empty);
        result.Add(nameof(MetadataExtension), MetadataExtension != null ? JsonConvert.SerializeObject(MetadataExtension) : string.Empty);
        result.Add(nameof(SizeInBytes), SizeInBytes);
        result.Add(nameof(ImageUrl), ImageUrl != null ? JsonConvert.SerializeObject(ImageUrl) : string.Empty);
      }
      catch
      {
        // Fall through
      }

      return result;
    }

    public FileMetadata? FromDictionary(IDictionary<string,string?> dictionary)
    {
      var result = default(FileMetadata);

      try
      {
        result = new FileMetadata();

        result.FileId = dictionary.ContainsKey(nameof(FileId)) ? Guid.Parse(dictionary[nameof(FileId)]) : Guid.Empty;
        result.LastUpdateByUserId = dictionary.ContainsKey(nameof(LastUpdateByUserId)) ? Guid.Parse(dictionary[nameof(LastUpdateByUserId)]) : Guid.Empty;
        result.CreatedByUserId = dictionary.ContainsKey(nameof(CreatedByUserId)) ? Guid.Parse(dictionary[nameof(CreatedByUserId)]) : Guid.Empty;

        result.Tags = dictionary.ContainsKey(nameof(Tags)) ? JsonConvert.DeserializeObject<IList<FileTagDto>>(HttpUtility.UrlDecode(dictionary[nameof(Tags)])) : null;
        result.MetadataExtension = dictionary.ContainsKey(nameof(MetadataExtension)) ? JsonConvert.DeserializeObject<FileMetadataExtension>(dictionary[nameof(MetadataExtension)]) : null;

        result.MnemonicId = dictionary.ContainsKey(nameof(MnemonicId)) ? dictionary[nameof(MnemonicId)] : string.Empty;
        result.MnemonicName = dictionary.ContainsKey(nameof(MnemonicName)) ? HttpUtility.UrlDecode(dictionary[nameof(MnemonicName)]) : string.Empty;
        result.EntityId = dictionary.ContainsKey(nameof(EntityId)) ? dictionary[nameof(EntityId)] : string.Empty;
        result.EntityName = dictionary.ContainsKey(nameof(EntityName)) ? dictionary[nameof(EntityName)] : string.Empty;
        result.ThumbnailUrl = dictionary.ContainsKey(nameof(ThumbnailUrl)) ? dictionary[nameof(ThumbnailUrl)] : string.Empty;
        result.UpdatedTimestampUtc = dictionary.ContainsKey(nameof(UpdatedTimestampUtc)) ? dictionary[nameof(UpdatedTimestampUtc)] : string.Empty;
        result.CreatedTimestampUtc = dictionary.ContainsKey(nameof(CreatedTimestampUtc)) ? dictionary[nameof(CreatedTimestampUtc)] : string.Empty;
        result.ContentType = dictionary.ContainsKey(nameof(ContentType)) ? dictionary[nameof(ContentType)] : string.Empty;
        result.SizeInBytes = dictionary.ContainsKey(nameof(SizeInBytes)) ? dictionary[nameof(SizeInBytes)] : string.Empty;
        result.ImageUrl = dictionary.ContainsKey(nameof(ImageUrl)) ? JsonConvert.DeserializeObject<ImageUrlDto>(dictionary[nameof(ImageUrl)]) : null;
      }
      catch(Exception ie)
      {
        // Fall through
      }

      return result;
    }

    #nullable disable
  }
}
