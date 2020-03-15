namespace BlackdotTechTest.Infrastructure.Services.SearchEngine
{
	using System.ComponentModel.DataAnnotations;

	public enum DriverType
	{
		[Display(Name = "Firefox")]
		Firefox = 1,
		[Display(Name = "Chrome")]
		Chrome = 2
	}
}
