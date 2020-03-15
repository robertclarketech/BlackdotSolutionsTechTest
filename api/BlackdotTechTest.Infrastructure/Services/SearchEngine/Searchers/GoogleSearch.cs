namespace BlackdotTechTest.Infrastructure.Services.SearchEngine
{
	using System.Collections.Generic;
	using System.Linq;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using OpenQA.Selenium.Remote;

	public class GoogleSearch : ISearchEngineSearcher
	{
		public const string SELECTOR = "#main > div > div > div:nth-child(1) > a";
		public string SearchEngineType => "Google";

		private string GoogleUrl(string query) => $"https://www.google.com/search?q={query}";

		public IEnumerable<SearchEngineQueryResultBuilderParameters> Search(RemoteWebDriver driver, string query)
		{
			driver.Navigate().GoToUrl(GoogleUrl(query));
			var links = driver.FindElementsByCssSelector(SELECTOR);
			return links.Select(link => new SearchEngineQueryResultBuilderParameters
			{
				ResultLink = link.GetAttribute("href"),
				SearchEngineType = SearchEngineType,
				ResultText = link.Text
			}).ToList();
		}
	}
}
