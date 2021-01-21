using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class QueueMessageResponseDto : OperationStatus
  {
    public QueueMessageDto Message { get; set; }

    public QueueMessageResponseDto() { }

    public QueueMessageResponseDto(bool isOperationSuccessful) { IsOperationSuccessful = isOperationSuccessful; }
  }
}
