using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PageDataDataServices.Models;
using PageDataDataServices.Services;
using PageDataQueueServices.Services;
using PageDataWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PageDataWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PageDataController : ControllerBase
    {
        private readonly IPageBehaviourService _pageBehaviourService;

        private readonly IQueuePublisherService<PageBehaviour> _queuePublisherService;


        public PageDataController(IPageBehaviourService pageBehaviourService,
                                  IQueuePublisherService<PageBehaviour> queuePublisherService)
        {
            _pageBehaviourService = pageBehaviourService;
            _queuePublisherService = queuePublisherService;
        }

        [HttpPost]
        public ActionResult Post([FromBody]PageBehaviour pageBehaviour)
        {
            string remoteIpAddress = HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                remoteIpAddress = Request.Headers["X-Forwarded-For"];

            pageBehaviour.Ip = remoteIpAddress;
            _queuePublisherService.PublishMessage(pageBehaviour);

            return Ok();
        }

        [HttpGet]
        public async Task<IEnumerable<PageBehaviourData>> Get([FromQuery]string ip, [FromQuery]string pageName)
        {
            Task<IEnumerable<PageBehaviourData>> pageBehavioursData;

            if (string.IsNullOrWhiteSpace(ip) && string.IsNullOrWhiteSpace(pageName))
                pageBehavioursData = _pageBehaviourService.GetAll();
            else
                pageBehavioursData = _pageBehaviourService.GetByIdOrPageName(ip, pageName);

            return (await pageBehavioursData);
        }
    }
}
