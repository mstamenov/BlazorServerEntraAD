# Create a blazor server app with Entra AD identity

## 1. Create blazor server app

Create new blazor server app as follows:
```powershell
mkdir <new-project-folder>
cd <new-project-folder>
dotnet new blazorserver --auth SingleOrg --calls-graph
```

## 2. Add Entra Id information 
If **msidentity-app-sync** is not installed, install it globally:
```powershell
dotnet tool install --global msidentity-app-sync
```

Enter Entra AD username and organization Id:
```powershell
msidentity-app-sync --username <username/upn> --tenant-id <tenantID>
```
This will create app registration for provided Tenant with name **&lt;new-project-folder&gt;**.
API permission for Microsoft.Graph is already selected with User.Read - delegated permission.
Add Storage Account permission: user_impersonation with delegated type.
msidentity-app-sync will also create Client Secret and add it to User Secrets. Insensitive data for app registration is added in appsettings.json

In appsettings.json add the scope for storage account with space separator:
```json
"DownstreamApi": {
  "BaseUrl": "https://graph.microsoft.com/beta",
  "Scopes": "user.read https://storage.azure.com/user_impersonation"
}
```
Microsoft identity for Entra AD will be configured in program.cs by reading AzureAd section in appsettings.json and user secrets.

## 3. Get Azure Storage Account data
To work with blobs install nuget package:
```powershell
dotnet add package Azure.Storage.Blobs
```
To get container, blobs etc. may be used as follows:
```cs
public class BlobService : IBlobService
{
    private BlobServiceClient blobServiceClient;
    public BlobService(ITokenAcquisition tokenAcquisition)
    {
        TokenAcquisitionTokenCredential token = new TokenAcquisitionTokenCredential(tokenAcquisition);
        
        var blobUri = new Uri("https://<somestorage>.blob.core.windows.net/");
        this.blobServiceClient = new BlobServiceClient(blobUri, token);
    }

    public BlobContainerClient GetBlobContainerClient(string containerName)
    {
        return this.blobServiceClient.GetBlobContainerClient(containerName);
    }
}
```

