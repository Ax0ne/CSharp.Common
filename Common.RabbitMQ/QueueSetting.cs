using System;
using System.Collections.Generic;
using System.Text;

namespace Common.RabbitMQ
{
    /// <summary>
    /// 消息队列参数配置
    /// </summary>
    public class QueueSetting
    {
        /// <summary>
        /// 交换机标识
        /// </summary>
        public string Exchange { get; set; } = "Ax0ne.Exchange.Default";
        /// <summary>
        /// 路由标识
        /// </summary>
        public string RoutingKey { get; set; } = "";

        /// <summary>
        /// 交换机类型（默认：fanout）取值：fanout,direct,topic
        /// </summary>
        public string ExchangeType { get; set; } = "fanout";
        /// <summary>
        /// 队列标识
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 是否持久化（默认：false）
        /// </summary>
        public bool IsDurable { get; set; }
        /// <summary>
        /// 是否排他（默认：true）
        /// </summary>
        public bool IsExclusive { get; set; } = true;

        /// <summary>
        /// 是否自动删除（默认：true）
        /// </summary>
        public bool IsAutoDelete { get; set; } = true;
        ///// <summary>
        ///// [接受方法专用参数]是否自动消息确认（默认：true）
        ///// </summary>
        //public bool IsAutoAck { get; set; } = true;
    }
}
