using AutoMapper;
using Fixit.Core.Storage.FileSystem.Adapters;
using Fixit.Core.Storage.FileSystem.Managers;
using Fixit.Core.Storage.Storage.Blob.Adapters;

namespace Fixit.Core.Storage.FileSystem.Resolvers
{
  public class FileSystemResolvers
  {
    public delegate IFileSystemClient FileSystemClientResolver(IDataLakeFileSystemAdapter dataLakeFileSystemAdapter, IBlobStorageClientAdapter blobStorageClientAdapter, IMapper mapper);

  }
}
