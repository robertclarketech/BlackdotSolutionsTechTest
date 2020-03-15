namespace BlackdotTechTest.WebApi
{
	using System;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Hosting;
	using Serilog;
	using Serilog.Events;

	public static class Program
	{
		public static void Main(string[] args)
		{
			// See https://github.com/serilog/serilog-aspnetcore and https://nblumhardt.com/2019/10/serilog-in-aspnetcore-3/
			Log.Logger = new LoggerConfiguration()
						.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
						.Enrich.FromLogContext()
						.WriteTo.Console()
						.CreateLogger();

			try
			{
				Log.Information("Starting up");
				CreateHostBuilder(args).Build().Run();
			}
			catch (Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog()
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}
