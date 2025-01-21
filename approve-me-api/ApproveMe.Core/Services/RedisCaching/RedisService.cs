using StackExchange.Redis;

namespace ApproveMe.Core.Services.RedisCaching;

public class RedisService
{
    private readonly IDatabase _db;
    private readonly ConnectionMultiplexer _redis;

    public RedisService(string connectionString)
    {
        _redis = ConnectionMultiplexer.Connect(connectionString);
        _db = _redis.GetDatabase();
    }

    public Task<bool> SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        return _db.StringSetAsync(key, value, expiry);
    }

    public async Task<string?> GetAsync(string key)
    {
        var value = await _db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public Task<bool> DeleteAsync(string key)
    {
        return _db.KeyDeleteAsync(key);
    }

    public Task<bool> KeyExistsAsync(string key)
    {
        return _db.KeyExistsAsync(key);
    }

    public Task<long> ListPushAsync(string listKey, string value)
    {
        return _db.ListRightPushAsync(listKey, value);
    }

    public async Task<List<string>> ListGetAllAsync(string listKey)
    {
        var values = await _db.ListRangeAsync(listKey);
        var result = new List<string>();
        foreach (var value in values)
        {
            result.Add(value.ToString());
        }
        return result;
    }

    public Task<long> ListRemoveAsync(string listKey, string value)
    {
        return _db.ListRemoveAsync(listKey, value);
    }

    public Task<bool> HashSetAsync(string hashKey, string field, string value)
    {
        return _db.HashSetAsync(hashKey, field, value);
    }

    public async Task<string?> HashGetAsync(string hashKey, string field)
    {
        var value = await _db.HashGetAsync(hashKey, field);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task<Dictionary<string, string>> HashGetAllAsync(string hashKey)
    {
        var entries = await _db.HashGetAllAsync(hashKey);
        var result = new Dictionary<string, string>();
        foreach (var entry in entries)
        {
            result[entry.Name.ToString()] = entry.Value.ToString();
        }
        return result;
    }

    public Task<bool> HashDeleteAsync(string hashKey, string field)
    {
        return _db.HashDeleteAsync(hashKey, field);
    }

    public void Dispose()
    {
        _redis.Dispose();
    }
}