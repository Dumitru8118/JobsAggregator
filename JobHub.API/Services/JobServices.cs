using JobHub.API.Data;
using JobHub.API.Helpers;
using JobHub.API.Models;
using JobHub.API.Models.Interfaces;
using JobHub.API.Models.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace JobHub.API.Services
{
	public class JobService
	{
		private readonly IJobRepository _jobRepository;
		public JobService(IJobRepository jobRepository)
		{
			this._jobRepository = jobRepository;
		}

		//public Job[] GetAllForUser(string email)
		//{
		//	var user = this.dataContext.Users
		//		.FirstOrDefault(user => user.Email == email);

		//	return this.dataContext.Jobs
		//		.Include(ev => ev.UserJobs)
		//		.Where(e => e.UserJobs.FirstOrDefault(ue => ue.UserId == user.UserId) != null)
		//		.ToArray();
		//}

		//public Job GetById(int id)
		//{
		//	return this.dataContext.Jobs
		//		.Include(ev => ev.UserJobs)
		//	.FirstOrDefault(c => c.Id == id);
		//}

		public List<string> CreateJobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<Job> jobs = MultiScraper<JobRadar24>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

			foreach (Job job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();

			return scrapedAnchorTexts;
		}

		//public Job Delete(int id)
		//{
		//	var eventEntity = this.GetById(id);

		//	if (eventEntity != null)
		//	{
		//		this.dataContext.Jobs.Remove(eventEntity);
		//		this.dataContext.SaveChanges();
		//	}

		//	return eventEntity;
		//}
	}
}
