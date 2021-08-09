using System;
using System.IO;
using System.Web;

namespace Fixit.Core.Storage.DataContracts.Helpers
{
  public static class StringHelper
  {
    public static string TrimCharacter(string stringToTrim, char charToTrim = '/')
    {
      return stringToTrim.Trim(charToTrim);
    }

    public static string ToAzureDirectoryPath(string directory)
    {
      return directory.Replace('\\', '/');
    }

    public static void ObtainFilePathAndSasExpiryDateFromFileUrl(string fileUrl, string fileSystemName, out string filePath, out DateTime expiryDate)
    {
      string filePathFromThumbnailUrl = string.Empty;
      var sasExpiryDate = default(DateTime);

      if (!string.IsNullOrWhiteSpace(fileUrl))
      {
        var urlDecoded = HttpUtility.UrlDecode(fileUrl);
        var indexOfExpiryDate = urlDecoded.IndexOf("&se=");
        var indexOfSignedPermission = urlDecoded.IndexOf("&sp=");
        var indexOfEntityName = urlDecoded.IndexOf(fileSystemName);
        var indexOfInterogationMark = urlDecoded.IndexOf('?');

        if (indexOfEntityName > -1  && indexOfInterogationMark > -1 && indexOfExpiryDate > -1)
        {          
          var expiryDateInString = urlDecoded.Remove(indexOfSignedPermission).Substring(indexOfExpiryDate).Replace("&se=", "");
          DateTime.TryParse(expiryDateInString, out sasExpiryDate);

          var azureBlobShard = urlDecoded.Substring(indexOfInterogationMark);
          filePathFromThumbnailUrl = urlDecoded.Substring(indexOfEntityName)
                                               .Replace(fileSystemName, "")
                                               .Replace(azureBlobShard, "")                                               
                                               .Trim('/');
          var extension = Path.GetExtension(filePathFromThumbnailUrl);
          filePathFromThumbnailUrl = filePathFromThumbnailUrl.Replace(extension, $"{fileSystemName}{extension}");
        }
      }

      expiryDate = sasExpiryDate;
      filePath = filePathFromThumbnailUrl;
    }
  }
}