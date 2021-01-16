using System;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class QueueMessageDto
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
