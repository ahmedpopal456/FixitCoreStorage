using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Events
{
  public class RegenerateImageUrlEvent
  {
    [DataMember]
    public IEnumerable<FileToRegenerateUrlDto> FilesToRegenerateUrls { get; set; }
  }
}
