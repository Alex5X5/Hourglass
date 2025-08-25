namespace DatabaseUtil;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


public class BasicDatabaseAcessor<T>(string? path, DatabasePathFormat pathFormat, ConfigurationManager? config)
    : DatabaseAccessor<BasicDbContext<T>>(path, pathFormat, config) where T : class {

    public bool PrimaryKeyExistsInDatabase(object? key) =>
        base.PrimaryKeyValueExistsInDatabase<T>(key);

    public async Task<T?> QuerySingleByKeyAsync(object key) =>
        await base.QuerySingleByKeyAsync<T>(key);

    public T? QuerySingleByKeyBlocking(object key) =>
        base.QuerySingleByKeyBlocking<T>(key);

    public async Task<IEnumerable<T>?> QueryRangeByPropertiesAsyn(object filter) =>
        await base.QueryRangeByPropertiesAsync<T>(filter);

    public async Task<List<T>> QueryAllAsync() =>
        await base.QueryAllAsync<T>();

    public List<T> QueryAllBlocking() =>
        base.QueryAllBlocking<T>();

    public async Task<bool> AddAsync(T item, bool updateIfExists) =>
        await base.AddAsync(item, updateIfExists);

    public bool AddBlocking(T item, bool updateIfExists) =>
        base.AddBlocking<T>(item, updateIfExists);

    public async Task<bool> UpdateAsync(T updatedObject, bool createIfNotExists) =>
        await base.UpdateAsync<T>(updatedObject, updatedObject, createIfNotExists);

    public bool UpdateBlocking(T updatedObject, bool createIfNotExists) =>
        base.UpdateBlocking<T>(updatedObject, updatedObject, createIfNotExists);

    public async Task<bool> UpdateAsync(object filter, T value, bool createIfNotExists) =>
        await base.UpdateAsync<T>(filter, value, createIfNotExists);

    public bool UpdateBlocking(object filter, T value, bool createIfNotExists) =>
        base.UpdateBlocking<T>(filter, value, createIfNotExists);

    public async Task<bool> DeleteAsync(T item) =>
        await base.DeleteAsync<T>(item);

    public bool DeleteBlocking(T item) =>
        base.DeleteBlocking<T>(item);

    public async Task<bool> DeleteByKeyAsync(object? key) =>
        await base.DeleteByKeyAsync<T>(key);

    public bool DeleteByKeyBlocking(object? key) =>
        base.DeleteByKeyBlocking<T>(key);
}


public class BasicDbContext<T>(DbContextOptions options) : DbContext(options) where T : class {
    public DbSet<T> Items { get; set; }
}
