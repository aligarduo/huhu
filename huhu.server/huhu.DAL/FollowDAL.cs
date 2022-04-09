using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class FollowDAL : BaseDAL<user_follow>, IFollowDAL
    {
        public List<user_follow> Is_Follow(user_follow follow) {
            return GetEntities(p => p.user_id == follow.user_id & p.follow_id == follow.follow_id & p.follow_type == follow.follow_type).ToList();
        }

        public List<user_follow> Follow(user_all user, user_follow follow) {
            return GetEntities(p => p.user_id == user.user_id & p.follow_type == follow.follow_type).ToList();
        }

        public List<user_follow> Fans(user_all user, user_follow follow) {
            return GetEntities(p => p.follow_id == user.user_id & p.follow_type == follow.follow_type).ToList();
        }

    }
}
