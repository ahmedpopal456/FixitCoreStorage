using System;
using Fixit.Core.Storage.FileSystem.Managers;
using static Fixit.Core.Storage.FileSystem.Resolvers.FileSystemResolvers;

namespace Fixit.Core.Storage.FileSystem
{
  public interface IFileSystemFactory : IDisposable
  {
    /// <summary>
    /// A FileSystemClient allows you to manipulate a given storage's file systems and their directories and files.
    /// </summary>
    /// <param name="fileSystemClientResolver">The file system client resolver</param>
    /// <returns></returns>
    IFileSystemServiceClient CreateDataLakeFileSystemServiceClient(FileSystemClientResolver fileSystemClientResolver = null);

  }
}
