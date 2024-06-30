using JobHub.API.Models.Interfaces;
using OpenQA.Selenium;

namespace JobHub.API.Models.Providers
{
    public class JobRadar24 : IJobProvider
    {
        public string CommonXpath => "/html/body/main/section/div[2]/div[1]/div[1]";

        public int JobsStartingNumber => 5;

        public int JobsEndingNumber => 23;

        public string PoliciesButton => "//*[@id=\"onetrust-accept-btn-handler\"]";

        public string GetUrl(int currentPageNumber)
        {
            string url = "https://www.jobradar24.ro/locuri-de-munca";

            if (currentPageNumber == 1)
            {
                return url;
            }

            int index = url.LastIndexOf('.');

            return url + "?page=" + currentPageNumber.ToString();
        }

        public string GetJobXpath(int position)
        {
            string jobXpath = this.CommonXpath + "/div[" + position.ToString() + "]";
            return jobXpath;
        }

        public string GetJobHrefAttXpath(int position)
        {

            string hrefAtt = this.CommonXpath + "/div[" + position.ToString() + "]/div/div/div[1]/div/div[1]/div[1]/div/a";
            return hrefAtt;
        }

        public string GetJobTitleXpath(int position)
        {
            string jobTitle = this.CommonXpath + "/div[" + position.ToString() + "]/div/div/div[1]/div/div[1]/div[1]/div/a/p";
            return jobTitle;
        }

        public string GetCompanyXpath(int position)
        {
            string company = this.CommonXpath + "/div[" + position.ToString() + "]/div/div/div[1]/div/div[1]/div[1]/div/p";
            return company;
        }

        public string GetDatePostedXpath(int position)
		{                                 ///html/body/main/section/div[2]/div[1]/div[1]/div[18]/div/div/div[2]/span/time
			string datePosted = this.CommonXpath + "/div[" + position.ToString() + "]/div/div/div[2]/span[2]";
            return datePosted;
        }
		public void ScrollToBottom(IWebDriver driver)
		{
			IJavaScriptExecutor jse = (IJavaScriptExecutor)driver;
			jse.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
		}
	}
}
