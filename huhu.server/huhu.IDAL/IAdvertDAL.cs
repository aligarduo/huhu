using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IAdvertDAL : IBaseDAL<advert_all>
    {
        List<advert_all> Query_Advert_ID(advert_all advert);
        List<advert_all> ConditionPagingQuery(int PageIndex, int PageSize, advert_all advert);
        int ConditionSumQuery(advert_all advert);
        bool Update_Condition(advert_all advert, string[] condition);
    }
}
