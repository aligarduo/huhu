using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IFeedBackDAL : IBaseDAL<feedback_all>
    {
        List<feedback_all> PagingQuery(int pageIndex, int pageSize);
        int TotalVolume();
    }
}
