namespace BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces
{
	using System.Collections.Generic;
	using BlackdotTechTest.Domain.Builders;
	using OpenQA.Selenium.Remote;

	public interface ISearchEngineSearcher
	{
		string SearchEngineType { get; }
		IEnumerable<SearchEngineQueryResultBuilderParameters> Search(RemoteWebDriver driver, string query);
	}
}
