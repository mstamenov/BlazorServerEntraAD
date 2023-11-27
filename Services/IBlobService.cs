using Azure.Storage.Blobs;

namespace BlazorServerEntraAD.Services
{
    public interface IBlobService
    {
        BlobContainerClient GetBlobContainerClient(string containerName);
    }
}