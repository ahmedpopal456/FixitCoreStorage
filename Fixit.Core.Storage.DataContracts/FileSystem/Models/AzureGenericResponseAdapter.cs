using System;
using Azure;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Models
{

	public class AzureGenericResponseAdapter<T> : Response<T>
	{
		public override T Value { get; }

		public AzureGenericResponseAdapter(T value)
		{
			Value = value;
		}

		public override Response GetRawResponse()
		{
			throw new NotImplementedException();
		}
	}
}
