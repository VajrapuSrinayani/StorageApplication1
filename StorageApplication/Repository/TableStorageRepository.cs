using StorageApplication.Interface;
using Azure.Data.Tables;
using Azure;

namespace StorageApplication.Repository
{
    public class TableStorageRepository: ITableStorageRepository
    {
        private readonly IConfiguration _configuration;
        public TableStorageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private async Task<TableClient> GetTableClient()
        {
            var serviceClient = new TableServiceClient(_configuration["ConnectionStrings:StorageConnectionString"]);
            var tableClient = serviceClient.GetTableClient(_configuration["TableStorage:TableName"]);
            await tableClient.CreateIfNotExistsAsync();
            return tableClient;
        }

        public async Task<Model.TableEntity> GetEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            try
            {
                return await tableClient.GetEntityAsync<Model.TableEntity>(category, id);
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == 404)
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<Model.TableEntity> AddEntityAsync(Model.TableEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.AddEntityAsync(entity);
            return entity;
        }
        public async Task<Model.TableEntity> UpsertEntityAsync(Model.TableEntity entity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(entity);
            return entity;
        }
        public async Task<string> DeleteEntityAsync(string category, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(category, id);
            return "Deleted the item from the Table";
        }
    }
}
