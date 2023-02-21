using StorageApplication.Model;

namespace StorageApplication.Interface
{
    public interface ITableStorageRepository
    {
        Task<TableEntity> GetEntityAsync(string category, string id);
        Task<TableEntity> AddEntityAsync(TableEntity entity);
        Task<TableEntity> UpsertEntityAsync(TableEntity entity);
        Task DeleteEntityAsync(string category, string id);
    }
}
