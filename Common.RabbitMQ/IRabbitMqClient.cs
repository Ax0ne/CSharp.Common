using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ
{
    public interface IRabbitMqClient : IDisposable
    {
        /// <summary>
        /// 发布数据到队列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">要发布的数据</param>
        /// <param name="setting">队列发布配置</param>
        void Publish<T>(T value, QueueSetting setting);

        /// <summary>
        /// 发布数据到队列
        /// </summary>
        /// <param name="value">要发布的数据</param>
        /// <param name="setting">队列发布配置</param>
        void Publish(string value, QueueSetting setting);
        /// <summary>
        /// [消息自动确认]接收队列消息
        /// </summary>
        /// <param name="setting">队列接收配置</param>
        /// <param name="receivedAction">接收操作委托</param>
        void Receive(QueueSetting setting, Action<string> receivedAction);
        /// <summary>
        /// [消息手动确认]接收队列消息
        /// </summary>
        /// <param name="setting">队列接收配置</param>
        /// <param name="receivedAction">接收操作委托，返回true才会发送确认请求</param>
        void Receive(QueueSetting setting, Func<string, bool> receivedAction);

    }
}
