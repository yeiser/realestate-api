namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> UploadFileAsync(string base64);
    }
}
