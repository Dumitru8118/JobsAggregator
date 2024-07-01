using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.API.Models.Dtos.Response
{
    public class JobItemResponse
    {
        public string Url { get; set; }
        public string CompanyName { get; set; }
        public string JobName { get; set; }
        public string DatePosted { get; set; }
    }
}
