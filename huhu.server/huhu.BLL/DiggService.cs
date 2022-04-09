using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class DiggService : BaseService<user_digg>, IDiggService
    {
        /// <summary>
        /// 用户交互
        /// </summary>
        /// <param name="digg"></param>
        /// <returns></returns>
        public List<user_digg> User_Interact(user_digg digg)
        {
            return db.DiggDal.User_Interact(digg);
        }

        public List<user_digg> Digg_Count(user_digg digg)
        {
            return db.DiggDal.Digg_Count(digg);
        }

        public List<user_digg> Digg_User_Ranking(user_digg digg, int pageIndex, int pageSize)
        {
            return db.DiggDal.Digg_User_Ranking(digg, pageIndex, pageSize);
        }


    }
}
