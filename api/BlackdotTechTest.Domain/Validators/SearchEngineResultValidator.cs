namespace BlackdotTechTest.Domain.Validators
{
	using BlackdotTechTest.Domain.Entities;
	using FluentValidation;

	public class SearchEngineResultValidator : AbstractValidator<SearchEngineQuery>
    {
        public SearchEngineResultValidator()
        {
            RuleFor(x => x.Query).NotEmpty();
        }
    }
}
