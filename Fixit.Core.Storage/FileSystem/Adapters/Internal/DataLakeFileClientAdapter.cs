using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Files.DataLake;
using Azure.Storage.Files.DataLake.Models;

namespace Fixit.Core.Storage.FileSystem.Adapters.Internal
{
  internal class DataLakeFileClientAdapter  : IDataLakeFileClientAdapter
  {
    private readonly DataLakeFileClient _dataLakeFileClient;

    public string Path => _dataLakeFileClient.Path;
    public string Name => _dataLakeFileClient.Name;

    public DataLakeFileClientAdapter(DataLakeFileClient iDataLakeFileClient)
    {
      _dataLakeFileClient = iDataLakeFileClient ?? throw new ArgumentNullException($"{nameof(DataLakeDirectoryClientAdapter)} expects a value for {nameof(iDataLakeFileClient)}... null argument was provided");
    }

    public Response<bool> DeleteIfExists()
    {
      return _dataLakeFileClient.DeleteIfExists();
    }

    public Task<Response<bool>> DeleteIfExistsAsync(CancellationToken cancellationToken)
    {
      return _dataLakeFileClient.DeleteIfExistsAsync(cancellationToken:cancellationToken);
    }

    public async Task<Response> ReadToAsync(Stream stream, CancellationToken cancellationToken)
    {
      return await _dataLakeFileClient.ReadToAsync(destination:stream,cancellationToken:cancellationToken);
    }

    public Response<PathInfo> Create()
    {
      return _dataLakeFileClient.Create();
    }

    public Task<Response<PathInfo>> CreateAsync(CancellationToken cancellationToken)
    {
      return _dataLakeFileClient.CreateAsync(cancellationToken:cancellationToken);
    }

    public Response Delete()
    {
      return _dataLakeFileClient.Delete();
    }

    public Task<Response> DeleteAsync(CancellationToken cancellationToken)
    {
      return _dataLakeFileClient.DeleteAsync(cancellationToken:cancellationToken);
    }

    public Response<DataLakeFileClient> Rename(string destinationPath)
    {
      return _dataLakeFileClient.Rename(destinationPath);
    }

    public Task<Response<DataLakeFileClient>> RenameAsync(string destinationPath, CancellationToken cancellationToken)
    {
      return _dataLakeFileClient.RenameAsync(destinationPath, cancellationToken:cancellationToken);
    }

    public IDictionary<string, string> GetMetadata()
    {
      return _dataLakeFileClient.GetProperties().Value.Metadata;
    }

    public async Task<IDictionary<string, string>> GetMetadataAsync(CancellationToken cancellationToken)
    {
      var wMetadata = await _dataLakeFileClient.GetPropertiesAsync(cancellationToken: cancellationToken);
      return wMetadata.Value.Metadata;
    }

    public Response<PathInfo> SetMetadata(IDictionary<string, string> metadata)
    {
      return _dataLakeFileClient.SetMetadata(metadata);
    }

    public Task<Response<PathInfo>> SetMetadataAsync(IDictionary<string, string> metadata, CancellationToken cancellationToken)
    {
      return _dataLakeFileClient.SetMetadataAsync(metadata, cancellationToken:cancellationToken);
    }

    public Response<PathInfo> Upload(Stream content, bool overwrite = false)
    {
      return _dataLakeFileClient.Upload(content, overwrite);
    }

    public Task<Response<PathInfo>> UploadAsync(Stream content, CancellationToken cancellationToken, bool overwrite = false)
    {
      return _dataLakeFileClient.UploadAsync(content, overwrite, cancellationToken);
    }

    public Response<bool> Exists(CancellationToken cancellationToken = new CancellationToken())
    {
      return _dataLakeFileClient.Exists(cancellationToken);
    }

    public Task<Response<bool>> ExistsAsync(CancellationToken cancellationToken = new CancellationToken())
    {
      return _dataLakeFileClient.ExistsAsync(cancellationToken);
    }
  }
}