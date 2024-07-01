using JobHub.API.Models;
using Microsoft.EntityFrameworkCore;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;

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
					if ( page <= 5)
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

			// Find and click the accept cookies button (assuming it has a class or id)
			try
			{
				var acceptCookiesButton = driver.FindElement(By.CssSelector(".CookiesPopup__AcceptButton"));
				if (acceptCookiesButton != null)
				{
					acceptCookiesButton.Click();
				}
			}catch
			{

			}
			JobPageModel jobPage;

			// Scrape relevant data
			var salary = string.Empty;
			var city = string.Empty;
			var jobType = string.Empty;
			var expertiseLvl = string.Empty;
			var industry = string.Empty;
			var qualificationLvl = string.Empty;
			var department = string.Empty;
			var language = string.Empty;


			// Find and Click the "See More" button
			//IWebElement showMoreOptionsButton = driver.FindElement(By.PartialLinkText("Vezi mai multe"));

			var showMoreOptionsButton = driver.FindElement(RelativeBy.WithLocator(By.TagName("button"))
	.Below(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[5]")));


			//// Define XPaths for the elements to be checked
			//string[] xpaths = new string[]
			//{
			//	"//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/button[1]",
			//	"//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/button[2]",
			//	"//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/button[1]/span/div",
			//	"//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/button[2]/i",
			//	"(//button[@class='JDCard__Bottom eButton eButton--Secondary'])[1]",
			//	"(//button[@class='JDCard__Bottom eButton eButton--Secondary'])[2]"
			//};

			//foreach (var xpath in xpaths)
			//{
			//	try
			//	{
			//		// Try to find the element using the current XPath
			//		if (showMoreOptionsButton != null)
			//		{
			//			break; // Element found, exit the loop
			//		}
			//	}
			//	catch (NoSuchElementException)
			//	{
			//		// Element not found, continue to the next XPath
			//	}
			//}

			if (showMoreOptionsButton != null) 
			{
				showMoreOptionsButton.Click();
			}

			try
			{
													   //*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[1]
				salary = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[1]")).Text;
			}
			catch
			{
				salary = "Not specified";
			}

			try
			{										 //*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[2]
				city = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[2]")).Text;
			}
			catch
			{
				city = "Not specified";
			}

			try
			{												//*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[4]
				jobType = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[4]")).Text;
			}
			catch
			{
				jobType = "Not specified";
			}

			try
			{												//*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[5]
				expertiseLvl = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[5]")).Text;
			}
			catch
			{
				expertiseLvl = "Not specified";
			}

			try
			{                                           //*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[6]
				industry = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[6]")).Text;
			}
			catch
			{
				industry = "Not specified";
			}

			try
			{														 //*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[7]
				qualificationLvl = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[7]")).Text;
			}
			catch
			{
				qualificationLvl = "Not specified";
			}

			try
			{                                               //*[@id="__layout"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[8]
				department = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[8]")).Text;
			}
			catch
			{
				department = "Not specified";
			}

			try
			{	
				language = driver.FindElement(By.XPath("//*[@id=\"__layout\"]/div/div[4]/section/div[2]/aside/div[2]/div/div[2]/div[2]/div[9]")).Text;
			}
			catch
			{
				language = "Not specified";
			}


			jobPage = new JobPageModel(
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

			Console.WriteLine(jobPage.Salary);
			Console.WriteLine(jobPage.City);
			Console.WriteLine(jobPage.JobType);
			Console.WriteLine(jobPage.ExpertiseLvl);
			Console.WriteLine(jobPage.Industry);
			Console.WriteLine(jobPage.QualificationLvl);
			Console.WriteLine(jobPage.Department);
			Console.WriteLine(jobPage.Language);
			Console.WriteLine();

			return jobPage;
		}
	}
}
