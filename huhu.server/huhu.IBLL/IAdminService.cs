using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface IAdminService : IBaseService<admin_all>
    {
        List<admin_all> Query_ALL();
        List<admin_all> Query_ID(admin_all admin);
        List<admin_all> Query_Phone_Password(admin_all admin);
        List<admin_all> Query_Phone(admin_all admin);
        bool Update_Condition(admin_all admin, string[] condition);
    }
}
