using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ArticleDraftDAL : BaseDAL<article_draft>, IArticleDraftDAL
    {
        public bool Update_Condition(article_draft draft, string[] condition)
        {
            return UpdateCondition(draft, condition);
        }

        public List<article_draft> Condition_PagingQuery(int pageIndex, int pageSize, article_draft draft) {
            return GetConditionPagingQuery(x => x.user_id == draft.user_id, x => x.draft_id != null, pageIndex, pageSize).ToList();
        }

        public List<article_draft> Condition_PagingQuery_DESC(int pageIndex, int pageSize, article_draft draft) {
            return GetConditionPagingQuery_DESC(x => x.user_id == draft.user_id, k => k.draft_id, pageIndex, pageSize).ToList();
        }

        public List<article_draft> Query_ID(article_draft draft)
        {
            return GetEntities(x => x.draft_id == draft.draft_id).ToList();
        }

        public int CriteriaTotalVolume(article_draft draft)
        {
            return Count(x => x.user_id == draft.user_id);
        }

        public bool Deletes(article_draft draft)
        {
            return Delete(draft);
        }

    }
}
