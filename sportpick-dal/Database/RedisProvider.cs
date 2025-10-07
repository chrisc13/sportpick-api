using StackExchange.Redis;

public class RedisProvider
{
    private readonly IConnectionMultiplexer _redis;
    public IDatabase Db => _redis.GetDatabase();
    public ISubscriber Sub => _redis.GetSubscriber();

    public RedisProvider(string host, int port, string user, string password)
    {
        var config = new ConfigurationOptions
        {
            EndPoints = { { host, port } },
            User = user,           // usually "default" for Azure Redis
            Password = password,
            Ssl = true,            // Azure Redis usually requires SSL
            AbortOnConnectFail = false, // recommended for cloud Redis
            ConnectRetry = 3,
            ConnectTimeout = 15000
        };
        _redis = ConnectionMultiplexer.Connect(
            new ConfigurationOptions{
                EndPoints= { { host, port } },
                User=user,
                Password=password
            }
        );

        //_redis = ConnectionMultiplexer.Connect(config);
    }
}
