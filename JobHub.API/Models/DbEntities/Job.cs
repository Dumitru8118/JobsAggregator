using JobHub.API.Models.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.API.Models
{
	public class Job
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string? Url { get; set; }

		public string? CompanyName { get; set; }

		public string? JobName { get; set; }

		[Column("Date Posted")]
		public string? DatePosted { get; set; }
		
		public IList<UserJob> UserJobs { get; set; }
        public Job()
        {
            
        }
        public Job(
			string url,
			string companyName,
			string jobName,
			string datePosted
			)
		{
			Url = url;
			CompanyName = companyName;
			JobName = jobName;
			DatePosted = datePosted;
		}
	}
}
