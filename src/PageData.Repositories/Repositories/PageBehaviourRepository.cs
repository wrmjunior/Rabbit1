using PageDataRepositories.Models;
using PageDataRepositories.Repositories.DataSources;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageDataRepositories.Repositories
{
    public interface IPageBehaviourRepository
    {
        Task SavePageBehaviour(PageBehaviour pageBehaviour);

        Task<IEnumerable<PageBehaviour>> GetPageBehaviours();

        Task<IEnumerable<PageBehaviour>> FilterPageBehavioursBy(string ip, string pageName);
    }

    public class PageBehaviourRepository : IPageBehaviourRepository
    {
        private readonly IDataSourceFactory _dataSourceFactory;

        private readonly IDataSource<PageBehaviour> _sqlDataSource;

        public PageBehaviourRepository(IDataSourceFactory dataSourceFactory)
        {
            _dataSourceFactory = dataSourceFactory;

            _sqlDataSource = _dataSourceFactory.CreateDataSource<PageBehaviour>("SQLServer");
        }

        public async Task<IEnumerable<PageBehaviour>> GetPageBehaviours()
        {
            var behaviours = await _sqlDataSource.GetMany();

            return behaviours;
        }

        public async Task<IEnumerable<PageBehaviour>> FilterPageBehavioursBy(string ip, string pageName)
        {
            if (string.IsNullOrWhiteSpace(ip) && string.IsNullOrWhiteSpace(pageName))
                throw new InvalidOperationException("Atleast one arugment must be provided.");

            var queryArgs = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(ip))
                queryArgs.Add("IP", ip);

            if (!string.IsNullOrWhiteSpace(pageName))
                queryArgs.Add("PageName", pageName);

            var filteredBehaviours = await _sqlDataSource.QueryMany(queryArgs);

            return filteredBehaviours;
        }

        public async Task SavePageBehaviour(PageBehaviour pageBehaviour)
        {
            if (pageBehaviour == null)
                throw new ArgumentNullException(nameof(pageBehaviour));

            await _sqlDataSource.Save(pageBehaviour);
        }
    }
}
