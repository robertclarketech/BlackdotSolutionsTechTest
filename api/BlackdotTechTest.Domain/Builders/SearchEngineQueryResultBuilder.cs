namespace BlackdotTechTest.Domain.Builders
{
	using BlackdotTechTest.Domain.Builders.Interfaces;
	using BlackdotTechTest.Domain.Entities;

	public class SearchEngineQueryResultBuilder : IBuilder<SearchEngineQueryResult, SearchEngineQueryResultBuilderParameters>
    {
        public SearchEngineQueryResult Build(SearchEngineQueryResultBuilderParameters command)
        {
			var searchEngineQueryResult = new SearchEngineQueryResult()
			{
				ResultText = command.ResultText,
				ResultLink = command.ResultLink,
				SearchEngineQueryId = command.RelatedQueryId,
				SearchEngineType = command.SearchEngineType
			};
            searchEngineQueryResult.EnsureValid();
            return searchEngineQueryResult;
        }
    }

	public class SearchEngineQueryResultBuilderParameters : IBuilderParameters<SearchEngineQueryResult>
	{
		public string SearchEngineType { get; set; } = string.Empty;
		public int RelatedQueryId { get; set; }
		public string ResultText { get; set; } = string.Empty;
		public string ResultLink { get; set; } = string.Empty;
	}
}
