using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IArticleViewDAL : IBaseDAL<article_view>
    {
        List<article_view> Query_All();
        article_view Reading_Quantity(article_view view);
    }
}
