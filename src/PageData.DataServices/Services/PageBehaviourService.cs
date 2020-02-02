using PageDataDataServices.Models;
using PageDataRepositories.Models;
using PageDataRepositories.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PageDataDataServices.Services
{
    public interface IPageBehaviourService
    {
        Task Create(PageBehaviourData pageBehaviourData);
        Task<IEnumerable<PageBehaviourData>> GetAll();

        Task<IEnumerable<PageBehaviourData>> GetByIdOrPageName(string ip, string pageName);
    }
    public class PageBehaviourService : IPageBehaviourService
    {
        private readonly IPageBehaviourRepository _navigationDataRepository;

        public PageBehaviourService(IPageBehaviourRepository navigationDataRepository)
        {
            _navigationDataRepository = navigationDataRepository;
        }

        public async Task Create(PageBehaviourData pageBehaviourData)
        {
            await _navigationDataRepository.SavePageBehaviour(new PageBehaviour
            {
                Browser = pageBehaviourData.Browser,
                Ip = pageBehaviourData.Ip,
                PageName = pageBehaviourData.PageName,
                PageParams = pageBehaviourData.PageParams
            });
        }

        public async Task<IEnumerable<PageBehaviourData>> GetAll()
        {
            var pageBehaviours = await _navigationDataRepository.GetPageBehaviours();
            return ConvertFromData(pageBehaviours);
        }

        public async Task<IEnumerable<PageBehaviourData>> GetByIdOrPageName(string ip, string pageName)
        {
            var pageBehaviours = await _navigationDataRepository.FilterPageBehavioursBy(ip, pageName);
            return ConvertFromData(pageBehaviours);
        }

        private IEnumerable<PageBehaviourData> ConvertFromData(IEnumerable<PageBehaviour> data)
        {
            return data?.Select(pageBehaviour => new PageBehaviourData
            {
                Browser = pageBehaviour.Browser,
                Ip = pageBehaviour.Ip,
                PageName = pageBehaviour.PageName,
                PageParams = pageBehaviour.PageParams
            }).AsEnumerable();
        }
    }
}
