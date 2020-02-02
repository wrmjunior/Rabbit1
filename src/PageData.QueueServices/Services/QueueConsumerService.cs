using System;

namespace PageDataQueueServices.Services
{
    public interface IQueueConsumerService<TMessage> : IDisposable
    {
        void InitConnection();

        void ConsumeMessage(Action<TMessage> messageHandler);
    }

    public class QueueConsumerService<TMessage> : BaseQueueService, IQueueConsumerService<TMessage>
    {
        public QueueConsumerService(IQueueSettings queueSettings) : base(queueSettings)
        {
        }

        public void InitConnection()
        {
            InitRabbitConnection();
            InitConnectionChannel();
        }

        public void ConsumeMessage(Action<TMessage> messageHandler)
        {
            base.ConsumeMessage(messageHandler);
        }
    }
}
