using Microsoft.AspNetCore.Mvc;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Xml.Linq;

namespace JobHub.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ScrapeController : ControllerBase
	{
		// GET: ScrapeController
		[HttpGet]
		public IEnumerable<string> Get()
		{
			// Send get request to ejobs
			const String url = "https://www.ejobs.ro/locuri-de-munca";
			var httpClient = new HttpClient();
			var html = httpClient.GetStringAsync(url).Result;
			var htmlDocument = new HtmlDocument();
			htmlDocument.LoadHtml(html);

			// Get the dashboard jobs list
			var ulNodes = htmlDocument.DocumentNode.SelectSingleNode("//ul[@class='JobList__List']");

			WriteToCSV(ulNodes.ChildNodes, "output.csv");

			List<string> hrefAttributes = new List<string>();
			if (ulNodes != null)
			{
				// Select all li elements within the ul
				HtmlNodeCollection liNodes = ulNodes.SelectNodes(".//li[@class='JobCard']");

				if (liNodes != null)
				{
					// Iterate through each anchor element
					foreach (HtmlNode liNode in liNodes)
					{
						// Get the value of the href attribute
						var anchorNode = liNode.SelectSingleNode("//a[@class='JCContent__Logo']");
						string href = anchorNode.GetAttributeValue("href", "");
						hrefAttributes.Add(href);
					}

					// Output the href attributes
					Console.WriteLine("Href Attributes:");
					foreach (string href in hrefAttributes)
					{
						Console.WriteLine(href);
					}

					Console.WriteLine(hrefAttributes.Count);
				}
			}
			else
			{
				Console.WriteLine("No li elements found.");
			}

			return hrefAttributes;
		}

		static void WriteToCSV(HtmlNodeCollection data, string filePath)
		{
			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.WriteLine(data.Count);
				//var data1 = data.Descendants();
				//writer.WriteLine(data.WriteContentTo());\
				//var nodeInnerElem = data.SelectNodes("//div[@class='JobCard']");
				foreach (var node in data)
				{
					var nodeInnerElem = node.SelectNodes("//a[@class='JCContent__Logo']");

					//foreach (var elem in nodeInnerElem)
					//{
					//	var nodeJobContent = elem.SelectNodes("//div[@class='JCContent']");
					//	var nodeAnchorElem = nodeJobContent.SelectNodes("//a[@class='JCContent__Logo']");

					//}

					//var nodeJobContent = nodeInnerElem.SelectSingleNode("//div[@class='JCContent']");
					//var nodeAnchorElem = nodeJobContent.SelectSingleNode("//a[@class='JCContent__Logo']");
					foreach(var childNode in nodeInnerElem)
					{
						writer.WriteLine(childNode.OuterHtml);
					}
					//writer.WriteLine(nodeJobContent.OuterHtml);
					//writer.WriteLine(nodeAnchorElem.OuterHtml);
					//writer.WriteLine();
				}
			}
		}

	}
}


