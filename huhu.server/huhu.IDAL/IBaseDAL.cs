using System;
using System.Linq;
using System.Linq.Expressions;

namespace huhu.IDAL
{
    public interface IBaseDAL<T>
    {
        IQueryable<T> GetEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetConditionPagingQuery(Expression<Func<T, bool>> Lambda_1, Expression<Func<T, bool>> Lambda_2, int pageIndex, int pageSize);
        T Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        int Count(Expression<Func<T, bool>> whereLambda);
    }
}
