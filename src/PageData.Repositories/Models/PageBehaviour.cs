using System;

namespace PageDataRepositories.Models
{
    public class PageBehaviour
    {
        public Guid BehaviourId { get; set; }
        public string Browser { get; set; }
        public string Ip { get; set; }
        public string PageName { get; set; }
        public string PageParams { get; set; }

        public PageBehaviour()
        {
            BehaviourId = Guid.NewGuid();
        }
    }
}
