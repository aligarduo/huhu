using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class AdminService : BaseService<admin_all>, IAdminService
    {
        public List<admin_all> Query_ALL()
        {
            return db.AdminDal.Query_ALL();
        }
        public List<admin_all> Query_ID(admin_all admin)
        {
            return db.AdminDal.Query_ID(admin);
        }
        public List<admin_all> Query_Phone_Password(admin_all admin)
        {
            return db.AdminDal.Query_Phone_Password(admin);
        }
        public List<admin_all> Query_Phone(admin_all admin)
        {
            return db.AdminDal.Query_Phone(admin);
        }
        public bool Update_Condition(admin_all admin, string[] condition)
        {
            return db.AdminDal.Update_Condition(admin, condition);
        }
    }
}
