using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthFit.Utilities
{
    public class FileOperationsUtility
    {
        public static string ImageToBase64(string FileServerPath, string imagePath, string DefaultImage)
        {
            try
            {
                string imageFullPath = Path.Join(FileServerPath, imagePath);
                if (!File.Exists(imageFullPath))
                    imageFullPath = Path.Join(FileServerPath, DefaultImage);

                string fileExtension = Path.GetExtension(imageFullPath).TrimStart('.');
                string mimeType = GetMimeTypeFromExtension(fileExtension);
                string base64String = Convert.ToBase64String(File.ReadAllBytes(imageFullPath));
                string dataUri = $"data:{mimeType};base64,{base64String}";
                return dataUri;
            }
            catch
            {
                return Path.Join(FileServerPath, DefaultImage);
            }
        }
        public static byte[] PdfToBytes(string FileServerPath, string imagePath, string DefaultPdf)
        {
            try
            {
                string imageFullPath = Path.Join(FileServerPath, imagePath);
                if (!File.Exists(imageFullPath))
                    imageFullPath = Path.Join(FileServerPath, DefaultPdf);

                return File.ReadAllBytes(imageFullPath);
            }
            catch
            {
                return default(byte[]);
            }
        }
        private static string GetMimeTypeFromExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case "jpg":
                case "jpeg":
                    return "image/jpeg";
                case "png":
                    return "image/png";
                case "gif":
                    return "image/gif";
                case "txt":
                    return "text/plain";
                case "pdf":
                    return "application/pdf";
                // Add more cases for other file extensions and corresponding MIME types
                default:
                    return "application/octet-stream";
            }
        }
    }
}
