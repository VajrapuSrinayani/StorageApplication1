using StorageApplication.Interface;
using StorageApplication.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace StorageApplication.Repository
{
    public class BlobRepository : IBlobRepository
    {
        private readonly CloudBlobContainer _container;

        //public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=charithastorage;AccountKey=sSXAoJNijEEyDePuazR02nj7z9Krca8LKtcxI49pIPL8o0NZOB5DyStVhqr7FL8g+4suwRz4EHSi+AStBH1JbQ==;EndpointSuffix=core.windows.net";
        //public static string containerName = "blob";

        public BlobRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StorageConnectionString");
            var containerName = configuration["BlobStorage:BlobName"];

            var blobClient = CloudStorageAccount.Parse(connectionString).CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public async Task<IEnumerable<Blob>> GetFileAsync()
        {
            var blobs = await _container.ListBlobsSegmentedAsync(null);
            return blobs.Results.Select(b => new Blob { BlobName = b.Uri.Segments.Last(), BlobUrl = b.Uri.AbsoluteUri });
        }

        public async Task<Blob> GetFileAsync(string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            if (await blob.ExistsAsync())
            {
                return new Blob
                {
                    BlobName = blobfileName,
                    BlobUrl = blob.Uri.AbsoluteUri
                };
            }
            return null;
        }

        public async Task<Blob> AddFileAsync(Stream stream, string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            await blob.UploadFromStreamAsync(stream);
            return new Blob
            {
                BlobName = blobfileName,
                BlobUrl = blob.Uri.AbsoluteUri
            };
        }

        //public async Task UpdateBlobAsync(Stream stream, string blobName)
        //{
        //    var blob = _container.GetBlockBlobReference(blobName);
        //    await blob.UploadFromStreamAsync(stream);
        //}

        public async Task DeleteFileAsync(string blobfileName)
        {
            var blob = _container.GetBlockBlobReference(blobfileName);
            await blob.DeleteAsync();
        }
    }
}
