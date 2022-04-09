using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IUserCollectService : IBaseService<user_collect>
    {
        List<user_collect> Is_Collect_Item(user_collect collect);

        List<user_collect> Condition_PagingQuery(int pageIndex, int pageSize, user_collect collect);

        int Criteria_TotalVolume(user_collect collect);

        List<user_collect> Is_Add_Item(user_collect collect);

        List<user_collect> Query_CollectionID(user_collect collect);
    }
}
