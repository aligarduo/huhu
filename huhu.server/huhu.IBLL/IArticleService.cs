using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IArticleService : IBaseService<article_all>
    {
        List<article_all> Query_All();

        List<article_all> Query_ID(article_all article);

        List<article_all> Query_UserID(user_all user);

        List<article_all> Query_UserID_ArticleID(user_all user, article_all article);

        List<article_all> GetRandomSortPagingQuery(int pageSize);

        int TotalVolume();

        int TotalVolume(user_all user);

        List<article_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word);

        List<article_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word);

        bool Update_Condition(article_all article, string[] condition);

        List<article_all> PagingQuery_UserID(user_all user, int pageIndex, int pageSize);
    }
}
