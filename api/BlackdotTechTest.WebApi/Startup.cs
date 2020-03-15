namespace BlackdotTechTest.WebApi
{
	using System;
	using System.Linq;
	using System.Reflection;
	using BlackdotTechTest.Domain.Extensions;
	using BlackdotTechTest.Infrastructure.EntityFramework;
	using BlackdotTechTest.Infrastructure.Extensions;
	using BlackdotTechTest.WebApi.Extensions;
	using HealthChecks.UI.Client;
	using MediatR;
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Diagnostics.HealthChecks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;
	using Serilog;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.BuildConfiguration(Configuration);

			services.AddDbContext<BlackdotTechTestContext>(options =>
				options.UseSqlite(
					Configuration.GetConnectionString("Context"),
					builder => builder.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name)
				)
			);

			services.AddHealthChecks().AddSqlite(Configuration.GetConnectionString("Context"));

			services.RegisterDomainServices();
			services.RegisterInfrastructureServices();

			services.AddMediatR(AppDomain
				.CurrentDomain
				.GetAssemblies()
				.First(assembly => assembly.GetName().Name == "BlackdotTechTest.Infrastructure"));

			services.AddControllers().AddNewtonsoftJson(x => x
					.SerializerSettings
					.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			services.AddSwaggerGen(c => c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }));
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseSerilogRequestLogging();

			app.UseRouting();

			app.UseAuthorization();

			app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

			app.UseSwagger();
			app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapHealthChecks("/healthz", new HealthCheckOptions
				{
					Predicate = _ => true,
					ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
				});
			});
		}
	}
}
