using huhu.DalFactory;
using huhu.IDAL;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace huhu.DAL
{
    public class BaseDAL<T> : IBaseDAL<T> where T : class
    {
        public DbContext Db { get { return DbContextFactory.GetCurrentDbContext(); } }

        public IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda) {
            return Db.Set<T>().AsNoTracking().Where(whereLambda).AsQueryable();
        }

        public IQueryable<T> GetConditionPagingQuery(Expression<Func<T, bool>> Lambda_1, Expression<Func<T, bool>> Lambda_2, int pageIndex, int pageSize) {
            return Db.Set<T>().Where(Lambda_1).OrderBy(Lambda_2).Skip(pageIndex).Take(pageSize).AsQueryable();
        }

        public IQueryable<T> GetConditionPagingQuery_DESC(Expression<Func<T, bool>> Lambda_1, Expression<Func<T, object>> Lambda_2, int pageIndex, int pageSize) {
            return Db.Set<T>().Where(Lambda_1).OrderByDescending(Lambda_2).Skip(pageIndex).Take(pageSize).AsQueryable();
        }

        public IQueryable<T> GetRandomSortPagingQuery(int pageSize) {
            return Db.Set<T>().AsQueryable().ToList().OrderBy(d => Guid.NewGuid()).Take(pageSize).AsQueryable();
        }

        public T Add(T entity) {
            return Db.Set<T>().Add(entity);
        }

        public bool Update(T entity) {
            Db.Set<T>().Attach(entity);
            Db.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool UpdateCondition(T entity, string[] condition) {
            Db.Set<T>().Attach(entity);
            ObjectStateEntry SetEntry = ((IObjectContextAdapter)Db).ObjectContext.ObjectStateManager.GetObjectStateEntry(entity);
            foreach (var t in condition) {
                SetEntry.SetModifiedProperty(t);
            }
            return true;
        }

        public bool Delete(T entity) {
            Db.Set<T>().Attach(entity);
            Db.Entry(entity).State = EntityState.Deleted;
            return true;
        }

        public int Count(Expression<Func<T, bool>> whereLambda) {
            return Db.Set<T>().Count(whereLambda);
        }

    }
}