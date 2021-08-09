namespace Fixit.Core.Storage.Storage.Blob.Adapters
{
  public interface IBlobStorageServiceClientAdapter
  {
    public IBlobStorageClientAdapter GetContainerReference(string containerName);
  }
}