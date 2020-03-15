namespace BlackdotTechTest.Domain.Queries
{
	using System.Collections.Generic;
	using BlackdotTechTest.Domain.Entities;
	using MediatR;

	public class GetAllSearchEngineResultsQuery : IRequest<IEnumerable<SearchEngineQuery>>
    {
    }
}
