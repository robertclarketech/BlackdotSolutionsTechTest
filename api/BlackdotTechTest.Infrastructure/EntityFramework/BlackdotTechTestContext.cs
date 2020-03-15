namespace BlackdotTechTest.Infrastructure.EntityFramework
{
	using BlackdotTechTest.Domain.Entities;
	using Microsoft.EntityFrameworkCore;

	public class BlackdotTechTestContext : DbContext
	{
		public BlackdotTechTestContext(DbContextOptions<BlackdotTechTestContext> options)
			: base(options)
		{
		}

		public DbSet<SearchEngineQuery>? SearchEngineQueries { get; set; }
		public DbSet<SearchEngineQueryResult>? SearchEngineQueryResults { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<SearchEngineQuery>(builder =>
			{
				builder.Property(x => x.Id).IsRequired().Metadata.IsPrimaryKey();
				builder.Property(x => x.Query).IsRequired();
			});

			modelBuilder.Entity<SearchEngineQueryResult>(builder =>
			{
				builder.Property(x => x.Id).IsRequired().Metadata.IsPrimaryKey();
				builder.Property(x => x.ResultLink).IsRequired();
				builder.Property(x => x.ResultText).IsRequired();
				builder
					.HasOne(x => x.SearchEngineQuery)
					.WithMany(x => x!.QueryResults)
					.HasForeignKey(x => x.SearchEngineQueryId);
			});
		}
	}
}
