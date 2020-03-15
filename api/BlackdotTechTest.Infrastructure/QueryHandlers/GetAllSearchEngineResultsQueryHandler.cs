namespace BlackdotTechTest.Infrastructure.QueryHandlers
{
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using BlackdotTechTest.Domain.Entities;
	using BlackdotTechTest.Domain.Queries;
	using BlackdotTechTest.Infrastructure.EntityFramework;
	using MediatR;
	using Microsoft.EntityFrameworkCore;

	public class GetAllSearchEngineResultsQueryHandler : IRequestHandler<GetAllSearchEngineResultsQuery, IEnumerable<SearchEngineQuery>>
	{
		private readonly BlackdotTechTestContext _context;

		public GetAllSearchEngineResultsQueryHandler(BlackdotTechTestContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<SearchEngineQuery>> Handle(GetAllSearchEngineResultsQuery request, CancellationToken cancellationToken)
		{
			return await _context
				.SearchEngineQueries
				.ToListAsync()
				.ConfigureAwait(false);
		}
	}
}
