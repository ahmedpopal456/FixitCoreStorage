using System;
using System.Collections.Generic;
using System.Text;
using Fixit.Core.Storage.Table.Adapters;

namespace Fixit.Core.Storage.Table.Mediators.Internal
{
  internal class TableClientMediator : ITableClientMediator
  {
    private ITableClientAdapter _tableClientAdapter;

    public TableClientMediator(ITableClientAdapter tableClientAdapter)
    {
      _tableClientAdapter = tableClientAdapter ?? throw new ArgumentNullException($"{nameof(TableClientMediator)} expects a value for {nameof(tableClientAdapter)}... null argument was provided");
    }
  }
}
