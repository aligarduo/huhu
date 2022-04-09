using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class DiggDAL : BaseDAL<user_digg>, IDiggDAL
    {
        /// <summary>
        /// 用户交互
        /// </summary>
        /// <param name="digg"></param>
        /// <returns></returns>
        public List<user_digg> User_Interact(user_digg digg)
        {
            return GetEntities(x => x.user_id == digg.user_id & x.digg_id == digg.digg_id & x.type == digg.type).ToList();
        }

        public List<user_digg> Digg_Count(user_digg digg)
        {
            return GetEntities(x => x.digg_id == digg.digg_id & x.type == digg.type).ToList();
        }


        public List<user_digg> Digg_User_Ranking(user_digg digg, int pageIndex, int pageSize)
        {
            return GetConditionPagingQuery(p => p.user_id == digg.user_id & p.type == digg.type, p => p.user_id != null, pageIndex, pageSize).ToList();
        }

    }
}
