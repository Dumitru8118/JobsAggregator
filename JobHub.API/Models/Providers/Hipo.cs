using JobHub.API.Models.Interfaces;
using OpenQA.Selenium;

namespace JobHub.API.Models.Providers
{
    public class Hipo : IJobProvider
    {
        public string CommonXpath => "/html/body/div[3]/section/div/div/div[1]";

        public int JobsStartingNumber => 3;

        public int JobsEndingNumber => 42;

        public string PoliciesButton => string.Empty;

        public string GetUrl(int currentPageNumber)
        {
            string url = "https://www.hipo.ro/locuri-de-munca/cautajob";

            if (currentPageNumber == 1)
            {
                return url;
            }

            return url + "/" + currentPageNumber.ToString();
        }

        public string GetJobXpath(int position)
        {
            string jobXpath = this.CommonXpath + "/div[" + position.ToString() + "]";
            return jobXpath;
        }

        public string GetJobHrefAttXpath(int position)
        {

            string hrefAtt = this.CommonXpath + "/div[" + position.ToString() + "]/div/div[1]/div[2]/a";
            return hrefAtt;
        }

        public string GetJobTitleXpath(int position)
        {
            string jobTitle = this.CommonXpath + "/div[" + position.ToString() + "]/div/div[1]/div[2]/a";
            return jobTitle;
        }

        public string GetCompanyXpath(int position)
        {
            string company = this.CommonXpath + "/div[" + position.ToString() + "]/div/div[1]/div[2]/p";
            return company;
        }

        public string GetDatePostedXpath(int position)
        {
            string datePosted = this.CommonXpath + "/div[" + position.ToString() + "]/div/div[1]/div[2]/div[1]";
            return datePosted;
        }
		public void ScrollToBottom(IWebDriver driver)
		{
			IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
			jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
		}
	}
}
