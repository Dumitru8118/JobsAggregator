using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobHub.API.Models
{
	public class JobModel
	{
		/// <summary>
		/// The id is going to be extracted from the URL 
		/// Eg: /italian-speaker-remote-work-at-home-customer-support-specialist/1776354 => 1776354
		/// </summary>
		[Key]
		public int Id { get; private set; }

		[Required]
		public string Url { get; set; }

		//[Required]
		public string? CompanyName { get; set; }

		//[Required]
		public string? JobName { get; set; }

		[Column("Date Posted")]
		public string? DatePosted { get; set; }

		// Navigation property for JobPageModel
		public JobPageModel? JobPage { get; set; }
		public JobModel(
			string url,
			string companyName,
			string jobName,
			string datePosted)
		{
			Url = url;
			CompanyName = companyName;
			JobName = jobName;
			DatePosted = datePosted;
			Id = ExtractIdFromUrl(url);
		}

		// Method to extract the ID from the URL
		private int ExtractIdFromUrl(string url)
		{
			if (!string.IsNullOrEmpty(url))
			{
				var parts = url.Split('/');
				string result = parts[parts.Length - 1];

				return Int32.Parse(result); // Last part of the URL
			}

			Random random = new Random();
			return random.Next();
		}
	}
}
