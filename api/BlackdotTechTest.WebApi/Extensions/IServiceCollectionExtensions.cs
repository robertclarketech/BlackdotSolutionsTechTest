namespace BlackdotTechTest.WebApi.Extensions
{
	using BlackdotTechTest.Infrastructure.Services.SearchEngine;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Options;

	public static class IServiceCollectionExtensions
    {
        public static IServiceCollection BuildConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
			serviceCollection.Configure<SearchEngineServiceSettings>(configuration.GetSection("SearchEngineServiceSettings"));
            return serviceCollection;
        }
    }
}
