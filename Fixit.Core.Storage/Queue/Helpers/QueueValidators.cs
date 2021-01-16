using System;
using System.Runtime.CompilerServices;

namespace Fixit.Core.Storage.Queue.Helpers
{
  public static class QueueValidators
  {
    /// <summary>
    /// Throws an ArgumentNullException if the given values are invalid
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="popReceipt"></param>
    /// <param name="callerName"></param>
    public static void ValidateMessageIdAndPopReceipt(string messageId, string popReceipt, [CallerMemberName] string callerName = "")
    {
      if (string.IsNullOrWhiteSpace(messageId))
      {
        throw new ArgumentNullException($"{callerName} expects a valid value for {nameof(messageId)}");
      }
      if (string.IsNullOrWhiteSpace(popReceipt))
      {
        throw new ArgumentNullException($"{callerName} expects a valid value for {nameof(popReceipt)}");
      }
    }

    public static bool IsSuccessStatusCode(int statusCode)
    {
      return (statusCode >= 200) && (statusCode <= 299);
    }
  }
}
