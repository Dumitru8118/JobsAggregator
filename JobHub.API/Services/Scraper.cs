using JobHub.API.Models;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection.Metadata;

namespace JobHub.API.Services
{
	public class Scraper
	{
		public static List<JobModel> ScrapeJobs()
		{
			List<JobModel> jobs = new List<JobModel>();

			ChromeDriver driver = ChromeDriverSingleton.Instance;

			int page = 1;

			while (true)
			{
				if (page == 1)
				{
					// Navigate to the webpage "https://www.ejobs.ro/locuri-de-munca"
					driver.Navigate().GoToUrl(Constants.ejobsUrl);

					// Find and click the accept cookies button (assuming it has a class or id)
					var acceptCookiesButton = driver.FindElement(By.CssSelector(".CookiesPopup__AcceptButton"));
					if (acceptCookiesButton != null)
					{
						acceptCookiesButton.Click();
					}
					var values = GetJobs(driver);

					foreach (var value in values)
					{
						jobs.Add(value);
					}
					page++;
				}
				else
				{
					if ( page <= 10)
					{
						// Navigate to the webpage "https://www.ejobs.ro/locuri-de-munca"
						driver.Navigate().GoToUrl(Constants.ejobsUrl + "/pagina" + page.ToString());

						var values = GetJobs(driver);

						foreach (var value in values)
						{
							jobs.Add(value);
						}
						page++;
					}
					else
					{
						break;
					}
				}
			}
			return jobs;
		}

		public static List<JobModel> GetJobs(ChromeDriver driver)
		{
			List<JobModel> jobs = new List<JobModel>();

			IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

			for (int i = 0; i < 2200; i++)
			{
				jse.ExecuteScript("window.scrollBy(0,5)", "");
			}


			const string commonXPath = "//*[@id=\"__layout\"]/div/div[4]/section[2]/div/main/ul/li";

			for (int i=1; i<=40; i++)
			{
				var job = driver.FindElement(By.XPath($"{commonXPath}[{i.ToString()}]"));

				// Get the value of the href attribute of the anchor element
				var url = job.FindElement(By.XPath($"{commonXPath}[{i.ToString()}]/div/div[1]/a")).GetAttribute("href");

				// Get the Job Title
				var jobTitle = job.FindElement(By.XPath($"{commonXPath}[{i.ToString()}]/div/div[1]/div[2]/h2/a/span")).Text;

				// Get the Company Name
				var jobCompany = job.FindElement(By.XPath($"{commonXPath}[{i.ToString()}]/div/div[1]/div[2]/h3")).Text;

				Console.WriteLine("Job Title: " + jobTitle + ", Company: " + jobCompany);

				// Get the Date when the job was posted
				var jobDatePosted = job.FindElement(By.XPath($"{commonXPath}[{i.ToString()}]/div/div[1]/div[1]/div[1]/div[1]")).Text;

				Console.WriteLine("Date: " + jobDatePosted);

				// Save the scraped job to the database
				JobModel jobEntity = new JobModel(url, jobTitle, jobCompany, jobDatePosted);

				jobs.Add(jobEntity);
			}

			return jobs;
		}

		public static JobPageModel ScrapeDescriptions(JobModel job)
		{

			ChromeDriver driver = ChromeDriverSingleton.Instance;
			
			// Navigate to the webpage "https://www.ejobs.ro/locuri-de-munca"
			driver.Navigate().GoToUrl(job.Url);

			// Find and Click the "See More" button
			IWebElement showMoreOptionsButton = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/button[1]"));
			showMoreOptionsButton.Click();

			// Scrape relevant data
			var salary = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[1]/span[2]")).Text;
			var city = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[2]/div")).Text;
			var jobType = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[4]/div/div/a")).Text;
			var expertiseLvl = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[5]/div/div/a")).Text;
			var industry = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[6]/div/div/a")).Text;
			var qualificationLvl = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[7]/div/div/a")).Text;
			var department = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[8]/div/div/a")).Text;
			var language = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[9]")).Text;

			JobPageModel jobPage = new JobPageModel(
				id: job.Id,
				salary: salary,
				city: city,
				jobType: jobType,
				expertiseLvl: expertiseLvl,
				industry: industry,
				qualificationLvl: qualificationLvl,
				department: department,
				language: language
				);

			return jobPage;
		}
	}
}
