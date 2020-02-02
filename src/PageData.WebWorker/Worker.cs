using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PageDataDataServices.Services;
using PageDataQueueServices.Services;
using PageDataWebWorker.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PageDataWebWorker
{
    public class Worker : IHostedService, IDisposable
    {
        private readonly ILogger<Worker> _logger;

        private readonly IPageBehaviourService _pageBehaviourService;

        private readonly IQueueConsumerService<PageBehaviour> _queueConsumerService;

        public Worker(ILogger<Worker> logger,
                      IPageBehaviourService pageBehaviourService,
                      IQueueConsumerService<PageBehaviour> queueConsumerService)
        {
            _logger = logger;
            _pageBehaviourService = pageBehaviourService;
            _queueConsumerService = queueConsumerService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting RabbitMQ consumer.");

            try
            {
                _queueConsumerService.InitConnection();

                _queueConsumerService.ConsumeMessage(msg =>
                {
                    try
                    {
                        _pageBehaviourService.Create(new PageDataDataServices.Models.PageBehaviourData
                        {
                            Browser = msg.Browser,
                            Ip = msg.Ip,
                            PageName = msg.PageName,
                            PageParams = msg.PageParams
                        });
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unable to store SQL Server Data: ", msg);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to start RabbitMQ.");
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Stopping rabbitmq consumer.");

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _queueConsumerService.Dispose();
        }

    }
}
