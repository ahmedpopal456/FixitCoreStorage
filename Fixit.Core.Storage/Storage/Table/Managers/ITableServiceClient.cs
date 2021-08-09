using System;
using System.Threading;
using System.Threading.Tasks;
using Fixit.Core.DataContracts;

namespace Fixit.Core.Storage.Storage.Table.Managers
{
  public interface ITableServiceClient : IDisposable
  {
    Task<ITableStorage> CreateOrGetTableAsync(string tableName, CancellationToken cancellationToken);

    Task<ITableStorage> GetTableAsync(string tableName, CancellationToken cancellationToken);

    ITableStorage CreateOrGetTable(string tableName);

    ITableStorage GetTable(string tableName);

    Task<OperationStatus> DeleteTableIfExistsAsync(string tableName, CancellationToken cancellationToken);

    OperationStatus DeleteTableIfExists(string tableName);
  }
}