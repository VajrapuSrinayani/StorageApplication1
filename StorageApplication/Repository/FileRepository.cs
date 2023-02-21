using Azure.Storage.Files.Shares;
using Azure;
using StorageApplication.Interface;

namespace StorageApplication.Repository
{
    public class FileRepository: IFileRepository
    {
        private readonly string connectionString;
        public static string fileShare;
        private readonly ShareClient shareClient;

        public FileRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("StorageConnectionString");
            fileShare = configuration.GetValue<string>("FileStorage:FileShare");
            shareClient = new ShareClient(connectionString, fileShare);
        }

        //Retrieve the file
        public async Task<byte[]> DownloadFile(string fileName)
        {
            var shareDirectoryClient = shareClient.GetDirectoryClient("");
            var shareFileClient = shareDirectoryClient.GetFileClient(fileName);

            var response = await shareFileClient.DownloadAsync();
            using var memoryStream = new MemoryStream();
            await response.Value.Content.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        //Create the file
        public async Task<bool> UploadFile(IFormFile file)
        {
            var shareDirectoryClient = shareClient.GetDirectoryClient("");
            var shareFileClient = shareDirectoryClient.GetFileClient(file.FileName);

            using (var stream = file.OpenReadStream())
            {
                shareFileClient.Create(stream.Length);
                await shareFileClient.UploadRangeAsync(new HttpRange(0, file.Length), stream);
            }
            return true;
        }

        //Delete the file
        public async Task<bool> DeleteFile(string fileName)
        {
            var shareDirectoryClient = shareClient.GetDirectoryClient("");
            var shareFileClient = shareDirectoryClient.GetFileClient(fileName);

            await shareFileClient.DeleteAsync();
            return true;
        }
    }
}