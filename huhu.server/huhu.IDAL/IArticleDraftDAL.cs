using huhu.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace huhu.IDAL
{
    public interface IArticleDraftDAL : IBaseDAL<article_draft>
    {
        bool Update_Condition(article_draft draft, string[] condition);

        List<article_draft> Condition_PagingQuery(int pageIndex, int pageSize, article_draft draft);

        List<article_draft> Condition_PagingQuery_DESC(int pageIndex, int pageSize, article_draft draft);

        List<article_draft> Query_ID(article_draft draft);

        int CriteriaTotalVolume(article_draft draft);

        bool Deletes(article_draft draft);

    }
}
