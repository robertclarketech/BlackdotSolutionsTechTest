namespace BlackdotTechTest.Domain.Commands.Interfaces
{
	using BlackdotTechTest.Domain.Builders.Interfaces;
	using BlackdotTechTest.Domain.Entities.Abstract;
	using MediatR;

	public interface ICreateCommand<TEntity> : IRequest, IBuilderParameters<TEntity> where TEntity : BaseEntity
    {
    }
}
