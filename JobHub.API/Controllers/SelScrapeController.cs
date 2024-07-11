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
using JobHub.API.Services;
using AutoMapper;
using JobHub.API.Models.Repository;
using JobHub.API.Models.Providers;
using JobHub.API.Models.Interfaces;
using JobHub.API.Models.Database;
using JobHub.API.Models.Dtos.Response;
using JobHub.API.Models;
using Microsoft.AspNetCore.Authorization;

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
		[Authorize(Roles = "Administrator")]
		public IEnumerable<JobItemResponse> PostHipoJobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<Job> jobs = MultiScraper<Hipo>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

			foreach (Job job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();
            
			var jobsDto = _mapper.Map<List<JobItemResponse>>(jobs);

            return jobsDto;
        }

		// GET: ScrapeController
		[HttpPost("ScrapeFromEJobs")]
		[Authorize(Roles = "Administrator")]
		public IEnumerable<JobItemResponse> PostEjobsJobs(int pagesNumber)
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			List<Job> jobs = MultiScraper<EJobs>.ScrapeJobs(pagesNumber);

			_jobRepository.SaveRange(jobs);

			foreach (Job job in jobs)
			{
				scrapedAnchorTexts.Add(job.Url);
			}

			scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());

			ChromeDriverSingleton.Quit();
            
			var jobsDto = _mapper.Map<List<JobItemResponse>>(jobs);

            return jobsDto;
        }

		// GET: ScrapeController
		[HttpPost("ScrapeFromJobRadar24")]
		[Authorize(Roles = "Administrator")]
		public IEnumerable<JobItemResponse> PostJobRadar24Jobs(int pagesNumber)
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

            var jobsDto = _mapper.Map<List<JobItemResponse>>(jobs);

            return jobsDto;
		}


		[HttpGet("GetItems")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> GetItems(int reference = 0, int pageSize = 40)
		{
			if (pageSize <= 0)
				return BadRequest($"{nameof(pageSize)} size must be greater than 0.");

			var pagedJobs = await _jobRepository.GetWithKeysetPagination(reference, pageSize);

			var pagedJobsDto = _mapper.Map<PagedResponseKeysetDto<JobItemResponse>>(pagedJobs);

			return Ok(pagedJobsDto);
		}

		// DELETE api/job/duplicates
		[HttpDelete("duplicates")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> DeleteDuplicates()
		{
			var result = await _jobRepository.RemoveDuplicatesAsync();

			if (result.Succes)
			{
				//return Ok($"{result.DeletedCount} Duplicates deleted successfully.");
				return Ok(result);
			}
			else
			{
				return Ok(result); // You can customize the error response
			}
		}
	}
}
