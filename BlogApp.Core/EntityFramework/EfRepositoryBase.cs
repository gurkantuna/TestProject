using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using IEntity = BlogApp.Core.Entities.IEntity;

namespace BlogApp.Core.EntityFramework {
    public abstract class EfRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
        where TContext : DbContext, new() {
        private DbContext _dbContext;

        public TEntity Get(Expression<Func<TEntity, bool>> expression) {

            using (_dbContext = new TContext()) {
                return _dbContext.Set<TEntity>().FirstOrDefault(expression);
            }
        }

        public IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> expression = null) {

            using (_dbContext = new TContext()) {
                return expression != null ?
                            _dbContext.Set<TEntity>().Where(expression).ToList()
                            : _dbContext.Set<TEntity>().ToList();
            }
        }

        public void Add(TEntity entity) {

            using (_dbContext = new TContext()) {
                _dbContext.Entry(entity).State = EntityState.Added;
                _dbContext.SaveChanges();//TODO: UnitOfWork'e çevrilebilir.
            }
        }

        public void Update(TEntity entity) {

            using (_dbContext = new TContext()) {
                _dbContext.Entry(entity).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
        }

        public void Delete(TEntity entity) {

            using (_dbContext = new TContext()) {
                _dbContext.Entry(entity).State = EntityState.Deleted;
                _dbContext.SaveChanges();
            }
        }
    }
}
