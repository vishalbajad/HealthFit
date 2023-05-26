namespace HealthFit_APIs.Model
{
    public class AppSettingsConfigurations
    {
        public string FileServerPath { get; set; }
        public string HealthFitDBConnectionString { get; set; }
        public string AzureBlobStoarageConnectionString { get; set; }
        public string BlobContainerName { get; set; }
        public string StorageSharedKeyCredential_AccountName { get; set; }
        public string StorageSharedKeyCredential_AccountKey { get; set; }
        public string JwtValidAudienceUrl { get; set; }
        public string JwtValidIssuer { get; set; }
        public string JwtSecret { get; set; }
        public string JwtAuthenticationUsername { get; set; }
        public string JwtAuthenticationPassword { get; set; }
    }
}
