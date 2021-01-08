using System;
using System.Collections.Generic;
using Fixit.Core.DataContracts;
using Fixit.Core.Storage.DataContracts.Queue;

namespace Fixit.Core.Storage.UnitTests.FakeDataProviders
{
  public class FakeMessageDtoSeeder : IFakeSeederAdapter<MessageDto>
  {
    public IList<MessageDto> SeedFakeDtos()
    {
      MessageDto firstMessage = new MessageDto()
      {
        Body = new BinaryData("Hello"),
        DequeueCount = 0,
        ExpiresOnUtc = DateTimeOffset.UtcNow,
        InsertedOnUtc = DateTimeOffset.UtcNow,
        MessageId = "123456",
        NextVisibleOnUtc = DateTimeOffset.UtcNow,
        PopReceipt = "123"
      };
      MessageDto secondMessage = null;

      return new List<MessageDto>
      {
        firstMessage,
        secondMessage
      };
    }
  }
}
