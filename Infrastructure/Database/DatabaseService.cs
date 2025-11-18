using SQLite;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace MauiTemplate.Infrastructure.Database;

public class DatabaseService<T> : IDatabaseService<T> where T : class, new()
{
    private readonly SQLiteAsyncConnection _database;
    private readonly ILogger<DatabaseService<T>> _logger;
    private bool _initialized;

    public DatabaseService(ILogger<DatabaseService<T>> logger)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "mauitemplate.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
            return;

        try
        {
            await _database.CreateTableAsync<T>();
            _initialized = true;
            _logger.LogInformation("Database table created for {Type}", typeof(T).Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize database for {Type}", typeof(T).Name);
            throw;
        }
    }

    public async Task<List<T>> GetAllAsync()
    {
        await InitializeAsync();
        return await _database.Table<T>().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        await InitializeAsync();
        return await _database.FindAsync<T>(id);
    }

    public async Task<int> InsertAsync(T item)
    {
        await InitializeAsync();
        return await _database.InsertAsync(item);
    }

    public async Task<int> UpdateAsync(T item)
    {
        await InitializeAsync();
        return await _database.UpdateAsync(item);
    }

    public async Task<int> DeleteAsync(T item)
    {
        await InitializeAsync();
        return await _database.DeleteAsync(item);
    }

    public async Task<List<T>> QueryAsync(Expression<Func<T, bool>> predicate)
    {
        await InitializeAsync();
        return await _database.Table<T>().Where(predicate).ToListAsync();
    }

    public async Task<int> DeleteAllAsync()
    {
        await InitializeAsync();
        return await _database.DeleteAllAsync<T>();
    }
}
