namespace BlackdotTechTest.Infrastructure.CommandHandlers
{
	using System.Threading;
	using System.Threading.Tasks;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Domain.Commands;
	using BlackdotTechTest.Infrastructure.EntityFramework;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using FluentValidation;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Logging;

	public class PerformSearchEngineCommandHandler : AsyncRequestHandler<PerformSearchEngineCommand>
    {
        private readonly BlackdotTechTestContext _blackdotTechTestContext;
        private readonly ILogger<PerformSearchEngineCommandHandler> _logger;
        private readonly SearchEngineQueryBuilder _searchEngineQueryBuilder;
		private readonly SearchEngineQueryResultBuilder _searchEngineQueryResultBuilder;
		private readonly ISearchEngineService _searchEngineService;

		public PerformSearchEngineCommandHandler(
			BlackdotTechTestContext blackdotTechTestContext,
			ILogger<PerformSearchEngineCommandHandler> logger,
			SearchEngineQueryBuilder builder,
			ISearchEngineService searchEngineService,
			SearchEngineQueryResultBuilder searchEngineQueryResultBuilder)
		{
			_blackdotTechTestContext = blackdotTechTestContext;
            _logger = logger;
            _searchEngineQueryBuilder = builder;
			_searchEngineService = searchEngineService;
			_searchEngineQueryResultBuilder = searchEngineQueryResultBuilder;
		}

        protected override async Task Handle(PerformSearchEngineCommand request, CancellationToken cancellationToken)
        {
            try
            {
				if (string.IsNullOrWhiteSpace(request.Query))
				{
					throw new System.ArgumentException("Query cannot be null or whitespace", nameof(request.Query));
				}

                var searchEngineQuery = await _blackdotTechTestContext
					.SearchEngineQueries
					.Include(e => e.QueryResults)
					.FirstOrDefaultAsync(e => e.Query == request.Query)
					.ConfigureAwait(false);

				if(searchEngineQuery == null)
				{
					searchEngineQuery = _searchEngineQueryBuilder.Build(request);
					_blackdotTechTestContext.Add(searchEngineQuery);
					await _blackdotTechTestContext.SaveChangesAsync().ConfigureAwait(false);
					_blackdotTechTestContext.Entry(searchEngineQuery)
						.Collection(e => e.QueryResults)
						.Load();
				}

				var result = _searchEngineService.PerformSearch(searchEngineQuery.Query);

				searchEngineQuery = searchEngineQuery.UpdateSearchResults(_searchEngineQueryResultBuilder, result);
				_blackdotTechTestContext.Update(searchEngineQuery);
				await _blackdotTechTestContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (ValidationException argumentException)
            {
                _logger.LogError(argumentException, "Failed to create entity");
                throw;
            }
            catch (DbUpdateException dbUpdateException)
            {
                _logger.LogError(dbUpdateException, "Failed to insert entity into database");
                throw;
            }
        }
    }
}
