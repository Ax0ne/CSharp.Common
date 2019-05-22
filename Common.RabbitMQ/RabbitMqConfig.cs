using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ
{
    public class RabbitMqConfig
    {
        /// <summary>
        /// RabbitMQ服务器地址
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; } = 5672;
    }
}
