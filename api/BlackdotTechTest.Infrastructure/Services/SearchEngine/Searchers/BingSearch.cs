namespace BlackdotTechTest.Infrastructure.Services.SearchEngine
{
	using System.Collections.Generic;
	using System.Linq;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using OpenQA.Selenium.Remote;

	public class BingSearch : ISearchEngineSearcher
	{
		public const string SELECTOR = "#b_results > li:not([class=topborder]) > h2 > a";
		public string SearchEngineType => "Bing";

		private string BingUrl(string query) => $"https://www.bing.com/search?q={query}";

		public IEnumerable<SearchEngineQueryResultBuilderParameters> Search(RemoteWebDriver driver, string query)
		{
			driver.Navigate().GoToUrl(BingUrl(query));
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
