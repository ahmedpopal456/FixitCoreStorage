using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.FileSystem.Directories;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Fixit.Core.Storage.DataContracts.Helpers;
using Fixit.Core.Storage.FileSystem.Adapters;
using Fixit.Core.Storage.FileSystem.Constants;
using Fixit.Core.Storage.FileSystem.Models;
using Fixit.Core.Storage.Storage.Blob.Adapters;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.FileSystem.Managers.Internal
{
  public class DataLakeFileSystemManager : IFileSystemClient
  {
    private readonly IMapper _mapper;
    private IDataLakeFileSystemAdapter _dataLakeFileSystemClient { get; set; }
    private IBlobStorageClientAdapter _cloudBlobContainer { get; set; }   

    public DataLakeFileSystemManager(IDataLakeFileSystemAdapter dataLakeFileSystemClient,
                                     IBlobStorageClientAdapter cloudBlobContainer,
                                     IMapper mapper)
    {
      _dataLakeFileSystemClient = dataLakeFileSystemClient ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(dataLakeFileSystemClient)}... null argument was provided");
      _cloudBlobContainer = cloudBlobContainer ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(cloudBlobContainer)}... null argument was provided");
      _mapper = mapper ?? throw new ArgumentNullException($"{nameof(FileSystemFactory)} expects a value for {nameof(mapper)}... null argument was provided");
    }

    #region File Domain

    #region Generate Files
    public virtual ImageUrlDto GenerateImageUrl(string filePath, int? expirationTime)
    {
      var result = default(ImageUrlDto);

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);
      if (dataLakeFileClient.Exists())
      {
        var blob = _cloudBlobContainer.GetBlockBlobReference(filePath);

        var dateTimeOffset = (DateTimeOffset)(expirationTime == null ? DateTime.UtcNow.AddYears(FileSystemConstants.DefaultExpiryTime) : DateTime.UtcNow.AddMinutes((int)expirationTime));
        SharedAccessBlobPolicy sasBlobPolicy = new SharedAccessBlobPolicy()
        {
          SharedAccessExpiryTime = dateTimeOffset,
          Permissions = SharedAccessBlobPermissions.Read
        };

        var sasToken = blob.GetSharedAccessSignature(sasBlobPolicy);             
        result = new ImageUrlDto
        {          
          Url = HttpUtility.UrlEncode(blob.Uri + sasToken),
          ExpiryDate = dateTimeOffset.UtcDateTime
        };

      }
      return result;
    }
    #endregion

    #region Get Files

    public virtual Stream GetFile(string filePath)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(GetFile)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var stream = default(MemoryStream);
      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (dataLakeFileClient.Exists())
      {
        stream = new MemoryStream();
        dataLakeFileClient.ReadToAsync(stream, CancellationToken.None)
                          .GetAwaiter()
                          .GetResult();
      }

      return stream;
    }

    public virtual async Task<Stream> GetFileAsync(string filePath, CancellationToken cancellationToken)
    {
      if (string.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(GetFileAsync)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var stream = default(MemoryStream);

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (await dataLakeFileClient.ExistsAsync(cancellationToken))
      {
        stream = new MemoryStream();
        await dataLakeFileClient.ReadToAsync(stream, cancellationToken);
      }

      return stream;
    }

    public virtual FileMetadata GetFileMetadata(string filePath)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(GetFileMetadata)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var fileDownloadDto = default(FileMetadata);

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (dataLakeFileClient.Exists())
      {
        fileDownloadDto = new FileMetadata().FromDictionary(dataLakeFileClient.GetMetadata());      
      }

      return fileDownloadDto;
    }

    public virtual async Task<FileMetadata> GetFileMetadataAsync(string filePath, CancellationToken cancellationToken)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(GetFileMetadataAsync)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var fileDownloadDto = default(FileMetadata);

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (await dataLakeFileClient.ExistsAsync(cancellationToken))
      {
        var fileMetadata = await dataLakeFileClient.GetMetadataAsync(cancellationToken);

        fileDownloadDto = new FileMetadata().FromDictionary(fileMetadata);       
      }

      return fileDownloadDto;
    }

    #endregion

    #region Create & Upload Files

    public virtual async Task<FileUploadDto> CreateAndUploadFileAsync(Stream fileContent, string filePath, CancellationToken cancellationToken, FileMetadataDto fileMetadata = null, bool iOverwrite = true)
    {
      if (fileContent == null)
      {
        throw new ArgumentNullException($"{nameof(CreateAndUploadFileAsync)} expects a value for {nameof(fileContent)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(CreateAndUploadFileAsync)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var fileUploadStatusDto = new FileUploadDto();
      var mappedFileMetadata = _mapper.Map<FileMetadataDto, FileMetadata>(fileMetadata);

      mappedFileMetadata = FillFileMetadataAtCreation(mappedFileMetadata, fileContent.Length.ToString()); 
      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      try
      {
        // Setup/Check File
        if (!iOverwrite && await dataLakeFileClient.ExistsAsync(cancellationToken))
        {
          throw new InvalidOperationException($"{filePath} already exists and was specified to not be overwritten.. did you mean to overwrite?");
        }

        // Perform Create
        await dataLakeFileClient.UploadAsync(fileContent, cancellationToken, iOverwrite);

        mappedFileMetadata.ImageUrl = this.GenerateImageUrl(filePath, null);
        await dataLakeFileClient.SetMetadataAsync(metadata: mappedFileMetadata.ToDictionary(), cancellationToken);

        var fileProperties = dataLakeFileClient.GetMetadata();

        fileUploadStatusDto.FileCreatedId = mappedFileMetadata.FileId;
        fileUploadStatusDto.IsOperationSuccessful = true;

        fileUploadStatusDto.FileCreatedPath = dataLakeFileClient.Path;
        fileUploadStatusDto.FileCreatedLength = long.Parse(mappedFileMetadata.SizeInBytes);
        fileUploadStatusDto.FileCreatedTimestampUtc = mappedFileMetadata.CreatedTimestampUtc;
        fileUploadStatusDto.FileCreatedName = dataLakeFileClient.Name;
        fileUploadStatusDto.ImageUrl = mappedFileMetadata.ImageUrl;
      }
      catch (Exception iException)
      {
        fileUploadStatusDto.IsOperationSuccessful = false;
        fileUploadStatusDto.OperationException = iException;
      };

      return fileUploadStatusDto;
    }

    public virtual FileUploadDto CreateAndUploadFile(Stream fileContent, string filePath, FileMetadataDto fileMetadata = null, bool iOverwrite = true)
    {
      if (fileContent == null)
      {
        throw new ArgumentNullException($"{nameof(CreateAndUploadFile)} expects a value for {nameof(fileContent)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(CreateAndUploadFile)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var fileUploadStatusDto = new FileUploadDto();
      var mappedFileMetadata = _mapper.Map<FileMetadataDto, FileMetadata>(fileMetadata);

      mappedFileMetadata = FillFileMetadataAtCreation(mappedFileMetadata, fileContent.Length.ToString());     
      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      try
      {
        // Setup/Check File
        if (!iOverwrite && dataLakeFileClient.Exists())
        {
          throw new InvalidOperationException($"{filePath} already exists and was specified to not be overwritten.. did you mean to overwrite?");
        }

        // Perform Create
        dataLakeFileClient.Upload(fileContent, iOverwrite);

        mappedFileMetadata.ImageUrl = this.GenerateImageUrl(filePath, null);
        dataLakeFileClient.SetMetadata(metadata: mappedFileMetadata.ToDictionary());

        var fileProperties = dataLakeFileClient.GetMetadata();

        fileUploadStatusDto.FileCreatedId = mappedFileMetadata.FileId;
        fileUploadStatusDto.IsOperationSuccessful = true;

        fileUploadStatusDto.FileCreatedPath = dataLakeFileClient.Path;
        fileUploadStatusDto.FileCreatedLength = long.Parse(mappedFileMetadata.SizeInBytes);
        fileUploadStatusDto.FileCreatedTimestampUtc = mappedFileMetadata.CreatedTimestampUtc;
        fileUploadStatusDto.FileCreatedName = dataLakeFileClient.Name;
        fileUploadStatusDto.ImageUrl = mappedFileMetadata.ImageUrl;
      }
      catch (Exception iException)
      {
        fileUploadStatusDto.IsOperationSuccessful = false;
        fileUploadStatusDto.OperationException = iException;
      };

      return fileUploadStatusDto;
    }

    #endregion

    #region Delete Files

    public virtual async Task<OperationStatus> DeleteFileAsync(string filePath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(DeleteFileAsync)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      var response = await dataLakeFileClient.DeleteIfExistsAsync(cancellationToken);

      return new OperationStatus() { IsOperationSuccessful = response.Value };
    }

    public OperationStatus DeleteFile(string filePath)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(DeleteFile)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      var response = dataLakeFileClient.DeleteIfExists();

      return new OperationStatus() { IsOperationSuccessful = response.Value };
    }

    #endregion

    #region Create File Metadata

    public virtual async Task<OperationStatus> SetFileMetadataAsync(string filePath, FileMetadataDto fileMetadata, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(SetFileMetadataAsync)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      if (fileMetadata == null)
      {
        throw new ArgumentNullException($"{nameof(SetFileMetadataAsync)} expects a value for {nameof(fileMetadata)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();


      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (!await dataLakeFileClient.ExistsAsync(cancellationToken: cancellationToken))
      {
        throw new InvalidOperationException($"{filePath} does not exist... cannot set iMetadata");
      }

      try
      {
        var currentFileMetadata = await GetFileMetadataAsync(filePath, cancellationToken);
        var updatedFileMetadata = UpdateFromFileMetadataDto(fileMetadata, currentFileMetadata);

        await dataLakeFileClient.SetMetadataAsync(metadata: updatedFileMetadata.ToDictionary(), cancellationToken);

        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    public OperationStatus SetFileMetadata(string filePath, FileMetadataDto fileMetadata)
    {
      if (String.IsNullOrWhiteSpace(filePath))
      {
        throw new ArgumentNullException($"{nameof(SetFileMetadata)} expects a value for {nameof(filePath)}... null argument was provided");
      }

      if (fileMetadata == null)
      {
        throw new ArgumentNullException($"{nameof(SetFileMetadata)} expects a value for {nameof(fileMetadata)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();


      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(filePath);

      if (!dataLakeFileClient.Exists())
      {
        throw new InvalidOperationException($"{filePath} does not exist... cannot set iMetadata");
      }

      try
      {
        var currentFileMetadata = GetFileMetadata(filePath);
        var updatedFileMetadata = UpdateFromFileMetadataDto(fileMetadata, currentFileMetadata);

        dataLakeFileClient.SetMetadata(metadata: updatedFileMetadata.ToDictionary());

        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    #endregion

    #region Renaming Files

    public virtual async Task<OperationStatus> RenameFileAsync(string currentFilePath, string updatedFilePath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(currentFilePath))
      {
        throw new ArgumentNullException($"{nameof(RenameFileAsync)} expects a value for {nameof(currentFilePath)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(updatedFilePath))
      {
        throw new ArgumentNullException($"{nameof(RenameFileAsync)} expects a value for {nameof(updatedFilePath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(fileName: currentFilePath);

      try
      {
        await dataLakeFileClient.RenameAsync(destinationPath: updatedFilePath, cancellationToken);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    public virtual OperationStatus RenameFile(string currentFilePath, string updatedFilePath)
    {
      if (String.IsNullOrWhiteSpace(currentFilePath))
      {
        throw new ArgumentNullException($"{nameof(RenameFile)} expects a value for {nameof(currentFilePath)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(updatedFilePath))
      {
        throw new ArgumentNullException($"{nameof(RenameFile)} expects a value for {nameof(updatedFilePath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeFileClient = _dataLakeFileSystemClient.GetFileClient(fileName: currentFilePath);

      try
      {
        dataLakeFileClient.Rename(destinationPath: updatedFilePath);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    #endregion

    #endregion

    #region Directory Domain

    #region Rename Directories

    public virtual async Task<OperationStatus> RenameDirectoryAsync(string currentDirectoryPath, string updatedDirectoryPath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(currentDirectoryPath))
      {
        throw new ArgumentNullException($"{nameof(RenameDirectoryAsync)} expects a value for {nameof(currentDirectoryPath)}... null argument was provided");
      }

      if (String.IsNullOrWhiteSpace(updatedDirectoryPath))
      {
        throw new ArgumentNullException($"{nameof(RenameDirectoryAsync)} expects a value for {nameof(updatedDirectoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeFileClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryName: currentDirectoryPath);

      try
      {
        await dataLakeFileClient.RenameAsync(destinationPath: updatedDirectoryPath, cancellationToken);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    public virtual OperationStatus RenameDirectory(string currentDirectoryPath, string updatedDirectoryPath)
    {
      if (currentDirectoryPath == null)
      {
        throw new ArgumentNullException($"{nameof(RenameDirectory)} expects a value for {nameof(currentDirectoryPath)}... null argument was provided");
      }

      if (updatedDirectoryPath == null)
      {
        throw new ArgumentNullException($"{nameof(RenameDirectory)} expects a value for {nameof(updatedDirectoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeFileClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryName: currentDirectoryPath);

      try
      {
        dataLakeFileClient.Rename(destinationPath: updatedDirectoryPath);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.IsOperationSuccessful = false;
        operationStatus.OperationException = iException;
      }

      return operationStatus;
    }

    #endregion

    #region Delete Directories

    public virtual async Task<OperationStatus> DeleteDirectoryIfExistsAsync(string directoryPath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(DeleteDirectoryIfExistsAsync)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryPath);

      try
      {
        await dataLakeDirectoryClient.DeleteIfExistsAsync(cancellationToken);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.OperationException = iException;
        operationStatus.IsOperationSuccessful = false;
      }

      return operationStatus;
    }

    public OperationStatus DeleteDirectoryIfExists(string directoryPath)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(DeleteDirectoryIfExists)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryPath);

      try
      {
        dataLakeDirectoryClient.DeleteIfExists();
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.OperationException = iException;
        operationStatus.IsOperationSuccessful = false;
      }

      return operationStatus;
    }

    #endregion

    #region Create Directories

    public virtual async Task<OperationStatus> CreateDirectoryIfNotExistsAsync(string directoryPath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(CreateDirectoryIfNotExistsAsync)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryPath);

      try
      {
        var response = await dataLakeDirectoryClient.CreateIfNotExistsAsync(cancellationToken);
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.OperationException = iException;
        operationStatus.IsOperationSuccessful = false;
      }

      return operationStatus;
    }

    public OperationStatus CreateDirectoryIfNotExists(string directoryPath)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(CreateDirectoryIfNotExists)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var operationStatus = new OperationStatus();

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryPath);

      try
      {
        var response = dataLakeDirectoryClient.CreateIfNotExists();
        operationStatus.IsOperationSuccessful = true;
      }
      catch (Exception iException)
      {
        operationStatus.OperationException = iException;
        operationStatus.IsOperationSuccessful = false;
      }

      return operationStatus;
    }

    #endregion

    #region List Directory
    public virtual async Task<FileSystemDirectoryDto> GetDirectoryItemsAsync(string prefix, CancellationToken cancellationToken)
    {
      var result = new FileSystemDirectoryDto();
      var blobSegment = default(BlobResultSegment);
      var items = new List<IListBlobItem>();

      while (blobSegment == null || blobSegment.ContinuationToken != null)
      {
        blobSegment = await _cloudBlobContainer.ListBlobsSegmentedAsync(prefix, false, BlobListingDetails.Metadata, int.MaxValue, null, null, null, cancellationToken);
        items.AddRange(blobSegment.Results);
      }

      result.Path = prefix.Trim('/');
      result.DirectoryItems = NodeBuilder.GetDirectoryItems(items);

      return result;
    }

    public virtual FileSystemDirectoryDto GetDirectoryItems(string prefix)
    {
      var result = new FileSystemDirectoryDto { Path = prefix.Trim('/') };
      var blobSegment = default(BlobResultSegment);
      var items = new List<IListBlobItem>();

      while (blobSegment == null || blobSegment.ContinuationToken != null)
      {
        blobSegment = _cloudBlobContainer.ListBlobsSegmentedAsync(prefix, false, BlobListingDetails.Metadata, int.MaxValue, null, null, null).Result;
        items.AddRange(blobSegment.Results);
      }
     
      result.DirectoryItems = NodeBuilder.GetDirectoryItems(items);
      return result;
    }

    public virtual async Task<FileSystemDirectoryDto> GetDirectoryStructureAsync(string prefix, CancellationToken cancellationToken, bool includeItems = false, bool getSingleLevel = false)
    {
      cancellationToken.ThrowIfCancellationRequested();

      if (string.IsNullOrWhiteSpace(prefix))
      {
        throw new ArgumentNullException(nameof(prefix));
      }

      var result = default(FileSystemDirectoryDto);
      var blobSegment = default(BlobResultSegment);

      var items = new List<IListBlobItem>();

      while (blobSegment == null || blobSegment.ContinuationToken != null)
      {
        blobSegment = await _cloudBlobContainer.ListBlobsSegmentedAsync(prefix, !getSingleLevel, BlobListingDetails.Metadata, int.MaxValue, null, null, null, cancellationToken);
        items.AddRange(blobSegment.Results);
      }

      if (getSingleLevel)
      {
        result = NodeBuilder.PopulateDirectories(prefix, items, includeItems);
      }
      else
      {
        var filteredItems = items.Where(item =>
        {
          var blob = (item as CloudBlockBlob)?.Name;
          return blob != null && blob.Contains(StringHelper.TrimCharacter(prefix));
        });

        result = NodeBuilder.GenerateNode(prefix, filteredItems.ToList(), includeItems);
      }

      return result;
    }

    public virtual FileSystemDirectoryDto GetDirectoryStructure(string prefix, bool includeItems = false, bool getSingleLevel = false)
    {
      if (string.IsNullOrWhiteSpace(prefix))
      {
        throw new ArgumentNullException(nameof(prefix));
      }

      var result = default(FileSystemDirectoryDto);
      var blobSegment = default(BlobResultSegment);

      var items = new List<IListBlobItem>();            
      while (blobSegment == null || blobSegment.ContinuationToken != null)
      {
        blobSegment = _cloudBlobContainer.ListBlobsSegmentedAsync(prefix, !getSingleLevel, BlobListingDetails.Metadata, int.MaxValue, null, null, null).Result;
        items.AddRange(blobSegment.Results);
      }

      if (getSingleLevel)
      {
        result = NodeBuilder.PopulateDirectories(prefix, items, includeItems);
      }
      else
      {       
        var filteredItems = items.Where(item =>
        {
          var blob = (item as CloudBlockBlob)?.Name;
          return blob != null && blob.Contains(StringHelper.TrimCharacter(prefix));
        });

        result = NodeBuilder.GenerateNode(prefix, filteredItems.ToList(), includeItems);
      }
      
      return result;
    }

    #endregion

    #region Check Directory

    public virtual async Task<bool> IsDirectoryCreatedAsync(string directoryPath, CancellationToken cancellationToken)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(RenameDirectoryAsync)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryName: directoryPath);

      var result = await dataLakeDirectoryClient.ExistsAsync(cancellationToken);

      return result;
    }

    public virtual bool IsDirectoryCreated(string directoryPath)
    {
      if (String.IsNullOrWhiteSpace(directoryPath))
      {
        throw new ArgumentNullException($"{nameof(RenameDirectoryAsync)} expects a value for {nameof(directoryPath)}... null argument was provided");
      }

      var dataLakeDirectoryClient = _dataLakeFileSystemClient.GetDirectoryClient(directoryName: directoryPath);

      var result = dataLakeDirectoryClient.Exists();

      return result;
    }

    #endregion

    #endregion

    #region IDisposable

    private void ReleaseUnmanagedResources()
    {
    }

    public void Dispose()
    {
      ReleaseUnmanagedResources();
    }

    #endregion

    #region Helper Methods

    #region Metadata Helpers


    private FileMetadata UpdateFromFileMetadataDto(FileMetadataDto fileMetadataDto, FileMetadata fileMetadata)
    {
      if (fileMetadata == null)
      {
        throw new ArgumentNullException();
      }

      if (fileMetadataDto == null)
      {
        throw new ArgumentNullException();
      }

      fileMetadata.ContentType = string.IsNullOrEmpty(fileMetadataDto.ContentType) ? fileMetadata.ContentType : fileMetadataDto.ContentType;
      fileMetadata.EntityName = string.IsNullOrEmpty(fileMetadataDto.EntityName) ? fileMetadata.EntityName : fileMetadataDto.EntityName;
      fileMetadata.ThumbnailUrl = string.IsNullOrEmpty(fileMetadataDto.ThumbnailUrl) ? fileMetadata.ThumbnailUrl : fileMetadataDto.ThumbnailUrl;
      fileMetadata.ImageUrl = fileMetadataDto.ImageUrl ?? fileMetadata.ImageUrl;
      fileMetadata.MnemonicName = string.IsNullOrEmpty(fileMetadataDto.MnemonicName) ? fileMetadata.MnemonicName : fileMetadataDto.MnemonicName;
      fileMetadata.MnemonicId = string.IsNullOrEmpty(fileMetadataDto.MnemonicId) ? fileMetadata.MnemonicId : fileMetadataDto.MnemonicId;

      fileMetadata.LastUpdateByUserId = fileMetadataDto.LastUpdatedByUserId;
      fileMetadata.UpdatedTimestampUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
      fileMetadata.EntityId = fileMetadataDto.EntityId;
      fileMetadata.MetadataExtension = fileMetadataDto.MetadataExtension ?? fileMetadata.MetadataExtension;
      fileMetadata.Tags = fileMetadataDto.Tags == null || !fileMetadataDto.Tags.Any() ? fileMetadata.Tags : fileMetadataDto.Tags;

      return fileMetadata;
    }

    private FileMetadata FillFileMetadataAtCreation(FileMetadata fileMetadata, string fileLength)
    {
      if (String.IsNullOrWhiteSpace(fileLength))
      {
        throw new ArgumentNullException();
      }

      if (fileMetadata == null)
      {
        fileMetadata = new FileMetadata();
      }

      var fileId = Guid.NewGuid();

      // Set System Metadata
      fileMetadata.FileId = fileId;
      fileMetadata.SizeInBytes = fileLength;

      // Set Time Metadata
      var currentDateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString();
      fileMetadata.CreatedTimestampUtc = currentDateTime;
      fileMetadata.UpdatedTimestampUtc = currentDateTime;

      fileMetadata.CreatedByUserId = fileMetadata.LastUpdateByUserId;

      // Check for User Defined Metadata
      fileMetadata.MnemonicName = string.IsNullOrEmpty(fileMetadata.MnemonicName) ? string.Empty : fileMetadata.MnemonicName;
      fileMetadata.MnemonicId = string.IsNullOrEmpty(fileMetadata.MnemonicId) ? string.Empty : fileMetadata.MnemonicId;
      fileMetadata.EntityName = string.IsNullOrEmpty(fileMetadata.EntityName) ? string.Empty : fileMetadata.EntityName;
      fileMetadata.ContentType = string.IsNullOrEmpty(fileMetadata.ContentType) ? string.Empty : fileMetadata.ContentType;
      fileMetadata.Tags = fileMetadata.Tags == null || !fileMetadata.Tags.Any() ? default : fileMetadata.Tags;
      fileMetadata.MetadataExtension = fileMetadata.MetadataExtension == null ? null : fileMetadata.MetadataExtension;

      return fileMetadata;
    }

    #endregion

    #endregion
  }
}
