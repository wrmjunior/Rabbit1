using System.Collections.Generic;

namespace PageDataQueueServices
{
    public interface IQueueSettings
    {
        string HostName { get; set; }
        int Port { get; set; }
        int RequestedConnectionTimeout { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string QueueName { get; set; }
        string ExchangeName { get; set; }
        string RoutingKey { get; set; }
        bool Mandatory { get; set; }
        int QosSize { get; set; }
        bool Durable { get; set; }
        bool Exclusive { get; set; }
        bool AutoDelete { get; set; }
        Dictionary<string, object> Arguments { get; set; }
    }

    public class QueueSettings : IQueueSettings
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public int RequestedConnectionTimeout { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string QueueName { get; set; }
        public string ExchangeName { get; set; }
        public string RoutingKey { get; set; }
        public bool Mandatory { get; set; }
        public int QosSize { get; set; }
        public bool Durable { get; set; }
        public bool Exclusive { get; set; }
        public bool AutoDelete { get; set; }
        public Dictionary<string, object> Arguments { get; set; }
    }
}
