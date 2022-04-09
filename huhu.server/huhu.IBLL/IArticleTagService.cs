using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IArticleTagService : IBaseService<article_tag>
    {
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<article_tag> Query_ID(article_tag n);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<article_tag> PagingQuery(int PageIndex, int PageSize);

        /// <summary>
        /// 总数据量
        /// </summary>
        /// <returns></returns>
        int TotalVolume();

        List<article_tag> Query_Name(article_tag tag);

        bool Update_Condition(article_tag tag, string[] condition);
    }
}
