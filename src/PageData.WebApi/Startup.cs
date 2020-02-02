using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PageDataDataServices.Services;
using PageDataQueueServices;
using PageDataQueueServices.Services;
using PageDataRepositories.Repositories;
using PageDataRepositories.Repositories.DataSources;
using PageDataRepositories.Repositories.DataSources.Contexts;
using PageDataWebApi.Models;

namespace PageDataWebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.Configure<QueueSettings>(Configuration.GetSection(nameof(QueueSettings)));
            services.AddSingleton<IQueueSettings>(sp => sp.GetRequiredService<IOptions<QueueSettings>>().Value);

            services.AddDbContext<SqlDataContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));

            services.AddTransient<IPageBehaviourService, PageBehaviourService>();
            services.AddTransient<IPageBehaviourRepository, PageBehaviourRepository>();

            services.AddScoped<IQueuePublisherService<PageBehaviour>, QueuePublisherService<PageBehaviour>>();

            services.AddScoped<IDataSourceFactory, DataSourceFactory>();
            services.AddScoped<PageBehaviourSqlDataSource>();
            services.AddScoped<PageBehaviourCouchbaseDataSource>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
