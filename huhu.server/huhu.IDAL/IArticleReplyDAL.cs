using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IArticleReplyDAL : IBaseDAL<article_reply>
    {
        List<article_reply> Query_Reply_ID(article_reply reply);
        List<article_reply> Query_Reply_Comment_ID(article_reply reply);
        List<article_reply> Query_Reply_To_Reply_ID(article_reply reply);
    }
}
