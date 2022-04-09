using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class ReportOptionService : BaseService<report_option>, IReportOptionService
    {
        public List<report_option> Query_report_ID(report_option option)
        {
            return db.ReportOptionDal.Query_report_ID(option);
        }
        public List<report_option> Query_report_Name(report_option option)
        {
            return db.ReportOptionDal.Query_report_Name(option);
        }
        public List<report_option> Query_ALL()
        {
            return db.ReportOptionDal.Query_ALL();
        }

        public bool Update_Condition(report_option option, string[] condition)
        {
            return db.ReportOptionDal.Update_Condition(option, condition);
        }
    }
}
