using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    class UserCollectService : BaseService<user_collect>, IUserCollectService
    {
        public List<user_collect> Is_Collect_Item(user_collect collect)
        {
            return db.UserCollectDal.Is_Collect_Item(collect);
        }

        public List<user_collect> Condition_PagingQuery(int pageIndex, int pageSize, user_collect collect)
        {
            return db.UserCollectDal.Condition_PagingQuery(pageIndex, pageSize, collect);
        }

        public int Criteria_TotalVolume(user_collect collect)
        {
            return db.UserCollectDal.Criteria_TotalVolume(collect);
        }

        public List<user_collect> Is_Add_Item(user_collect collect)
        {
            return db.UserCollectDal.Is_Add_Item(collect);
        }

        public List<user_collect> Query_CollectionID(user_collect collect)
        {
            return db.UserCollectDal.Query_CollectionID(collect);
        }
    }
}
