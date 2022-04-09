using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class TopicCircleDAL : BaseDAL<topic_circle>, ITopicCircleDAL
    {
        public List<topic_circle> Query_All()
        {
            return GetEntities(p => p.circle_id != null).ToList();
        }

        public List<topic_circle> Query_Circle_ID(topic_circle circle)
        {
            return GetEntities(p => p.circle_id == circle.circle_id).ToList();
        }

        public List<topic_circle> Query_Circle_Name(topic_circle circle)
        {
            return GetEntities(p => p.circle_name == circle.circle_name).ToList();
        }

        public bool Update_Condition(topic_circle circle, string[] condition)
        {
            return UpdateCondition(circle, condition);
        }
    }
}
