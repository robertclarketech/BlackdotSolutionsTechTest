namespace BlackdotTechTest.Infrastructure.Services.SearchEngine
{
	using System;
	using System.Collections.Generic;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using Microsoft.Extensions.Options;
	using OpenQA.Selenium.Chrome;
	using OpenQA.Selenium.Firefox;
	using OpenQA.Selenium.Remote;

	public class SearchEngineService : ISearchEngineService
	{
		private readonly IEnumerable<ISearchEngineSearcher> _searchers;
		private readonly SearchEngineServiceSettings _settings;

		public SearchEngineService(IEnumerable<ISearchEngineSearcher> searchers,
			IOptions<SearchEngineServiceSettings> settings)
		{
			_searchers = searchers;
			_settings = settings.Value;
		}

		public IEnumerable<SearchEngineQueryResultBuilderParameters> PerformSearch(string query)
		{
			var result = new List<SearchEngineQueryResultBuilderParameters>();

			foreach (var searcher in _searchers)
			{
				using (var driver = GetDriver())
				{
					result.AddRange(searcher.Search(driver, query));
				}
			}
			return result;
		}

		private RemoteWebDriver GetDriver()
		{
			switch (_settings.DriverType)
			{
				case DriverType.Firefox:
					var firefoxOptions = new FirefoxOptions();
					firefoxOptions.SetPreference("javascript.enabled", false);
					return new FirefoxDriver(firefoxOptions);
				case DriverType.Chrome:
					var chromeOptions = new ChromeOptions();
					chromeOptions.AddArgument("-disable-javascript");
					chromeOptions.AddUserProfilePreference("profile.managed_default_content_settings.javascript", 2);
					return new ChromeDriver(chromeOptions);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
	}
}
