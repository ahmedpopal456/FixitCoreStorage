﻿using System;
using System.Runtime.Serialization;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Files
{
  [DataContract]
  public class FileUploadDto : OperationStatus
  {
    [DataMember]
    public Guid FileCreatedId{ get; set; }

    [DataMember] 
    public string FileCreatedPath { get; set;  }

    [DataMember]
    public string FileCreatedName { get; set; }

    [DataMember] 
    public long FileCreatedLength { get; set; }

    [DataMember]
    public ImageUrlDto ImageUrl { get; set; }

    [DataMember]
    public string FileCreatedTimestampUtc { get; set; }

  }
}
