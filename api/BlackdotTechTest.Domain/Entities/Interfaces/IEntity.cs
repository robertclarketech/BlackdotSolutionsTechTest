namespace BlackdotTechTest.Domain.Entities.Interfaces
{
	using System;
	using BlackdotTechTest.Domain.Entities.Abstract;

	internal interface IEntity<TEntity> where TEntity : BaseEntity
    {
        TEntity UpdateDateEdited(DateTime? dateEdited);

        TEntity EnsureValid();
    }
}
