using System;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.Storage.Blob.Adapters.Internal
{
	public class CloudBlockBlobAdapter : ICloudBlockBlobAdapter
  {
    private readonly CloudBlockBlob _cloudBlockBlob;

    Uri ICloudBlockBlobAdapter.Uri => _cloudBlockBlob.Uri;

    public CloudBlockBlobAdapter(CloudBlockBlob cloudBlockBlob)
    {
      _cloudBlockBlob = cloudBlockBlob ?? throw new ArgumentNullException($"{nameof(BlobStorageClientAdapter)} expects a value for {nameof(cloudBlockBlob)}... null argument was provided");
    }

    string ICloudBlockBlobAdapter.GetSharedAccessSignature(SharedAccessBlobPolicy policy)
    {
      return _cloudBlockBlob.GetSharedAccessSignature(policy);
    }
  }
}
