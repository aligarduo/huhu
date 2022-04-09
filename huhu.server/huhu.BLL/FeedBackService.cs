using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class FeedBackService : BaseService<feedback_all>, IFeedBackService
    {
        public List<feedback_all> PagingQuery(int pageIndex, int pageSize)
        {
            return db.FeedBackDal.PagingQuery(pageIndex, pageSize);
        }

        public int TotalVolume()
        {
            return db.FeedBackDal.TotalVolume();
        }
    }
}
