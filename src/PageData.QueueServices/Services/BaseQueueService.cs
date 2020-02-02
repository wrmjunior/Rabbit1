'using Newtonsoft.Json;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace PageDataQueueServices.Services
{
    public abstract class BaseQueueService : IDisposable
    {
        protected readonly IQueueSettings QueueSettings;

        protected IConnection Connection;

        protected IModel Channel;

        protected readonly Action<object> ConsumeAction;

        public BaseQueueService(IQueueSettings queueSettings)
        {
            QueueSettings = queueSettings;
        }

        protected virtual void InitRabbitConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = QueueSettings.HostName,
                Port = QueueSettings.Port,
                UserName = QueueSettings.UserName,
                Password = QueueSettings.Password,
                RequestedConnectionTimeout = QueueSettings.RequestedConnectionTimeout
            };

            Connection = factory.CreateConnection();
        }

        protected virtual void InitConnectionChannel()
        {
            Channel = Connection.CreateModel();
            Channel.QueueDeclare(queue: QueueSettings.QueueName,
                                 durable: QueueSettings.Durable,
                                 exclusive: QueueSettings.Exclusive,
                                 autoDelete: QueueSettings.AutoDelete,
                                 arguments: QueueSettings.Arguments);
        }

        protected virtual void PublishMessage<TMessage>(TMessage message)
        {
            var messageData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            Channel.BasicPublish(exchange: QueueSettings.ExchangeName,
                                 routingKey: QueueSettings.RoutingKey,
                                 basicProperties: null,
                                 mandatory: QueueSettings.Mandatory,
                                 body: messageData);
        }

        protected virtual void ConsumeMessage<TMessage>(Action<TMessage> action)
        {
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender, args) =>
            {
                var queueMessage = Encoding.UTF8.GetString(args.Body);
                var message = JsonConvert.DeserializeObject<TMessage>(queueMessage);

                action.Invoke(message);

                Channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            };

            Channel.BasicQos(0, (ushort)QueueSettings.QosSize, false);
            Channel.BasicConsume(queue: QueueSettings.QueueName,
                                  autoAck: false,
                                  consumer: consumer);
        }

        public void Dispose()
        {
            if (Channel != null && Channel.IsOpen)
                Channel.Close();

            if (Connection != null && Connection.IsOpen)
                Connection.Close();

            Channel.Dispose();
            Connection.Dispose();
        }
    }
}
