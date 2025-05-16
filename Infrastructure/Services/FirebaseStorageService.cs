using Application.Interfaces;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using Shared.Settings;

namespace Infrastructure.Services
{
    public class FirebaseStorageService : IFileStorageService
    {
        private readonly FirebaseSettings _settings;
        private readonly StorageClient _storageClient;

        public FirebaseStorageService(IOptions<FirebaseSettings> options)
        {
            _settings = options.Value;

            var firebaseKeyJson = Environment.GetEnvironmentVariable("FIREBASE_KEY_JSON");

            if (string.IsNullOrWhiteSpace(firebaseKeyJson))
            {
                throw new InvalidOperationException("La variable de entorno FIREBASE_KEY_JSON no está definida.");
            }

            var credential = GoogleCredential.FromJson(firebaseKeyJson);

            _storageClient = StorageClient.Create(credential);
        }

        public async Task<string> UploadFileAsync(string base64)
        {
            var base64Data = base64.Split(',').Last();
            var bytes = Convert.FromBase64String(base64Data);
            var extension = GetImageExtensionFromBytes(bytes);
            using var stream = new MemoryStream(bytes);

            var objectName = $"{Guid.NewGuid()}{extension}";

            var result = await _storageClient.UploadObjectAsync(
                bucket: _settings.Bucket,
                objectName: objectName,
                contentType: GetContentType($"{objectName}{extension}"),
                source: stream
            );

            return GeneratePublicUrl(objectName);
        }

        public static string GetImageExtensionFromBytes(byte[] imageBytes)
        {
            // JPEG
            if (imageBytes[0] == 0xFF && imageBytes[1] == 0xD8)
                return ".jpg";

            // PNG
            if (imageBytes[0] == 0x89 && imageBytes[1] == 0x50 && imageBytes[2] == 0x4E && imageBytes[3] == 0x47)
                return ".png";

            throw new NotSupportedException("Formato de imagen desconocido.");
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                _ => "application/octet-stream"
            };
        }

        private string GeneratePublicUrl(string objectName)
        {
            return $"https://firebasestorage.googleapis.com/v0/b/{_settings.Bucket}/o/{objectName}?alt=media";
        }
    }
}
