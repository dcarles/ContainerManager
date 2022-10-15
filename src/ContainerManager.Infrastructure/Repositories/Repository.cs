using ContainerManager.Domain.Exceptions;
using ContainerManager.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
	public abstract class Repository<TEntity> where TEntity : BaseEntity
	{
        protected readonly ContainerManagerDbContext _dbContext;

        protected Repository(ContainerManagerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Get object <see cref="TEntity"/> its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the entity</param>
        /// <returns><see cref="TEntity"/> entity, if the record does not exist, it returns null</returns>
        protected async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        protected async Task<TEntity?> GetByIdNoTrackingAsync(Guid id)
        {
            return await _dbContext.Set<TEntity>().AsNoTracking().Where(e => e.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Get  <see cref="TEntity"/>  entities by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>List of entities</returns>
        protected async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> query, bool avoidTracking = false)
        {
            var entities = GetEntitiesByQuery(query);
            
            if(avoidTracking) entities = entities.AsNoTracking();

            return await entities.ToListAsync();
        }

        /// <summary>
        /// Get  a <see cref="TEntity"/>  entity by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>The <see cref="TEntity"/>  entity</returns>
        protected virtual Task<TEntity?> GetSingleByQueryAsync(Expression<Func<TEntity, bool>> query = null)
        {
            return GetEntitiesByQuery(query).SingleOrDefaultAsync();
        }

        /// <summary>
        /// Creates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be created</param>
        protected virtual async Task AddAsync(TEntity entity)
        {   
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        ///  Updates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be updated</param>
        protected virtual async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        private IQueryable<TEntity> GetEntitiesByQuery(Expression<Func<TEntity, bool>> query)
        {
            return _dbContext.Set<TEntity>().Where(query);
        }

        protected virtual async Task DeleteAsync(TEntity entity)
        {
			_dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
