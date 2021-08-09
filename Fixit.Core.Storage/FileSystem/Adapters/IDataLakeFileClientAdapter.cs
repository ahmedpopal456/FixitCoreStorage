using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace Fixit.Core.Storage.FileSystem.Adapters
{
  public interface IDataLakeFileClientAdapter
  {
    public string Path { get; }

    public string Name { get; }

    public Response<bool> DeleteIfExists();
    public Task<Response<bool>> DeleteIfExistsAsync(CancellationToken cancellationToken);

    public Response<PathInfo> Create();

    public Task<Response<PathInfo>> CreateAsync(CancellationToken cancellationToken);

    public Response Delete();

    public Task<Response> DeleteAsync(CancellationToken cancellationToken);

    public Response<DataLakeFileClient> Rename(string destinationPath);

    public Task<Response<DataLakeFileClient>> RenameAsync(string destinationPath, CancellationToken cancellationToken);

    public IDictionary<string, string> GetMetadata();

    public Task<IDictionary<string, string>> GetMetadataAsync(CancellationToken cancellationToken);

    public Response<PathInfo> SetMetadata(IDictionary<string, string> metadata);

    public Task<Response<PathInfo>> SetMetadataAsync(IDictionary<string, string> metadata, CancellationToken cancellationToken);

    public Response<PathInfo> Upload(Stream content, bool overwrite = false);

    public Task<Response<PathInfo>> UploadAsync(Stream content, CancellationToken cancellationToken, bool overwrite = false);

    public Response<bool> Exists(CancellationToken cancellationToken = new CancellationToken());

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken = new CancellationToken());

    public Task<Response> ReadToAsync(Stream stream, CancellationToken cancellationToken);
  }
}