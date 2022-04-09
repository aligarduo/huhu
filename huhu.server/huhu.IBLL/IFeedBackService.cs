using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IFeedBackService : IBaseService<feedback_all>
    {
        List<feedback_all> PagingQuery(int pageIndex, int pageSize);
        int TotalVolume();
    }
}
