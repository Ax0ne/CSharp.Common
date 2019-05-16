namespace Common.Redis.Args
{
    public class RedisErrorEventArgs
    {
        public RedisErrorEventArgs(string endPoint, string message)
        {
            EndPoint = endPoint;
            Message = message;
        }

        /// <summary>
        ///     The origin of the message
        /// </summary>
        public string EndPoint { get; }

        /// <summary>
        ///     The message from the server
        /// </summary>
        public string Message { get; }
    }
}