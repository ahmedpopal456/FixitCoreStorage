using AutoMapper;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;
using Fixit.Core.Storage.DataContracts.FileSystem.Models;

namespace Fixit.Core.Storage.FileSystem.Mappers
{
  public class FileSystemDtoMapper : Profile
  {
    public FileSystemDtoMapper()
    {    
      CreateMap<FileMetadataDto, FileMetadata>();
      CreateMap<FileMetadata, FileMetadataDto>();
    }
  }
}
