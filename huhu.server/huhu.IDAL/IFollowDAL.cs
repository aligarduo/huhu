using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IFollowDAL : IBaseDAL<user_follow>
    {
        List<user_follow> Is_Follow(user_follow follow);

        List<user_follow> Follow(user_all user, user_follow follow);

        List<user_follow> Fans(user_all user, user_follow follow);

    }
}
