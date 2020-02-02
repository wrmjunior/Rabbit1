using Microsoft.Extensions.DependencyInjection;
using System;

namespace PageDataRepositories.Repositories.DataSources
{
    public interface IDataSourceFactory
    {
        IDataSource<T> CreateDataSource<T>(string dataSource) where T : class;
    }

    public class DataSourceFactory : IDataSourceFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DataSourceFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDataSource<T> CreateDataSource<T>(string dataSource) where T : class
        {
            if (string.IsNullOrWhiteSpace(dataSource))
                throw new ArgumentNullException(nameof(dataSource));

            switch (dataSource)
            {
                case "SQLServer":
                    return (IDataSource<T>)_serviceProvider.GetService<PageBehaviourSqlDataSource>();
                case "Couchbase":
                    return (IDataSource<T>)_serviceProvider.GetService<PageBehaviourCouchbaseDataSource>();
                default:
                    throw new Exception("Unknown data source type.");
            }
        }
    }
}
