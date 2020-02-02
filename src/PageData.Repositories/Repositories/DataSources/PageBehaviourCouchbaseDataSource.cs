using PageDataRepositories.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageDataRepositories.Repositories.DataSources
{
    public class PageBehaviourCouchbaseDataSource : IDataSource<PageBehaviour>
    {
        public Task<IEnumerable<PageBehaviour>> GetMany()
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<PageBehaviour>> QueryMany(Dictionary<string, object> queryArgs)
        {
            throw new System.NotImplementedException();
        }


        public async Task Save(PageBehaviour item)
        {
            throw new System.NotImplementedException();
        }
    }
}
