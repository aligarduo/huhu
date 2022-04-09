using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class TopicCircleService : BaseService<topic_circle>, ITopicCircleService
    {
        public List<topic_circle> Query_All()
        {
            return db.TopicCircleDal.Query_All();
        }

        public List<topic_circle> Query_Circle_ID(topic_circle circle)
        {
            return db.TopicCircleDal.Query_Circle_ID(circle);
        }

        public List<topic_circle> Query_Circle_Name(topic_circle circle)
        {
            return db.TopicCircleDal.Query_Circle_Name(circle);
        }

        public bool Update_Condition(topic_circle circle, string[] condition)
        {
            return db.TopicCircleDal.Update_Condition(circle, condition);
        }
    }
}
