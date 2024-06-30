using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.API.Dtos.Response
{
    public class JobItemDto
    {
        public string Url { get; set; }
        public string CompanyName { get; set; }
        public string JobName { get; set; }
        public string DatePosted { get; set; }
    }
}
