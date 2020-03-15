namespace BlackdotTechTest.Domain.Extensions
{
	using BlackdotTechTest.Domain.Builders;
	using Microsoft.Extensions.DependencyInjection;

	public static class IServiceCollectionExtensions
    {
        public static IServiceCollection RegisterDomainServices(this IServiceCollection serviceProvider)
        {
            serviceProvider.AddTransient<SearchEngineQueryBuilder>();
            serviceProvider.AddTransient<SearchEngineQueryResultBuilder>();
            return serviceProvider;
        }
    }
}
