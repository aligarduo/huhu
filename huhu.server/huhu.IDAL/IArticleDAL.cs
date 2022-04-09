using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IArticleDAL : IBaseDAL<article_all>
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        List<article_all> Query_All();

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<article_all> Query_ID(article_all n);

        /// <summary>
        /// 根据用户id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        List<article_all> Query_UserID(user_all user);

        List<article_all> Query_UserID_ArticleID(user_all user, article_all article);

        /// <summary>
        /// 随机分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        List<article_all> RandomSortPagingQuery(int pageSize);

        /// <summary>
        /// 总数据量
        /// </summary>
        /// <returns></returns>
        int TotalVolume();

        int TotalVolume(user_all user);

        /// <summary>
        /// 搜索-综合排序
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        List<article_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word);

        List<article_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word);

        bool Update_Condition(article_all article, string[] condition);

        List<article_all> PagingQuery_UserID(user_all user, int pageIndex, int pageSize);
    }
}
