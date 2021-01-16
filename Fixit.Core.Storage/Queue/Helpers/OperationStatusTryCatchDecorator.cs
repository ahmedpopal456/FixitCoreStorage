using System;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.Queue.Helpers
{
  public class OperationStatusTryCatchDecorator
  {
    public async Task<T> ExecuteOperationAsync<T>(T result, Func<Task> executingFunction) where T : OperationStatus
    {
      try
      {
        await executingFunction.Invoke();
      }
      catch (Exception exception)
      {
        result.OperationException = exception;
        result.IsOperationSuccessful = false;
      }
      return result;
    }
  }
}
