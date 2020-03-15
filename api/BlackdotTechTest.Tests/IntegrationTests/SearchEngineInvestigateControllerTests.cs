namespace BlackdotTechTest.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Domain.Entities;
	using BlackdotTechTest.Domain.Extensions;
	using BlackdotTechTest.Infrastructure.EntityFramework;
	using BlackdotTechTest.Infrastructure.Services.SearchEngine.Interfaces;
	using BlackdotTechTest.WebApi.Controllers;
	using FluentValidation;
	using MediatR;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Moq;
	using Xunit;

	public class SearchEngineInvestigateControllerTests
    {
		private readonly List<SearchEngineQueryResultBuilderParameters> _searchQueryResultReturn;
		private readonly SearchEngineInvestigateController _controller;
		private readonly Mock<ISearchEngineService> _searchMock;

		public SearchEngineInvestigateControllerTests()
        {
			var services = new ServiceCollection();

			// setup EF Core In Memory
			services.AddDbContext<BlackdotTechTestContext>(options =>
				options.UseInMemoryDatabase(Guid.NewGuid().ToString())
			);

			// setup domain services
			services.RegisterDomainServices();

			// this needs to be mocked, since we don't actually want to call the search service
			_searchMock = new Mock<ISearchEngineService>();
			services.AddTransient(_ => _searchMock.Object);

			// add mediatr
			services.AddMediatR(AppDomain
				.CurrentDomain
				.GetAssemblies()
				.First(assembly => assembly.GetName().Name == "BlackdotTechTest.Infrastructure"));

			// add logging
			services.AddLogging();

			// build service provider
			var serviceProvider = services.BuildServiceProvider();

			// create controller
			_controller = new SearchEngineInvestigateController(serviceProvider.GetRequiredService<IMediator>());
			_searchQueryResultReturn = new List<SearchEngineQueryResultBuilderParameters>();
		}

        [Fact]
        public async Task GivenAnEmptyDatabase_AndPerformQueryIsNeverCalled_WhenGetAllResultsIsCalled_ThenAnEmptyCollectionWillBeReturned()
        {
			//arrange

			//act
			var result = await _controller.GetAllResults().ConfigureAwait(false);

			//assert
			Assert.Empty(result);
		}

		[Theory]
		[InlineData("")]
		[InlineData("Test")]
		[InlineData("Query")]
		public async Task GivenAnEmptyDatabase_AndPerformQueryIsNeverCalled_WhenGetResultIsCalled_ThenNullWillBeReturned(string query)
		{
			//arrange

			//act
			var result = await _controller.GetResult(query).ConfigureAwait(false);

			//assert
			Assert.Null(result);
		}

		[Fact]
		public async Task GivenPerformQueryIsCalled_WhenGetAllResultsIsCalled_ThenThereWillBeOneElementWithTheGivenQueryName()
		{
			//arrange
			const string query = "Test";
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await _controller.PerformQuery(query).ConfigureAwait(false);
			var result = await _controller.GetAllResults().ConfigureAwait(false);

			//assert
			Assert.Single(result);
			Assert.Equal(query, result.First().Query);
		}

		[Theory]
		[InlineData("Test")]
		[InlineData("Test", "Query", "SomeOtherQuery", "ThisIsATest")]
		public async Task GivenPerformQueryIsCalledMoreThanOnce_WhenGetAllResultsIsCalled_ThenThereWillBeTheSameCountOfElementsWithTheGivenQueryNames(params string[] queries)
		{
			//arrange
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			foreach (var query in queries)
			{
				await _controller.PerformQuery(query).ConfigureAwait(false);
			}
			var result = await _controller.GetAllResults().ConfigureAwait(false);

			//assert
			Assert.Equal(queries.Length, result.Count());
			var queryNames = result.Select(e => e.Query);
			foreach (var query in queries)
			{
				Assert.Contains(query, queryNames);
			}
		}

		[Fact]
		public async Task GivenPerformQueryIsCalled_AndNoResultsAreReturnedFromPerformSearch_WhenGetResultIsCalled_ThenTheResultWillHaveTheGivenQueryName_AndAnEmptyListOfResults()
		{
			//arrange
			const string query = "Test";
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await _controller.PerformQuery(query).ConfigureAwait(false);
			var result = await _controller.GetResult(query).ConfigureAwait(false);

			//assert
			Assert.Equal(query, result.Query);
			Assert.Empty(result.QueryResults);
		}

		[Theory]
		[InlineData("Test")]
		[InlineData("Test", "Query", "SomeOtherQuery", "ThisIsATest")]
		public async Task GivenPerformQueryIsCalledMoreThanOnce_WhenGetResultIsCalledForEachQuery_ThenEachResultWillHaveTheGivenQueryName_AndAnEmptyListOfResults(params string[] queries)
		{
			//arrange
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			foreach (var query in queries)
			{
				await _controller.PerformQuery(query).ConfigureAwait(false);
			}

			//assert
			foreach (var query in queries)
			{
				var result = await _controller.GetResult(query).ConfigureAwait(false);
				Assert.Equal(query, result.Query);
				Assert.Empty(result.QueryResults);
			}
		}

		[Fact]
		public async Task GivenPerformQueryIsCalled_AndThereIsOneInvalidQueryResult_WhenPerformQueryIsCalled_ThenAValidationExceptionWillBeThrown()
		{
			//arrange
			const string query = "Test";
			var expectedQueryResult = new SearchEngineQueryResultBuilderParameters { ResultLink = "", ResultText = "", SearchEngineType = "" };
			_searchQueryResultReturn.Add(expectedQueryResult);
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await Assert.ThrowsAsync<ValidationException>(async() => await _controller.PerformQuery(query).ConfigureAwait(false)).ConfigureAwait(false);
		}

		[Fact]
		public async Task GivenPerformQueryIsCalledWithAnInvalidString_WhenPerformQueryIsCalled_ThenAnArgumentExceptionWillBeThrown()
		{
			//arrange
			var query = string.Empty;
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await Assert.ThrowsAsync<ArgumentException>(async () => await _controller.PerformQuery(query).ConfigureAwait(false)).ConfigureAwait(false);
		}

		[Fact]
		public async Task GivenPerformQueryIsCalled_AndThereIsOneValidQueryResult_WhenGetResultIsCalled_ThenThereWillBeOneQueryWithASingleQueryResult()
		{
			//arrange
			const string query = "Test";
			var expectedQueryResult = new SearchEngineQueryResultBuilderParameters { ResultLink = "TestLink", ResultText = "ResultText", SearchEngineType = "TestEngine" };
			_searchQueryResultReturn.Add(expectedQueryResult);
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await _controller.PerformQuery(query).ConfigureAwait(false);
			var result = await _controller.GetResult(query).ConfigureAwait(false);

			//assert
			Assert.Single(result.QueryResults);
			AssertQueryResultMatch(expectedQueryResult, result.QueryResults[0]);
		}

		[Theory]
		[InlineData(0)]
		[InlineData(1)]
		[InlineData(4)]
		[InlineData(12)]
		public async Task GivenPerformQueryIsCalled_AndThereAreMultipleValidQueryResults_WhenGetResultIsCalled_ThenThereWillBeOneQueryWithTheQueryResults(int expectedAmount)
		{
			//arrange
			const string query = "Test";
			for (var i = 0; i < expectedAmount; i++)
			{
				var expectedQueryResult = new SearchEngineQueryResultBuilderParameters { ResultLink = $"TestLink{i}", ResultText = $"ResultText{i}", SearchEngineType = $"TestEngine{i}" };
				_searchQueryResultReturn.Add(expectedQueryResult);
			}
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			//act
			await _controller.PerformQuery(query).ConfigureAwait(false);
			var result = await _controller.GetResult(query).ConfigureAwait(false);

			//assert
			Assert.Equal(expectedAmount, result.QueryResults.Count);
		}

		[Theory]
		[InlineData(1,0)]
		[InlineData(1,3)]
		[InlineData(4,10)]
		[InlineData(12,4)]
		public async Task GivenPerformQueryIsCalledMultipleTimes_WithDifferentQueries_AndThereAreMultipleValidQueryResults_WhenGetResultIsCalled_ThenThereWillBeMultipleQueriesWithTheCorrectAmountOfResults(int expectedQueriesAmount, int expectedResultsAmount)
		{
			//arrange
			const string query = "Test";
			for (var i = 0; i < expectedResultsAmount; i++)
			{
				var expectedQueryResult = new SearchEngineQueryResultBuilderParameters { ResultLink = $"TestLink{i}", ResultText = $"ResultText{i}", SearchEngineType = $"TestEngine{i}" };
				_searchQueryResultReturn.Add(expectedQueryResult);
			}
			_searchMock.Setup(e => e.PerformSearch(It.IsAny<string>())).Returns(_searchQueryResultReturn);

			for (var i = 0; i < expectedQueriesAmount; i++)
			{
				//act
				await _controller.PerformQuery($"{query}{i}").ConfigureAwait(false);

				//assert
				var result = await _controller.GetResult($"{query}{i}").ConfigureAwait(false);
				Assert.Equal(expectedResultsAmount, result.QueryResults.Count);
			}
		}

		private static void AssertQueryResultMatch(SearchEngineQueryResultBuilderParameters expected, SearchEngineQueryResult result)
		{
			Assert.Equal(expected.ResultLink, result.ResultLink);
			Assert.Equal(expected.ResultText, result.ResultText);
			Assert.Equal(expected.SearchEngineType, result.SearchEngineType);
		}
	}
}
