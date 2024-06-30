using JobHub.API.Models;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using JobHub.API.Models.Providers;
using JobHub.API.Models.Interfaces;

namespace JobHub.API.Services
{
	public class MultiScraper<T> where T : class, IJobProvider, new()	
	{
		public static List<JobModel> ScrapeJobs(int pagesNumber)
		{
			List<JobModel> jobs = new List<JobModel>();

			ChromeDriver driver = ChromeDriverSingleton.Instance;

			T jobProvider = new T();

			int page = 1;

			while (true)
			{
				if (page == 1)
				{
					// Navigate to the webpage "https://www.ejobs.ro/locuri-de-munca"
					driver.Navigate().GoToUrl(jobProvider.GetUrl(page));

					// Find and click the accept cookies button (assuming it has a class or id)
					if (!string.IsNullOrEmpty(jobProvider.PoliciesButton))
					{
						var acceptCookiesButton = driver.FindElement(By.XPath(jobProvider.PoliciesButton));
						if (acceptCookiesButton != null)
						{
							acceptCookiesButton.Click();
						}
					}
					var values = GetJobs(driver, jobProvider);

					foreach (var value in values)
					{
						jobs.Add(value);
					}
					page++;
				}
				else
				{
					if (page <= pagesNumber)
					{
						driver.Navigate().GoToUrl(jobProvider.GetUrl(page));

						var values = GetJobs(driver, jobProvider);

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

		public static List<JobModel> GetJobs(ChromeDriver driver, T jobProvider)
		{
			List<JobModel> jobs = new List<JobModel>();

			jobProvider.ScrollToBottom(driver);

			for (int i = jobProvider.JobsStartingNumber; i <= jobProvider.JobsEndingNumber; i++)
			{
				// href value
				var job = driver.FindElement(By.XPath(jobProvider.GetJobXpath(i)));

				var url = string.Empty;

				// if no url it s find we can continue to the next job
				try
				{
					url = job.FindElement(By.XPath(jobProvider.GetJobHrefAttXpath(i))).GetAttribute("href");
				}
				catch
				{
					continue;
				}
				Console.WriteLine(url);

				// Get the Job Title
				var jobTitle = job.FindElement(By.XPath(jobProvider.GetJobTitleXpath(i))).Text;

				// Get the Company Name
				var jobCompany = job.FindElement(By.XPath(jobProvider.GetCompanyXpath(i))).Text;

				Console.WriteLine("Job Title: " + jobTitle + ", Company: " + jobCompany);

				var jobDatePosted = string.Empty;
				try
				{
					// Get the Date when the job was posted
					jobDatePosted = job.FindElement(By.XPath(jobProvider.GetDatePostedXpath(i))).Text;
				}
				catch
				{
					jobDatePosted = "Date not found";
				}
				

				Console.WriteLine("Date: " + jobDatePosted);
				Console.WriteLine();

				// Save the scraped job to the database
				JobModel jobEntity = new JobModel(url, jobTitle, jobCompany, jobDatePosted);

				jobs.Add(jobEntity);
			}

			return jobs;
		}
	}
}
