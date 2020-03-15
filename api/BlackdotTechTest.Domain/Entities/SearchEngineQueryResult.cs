namespace BlackdotTechTest.Domain.Entities
{
	using System;
	using BlackdotTechTest.Domain.Entities.Abstract;
	using BlackdotTechTest.Domain.Entities.Interfaces;
	using BlackdotTechTest.Domain.Validators;
	using FluentValidation;

	public sealed class SearchEngineQueryResult : BaseEntity, IEntity<SearchEngineQueryResult>
    {
        public string ResultText { get; set; } = string.Empty;
        public string ResultLink { get; set; } = string.Empty;
		public string SearchEngineType { get; set; } = string.Empty;

		public int SearchEngineQueryId { get; set; }
        public SearchEngineQuery? SearchEngineQuery { get; set; }

        public SearchEngineQueryResult EnsureValid()
        {
            var validationResults = new SearchEngineQueryResultValidator().Validate(this);
            if (!validationResults.IsValid)
            {
                throw new ValidationException(validationResults.Errors);
            }
            return this;
        }

        public SearchEngineQueryResult UpdateDateEdited(DateTime? dateEdited)
        {
            SetDateEdited(dateEdited);
            return this;
        }
    }
}
