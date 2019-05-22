using System;
using System.Collections.Generic;
using Common.Redis;
using Common.RabbitMQ;

namespace Common.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var channel = "redis-channel-test";
            //Subscribe1(channel);
            //Subscribe2(channel);
            var rabbitClient = new RabbitMqClient(new RabbitMqConfig { HostName = "192.168.2.22", UserName = "admin", Password = "123456" });
            var queueSetting = new QueueSetting { QueueName = "r.test", IsExclusive = false };
            rabbitClient.Publish("start...", queueSetting);
            Receive1();
            //while (true)
            //{
            //    System.Console.WriteLine("请输入消息,按q推出");
            //    var message = System.Console.ReadLine();
            //    rabbitClient.Publish(message, queueSetting);
            //    if (message.Equals("q", StringComparison.InvariantCultureIgnoreCase))
            //        break;
            //}
            for (int i = 0; i < 20; i++)
            {
                rabbitClient.Publish(i, queueSetting);
            }

            System.Console.ReadLine();
        }

        private static void Receive1()
        {
            var config = new RabbitMqConfig { HostName = "192.168.2.22", UserName = "admin", Password = "123456" };
            var queueSetting = new QueueSetting { QueueName = "r.test", IsExclusive = false };
            new RabbitMqClient(config).Receive(queueSetting,
                value => { System.Console.WriteLine($"消息接收端.{DateTime.Now:HH:mm:ss},数据：{value}"); });
            new RabbitMqClient(config).Receive(queueSetting, value =>
            {
                System.Console.WriteLine($"消息接收端2.{DateTime.Now:HH:mm:ss},数据：{value}");
                System.Threading.Thread.Sleep(2000);
                return true;
            });
        }
        private static void Subscribe1(string channel)
        {
            var config = new RedisConfig
            {
                Host = "192.168.2.22"
            };
            // 正确用法
            //using (IRedisClient redisClient = new RedisClient(config))
            //{
            //    //redisClient.Add()
            //}
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
            Subscribe2(channel);
            _redisClient.Subscribe(channel, (c, v) => { System.Console.WriteLine($"订阅信息-通道{c}-内容-{v}"); });
            _redisClient.Publish(channel, "start1");
            for (var i = 0; i < 100; i++) _redisClient.Publish(channel, "循环-index-" + i);

            var key3 = "key3";
            var user = new User { Age = 22, Name = "fuck" };
            _redisClient.Add(key3, user);
            var key3Value = _redisClient.Get<User>(key3);
            System.Console.WriteLine($"Key3Value:{key3Value}");
            var key4 = "key4";
            var list = new List<User>();
            for (var i = 0; i < 22; i++) list.Add(new User { Age = 100 - i, Name = "Name" + i });

            _redisClient.Add(key4, list);
            var key4Value = _redisClient.Get<List<User>>(key4);
            System.Console.WriteLine("key4value----------------------------------");
            foreach (var user1 in key4Value) System.Console.WriteLine(user1);
            System.Console.ReadLine();
        }
        private static void Subscribe2(string channel)
        {
            var config = new RedisConfig
            {
                Host = "192.168.2.22"
            };
            var _redisClient = new RedisClient(config);
            _redisClient.Subscribe(channel, (c, v) => { System.Console.WriteLine($"订阅2-通道{c}-内容-{v}"); });
        }


        public class User
        {
            public string Name { get; set; }
            public int Age { get; set; }

            public override string ToString()
            {
                return $"Name:{Name},Age:{Age}";
            }
        }
    }
}