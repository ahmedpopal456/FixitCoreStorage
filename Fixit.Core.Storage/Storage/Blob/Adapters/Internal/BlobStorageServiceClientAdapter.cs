using System;
using Microsoft.Azure.Storage.Auth;
using Microsoft.Azure.Storage.Blob;

namespace Fixit.Core.Storage.Storage.Blob.Adapters.Internal
{
	public class BlobStorageServiceClientAdapter : IBlobStorageServiceClientAdapter
  {
    private readonly CloudBlobClient _cloudBlobClient;

    public BlobStorageServiceClientAdapter(Uri uri, StorageCredentials storageCredentials)
    {
      if (uri == null)
      {
        throw new ArgumentNullException($"{nameof(BlobStorageServiceClientAdapter)} expects a value for {nameof(uri)}... null argument was provided");
      }

      if (storageCredentials == null)
      {
        throw new ArgumentNullException($"{nameof(BlobStorageServiceClientAdapter)} expects a value for {nameof(storageCredentials)}... null argument was provided");
      }

      _cloudBlobClient = new CloudBlobClient(uri,storageCredentials);
    }

    public IBlobStorageClientAdapter GetContainerReference(string containerName)
    {
      return new BlobStorageClientAdapter(_cloudBlobClient.GetContainerReference(containerName)) as IBlobStorageClientAdapter;
    }
  }
}
