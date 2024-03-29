namespace ClinicManagementWebApp.Server.Infrastructure.Repositories
{
    public interface IRepository<T, TId> where TId : IEquatable<TId> where T : class, ITableObject<TId>
    {
        Task<IEnumerable<T>> GetAllAsync();

        Task<T?> GetByIdAsync(TId id);

        Task CreateAsync(T obj);

        Task ReplaceAsync(T obj);

        Task DeleteAsync(T obj);
    }
}
