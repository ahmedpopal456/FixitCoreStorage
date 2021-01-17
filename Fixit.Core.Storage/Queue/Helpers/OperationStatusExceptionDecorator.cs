using System;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.Queue.Helpers
{
  public class OperationStatusExceptionDecorator : IExceptionDecorator<OperationStatus>
  {
    public async Task<OperationStatus> ExecuteOperationAsync(OperationStatus result, Func<Task> executingFunction)
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
