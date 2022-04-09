using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class UserCollectionDAL : BaseDAL<user_collection>, IUserCollectionDAL
    {
        public List<user_collection> Is_Collection_Name(user_collection collection)
        {
            return GetEntities(p => p.collection_name == collection.collection_name & p.user_id == collection.user_id).ToList();
        }

        public List<user_collection> Is_Collection_ID(user_collection collection)
        {
            return GetEntities(p => p.collection_id == collection.collection_id & p.user_id == collection.user_id).ToList();
        }

        public List<user_collection> Condition_PagingQuery(int pageIndex, int pageSize, user_collection collection)
        {
            return GetConditionPagingQuery(p => p.user_id == collection.user_id, k => k.collection_id != null, pageIndex, pageSize).ToList();
        }

        public int Criteria_TotalVolume(user_collection collection)
        {
            return Count(p => p.user_id == collection.user_id);
        }

        public bool Update_Condition(user_collection collection, string[] condition)
        {
            return UpdateCondition(collection, condition);
        }
    }
}
