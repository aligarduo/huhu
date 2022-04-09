using huhu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.IDAL
{
    public interface IAreaCodeDAL : IBaseDAL<area_code_all>
    {
        /// <summary>
        /// 获取数据总条数
        /// </summary>
        /// <returns></returns>
        int GetCount();

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<area_code_all> Query_ID(area_code_all n);

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        List<area_code_all> Query_All();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<area_code_all> PagingQuery(int PageIndex, int PageSize);
    }
}
