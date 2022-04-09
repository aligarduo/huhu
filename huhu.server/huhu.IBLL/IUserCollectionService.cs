using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IUserCollectionService : IBaseService<user_collection>
    {
        List<user_collection> Is_Collection_Name(user_collection collection);

        List<user_collection> Is_Collection_ID(user_collection collection);

        List<user_collection> Condition_PagingQuery(int pageIndex, int pageSize, user_collection collection);

        int Criteria_TotalVolume(user_collection collection);

        bool Update_Condition(user_collection collection, string[] condition);
    }
}
