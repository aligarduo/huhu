using huhu.DalFactory;
using huhu.IDAL;
using System;
using System.Linq;

namespace huhu.BLL
{
    public abstract class BaseService<T> : IBLL.IBaseService<T> where T : class
    {
        public IBaseDAL<T> currentDal { get; set; }
        public DbSession db { get; set; }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="whereLambda">Lambda表达式</param>
        /// <returns></returns>
        public IQueryable<T> GetEntities(System.Linq.Expressions.Expression<Func<T, bool>> whereLambda)
        {
            return currentDal.GetEntities(whereLambda);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T Add(T entity)
        {
            return currentDal.Add(entity);
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            return currentDal.Update(entity);
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            return currentDal.Delete(entity);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return db.SaveChanges();
        }

    }
}
