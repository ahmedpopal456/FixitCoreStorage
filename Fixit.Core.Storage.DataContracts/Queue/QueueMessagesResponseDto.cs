using System.Collections.Generic;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class QueueMessagesResponseDto : OperationStatus
  {
    private IList<QueueMessageDto> _messages;
    public IList<QueueMessageDto> Messages
    {
      get => _messages ??= new List<QueueMessageDto>();
      set => _messages = value;
    }
    public QueueMessagesResponseDto() { }

    public QueueMessagesResponseDto(bool isOperationSuccessful) { IsOperationSuccessful = isOperationSuccessful; }
  }
}
