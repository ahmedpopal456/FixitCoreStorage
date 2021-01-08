using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class MessageDto
  {
    public string MessageId { get; set; }

    public BinaryData Body { get; set; }

    public DateTimeOffset? InsertedOnUtc { get; set; }

    public DateTimeOffset? ExpiresOnUtc { get; set; }

    public long DequeueCount { get; set; }
    public string PopReceipt { get; set; }

    public DateTimeOffset? NextVisibleOnUtc { get; set; }
  }
}
