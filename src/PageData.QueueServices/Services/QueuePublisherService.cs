using System;

namespace PageDataQueueServices.Services
{
    public interface IQueuePublisherService<TMessage>
    {
        void PublishMessage(TMessage message);
    }

    public class QueuePublisherService<TMessage> : BaseQueueService, IQueuePublisherService<TMessage>
    {
        public QueuePublisherService(IQueueSettings queueSettings) : base(queueSettings)
        {
            InitRabbitConnection();
            InitConnectionChannel();
        }

        public void PublishMessage(TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            base.PublishMessage(message);
        }
    }
}
