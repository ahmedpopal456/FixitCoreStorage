using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.Table.Mediators
{
  public interface ITableServiceClientMediator
  {
    Task<ITableClientMediator> CreateTableIfNotExistsAsync(string tableName, CancellationToken cancellationToken);

    Task<OperationStatus> DeleteTableIfExistsAsync(string tableName, CancellationToken cancellationToken);

    Task<ITableClientMediator> GetTableAsync(string tableName, CancellationToken cancellationToken);
  }
}
