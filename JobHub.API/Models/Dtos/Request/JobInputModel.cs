using JobHub.API.Models.Database;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JobHub.API.Models.Dtos.Request
{
	public class JobInputModel
	{
		public string Id { get; private set; }
		public string? Url { get; set; }

		public string? CompanyName { get; set; }

		public string? JobName { get; set; }

		public string? DatePosted { get; set; }

		public IList<string> UserEmails { get; set; }
	}
}
