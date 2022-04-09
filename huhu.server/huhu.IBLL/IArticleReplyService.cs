using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IArticleReplyService : IBaseService<article_reply>
    {
        List<article_reply> Query_Reply_ID(article_reply reply);
        List<article_reply> Query_Reply_Comment_ID(article_reply reply);
        List<article_reply> Query_Reply_To_Reply_ID(article_reply reply);
    }
}
