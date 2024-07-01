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
        public string Salary { get; set; }
        public string City { get; set; }
        public string JobType { get; set; }
        public string ExpertiseLvl { get; set; }
        public string Industry { get; set; }
        public string QualificationLvl { get; set; }
        public string Department { get; set; }
        public string Language { get; set; }
    }
}
