using System;
using System.Collections.Generic;
using System.Text;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class MessageDto
  {
    public string MessageId { get; set; }

    public BinaryData Body { get; set; }

    public DateTimeOffset? InsertedOnUtc { get; set; }

    public DateTimeOffset? ExpiresOnUtc { get; set; }

    public long DequeueCount { get; set; }
  }
}
