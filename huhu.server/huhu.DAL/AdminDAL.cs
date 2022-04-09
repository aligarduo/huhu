using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class AdminDAL : BaseDAL<admin_all>, IAdminDAL
    {
        public List<admin_all> Query_ALL()
        {
            return GetEntities(p => p.admin_id != "").ToList();
        }
        public List<admin_all> Query_ID(admin_all admin)
        {
            return GetEntities(p => p.admin_id == admin.admin_id).ToList();
        }
        public List<admin_all> Query_Phone_Password(admin_all admin)
        {
            return GetEntities(p => p.phone == admin.phone & p.password == admin.password).ToList();
        }
        public List<admin_all> Query_Phone(admin_all admin)
        {
            return GetEntities(p => p.phone == admin.phone).ToList();
        }
        public bool Update_Condition(admin_all admin, string[] condition)
        {
            return UpdateCondition(admin, condition);
        }
    }
}
