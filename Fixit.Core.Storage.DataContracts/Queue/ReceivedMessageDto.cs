using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class ReceivedMessageDto : OperationStatus
  {
    public MessageDto Message { get; set; }
  }
}
