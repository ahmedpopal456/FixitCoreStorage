using System.Collections.Generic;
using Fixit.Core.Storage.DataContracts.FileSystem.Files;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Comparers
{
  public class FileTagDtoComparer : IEqualityComparer<FileTagDto>
  {
    public FileTagDtoComparer()
    {

    }

    public bool Equals(FileTagDto x, FileTagDto y)
    {
      bool result = false; 

      if (x.Name == y.Name)
      {
        result = true; 
      }

      return result;
    }

    public int GetHashCode(FileTagDto obj)
    {
      return obj.GetHashCode();
    }
  }
}
