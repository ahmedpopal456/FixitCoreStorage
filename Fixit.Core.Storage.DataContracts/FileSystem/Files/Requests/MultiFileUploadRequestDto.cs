using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files.Requests
{
  [DataContract]
  public class MultiFileUploadRequestDto
  {
    [DataMember]
    public IFormFileCollection FormFileCollection { get; set; }

    [DataMember]
    public string FilePathMetadataInfo { get; set; }


    public IDictionary<string, FileMetadataSummary> FilePathMetadataInfoToDictionary()
    {
      var wResult = default(IDictionary<string, FileMetadataSummary>);

      try
      {
        wResult = JsonSerializer.Deserialize<IDictionary<string, FileMetadataSummary>>(FilePathMetadataInfo);
      }
      catch
      {
        // Fall through
      }

      return wResult;
    }
  }
}
