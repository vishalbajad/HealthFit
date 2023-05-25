using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Azure.Storage;

namespace HealthFit.AzureCompoenents
{
    public class AzureFileUploadComponent
    {
        private string _azureBlobStoarageConnectionString;
        private string _blobContainerName;
        private string _storageSharedKeyCredential_AccountName;
        private string _storageSharedKeyCredential_AccountKey;

        public AzureFileUploadComponent(string azureBlobStoarageConnectionString, string blobContainerName, string storageSharedKeyCredential_AccountName, string storageSharedKeyCredential_AccountKey)
        {
            _azureBlobStoarageConnectionString = azureBlobStoarageConnectionString;
            _blobContainerName = blobContainerName;
            _storageSharedKeyCredential_AccountName = storageSharedKeyCredential_AccountName;
            _storageSharedKeyCredential_AccountKey = storageSharedKeyCredential_AccountKey;
        }
        public string UploadBlob(string filepath)
        {
            Azure.Storage.Blobs.BlobClient blobClient = new Azure.Storage.Blobs.BlobClient(connectionString: _azureBlobStoarageConnectionString, blobContainerName: _blobContainerName, blobName: filepath);
            blobClient.Upload(filepath);
            return blobClient.Uri.AbsoluteUri;
        }
        public string GetSasReadOnlyAccessToken(string filename)
        {
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = _blobContainerName,
                BlobName = filename,
                Resource = "b",
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            BlobServiceClient blobServiceClient = new BlobServiceClient(_azureBlobStoarageConnectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(_blobContainerName);
            string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(_storageSharedKeyCredential_AccountName, _storageSharedKeyCredential_AccountKey)).ToString();

            return "?" + sasToken;

        }
    }
}