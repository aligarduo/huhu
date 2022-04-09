using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class UserCollectionService : BaseService<user_collection>, IUserCollectionService
    {
        public List<user_collection> Is_Collection_Name(user_collection collection)
        {
            return db.UserCollectionDal.Is_Collection_Name(collection);
        }

        public List<user_collection> Is_Collection_ID(user_collection collection)
        {
            return db.UserCollectionDal.Is_Collection_ID(collection);
        }

        public List<user_collection> Condition_PagingQuery(int pageIndex, int pageSize, user_collection collection)
        {
            return db.UserCollectionDal.Condition_PagingQuery(pageIndex, pageSize, collection);
        }

        public int Criteria_TotalVolume(user_collection collection)
        {
            return db.UserCollectionDal.Criteria_TotalVolume(collection);
        }

        public bool Update_Condition(user_collection collection, string[] condition)
        {
            return db.UserCollectionDal.Update_Condition(collection, condition);
        }
    }
}
