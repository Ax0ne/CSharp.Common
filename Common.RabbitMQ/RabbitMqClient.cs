using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.RabbitMQ
{
    /// <summary>
    /// RabbitMQ客户端操作类
    /// </summary>
    public class RabbitMqClient : IRabbitMqClient
    {
        private readonly ConnectionFactory _factory;
        private IConnection _connection;
        private IModel _channel;

        public RabbitMqClient(RabbitMqConfig config)
        {
            _factory = new ConnectionFactory
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                Port = config.Port
            };
        }

        public void Publish<T>(T value, QueueSetting setting)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            Publish(json, setting);
        }
        public void Publish(string value, QueueSetting setting)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(setting.Exchange, setting.ExchangeType);
                    channel.QueueDeclare(setting.QueueName, setting.IsDurable, setting.IsExclusive, setting.IsAutoDelete);
                    channel.QueueBind(setting.QueueName, setting.Exchange, setting.RoutingKey);
                    var body = Encoding.UTF8.GetBytes(value);
                    channel.BasicPublish(setting.Exchange, setting.RoutingKey, body: body);
                }
            }
        }

        public void Receive(QueueSetting setting, Action<string> receivedAction)
        {
            Receive(setting, r => { receivedAction(r); return true; }, true);

        }
        public void Receive(QueueSetting setting, Func<string, bool> receivedAction)
        {
            Receive(setting, receivedAction, false);
        }

        private void Receive(QueueSetting setting, Func<string, bool> receivedAction, bool isAutoAck)
        {
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(setting.Exchange, setting.ExchangeType);
            _channel.QueueDeclare(setting.QueueName, setting.IsDurable, setting.IsExclusive, setting.IsAutoDelete);
            _channel.QueueBind(setting.QueueName, setting.Exchange, setting.RoutingKey);
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var value = Encoding.UTF8.GetString(ea.Body);
                if (isAutoAck)
                    receivedAction(value);
                else if (receivedAction(value))
                    _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(setting.QueueName, isAutoAck, consumer);
        }


        private static byte[] ObjectToByteArray(object obj)
        {
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
