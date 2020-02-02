using Microsoft.EntityFrameworkCore;
using PageDataRepositories.Models;
using PageDataRepositories.Repositories.DataSources.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PageDataRepositories.Repositories.DataSources
{
    public class PageBehaviourSqlDataSource : IDataSource<PageBehaviour>
    {
        private readonly SqlDataContext _dbContext;

        public PageBehaviourSqlDataSource(SqlDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<PageBehaviour>> GetMany()
        {
            var allData = _dbContext.Set<PageBehaviour>().AsNoTracking();

            return await allData.ToListAsync();
        }

        public async Task<IEnumerable<PageBehaviour>> QueryMany(Dictionary<string, object> queryArgs)
        {
            if (queryArgs == null)
                throw new ArgumentNullException(nameof(queryArgs));

            var isIpValid = queryArgs.ContainsKey("IP");
            var isPageNameValid = queryArgs.ContainsKey("PageName");

            var data = _dbContext.Set<PageBehaviour>().Where(pb =>
                (isIpValid && pb.Ip == queryArgs["IP"].ToString()) ||
                (isPageNameValid && pb.PageName.Contains(queryArgs["PageName"].ToString())));

            return await data.ToListAsync();
        }

        public async Task Save(PageBehaviour item)
        {
            try
            {
                await _dbContext.PageBehaviours.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
