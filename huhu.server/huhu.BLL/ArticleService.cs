using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleService : BaseService<article_all>, IArticleService
    {
        public List<article_all> Query_All()
        {
            return db.ArticleDal.Query_All();
        }

        public List<article_all> Query_ID(article_all n)
        {
            return db.ArticleDal.Query_ID(n);
        }

        public List<article_all> Query_UserID(user_all user) {
            return db.ArticleDal.Query_UserID(user);
        }

        public List<article_all> Query_UserID_ArticleID(user_all user, article_all article) {
            return db.ArticleDal.Query_UserID_ArticleID(user, article);
        }

        /// <returns></returns>
        public List<article_all> GetRandomSortPagingQuery(int pageSize)
        {
            return db.ArticleDal.RandomSortPagingQuery(pageSize);
        }

        public int TotalVolume()
        {
            return db.ArticleDal.TotalVolume();
        }

        public int TotalVolume(user_all user) {
            return db.ArticleDal.TotalVolume(user);
        }

        public List<article_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return db.ArticleDal.Search_Composite_Ranking(pageIndex, pageSize, key_word);
        }

        public List<article_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return db.ArticleDal.Search_Newest_Ranking(pageIndex, pageSize, key_word);
        }

        public bool Update_Condition(article_all article, string[] condition)
        {
            return db.ArticleDal.Update_Condition(article, condition);
        }

        public List<article_all> PagingQuery_UserID(user_all user, int pageIndex, int pageSize) {
            return db.ArticleDal.PagingQuery_UserID(user, pageIndex, pageSize);
        }
    }
}
