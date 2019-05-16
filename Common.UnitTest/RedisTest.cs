using System;
using Common.Redis;
using NUnit.Framework;

namespace Common.UnitTest
{
    public class RedisTest
    {
        private IRedisClient _redisClient;
        private RedisConfig config;

        [SetUp]
        public void Setup()
        {
            config = new RedisConfig
            {
                Host = "192.168.2.22"
            };
            _redisClient = new RedisClient(config);
        }

        [Test]
        public void Redis_Function_Test()
        {
            var key1 = "key1";
            _redisClient.Add(key1, "key1-value");
            _redisClient.Update(key1, "key1-value-update");
            var key2 = "key2";
            _redisClient.Add(key2, "expire-value", TimeSpan.FromSeconds(10));
            var key1Value = _redisClient.Get(key1);
            Console.WriteLine($"key1Value:{key1Value}");
            _redisClient.Remove(key1);
            key1Value = _redisClient.Get(key1);
            Console.WriteLine($"Delete-key1Value:{key1Value}");
            var channel = "redis-channel-test";
            _redisClient.Publish(channel, "start1");
            _redisClient.Subscribe(channel, (c, v) => { Console.WriteLine($"订阅信息-通道{c}-内容-{v}"); });
            for (var i = 0; i < 100; i++) _redisClient.Publish(channel, "循环-index-" + i);

            Assert.Pass();
        }
    }
}