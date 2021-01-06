using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class PeekedMessagesDto : OperationStatus
  {
    private IList<MessageDto> _messages;
    public IList<MessageDto> Messages
    {
      get => _messages ??= new List<MessageDto>();
      set => _messages = value;
    }
  }
}
