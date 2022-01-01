using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Fixit.Core.Storage.DataContracts.FileSystem.Directories;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Fixit.Core.Storage.DataContracts.FileSystem.Models;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.DataContracts.Helpers
{
  public static class NodeBuilder
  {

    #region Method Called by manager
    public static FileSystemDirectoryDto GenerateNode(string prefix, ICollection<IListBlobItem> listBlobs, bool includeItems)
    {
      var dictionary = GetDictionnaryForNodeGeneration(listBlobs);

      var result = new FileSystemDirectoryDto { Path = prefix.Trim('/') };

      foreach (var item in dictionary.Values)
      {
        var proposedParent = default(object);
        if (item.GetType() == typeof(FileSystemDirectoryDto))
        {
          //if it's a directoryDto we add it to the Directories of it's parent
          //and add the parent to the parent property of the directoryDto
          var directoryDto = item as FileSystemDirectoryDto;
          if (dictionary.TryGetValue(directoryDto.ParentUri, out proposedParent))
          {
            var castProposedParent = proposedParent as FileSystemDirectoryDto;
            directoryDto.ParentDirectory = castProposedParent;
            castProposedParent.Directories.Add(directoryDto);
          }
        }
        else if (item.GetType() == typeof(FileSystemFileDto) && includeItems)
        {
          //if it's a fileDto we add it to the DirectoryItems of it's parent
          var fileDto = item as FileSystemFileDto;
          if (dictionary.TryGetValue(fileDto.ParentUri, out proposedParent))
          {
            var castProposedParent = proposedParent as FileSystemDirectoryDto;
            castProposedParent.DirectoryItems.Add(fileDto);
          }
        }
      }
     
      result.Directories = dictionary.Values.OfType<FileSystemDirectoryDto>().Where(d => d.ParentDirectory is null).ToList();
      result.DirectoryItems = dictionary.Values.OfType<FileSystemFileDto>().Where(f => f.ParentUri.EndsWith(prefix.Trim('/'))).ToList();
      return result;
    }

    public static FileSystemDirectoryDto PopulateDirectories(string prefix, ICollection<IListBlobItem> blobs, bool includeItems = false)
    {
      if (blobs == null)
      {
        throw new ArgumentNullException(nameof(blobs));
      }

      var result = new FileSystemDirectoryDto { Path = prefix.Trim('/') };
      
      //Call method to get directory structure as FileSystemDirectoryDto
      var structure = GenerateDirectoryStructure(blobs, result, includeItems);
      if (structure != null)
      {
        //If the result of the method is not null add it to the list of directories
        result.Directories = structure;
      }

      return result;
    }

    public static List<FileSystemFileDto> GetDirectoryItems(ICollection<IListBlobItem> blobs)
    {
      if (blobs == null)
      {
        throw new ArgumentNullException(nameof(blobs));
      }

      var result = new List<FileSystemFileDto>();

      var files = blobs.OfType<CloudBlockBlob>().ToList();
      if (files.Any())
      {
        foreach (var item in files)
        {
          //Create the fileDto
          var fileDto = CreateFileSystemFileDto(item);
          //Add it to the list if it's not null
          if (fileDto != null)
          {
            result.Add(fileDto);
          }
        }
      }

      return result;
    }
    #endregion

    public static Dictionary<string, Object> GetDictionnaryForNodeGeneration(ICollection<IListBlobItem> listBlobs)
    {
      var dictionary = new Dictionary<string, Object>();
      foreach (var blob in listBlobs)
      {
        var cloudBlob = blob as CloudBlockBlob;
        //Verify if the cloudBlockBlob is a File
        if (!string.IsNullOrWhiteSpace(Path.GetExtension(cloudBlob?.Name)))
        {
          var item = CreateFileSystemFileDto(cloudBlob);
          if (item != null)
          {
            dictionary.Add(StringHelper.TrimCharacter(cloudBlob.Uri.ToString()), item);
          }
        }
        else
        {
          //The cloudBlockBlob is a directory
          dictionary.Add(StringHelper.TrimCharacter(cloudBlob.Uri.ToString()), new FileSystemDirectoryDto
          {
            Uri = cloudBlob?.Uri.ToString(),
            ParentUri = StringHelper.TrimCharacter(cloudBlob?.Parent.Uri.ToString()),
            Path = cloudBlob?.Name
          });
        }
      }

      return dictionary;
    }

    public static FileSystemFileDto CreateFileSystemFileDto(CloudBlockBlob blob)
    {
      var dto = default(FileSystemFileDto);

      var metadataItem = new FileMetadata().FromDictionary(blob.Metadata);
      if (!Guid.Empty.Equals(metadataItem.FileId))
      {
        if (metadataItem.ImageUrl != null)
        {
          metadataItem.ImageUrl.Url = HttpUtility.UrlDecode(metadataItem.ImageUrl.Url);
        }

        dto = new FileSystemFileDto()
        {
          Id = metadataItem.FileId.ToString(),
          ContentType = metadataItem.ContentType,
          ThumbnailUrl = HttpUtility.UrlDecode(metadataItem?.ThumbnailUrl),
          Name = Path.GetFileName(blob?.Name),
          FileTags = metadataItem.Tags,
          MnemonicId = metadataItem.MnemonicId,
          MnemonicName = metadataItem.MnemonicName,
          Uri = blob?.Uri.ToString(),
          ParentUri = StringHelper.TrimCharacter(blob?.Parent.Uri.ToString()),
          EntityId = metadataItem.EntityId,
          EntityName = metadataItem.EntityName,
          ImageUrl = metadataItem.ImageUrl        
        };
      }

      return dto;
    }

    public static List<FileSystemDirectoryDto> GenerateDirectoryStructure(ICollection<IListBlobItem> listBlobs, FileSystemDirectoryDto parentDirectory, bool includeItems = false)
    {

      //If listBlobs count = 0 return empty List
      if (!listBlobs.Any())
      {
        return new List<FileSystemDirectoryDto>(); ;
      }

      var result = new List<FileSystemDirectoryDto>();

      //Add items directly into the parentDirectory
      if (includeItems)
      {
        var items = listBlobs.OfType<CloudBlockBlob>().ToList();
        parentDirectory.DirectoryItems = items.Select(i => CreateFileSystemFileDto(i)).Where(f => f != null).ToList();
      }

      var directories = listBlobs.OfType<CloudBlobDirectory>().ToList();
      foreach (var item in directories)
      {
        var directory = new FileSystemDirectoryDto
        {
          Path = StringHelper.TrimCharacter(item.Prefix),
          ParentDirectory = parentDirectory
        };
        result.Add(directory);
      }

      return result;
    }
  }
}
