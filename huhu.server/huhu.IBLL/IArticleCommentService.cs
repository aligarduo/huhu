using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IArticleCommentService : IBaseService<article_comment>
    {
        List<article_comment> ConditionPagingQuery(int pageIndex, int pageSize, article_comment comment);

        List<article_comment> Query_ID(article_comment comment);

        int CriteriaTotalVolume(article_comment comment);

        List<article_comment> Query_Article_ID(article_comment comment);
    }
}
