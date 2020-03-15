namespace BlackdotTechTest.WebApi.Controllers
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using BlackdotTechTest.Domain.Commands;
	using BlackdotTechTest.Domain.Entities;
	using BlackdotTechTest.Domain.Queries;
	using MediatR;
	using Microsoft.AspNetCore.Mvc;

	[ApiController]
    [Route("[controller]")]
    public class SearchEngineInvestigateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SearchEngineInvestigateController(IMediator mediator)
        {
            _mediator = mediator;
        }

		[HttpGet]
		public async Task<IEnumerable<SearchEngineQuery>> GetAllResults()
		{
			return await _mediator.Send(new GetAllSearchEngineResultsQuery()).ConfigureAwait(false);
		}

		[HttpGet("{query}")]
        public async Task<SearchEngineQuery> GetResult(string query)
        {
            return await _mediator.Send(new GetSearchEngineResultQuery() { Query = query }).ConfigureAwait(false);
        }

        [HttpPost("{query}")]
        public async Task PerformQuery(string query)
        {
            await _mediator.Send(new PerformSearchEngineCommand() { Query = query }).ConfigureAwait(false);
        }
    }
}
