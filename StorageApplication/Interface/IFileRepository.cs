namespace StorageApplication.Interface
{
    public interface IFileRepository
    {
        Task<bool> UploadFile(IFormFile file);
        Task<byte[]> DownloadFile(string fileName);
        Task<bool> DeleteFile(string fileName);
    }
}
