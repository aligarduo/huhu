using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleDAL : BaseDAL<article_all>, IArticleDAL
    {
        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        public List<article_all> Query_All()
        {
            return GetEntities(p => p.article_id != null).ToList();
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<article_all> Query_ID(article_all article)
        {
            return GetEntities(p => p.article_id == article.article_id).ToList();
        }

        /// <summary>
        /// 根据用户id查询
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public List<article_all> Query_UserID(user_all user) {
            return GetEntities(p => p.user_id == user.user_id).ToList();
        }

        public List<article_all> Query_UserID_ArticleID(user_all user, article_all article) {
            return GetEntities(p => p.user_id == user.user_id && p.article_id == article.article_id).ToList();
        }

        /// <summary>
        /// 随机分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<article_all> RandomSortPagingQuery(int pageSize)
        {
            return GetRandomSortPagingQuery(pageSize).ToList();
        }

        /// <summary>
        /// 总数据量
        /// </summary>
        /// <returns></returns>
        public int TotalVolume()
        {
            return Count(p => p.article_id != null);
        }

        public int TotalVolume(user_all user) {
            return Count(p => p.user_id == user.user_id);
        }

        /// <summary>
        /// 搜索-综合排序
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="article"></param>
        /// <returns></returns>
        public List<article_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return GetConditionPagingQuery(p => p.title.Contains(key_word) | p.brief_content.Contains(key_word), p => p.article_id != "", pageIndex, pageSize).ToList();
        }

        public List<article_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return GetConditionPagingQuery(p => p.title.Contains(key_word) | p.brief_content.Contains(key_word), p => p.mtime != "", pageIndex, pageSize).ToList();
        }

        public bool Update_Condition(article_all article, string[] condition)
        {
            return UpdateCondition(article, condition);
        }


        public List<article_all> PagingQuery_UserID(user_all user,int pageIndex, int pageSize) {
            return GetConditionPagingQuery_DESC(p => p.user_id == user.user_id, p => p.article_id, pageIndex, pageSize).ToList();
        }

    }
}
