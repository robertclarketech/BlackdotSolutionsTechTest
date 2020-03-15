namespace BlackdotTechTest.Infrastructure.QueryHandlers
{
	using System.Threading;
	using System.Threading.Tasks;
	using BlackdotTechTest.Domain.Entities;
	using BlackdotTechTest.Domain.Queries;
	using BlackdotTechTest.Infrastructure.EntityFramework;
	using MediatR;
	using Microsoft.EntityFrameworkCore;

	public class GetSearchEngineResultQueryHandler : IRequestHandler<GetSearchEngineResultQuery, SearchEngineQuery>
    {
        private readonly BlackdotTechTestContext _context;

        public GetSearchEngineResultQueryHandler(BlackdotTechTestContext context)
        {
            _context = context;
        }

        public async Task<SearchEngineQuery> Handle(GetSearchEngineResultQuery request, CancellationToken cancellationToken)
        {
            return await _context
                .SearchEngineQueries
                .Include(e => e.QueryResults)
                .FirstOrDefaultAsync(e => e.Query == request.Query)
                .ConfigureAwait(false);
        }
    }
}
