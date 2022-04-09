using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IAdvertService : IBaseService<advert_all>
    {
        List<advert_all> Query_Advert_ID(advert_all advert);
        List<advert_all> ConditionPagingQuery(int pageIndex, int pageSize, advert_all advert);
        int ConditionSumQuery(advert_all advert);
        bool Update_Condition(advert_all advert, string[] condition);
    }
}
