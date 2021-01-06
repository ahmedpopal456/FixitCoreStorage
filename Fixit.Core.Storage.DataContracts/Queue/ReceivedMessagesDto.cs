using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class ReceivedMessagesDto : OperationStatus
  {
    private IList<QueueMessageDto> _messages;
    public IList<QueueMessageDto> Messages
    {
      get => _messages ??= new List<QueueMessageDto>();
      set => _messages = value;
    }
  }
}
