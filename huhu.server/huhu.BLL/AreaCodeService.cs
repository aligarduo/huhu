using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class AreaCodeService : BaseService<area_code_all>, IAreaCodeService
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return db.AreaCodeDal.GetCount();
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<area_code_all> Query_ID(area_code_all n)
        {
            return db.AreaCodeDal.Query_ID(n);
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<area_code_all> Query_All()
        {
            return db.AreaCodeDal.Query_All();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<area_code_all> PagingQuery(int pageIndex, int pageSize)
        {
            return db.AreaCodeDal.PagingQuery(pageIndex, pageSize);
        }

    }
}
