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

namespace JobHub.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SelScrapeController : ControllerBase
	{
		private readonly AppDbContext _dbContext;

		// Injecting DbContext through constructor
		public SelScrapeController(AppDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// GET: ScrapeController
		[HttpGet]
		public IEnumerable<string> Get()
		{
			// Initialize a list to store the scraped anchor texts
			List<string> scrapedAnchorTexts = new List<string>();

			// Set up Chrome WebDriver with options to ignore cookies prompt
			var options = new ChromeOptions();
			options.AddArgument("-ignore-certificate-errors");
			options.AddArgument("-disable-popup-blocking");
			options.AddArgument("-headless=new");

			// Set up Chrome WebDriver (make sure you have chromedriver installed and its location in PATH)
			using (var driver = new ChromeDriver(options))
			{
				// Navigate to the webpage "https://www.ejobs.ro/locuri-de-munca"
				driver.Navigate().GoToUrl(Constants.ejobsUrl);

				// Find and click the accept cookies button (assuming it has a class or id)
				var acceptCookiesButton = driver.FindElement(By.CssSelector(".CookiesPopup__AcceptButton"));
				if (acceptCookiesButton != null)
				{
					acceptCookiesButton.Click();
				}

				IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
				jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");

				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
				var jobCardsWrapper = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".JCContent")));

				foreach (var job in jobCardsWrapper)
				{
					var anchorElem = job.FindElement(By.TagName("a"));
					// Get the value of the href attribute of the anchor element
					string url = anchorElem.GetAttribute("href");

					// Add the href attribute value to the list
					scrapedAnchorTexts.Add(url);

					// Scrape Title and Company from the same node element;
					var jobTitleAndCompany = job.FindElement(By.CssSelector(".JCContentMiddle"));

					var jobCompany = jobTitleAndCompany.FindElement(By.TagName("h2")).Text;
					var jobTitle = jobTitleAndCompany.FindElement(By.TagName("h3")).Text;

					Console.WriteLine("Job Title: " + jobTitle + ", Company: " + jobCompany);

					// Scrape the Date when the job was posted
					var jobDatePosted = job.FindElement(By.CssSelector(".JCContentTop__Date")).Text;

					Console.WriteLine("Date: " + jobDatePosted);

					// Save the scraped job to the database
					JobModel jobEntity = new JobModel(url, "", jobTitle, jobCompany, jobDatePosted);

					_dbContext.Jobs.Add(jobEntity);
					_dbContext.SaveChanges();
					
				}
				Console.WriteLine(scrapedAnchorTexts.Count);

				driver.Quit();

				scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());
			}

			return scrapedAnchorTexts;
		}
	}
}
