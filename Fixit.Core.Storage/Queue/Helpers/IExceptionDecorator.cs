using System;
using System.Threading.Tasks;

namespace Fixit.Core.Storage.Queue.Helpers
{
  public interface IExceptionDecorator<T>
  {
    public Task<T> ExecuteOperationAsync(T result, Func<Task> executingFunction);
  }
}
