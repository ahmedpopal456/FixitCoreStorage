using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Fixit.Core.Storage.FileSystem.Managers
{
  public interface IFileSystemServiceClient : IDisposable
  {
    /// <summary>
    /// Get Client to manipulate a given File System
    /// </summary>
    /// <param name="fileSystemName"></param>
    /// <returns></returns>
    IFileSystemClient GetFileSystem(string fileSystemName);

    /// <summary>
    /// Delete File System from the Storage Medium asynchronously
    /// </summary>
    /// <param name="fileSystemName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<HttpStatusCode> DeleteFileSystemAsync(string fileSystemName, CancellationToken cancellationToken);

    /// <summary>
    /// Delete File System from the Storage Medium
    /// </summary>
    /// <param name="fileSystemName"></param>
    /// <returns></returns>
    HttpStatusCode DeleteFileSystem(string fileSystemName);

    /// <summary>
    /// Create File System in the Storage Medium asynchronously
    /// </summary>
    /// <param name="fileSystemName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IFileSystemClient> CreateOrGetFileSystemAsync(string fileSystemName, CancellationToken cancellationToken);

    /// <summary>
    /// Create File System in the Storage Medium
    /// </summary>
    /// <param name="fileSystemName"></param>
    /// <returns></returns>
    /// 
    IFileSystemClient CreateOrGetFileSystem(string fileSystemName);

    // TODO:  Get/Set FileSystemMetadata
  }
}