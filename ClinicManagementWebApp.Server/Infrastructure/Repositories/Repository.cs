using Microsoft.EntityFrameworkCore;

namespace ClinicManagementWebApp.Server.Infrastructure.Repositories
{
    public class Repository<T, TId>(AppDbContext context) : IRepository<T, TId> where TId : IEquatable<TId> where T : class, ITableObject<TId>
    {
        private readonly AppDbContext _context = context ?? throw new ArgumentNullException(nameof(context));
        private readonly DbSet<T> _entities = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(TId id)
        {
            return await _entities.Where(o => o.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T obj)
        {
            _entities.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T obj)
        {
            _entities.Remove(obj);
            await _context.SaveChangesAsync();
        }

        public async Task ReplaceAsync(T obj)
        {
            /*            var entity = await _entities.Where(o => o.Id.Equals(obj.Id)).FirstOrDefaultAsync();
                        if (entity != null)
                        {
                            _context.Entry(entity).State = EntityState.Modified;

                        }*/

            await _context.SaveChangesAsync();
        }
    }
}
