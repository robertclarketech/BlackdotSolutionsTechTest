namespace BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces
{
	using System.Collections.Generic;
	using BlackdotTechTest.Domain.Builders;

	public interface ISearchEngineService
	{
		IEnumerable<SearchEngineQueryResultBuilderParameters> PerformSearch(string query);
	}
}
