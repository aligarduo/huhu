using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class FeedBackDAL : BaseDAL<feedback_all>, IFeedBackDAL
    {
        public List<feedback_all> PagingQuery(int pageIndex, int pageSize)
        {
            return GetConditionPagingQuery(x => x.id != null, k => k.id != null, pageIndex, pageSize).ToList();
        }

        public int TotalVolume()
        {
            return Count(x => x.id != null);
        }
    }
}
