using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class UserCollectDAL : BaseDAL<user_collect>, IUserCollectDAL
    {
        public List<user_collect> Is_Collect_Item(user_collect collect)
        {
            return GetEntities(p => p.collection_id == collect.collection_id & p.item_id == collect.item_id & p.user_id == collect.user_id).ToList();
        }

        public List<user_collect> Condition_PagingQuery(int pageIndex, int pageSize, user_collect collect)
        {
            return GetConditionPagingQuery(p => p.user_id == collect.user_id & p.collection_id == collect.collection_id, k => k.collection_id != null, pageIndex, pageSize).ToList();
        }

        public int Criteria_TotalVolume(user_collect collect)
        {
            return Count(p => p.user_id == collect.user_id & p.collection_id == p.collection_id);
        }

        public List<user_collect> Is_Add_Item(user_collect collect)
        {
            return GetEntities(p => p.user_id == collect.user_id & p.item_id == collect.item_id).ToList();
        }

        public List<user_collect> Query_CollectionID(user_collect collect)
        {
            return GetEntities(p => p.user_id == collect.user_id & p.collection_id == collect.collection_id).ToList();
        }

    }
}
