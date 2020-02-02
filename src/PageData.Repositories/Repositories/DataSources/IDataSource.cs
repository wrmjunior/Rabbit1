using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageDataRepositories.Repositories.DataSources
{
    public interface IDataSource<T> where T : class
    {
        Task Save(T item);

        Task<IEnumerable<T>> GetMany();

        Task<IEnumerable<T>> QueryMany(Dictionary<string, object> queryArgs);
    }
}
