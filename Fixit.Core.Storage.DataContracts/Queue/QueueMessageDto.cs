using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.DataContracts.Queue
{
  public class QueueMessageDto : MessageDto
  {
    public string PopReceipt { get; set; }

    public DateTimeOffset? NextVisibleOnUtc { get; set; }
  }
}
