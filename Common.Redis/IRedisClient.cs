using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Redis
{
    public interface IRedisClient : IDisposable
    {
        /// <summary>
        /// Redis错误事件
        /// </summary>
        EventHandler<Args.RedisErrorEventArgs> RedisErrorEvent { get; set; }

        /// <summary>
        /// 添加缓存数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireTime">过期时间</param>
        /// <returns></returns>
        bool Add<T>(string key, T value, TimeSpan? expireTime = null);
        /// <summary>
        /// 添加字符串类型的缓存数据
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存数据</param>
        /// <param name="expireTime">过期时间</param>
        bool Add(string key, string value, TimeSpan? expireTime = null);
        /// <summary>
        /// 移除指定缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        bool Remove(string key);
        /// <summary>
        /// 移除多个缓存
        /// </summary>
        /// <param name="keys">缓存Key数组</param>
        void Remove(string[] keys);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存数据</param>
        /// <returns></returns>
        bool Update<T>(string key, T value);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存数据</param>
        /// <returns></returns>
        bool Update<T>(string key, string value);
        /// <summary>
        /// 是否存在指定缓存
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exists(string key);
        /// <summary>
        /// 获得字符类型缓存数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// 获得缓存对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        T Get<T>(string key) where T : class;
        /// <summary>
        /// 发布一条消息到指定通道
        /// </summary>
        /// <param name="channel">通道名称</param>
        /// <param name="value">消息内容</param>
        /// <returns></returns>
        long Publish(string channel, string value);
        /// <summary>
        /// 订阅指定通道
        /// </summary>
        /// <param name="channel">通道名称</param>
        /// <param name="handler">消息处理</param>
        void Subscribe(string channel, Action<string, string> handler);
    }
}
