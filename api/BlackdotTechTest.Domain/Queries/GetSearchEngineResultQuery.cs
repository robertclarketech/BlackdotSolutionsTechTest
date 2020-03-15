namespace BlackdotTechTest.Domain.Queries
{
	using BlackdotTechTest.Domain.Entities;
	using MediatR;

	public class GetSearchEngineResultQuery : IRequest<SearchEngineQuery>
    {
        public string Query { get; set; } = string.Empty;
    }
}
