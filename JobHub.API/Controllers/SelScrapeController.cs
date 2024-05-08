using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Xml.Linq;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace JobHub.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SelScrapeController : ControllerBase
	{
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
			//options.AddArgument("-headless");

			// Set up Chrome WebDriver (make sure you have chromedriver installed and its location in PATH)
			using (var driver = new ChromeDriver(options))
			{
				// Navigate to the webpage
				driver.Navigate().GoToUrl("https://www.ejobs.ro/locuri-de-munca");
				
				// Find and click the accept cookies button (assuming it has a class or id)
				var acceptCookiesButton = driver.FindElement(By.CssSelector(".CookiesPopup__AcceptButton"));
				if (acceptCookiesButton != null)
				{
					acceptCookiesButton.Click();
				}

				//var elemToScrollTo = driver.FindElement(By.CssSelector(".JobListPaginator"));

				IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
				// Scroll down the page to trigger visibility
				//System.Threading.Thread.Sleep(5000);

				jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight)");
				//System.Threading.Thread.Sleep(5000);

				// Wait for the elements to become visible (adjust wait time and conditions as needed)

				// Find the parent element with class selector "JobList__List"
				//var jobListElement = driver.FindElement(By.CssSelector(".JobList__List"));

				//var liElems = jobListElement.FindElements(By.TagName("li"));

				// Find all anchor elements within the parent element
				//var anchorElements = jobListElement.FindElements(By.TagName("a"));

				//var jobCardsWrapper = driver.FindElements(By.CssSelector(".JobList__List .JobCardWrapper.Visible .JobCard .JCContent"));
				
				var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
				var jobCardsWrapper = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(By.CssSelector(".JCContent")));
				
				//var jobCard = jobListElement.FindElements(By.XPath("//div[@class='JCContent']"));

				foreach (var job in jobCardsWrapper)
				{
					var anchorElem = job.FindElement(By.TagName("a"));
					// Get the value of the href attribute of the anchor element
					string href = anchorElem.GetAttribute("href");
					
					// Add the href attribute value to the list
					scrapedAnchorTexts.Add(href);
				}

				scrapedAnchorTexts.Add(scrapedAnchorTexts.Count.ToString());
			}

			return scrapedAnchorTexts;
		}
		}
}
