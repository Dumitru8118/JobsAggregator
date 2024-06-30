using OpenQA.Selenium;

namespace JobHub.API.Models.Interfaces
{
	public interface IJobProvider
	{
		string PoliciesButton { get; }
		string CommonXpath { get; }
		int JobsStartingNumber { get; }
		int JobsEndingNumber { get; }
		string GetUrl(int page);
		string GetJobXpath(int index);
		string GetJobHrefAttXpath(int index);
		string GetJobTitleXpath(int index);
		string GetCompanyXpath(int index);
		string GetDatePostedXpath(int index);
		void ScrollToBottom(IWebDriver driver);
	}
}
