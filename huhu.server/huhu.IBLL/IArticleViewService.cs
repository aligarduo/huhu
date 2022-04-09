using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IArticleViewService : IBaseService<article_view>
    {
        List<article_view> Query_All();
        article_view Reading_Quantity(article_view view);
    }
}
