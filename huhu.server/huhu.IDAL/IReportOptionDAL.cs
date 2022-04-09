using huhu.Model;
using System.Collections.Generic;

namespace huhu.IDAL
{
    public interface IReportOptionDAL : IBaseDAL<report_option>
    {
        List<report_option> Query_report_ID(report_option option);
        List<report_option> Query_report_Name(report_option option);
        List<report_option> Query_ALL();
        bool Update_Condition(report_option option, string[] condition);
    }
}
