using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class FollowService : BaseService<user_follow>, IFollowService
    {
        public List<user_follow> Is_Follow(user_follow follow)
        {
            return db.FollowDal.Is_Follow(follow);
        }

        public List<user_follow> Follow(user_all user, user_follow follow) {
            return db.FollowDal.Follow(user, follow);
        }

        public List<user_follow> Fans(user_all user, user_follow follow) {
            return db.FollowDal.Fans(user, follow);
        }

    }
}
