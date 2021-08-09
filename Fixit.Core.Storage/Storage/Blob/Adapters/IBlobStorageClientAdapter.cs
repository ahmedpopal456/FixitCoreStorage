using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.Storage.Blob.Adapters
{
  public interface IBlobStorageClientAdapter
  {
    public ICloudBlockBlobAdapter GetBlockBlobReference(string blobName);

    public Task<BlobResultSegment> ListBlobsSegmentedAsync(string prefix, bool useFlatBlobListing, BlobListingDetails blobListingDetails, int? maxResults, BlobContinuationToken currentToken, BlobRequestOptions options, OperationContext operationContext, CancellationToken cancellationToken = new CancellationToken());
  }
}
