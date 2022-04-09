using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class AreaCodeDAL : BaseDAL<area_code_all>, IAreaCodeDAL
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        public int GetCount()
        {
            return Count(x => x.item_id.Contains(""));
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<area_code_all> Query_ID(area_code_all n)
        {
            List<area_code_all> list = GetEntities(x => x.item_id == n.item_id).ToList();
            return list;
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<area_code_all> Query_All()
        {
            List<area_code_all> list = GetEntities(x => x.item_id != null).ToList();
            return list;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<area_code_all> PagingQuery(int PageIndex, int PageSize)
        {
            return GetConditionPagingQuery(x => x.item_id.Contains(""), k => k.item_id.Contains(""), PageIndex, PageSize).ToList();
        }


    }
}
