namespace BlackdotTechTest.Domain.Validators
{
	using BlackdotTechTest.Domain.Entities;
	using FluentValidation;

	public class SearchEngineQueryResultValidator : AbstractValidator<SearchEngineQueryResult>
    {
        public SearchEngineQueryResultValidator()
        {
            RuleFor(x => x.ResultLink).NotEmpty();
            RuleFor(x => x.ResultText).NotEmpty();
			RuleFor(x => x.SearchEngineType).NotEmpty();
			RuleFor(x => x.SearchEngineQueryId).NotEmpty();
        }
    }
}
