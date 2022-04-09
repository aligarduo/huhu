using System;
using System.Linq;
using System.Linq.Expressions;

namespace huhu.IBLL
{
    public interface IBaseService<T>
    {
        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda);
        T Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        int SaveChanges();
    }
}
