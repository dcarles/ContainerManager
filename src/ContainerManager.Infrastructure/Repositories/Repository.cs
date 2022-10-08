using ContainerManager.Domain;
using ContainerManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        public Repository()
        {
           
        }

        /// <summary>
        /// Get object <see cref="TEntity"/> its unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the entity</param>
        /// <returns><see cref="TEntity"/> entity, if the record does not exist, it returns null</returns>
        public async Task<TEntity> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get  <see cref="TEntity"/>  entities by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>List of entities</returns>
        public async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> query)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get  a <see cref="TEntity"/>  entity by query
        /// </summary>
        /// <param name="query">Predicate to be used for query</param>
        /// <returns>The <see cref="TEntity"/>  entity</returns>
        public virtual Task<TEntity> GetSingleByQueryAsync(Expression<Func<TEntity, bool>> query = null)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be created</param>
        public virtual async Task AddAsync(TEntity entity)
        {
         
        }

        /// <summary>
        ///  Updates a <see cref="TEntity"/> entity 
        /// </summary>
        /// <param name="entity">entity <see cref="TEntity"/> to be updated</param>
        public virtual async Task UpdateAsync(TEntity entity)
        {
          
        }

        private IQueryable<TEntity> GetEntitiesByQuery(Expression<Func<TEntity, bool>> query)
        {
            throw new NotImplementedException();
        }
    }
}
