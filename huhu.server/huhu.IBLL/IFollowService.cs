using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IFollowService : IBaseService<user_follow>
    {
        List<user_follow> Is_Follow(user_follow follow);

        List<user_follow> Follow(user_all user, user_follow follow);

        List<user_follow> Fans(user_all user, user_follow follow);

    }
}
