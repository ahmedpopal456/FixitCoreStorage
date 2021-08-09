using System;
using System.Collections.Generic;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Jobs.Responses
{
  public class FileDownloadJobResponseDto
  {
    public Guid JobId { get; set; }

    public string CreatedTimestampUTC { get; set; }

    public IEnumerable<string> FilePathsRequested { get; set; }
  }
}