using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IDiggService : IBaseService<user_digg>
    {
        List<user_digg> User_Interact(user_digg digg);

        List<user_digg> Digg_Count(user_digg digg);

        List<user_digg> Digg_User_Ranking(user_digg digg, int pageIndex, int pageSize);

    }
}
