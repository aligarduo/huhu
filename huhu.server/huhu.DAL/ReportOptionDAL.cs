using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class ReportOptionDAL : BaseDAL<report_option>, IReportOptionDAL
    {
        public List<report_option> Query_report_ID(report_option option)
        {
            return GetEntities(p => p.option_id == option.option_id).ToList();
        }
        public List<report_option> Query_report_Name(report_option option)
        {
            return GetEntities(p => p.option_name == option.option_name).ToList();
        }
        public List<report_option> Query_ALL()
        {
            return GetEntities(p => p.option_name != "").ToList();
        }

        public bool Update_Condition(report_option option, string[] condition)
        {
            return UpdateCondition(option, condition);
        }
    }
}
