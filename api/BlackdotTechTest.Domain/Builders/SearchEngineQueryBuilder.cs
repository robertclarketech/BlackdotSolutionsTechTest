namespace BlackdotTechTest.Domain.Builders
{
	using BlackdotTechTest.Domain.Builders.Interfaces;
	using BlackdotTechTest.Domain.Commands;
	using BlackdotTechTest.Domain.Entities;

	public class SearchEngineQueryBuilder : IBuilder<SearchEngineQuery, PerformSearchEngineCommand>
    {
        public SearchEngineQuery Build(PerformSearchEngineCommand command)
        {
            var searchEngineQueryResult = new SearchEngineQuery()
            {
                Query = command.Query
            };
            searchEngineQueryResult.EnsureValid();
            return searchEngineQueryResult;
        }
    }
}
