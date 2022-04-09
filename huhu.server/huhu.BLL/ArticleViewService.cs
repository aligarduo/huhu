using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleViewService : BaseService<article_view>, IArticleViewService
    {
        public List<article_view> Query_All() {
            return db.ArticleViewDal.Query_All();
        }

        public article_view Reading_Quantity(article_view view)
        {
            return db.ArticleViewDal.Reading_Quantity(view);
        }

    }
}
