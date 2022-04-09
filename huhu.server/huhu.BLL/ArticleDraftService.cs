using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ArticleDraftService : BaseService<article_draft>, IArticleDraftService
    {
        public bool Update_Condition(article_draft draft, string[] condition)
        {
            return db.ArticleDraftDal.Update_Condition(draft, condition);
        }

        public List<article_draft> Condition_PagingQuery(int pageIndex, int pageSize, article_draft draft)
        {
            return db.ArticleDraftDal.Condition_PagingQuery(pageIndex, pageSize, draft);
        }

        public List<article_draft> Condition_PagingQuery_DESC(int pageIndex, int pageSize, article_draft draft) {
            return db.ArticleDraftDal.Condition_PagingQuery_DESC(pageIndex, pageSize, draft);
        }

        public List<article_draft> Query_ID(article_draft draft)
        {
            return db.ArticleDraftDal.Query_ID(draft);
        }

        public int CriteriaTotalVolume(article_draft draft)
        {
            return db.ArticleDraftDal.CriteriaTotalVolume(draft);
        }

        public bool Deletes(article_draft draft)
        {
            return db.ArticleDraftDal.Deletes(draft);
        }

    }
}
