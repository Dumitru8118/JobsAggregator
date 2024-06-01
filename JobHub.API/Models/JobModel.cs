using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.API.Models
{
	public class JobModel
	{
		public string Url { get; set; }
		public string? Description { get; set; }
		public string CompanyName { get; set; }
		public string JobName { get; set; }
		public string Id { get; private set; }

		[Column("DatePosted")]
		public string DatePosted { get; set; }

		// Constructor
		public JobModel(string url, string description, string companyName, string jobName, string datePosted)
		{
			Url = url;
			Description = description;
			CompanyName = companyName;
			JobName = jobName;
			DatePosted = datePosted;
			Id = ExtractIdFromUrl(url);
		}

		// Method to extract the ID from the URL
		private string ExtractIdFromUrl(string url)
		{
			if (!string.IsNullOrEmpty(url))
			{
				var parts = url.Split('/');
				return parts[parts.Length - 1]; // Last part of the URL
			}
			return Guid.NewGuid().ToString();
		}
	}
}
