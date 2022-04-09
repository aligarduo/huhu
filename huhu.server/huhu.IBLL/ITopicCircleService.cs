using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface ITopicCircleService : IBaseService<topic_circle>
    {
        List<topic_circle> Query_All();
        List<topic_circle> Query_Circle_ID(topic_circle circle);
        List<topic_circle> Query_Circle_Name(topic_circle circle);
        bool Update_Condition(topic_circle topic, string[] condition);
    }
}
