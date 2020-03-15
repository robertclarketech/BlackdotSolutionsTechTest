namespace BlackdotTechTest.Infrastructure.Extensions
{
	using BlackdotTechTest.Infrastructure.Services.SearchEngine;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using Microsoft.Extensions.DependencyInjection;

	public static class IServiceCollectionExtensions
	{
		public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection serviceProvider)
		{
			serviceProvider.AddTransient<ISearchEngineService, SearchEngineService>();
			serviceProvider.AddTransient<ISearchEngineSearcher, GoogleSearch>();
			serviceProvider.AddTransient<ISearchEngineSearcher, BingSearch>();
			return serviceProvider;
		}
	}
}
