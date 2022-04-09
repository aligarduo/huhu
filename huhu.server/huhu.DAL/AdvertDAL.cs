using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class AdvertDAL : BaseDAL<advert_all>, IAdvertDAL
    {
        public List<advert_all> Query_Advert_ID(advert_all advert)
        {
            return GetEntities(x => x.advert_id == advert.advert_id).ToList();
        }
        public List<advert_all> ConditionPagingQuery(int pageIndex, int pageSize, advert_all advert)
        {
            return GetConditionPagingQuery(x => x.rank == advert.rank, k => k.advert_id != null, pageIndex, pageSize).ToList();
        }
        public int ConditionSumQuery(advert_all advert)
        {
            return Count(x => x.rank == advert.rank);
        }
        public bool Update_Condition(advert_all advert, string[] condition)
        {
            return UpdateCondition(advert, condition);
        }
    }
}
