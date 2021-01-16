using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.UnitTests.FakeDataProviders
{
  public class FakeMessageDtoSeeder : IFakeSeederAdapter<QueueMessageDto>
  {
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
