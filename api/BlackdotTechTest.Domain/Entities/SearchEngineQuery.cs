namespace BlackdotTechTest.Domain.Entities
{
	using System;
	using System.Collections.Generic;
	using BlackdotTechTest.Domain.Builders;
	using BlackdotTechTest.Domain.Entities.Abstract;
	using BlackdotTechTest.Domain.Entities.Interfaces;
	using BlackdotTechTest.Domain.Validators;
	using FluentValidation;
	using System.Linq;

	public sealed class SearchEngineQuery : BaseEntity, IEntity<SearchEngineQuery>
    {
        public string Query { get; internal set; } = string.Empty;
        public List<SearchEngineQueryResult>? QueryResults { get; internal set; }

        public SearchEngineQuery EnsureValid()
        {
            var validationResults = new SearchEngineResultValidator().Validate(this);
            if (!validationResults.IsValid)
            {
                throw new ValidationException(validationResults.Errors);
            }
            return this;
        }

        public SearchEngineQuery UpdateDateEdited(DateTime? dateEdited)
        {
            SetDateEdited(dateEdited);
            return this;
        }

		public SearchEngineQuery UpdateSearchResults(SearchEngineQueryResultBuilder searchEngineQueryResultBuilder,
			IEnumerable<SearchEngineQueryResultBuilderParameters> results)
		{
			if (QueryResults == null)
			{
				throw new InvalidOperationException("QueryResults must be loaded before adding");
			}

			QueryResults = new List<SearchEngineQueryResult>();
			foreach (var result in results)
			{
				result.RelatedQueryId = Id;
				QueryResults.Add(searchEngineQueryResultBuilder.Build(result));
			}

			return EnsureValid();
		}
	}
}
