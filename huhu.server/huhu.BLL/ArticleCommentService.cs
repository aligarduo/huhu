using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleCommentService : BaseService<article_comment>, IArticleCommentService
    {
        public List<article_comment> ConditionPagingQuery(int pageIndex, int pageSize, article_comment comment)
        {
            return db.ArticleCommentDal.ConditionPagingQuery(pageIndex, pageSize, comment);
        }

        public List<article_comment> Query_ID(article_comment comment)
        {
            return db.ArticleCommentDal.Query_ID(comment);
        }

        public int CriteriaTotalVolume(article_comment comment)
        {
            return db.ArticleCommentDal.CriteriaTotalVolume(comment);
        }

        public List<article_comment> Query_Article_ID(article_comment comment)
        {
            return db.ArticleCommentDal.Query_Article_ID(comment);
        }
    }
}
