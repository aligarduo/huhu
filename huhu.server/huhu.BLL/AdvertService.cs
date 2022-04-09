using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class AdvertService : BaseService<advert_all>, IAdvertService
    {
        public List<advert_all> Query_Advert_ID(advert_all advert)
        {
            return db.AdvertDal.Query_Advert_ID(advert);
        }
        public List<advert_all> ConditionPagingQuery(int PageIndex, int PageSize, advert_all advert)
        {
            return db.AdvertDal.ConditionPagingQuery(PageIndex, PageSize, advert);
        }
        public int ConditionSumQuery(advert_all advert)
        {
            return db.AdvertDal.ConditionSumQuery(advert);
        }
        public bool Update_Condition(advert_all advert, string[] condition)
        {
            return db.AdvertDal.Update_Condition(advert, condition);
        }

    }
}