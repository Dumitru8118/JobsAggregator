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
using AutoMapper;
using JobHub.API.Dtos.Response;

namespace JobHub.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SelScrapeController : ControllerBase
	{
		private readonly IJobRepository _repository;
		private readonly IMapper _mapper;

		public SelScrapeController(IJobRepository repository,
			IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
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


		[HttpGet("GetWithKeysetPagination")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetWithKeysetPagination(int reference = 0, int pageSize = 10)
		{
			if (pageSize <= 0)
				return BadRequest($"{nameof(pageSize)} size must be greater than 0.");

			var pagedJobs = await _repository.GetWithKeysetPagination(reference, pageSize);

			var pagedJobsDto = _mapper.Map<PagedResponseKeysetDto<JobItemDto>>(pagedJobs);

			return Ok(pagedJobsDto);
		}

		[HttpGet]
		public IActionResult GetJob()
		{
			var job = new JobModel(
				 "/work-at-home-customer-support-specialist/1776354",
				 "Some Company",
				 "Some Job",
				 "Some Date"
			);

			var jobPage = new JobPageModel("1776354", "12321", "PIT", "test", "test", "test", " test", "test", "test");

			job.JobPage = jobPage;

			var returnJob = _mapper.Map<JobItemDto>(job);
			
			return Ok(returnJob); 
		
		}
	}
}
