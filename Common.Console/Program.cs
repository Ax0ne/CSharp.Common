using System;
using System.Collections.Generic;
using Common.Redis;

namespace Common.Console
{
    class Program
    {
        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public override string ToString()
            {
                return $"Name:{Name},Age:{Age}";
            }
        }

        static void Subscribe2(string channel)
        {
            var config = new RedisConfig
            {
                Host = "192.168.2.22"
            };
            var _redisClient = new RedisClient(config);
            _redisClient.Subscribe(channel, (c, v) =>
            {
                System.Console.WriteLine($"订阅2-通道{c}-内容-{v}");
            });
        }
        static void Main(string[] args)
        {
            var config = new RedisConfig
            {
                Host = "192.168.2.22"
            };
            var _redisClient = new RedisClient(config);

            var key1 = "key1";
            _redisClient.Add(key1, "key1-value");
            _redisClient.Update(key1, "key1-value-update");
            var key2 = "key2";
            _redisClient.Add(key2, "expire-value", TimeSpan.FromSeconds(10));
            var key1Value = _redisClient.Get(key1);
            _redisClient.Add(key1, key1Value);
            key1Value = _redisClient.Get(key1);
            _redisClient.Add(key1, key1Value);
            key1Value = _redisClient.Get(key1);
            System.Console.WriteLine($"key1Value:{key1Value}");
            _redisClient.Remove(key1);
            key1Value = _redisClient.Get(key1);
            System.Console.WriteLine($"Delete-key1Value:{key1Value}");
            var channel = "redis-channel-test";
            Subscribe2(channel);
            _redisClient.Subscribe(channel, (c, v) =>
            {
                System.Console.WriteLine($"订阅信息-通道{c}-内容-{v}");
            });
            _redisClient.Publish(channel, "start1");
            for (int i = 0; i < 100; i++)
            {
                _redisClient.Publish(channel, "循环-index-" + i);
            }

            var key3 = "key3";
            var user = new User { Age = 22, Name = "fuck" };
            _redisClient.Add(key3, user);
            var key3Value = _redisClient.Get<User>(key3);
            System.Console.WriteLine($"Key3Value:{key3Value}");
            var key4 = "key4";
            var list = new List<User>();
            for (int i = 0; i < 22; i++)
            {
                list.Add(new User {Age = 100 - i, Name = "Name" + i});
            }

            _redisClient.Add(key4, list);
            var key4Value = _redisClient.Get<List<User>>(key4);
            System.Console.WriteLine("key4value----------------------------------");
            foreach (var user1 in key4Value)
            {
                System.Console.WriteLine(user1);
            }
            System.Console.ReadLine();
        }
    }
}
