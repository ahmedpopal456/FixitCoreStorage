using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.FileSystem.Directories;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Fixit.Core.Storage.FileSystem.Models;

namespace Fixit.Core.Storage.FileSystem.Managers
{
  public interface IFileSystemClient: IDisposable
  {
    /// <summary>
    /// Generate an ImageUrl
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="expirationTime"></param>
    /// <returns></returns>
    ImageUrlDto GenerateImageUrl(string filePath, int? expirationTime);

    /// <summary>
    /// Gets a File from the File System 
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <returns></returns>
    Stream GetFile(string iFilePath);

    /// <summary>
    /// Gets a File from the File System asynchronously
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<Stream> GetFileAsync(string iFilePath, CancellationToken iCancellationToken);

    /// <summary>
    /// Create (if not exists), and Upload a File to the File System asynchronously
    /// </summary>
    /// <param name="iFileContent"></param>
    /// <param name="iFilePath"></param>
    /// <param name="iCancellationToken"></param>
    /// <param name="iFileMetadata"></param>
    /// <param name="iOverwrite"></param>
    /// <returns></returns>
    Task<FileUploadDto> CreateAndUploadFileAsync(Stream iFileContent, string iFilePath, CancellationToken iCancellationToken, FileMetadataDto iFileMetadata = null, bool iOverwrite = true);

    /// <summary>
    /// Create (if not exists), and Upload a File to the File System
    /// </summary>
    /// <param name="iFileContent"></param>
    /// <param name="iFilePath"></param>
    /// <param name="iFileMetadata"></param>
    /// <param name="iOverwrite"></param>
    /// <returns></returns>
    FileUploadDto CreateAndUploadFile(Stream iFileContent, string iFilePath, FileMetadataDto iFileMetadata = null, bool iOverwrite = true);

    /// <summary>
    /// Set Metadata for a given File in the File System asynchronously
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <param name="iFileMetadata"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> SetFileMetadataAsync(string iFilePath,FileMetadataDto iFileMetadata, CancellationToken iCancellationToken);

    /// <summary>
    /// Set Metadata for a given File in the File System
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <param name="iFileMetadata"></param>
    /// <returns></returns>
    OperationStatus SetFileMetadata(string iFilePath, FileMetadataDto iFileMetadata);

    /// <summary>
    /// Get Metadata for a given File in the File System asynchronously
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<FileMetadata> GetFileMetadataAsync(string iFilePath, CancellationToken iCancellationToken);

    /// <summary>
    /// Get Metadata for a given File in the File System
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <returns></returns>
    FileMetadata GetFileMetadata(string iFilePath);

    /// <summary>
    /// Rename a Directory in the File System asynchronously
    /// </summary>
    /// <param name="iCurrentDirectoryPath"></param>
    /// <param name="iUpdatedDirectoryPath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> RenameDirectoryAsync(string iCurrentDirectoryPath,string iUpdatedDirectoryPath, CancellationToken iCancellationToken);

    /// <summary>
    /// Rename a Directory in the File System
    /// </summary>
    /// <param name="iCurrentDirectoryPath"></param>
    /// <param name="iUpdatedDirectoryPath"></param>
    /// <returns></returns>
    OperationStatus RenameDirectory(string iCurrentDirectoryPath, string iUpdatedDirectoryPath);

    /// <summary>
    /// Rename a File in the File System asynchronously
    /// </summary>
    /// <param name="iCurrentFilePath"></param>
    /// <param name="iUpdatedFilePath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> RenameFileAsync(string iCurrentFilePath, string iUpdatedFilePath, CancellationToken iCancellationToken);

    /// <summary>
    /// Rename a File in the File System
    /// </summary>
    /// <param name="iCurrentFilePath"></param>
    /// <param name="iUpdatedFilePath"></param>
    /// <returns></returns>
    OperationStatus RenameFile(string iCurrentFilePath, string iUpdatedFilePath);

    /// <summary>
    /// Delete a File in the File System asynchronously
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteFileAsync(string iFilePath, CancellationToken iCancellationToken);

    /// <summary>
    /// Delete a File in the File System
    /// </summary>
    /// <param name="iFilePath"></param>
    /// <returns></returns>
    OperationStatus DeleteFile(string iFilePath);

    /// <summary>
    /// Delete a Directory in the File System asynchronously
    /// </summary>
    /// <param name="iDirectoryPath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> DeleteDirectoryIfExistsAsync(string iDirectoryPath, CancellationToken iCancellationToken);

    /// <summary>
    /// Delete a Directory in the File System
    /// </summary>
    /// <param name="iDirectoryPath"></param>
    /// <returns></returns>
    OperationStatus DeleteDirectoryIfExists(string iDirectoryPath);

    /// <summary>
    /// Create a Directory in the File System asynchronously
    /// </summary>
    /// <param name="iDirectoryPath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<OperationStatus> CreateDirectoryIfNotExistsAsync(string iDirectoryPath, CancellationToken iCancellationToken);

    /// <summary>
    /// Create a Directory in the File System
    /// </summary>
    /// <param name="iDirectoryPath"></param>
    /// <returns></returns>
    OperationStatus CreateDirectoryIfNotExists(string iDirectoryPath);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iCurrentDirectoryPath"></param>
    /// <param name="iUpdatedDirectoryPath"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<bool> IsDirectoryCreatedAsync(string iCurrentDirectoryPath, CancellationToken iCancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="iCurrentDirectoryPath"></param>
    /// <param name="iUpdatedDirectoryPath"></param>
    /// <returns></returns>
    bool IsDirectoryCreated(string iCurrentDirectoryPath);

    /// <summary>
    /// Get Blob Directory Flat Structure asynchronously
    /// </summary>
    /// <param name="iPrefix"></param>
    /// <param name="iCancellationToken"></param>
    /// <returns></returns>
    Task<FileSystemDirectoryDto> GetDirectoryStructureAsync(string iPrefix, CancellationToken iCancellationToken, bool iIncludeItems = false, bool getAllStructure = false);

    /// <summary>
    /// Get Blob Directory Flat Structure
    /// </summary>
    /// <param name="iPrefix"></param>
    /// <returns></returns>
    FileSystemDirectoryDto GetDirectoryStructure(string iPrefix, bool iIncludeItems = false, bool getSingleLevel = false);
    /// <summary>
    /// Get the Items of a Blob Directory asynchronously
    /// </summary>
    /// <param name="prefix"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<FileSystemDirectoryDto> GetDirectoryItemsAsync(string prefix, CancellationToken cancellationToken);

    /// <summary>
    /// Get the items of a Blob Directory
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    FileSystemDirectoryDto GetDirectoryItems(string prefix);

        // TODO: Get/Set Directory Metadata
  }
}
