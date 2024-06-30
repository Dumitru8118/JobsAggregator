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
using AutoMapper;
using JobHub.API.Dtos.Response;
using JobHub.API.Models.Repository;
using JobHub.API.Models.Providers;
using JobHub.API.Models.Interfaces;

namespace JobHub.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SelScrapeController : ControllerBase
	{
		private readonly IJobRepository _jobRepository;
		private readonly IMapper _mapper;

		public SelScrapeController(
			IJobRepository repository,
			IMapper mapper)
		{
			_jobRepository = repository;
			_mapper = mapper;
		}

		// GET: ScrapeController
		[HttpPost("ScrapeFromHipo")]
		public IEnumerable<string> PostHipoJobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<JobModel> jobs = MultiScraper<Hipo>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

			foreach (JobModel job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();

			return scrapedAnchorTexts;
		}

		// GET: ScrapeController
		[HttpPost("ScrapeFromEJobs")]
		public IEnumerable<string> PostEjobsJobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<JobModel> jobs = MultiScraper<EJobs>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

			foreach (JobModel job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();

			return scrapedAnchorTexts;
		}

		// GET: ScrapeController
		[HttpPost("ScrapeFromJobRadar24")]
		public IEnumerable<string> PostJobRadar24Jobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<JobModel> jobs = MultiScraper<JobRadar24>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

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
		public async Task<IActionResult> GetWithKeysetPagination(int reference = 0, int pageSize = 40)
		{
			if (pageSize <= 0)
				return BadRequest($"{nameof(pageSize)} size must be greater than 0.");

			var pagedJobs = await _jobRepository.GetWithKeysetPagination(reference, pageSize);

			var pagedJobsDto = _mapper.Map<PagedResponseKeysetDto<JobItemDto>>(pagedJobs);

			return Ok(pagedJobsDto);
		}

		// DELETE api/job/duplicates
		[HttpDelete("duplicates")]
		public async Task<IActionResult> DeleteDuplicates()
		{
			var result = await _jobRepository.RemoveDuplicatesAsync();

			if (result.Success)
			{
				return Ok($"{result.DeletedCount} Duplicates deleted successfully.");
			}
			else
			{
				return StatusCode(500, "Failed to delete duplicates."); // You can customize the error response
			}
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

			var returnJob = _mapper.Map<JobItemDto>(job);
			
			return Ok(returnJob); 
		
		}


	}
}
