using System.Linq.Expressions;

namespace MauiTemplate.Infrastructure.Database;

public interface IDatabaseService<T> where T : class, new()
{
    Task InitializeAsync();
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<int> InsertAsync(T item);
    Task<int> UpdateAsync(T item);
    Task<int> DeleteAsync(T item);
    Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate);
    Task<int> DeleteAllAsync();
}
