using ContainerManager.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ContainerManager.Infrastructure.Repositories
{
	public class Repository<TEntity> where TEntity : BaseEntity
	{
		private static List<TEntity> fakeDB;

		public Repository()
		{
			fakeDB = new List<TEntity>();
		}

		/// <summary>
		/// Get object <see cref="TEntity"/> its unique identifier
		/// </summary>
		/// <param name="id">Unique identifier of the entity</param>
		/// <returns><see cref="TEntity"/> entity, if the record does not exist, it returns null</returns>
		public async Task<TEntity> GetByIdAsync(Guid id)
		{
			return fakeDB.FirstOrDefault(x => x.Id == id);
		}

		/// <summary>
		/// Get  <see cref="TEntity"/>  entities by query
		/// </summary>
		/// <param name="query">Predicate to be used for query</param>
		/// <returns>List of entities</returns>
		public async Task<IEnumerable<TEntity>> GetByQueryAsync(Expression<Func<TEntity, bool>> query)
		{
			return GetEntitiesByQuery(query).ToList();
		}

		/// <summary>
		/// Get  a <see cref="TEntity"/>  entity by query
		/// </summary>
		/// <param name="query">Predicate to be used for query</param>
		/// <returns>The <see cref="TEntity"/>  entity</returns>
		public virtual Task<TEntity> GetSingleByQueryAsync(Expression<Func<TEntity, bool>> query)
		{
			return Task.FromResult(GetEntitiesByQuery(query).SingleOrDefault());
		}

		/// <summary>
		/// Creates a <see cref="TEntity"/> entity 
		/// </summary>
		/// <param name="entity">entity <see cref="TEntity"/> to be created</param>
		public virtual async Task AddAsync(TEntity entity)
		{
			fakeDB.Add(entity);
		}		

		/// <summary>
		/// Creates a <see cref="TEntity"/> entity 
		/// </summary>
		/// <param name="entity">entity <see cref="TEntity"/> to be created</param>
		public virtual async Task DeleteAsync(Guid id)
		{
			var temp = fakeDB;
			temp.RemoveAll(s => s.Id == id);
			fakeDB = temp.ToList();
		}

		private IEnumerable<TEntity> GetEntitiesByQuery(Expression<Func<TEntity, bool>> query)
		{
			var func = query.Compile();
			return fakeDB.Where(func);
		}
	}
}
