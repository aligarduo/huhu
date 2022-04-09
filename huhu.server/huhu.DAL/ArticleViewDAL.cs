using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleViewDAL : BaseDAL<article_view>, IArticleViewDAL
    {
        public List<article_view> Query_All() {
            return GetEntities(p => p.article_id != null).ToList();
        }

        public article_view Reading_Quantity(article_view view)
        {
            return GetEntities(x => x.article_id == view.article_id).FirstOrDefault();
        }

    }
}
