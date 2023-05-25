using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using System.IO;
using Azure.Identity;
using Azure.Storage.Sas;
using Azure.Storage;

namespace HealthFit_APIs
{
    public class AzureFileUpload
    {
        static public string UploadBlob(string filepath)
        {
            string connectionString = "";

            // intialize BobClient 
            Azure.Storage.Blobs.BlobClient blobClient = new Azure.Storage.Blobs.BlobClient(connectionString: connectionString, blobContainerName: "healthfitcontainer", blobName: Path.GetFileName(filepath));

            // upload the file
            blobClient.Upload(filepath);

            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = "healthfitcontainer",
                BlobName = Path.GetFileName(filepath),
                Resource = "b", // "b" for blob, "c" for container
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
            };
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("healthfitcontainer");
            string sasToken = sasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential("healthfit", "")).ToString();


            return blobClient.Uri + "?" + sasToken;
        }
    }
}