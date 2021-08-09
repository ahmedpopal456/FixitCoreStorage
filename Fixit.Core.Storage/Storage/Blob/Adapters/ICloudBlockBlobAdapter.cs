using System;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.Storage.Blob.Adapters
{
	public interface ICloudBlockBlobAdapter
	{
		Uri Uri { get; }

		string GetSharedAccessSignature(SharedAccessBlobPolicy policy);
	}
}
