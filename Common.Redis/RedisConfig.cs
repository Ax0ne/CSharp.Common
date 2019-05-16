using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Redis
{
    public class RedisConfig
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 6379;
        /// <summary>
        /// 密码（可空)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 数据库 0-(Count-1)
        /// </summary>
        public int Database { get; set; } = 0;

        /// <summary>
        /// 连接超时(ms)
        /// </summary>
        public int ConnectTimeout { get; set; } = 15*1000;

        /// <summary>
        /// 连接重试次数
        /// </summary>
        public int ConnectRetry { get; set; } = 3;
    }
}
