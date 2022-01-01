using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.Storage.Blob.Adapters.Internal
{
  public class BlobStorageClientAdapter : IBlobStorageClientAdapter
  {
    private readonly CloudBlobContainer _cloudBlobContainer;

    public BlobStorageClientAdapter(CloudBlobContainer cloudBlobContainer)
    {
      _cloudBlobContainer = cloudBlobContainer ?? throw new ArgumentNullException($"{nameof(BlobStorageClientAdapter)} expects a value for {nameof(cloudBlobContainer)}... null argument was provided");
    }

    public ICloudBlockBlobAdapter GetBlockBlobReference(string blobName)
    {
      return new CloudBlockBlobAdapter(_cloudBlobContainer.GetBlockBlobReference(blobName)) as ICloudBlockBlobAdapter;
    }

    public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken())
    {
      return _cloudBlobContainer.ListBlobsSegmentedAsync(prefix, useFlatBlobListing, blobListingDetails, maxResults, currentToken, options, operationContext, cancellationToken);
    }
  }
}
