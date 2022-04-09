using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleReplyDAL : BaseDAL<article_reply>, IArticleReplyDAL
    {
        public List<article_reply> Query_Reply_ID(article_reply reply)
        {
            return GetEntities(x => x.reply_id == reply.reply_id).ToList();
        }

        public List<article_reply> Query_Reply_Comment_ID(article_reply reply)
        {
            return GetEntities(x => x.reply_comment_id == reply.reply_comment_id).ToList();
        }

        public List<article_reply> Query_Reply_To_Reply_ID(article_reply reply)
        {
            return GetEntities(x => x.reply_id == reply.reply_to_reply_id).ToList();
        }
    }
}
