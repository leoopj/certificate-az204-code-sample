using StackExchange.Redis;

string connectionString = "{myConnectionStringRedis}";

using (var cache = ConnectionMultiplexer.Connect(connectionString))
{
    IDatabase db = cache.GetDatabase();
    db.StringSet("key1", "value1");

    string getValue = await db.StringGetAsync("key1");
    Console.WriteLine(getValue);

    bool keyExists = await db.KeyExistsAsync("key1");
    Console.WriteLine(keyExists);

    db.KeyDelete("key1");
    keyExists = await db.KeyExistsAsync("key1");
    Console.WriteLine(keyExists);

    var execute = await db.ExecuteAsync("ping");
    Console.WriteLine(execute);

    var setValue = await db.StringSetAsync("key2", "value2");
    Console.WriteLine(setValue);

    var getValue2 = await db.StringGetAsync("key2");
    Console.WriteLine(getValue2);
}