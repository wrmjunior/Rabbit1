using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PageDataDataServices.Services;
using PageDataQueueServices;
using PageDataQueueServices.Services;
using PageDataRepositories.Repositories;
using PageDataRepositories.Repositories.DataSources;
using PageDataRepositories.Repositories.DataSources.Contexts;
using PageDataWebWorker.Models;
using System.Threading.Tasks;

namespace PageDataWebWorker
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
            {
                var configuration = hostContext.Configuration;

                services.Configure<QueueSettings>(configuration.GetSection(nameof(QueueSettings)));
                services.AddSingleton<IQueueSettings>(sp => sp.GetRequiredService<IOptions<QueueSettings>>().Value);

                var optionsBuilder = new DbContextOptionsBuilder<SqlDataContext>();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));

                services.AddSingleton<IPageBehaviourService, PageBehaviourService>();
                services.AddSingleton<IPageBehaviourRepository, PageBehaviourRepository>();

                services.AddSingleton<IQueueConsumerService<PageBehaviour>, QueueConsumerService<PageBehaviour>>();

                services.AddSingleton<IDataSourceFactory, DataSourceFactory>();

                //services.AddScoped<PageBehaviourSqlDataSource>();
                services.AddSingleton(s => new PageBehaviourSqlDataSource(new SqlDataContext(optionsBuilder.Options)));

                services.AddSingleton<PageBehaviourCouchbaseDataSource>();

                services.AddHostedService<Worker>();
            }).Build())
            {
                await host.StartAsync();

                await host.WaitForShutdownAsync();
            }
        }
    }
}
