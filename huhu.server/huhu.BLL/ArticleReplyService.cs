using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleReplyService : BaseService<article_reply>, IArticleReplyService
    {
        public List<article_reply> Query_Reply_ID(article_reply reply)
        {
            return db.ArticleReplyDal.Query_Reply_ID(reply);
        }

        public List<article_reply> Query_Reply_Comment_ID(article_reply reply)
        {
            return db.ArticleReplyDal.Query_Reply_Comment_ID(reply);
        }

        public List<article_reply> Query_Reply_To_Reply_ID(article_reply reply)
        {
            return db.ArticleReplyDal.Query_Reply_To_Reply_ID(reply);
        }
    }
}
