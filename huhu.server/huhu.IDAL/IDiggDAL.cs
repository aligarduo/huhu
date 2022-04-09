using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IDiggDAL : IBaseDAL<user_digg>
    {
        /// <summary>
        /// 用户交互
        /// </summary>
        /// <param name="digg"></param>
        /// <returns></returns>
        List<user_digg> User_Interact(user_digg digg);

        List<user_digg> Digg_Count(user_digg digg);

        List<user_digg> Digg_User_Ranking(user_digg digg, int pageIndex, int pageSize);

    }
}
