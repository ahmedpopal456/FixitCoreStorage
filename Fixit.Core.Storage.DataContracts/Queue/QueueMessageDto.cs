using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts.Seeders;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class QueueMessageDto : IFakeSeederAdapter<QueueMessageDto>
  {
    public string MessageId { get; set; }

    public BinaryData Body { get; set; }

    public DateTimeOffset? InsertedOnUtc { get; set; }

    public DateTimeOffset? ExpiresOnUtc { get; set; }

    public long DequeueCount { get; set; }
    public string PopReceipt { get; set; }

    public DateTimeOffset? NextVisibleOnUtc { get; set; }

    public IList<QueueMessageDto> SeedFakeDtos()
    {
      QueueMessageDto firstMessage = new QueueMessageDto()
      {
        Body = new BinaryData("Hello"),
        DequeueCount = 0,
        ExpiresOnUtc = DateTimeOffset.UtcNow,
        InsertedOnUtc = DateTimeOffset.UtcNow,
        MessageId = "123456",
        NextVisibleOnUtc = DateTimeOffset.UtcNow,
        PopReceipt = "123"
      };
      QueueMessageDto secondMessage = null;

      return new List<QueueMessageDto>
      {
        firstMessage,
        secondMessage
      };
    }
  }
}
