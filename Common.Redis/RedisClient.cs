using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace Common.Redis
{
    public class RedisClient : IRedisClient
    {
        private readonly IDatabase _db;
        private readonly ConnectionMultiplexer _redis;
        public RedisClient(RedisConfig config) : this($"{config.Host}:{config.Port},abortConnect=false,defaultDatabase={config.Database},ssl=false,ConnectTimeout={config.ConnectTimeout},allowAdmin=true,connectRetry={config.ConnectRetry}")
        {
        }
        public RedisClient(string configuration)
        {
            _redis = ConnectionMultiplexer.Connect(configuration);
            _redis.ErrorMessage += Redis_ErrorMessage;
            _db = _redis.GetDatabase();
        }

        public EventHandler<Args.RedisErrorEventArgs> RedisErrorEvent { get; set; }

        private void Redis_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            RedisErrorEvent?.Invoke(sender, new Args.RedisErrorEventArgs(e.EndPoint.ToString(), e.Message));
        }

        public bool Add<T>(string key, T value, TimeSpan? expireTime = null)
        {
            return Add(key, JsonConvert.SerializeObject(value), expireTime);
        }

        public bool Add(string key, string value, TimeSpan? expireTime = null)
        {
            return _db.StringSet(key, value, expireTime);
        }

        public bool Remove(string key)
        {
            return _db.KeyDelete(key);
        }

        public void Remove(string[] keys)
        {
            _db.KeyDelete(keys.Select(key => (RedisKey)key).ToArray());
        }

        public bool Update<T>(string key, T value)
        {
            return Add(key, value);
        }

        public bool Update<T>(string key, string value)
        {
            return Add(key, value);
        }

        public bool Exists(string key)
        {
            return _db.KeyExists(key);
        }

        public string Get(string key)
        {
            return _db.StringGet(key);
        }

        public T Get<T>(string key) where T : class
        {
            var value = _db.StringGet(key);
            if (!value.IsNull)
                return JsonConvert.DeserializeObject<T>(value);
            return null;
        }

        public long Publish(string channel, string value)
        {
            var subscribe = _redis.GetSubscriber();
            return subscribe.Publish(channel, value);
        }

        public void Subscribe(string channel, Action<string, string> handler)
        {
            var subscribe = _redis.GetSubscriber();
            subscribe.Subscribe(channel, (c, v) => { handler(c, v); });
        }

        public void Dispose()
        {
            _redis.Dispose();
        }
    }
}
