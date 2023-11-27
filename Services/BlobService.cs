using Azure.Storage.Blobs;
using Microsoft.Identity.Web;

namespace BlazorServerEntraAD.Services;

public class BlobService : IBlobService
{
    private BlobServiceClient blobServiceClient;
    public BlobService(ITokenAcquisition tokenAcquisition, IConfiguration configuration)
    {
        TokenAcquisitionTokenCredential token = new TokenAcquisitionTokenCredential(tokenAcquisition);
        var blobUrl = configuration.GetValue<string>("AzureAd:BlobServiceUrl");
        var blobUri = new Uri(blobUrl!);
        this.blobServiceClient = new BlobServiceClient(blobUri, token);
    }

    public BlobContainerClient GetBlobContainerClient(string containerName)
    {
        return this.blobServiceClient.GetBlobContainerClient(containerName);
    }
}
