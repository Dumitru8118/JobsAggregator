using JobHub.API.Models.Interfaces;
using OpenQA.Selenium;

namespace JobHub.API.Models.Providers
{
    public class EJobs : IJobProvider
    {
		public string CommonXpath => "//*[@id=\"__layout\"]/div/div[4]/section[2]/div/main/ul/li";

		public  int JobsStartingNumber => 1;

		public  int JobsEndingNumber => 40;

		public  string PoliciesButton => "//*[@id=\"__layout\"]/div/div[5]/div/div[3]/button[2]";

		public string GetUrl(int currentPageNumber)
		{
			string url = "https://www.ejobs.ro/locuri-de-munca";

			if (currentPageNumber == 1)
			{
				return url;
			}

			return url + "/pagina" + currentPageNumber.ToString();
		}

		public string GetJobXpath(int position)
		{
			string jobXpath = this.CommonXpath + "[" + position.ToString() + "]";
			return jobXpath;
		}

		public string GetJobHrefAttXpath(int position)
		{

			string hrefAtt = this.CommonXpath + "[" + position.ToString() + "]/div/div[1]/a";
			return hrefAtt;
		}

		public string GetJobTitleXpath(int position)
		{//*[@id="__layout"]/div/div[4]/section[2]/div/main/ul/li[1]               /div/div[1]/div[2]/h2/a/span
			string jobTitle = this.CommonXpath + "[" + position.ToString() + "]/div/div[1]/div[2]/h2";
			return jobTitle;
		}

		public string GetCompanyXpath(int position)
		{
			string company = this.CommonXpath + "[" + position.ToString() + "]/div/div[1]/div[2]/h3";
			return company;
		}

		public string GetDatePostedXpath(int position)
		{
			string datePosted = this.CommonXpath + "[" + position.ToString() + "]/div/div[1]/div[1]/div[1]/div[1]";
			return datePosted;
		}
		public void ScrollToBottom(IWebDriver driver)
		{
			IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;

			for (int i = 0; i < 2200; i++)
			{
				jse.ExecuteScript("window.scrollBy(0,5)", "");
			}
		}
	}
}
