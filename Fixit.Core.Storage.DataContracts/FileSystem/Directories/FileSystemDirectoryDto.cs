using System.Collections.Generic;
using System.Runtime.Serialization;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Newtonsoft.Json;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Directories
{
  [DataContract]
  public class FileSystemDirectoryDto
  {
    private IList<FileSystemFileDto> mDirectoryItems { get; set; }
    private IList<FileSystemDirectoryDto> mDirectories { get; set; }

    [DataMember]
    public string Path { get; set; }

    [DataMember]
    public IList<FileSystemDirectoryDto> Directories
    {
      get { return mDirectories ??= new List<FileSystemDirectoryDto>(); }
      set => mDirectories = value;
    }

    [DataMember]
    public IList<FileSystemFileDto> DirectoryItems {
      get { return mDirectoryItems ??= new List<FileSystemFileDto>(); }
      set => mDirectoryItems = value;
    }

    [JsonIgnore]
    [DataMember]
    public string Uri { get; set; }

    [JsonIgnore]
    [DataMember]
    public string ParentUri { get; set; }

    [JsonIgnore]
    [DataMember]
    public FileSystemDirectoryDto ParentDirectory { get; set; }
  }
}
