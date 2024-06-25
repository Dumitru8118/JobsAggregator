using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Xml.Linq;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using JobHub.API.Data;
using JobHub.API.Models;
using JobHub.API.Services;
using JobHub.API.Models.Repository.IRepository;

namespace JobHub.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SelScrapeController : ControllerBase
	{
		private readonly IJobRepository _repository;

		public SelScrapeController(IJobRepository repository)
		{
			_repository = repository;
		}

		// GET: ScrapeController
		[HttpPost]
		public IEnumerable<string> Post()
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<JobModel> jobs = Scraper.ScrapeJobs();

			 _repository.SaveRange(jobs);

			foreach (JobModel job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();
			return scrapedAnchorTexts;
		}
	}
}
