using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleCommentDAL : BaseDAL<article_comment>, IArticleCommentDAL
    {
        public List<article_comment> ConditionPagingQuery(int pageIndex, int pageSize, article_comment comment)
        {
            return GetConditionPagingQuery(x => x.article_id == comment.article_id, k => k.article_id != null, pageIndex, pageSize).ToList();
        }

        public List<article_comment> Query_ID(article_comment comment)
        {
            return GetEntities(x => x.comment_id == comment.comment_id).ToList();
        }

        public int CriteriaTotalVolume(article_comment comment)
        {
            return Count(x => x.comment_id == comment.comment_id);
        }

        public List<article_comment> Query_Article_ID(article_comment comment)
        {
            return GetEntities(x => x.article_id == comment.article_id).ToList();
        }
    }
}
