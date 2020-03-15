namespace BlackdotTechTest.Domain.Commands
{
	using BlackdotTechTest.Domain.Commands.Interfaces;
	using BlackdotTechTest.Domain.Entities;

	public class PerformSearchEngineCommand : ICreateCommand<SearchEngineQuery>
    {
        public string Query { get; set; } = string.Empty;
    }
}
