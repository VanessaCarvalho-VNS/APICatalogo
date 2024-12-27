using System.Linq.Expressions;

namespace APICatalogo.Repositories
{
    public interface IRepository<T>
    {
        // Cuidado para não violar o principio SOLID (ISP)-> Interface Segregation Principle 
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetAsync(Expression<Func<T, bool>> predicate);
        T Create(T entity);
        T Update(T entity);
        T Delete(T entity);
    }
}
