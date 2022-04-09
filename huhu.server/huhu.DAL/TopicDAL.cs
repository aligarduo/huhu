using huhu.IDAL;
using huhu.Model;
using System.Collections.Generic;
using System.Linq;

namespace huhu.DAL
{
    public class TopicDAL : BaseDAL<topic_all>, ITopicDAL
    {
        public List<topic_all> Query_All()
        {
            return GetEntities(p => p.topic_id != null).ToList();
        }

        public List<topic_all> Query_Topic_ID(topic_all topic)
        {
            return GetEntities(p => p.topic_id == topic.topic_id).ToList();
        }

        public List<topic_all> Query_Topic_IDUserID(topic_all topic)
        {
            return GetEntities(p => p.topic_id == topic.topic_id && p.user_id == topic.user_id).ToList();
        }

        public bool Update_Condition(topic_all topic, string[] condition)
        {
            return UpdateCondition(topic, condition);
        }

        public int TotalVolume()
        {
            return Count(p => p.topic_id != null);
        }

        public List<topic_all> PagingQuery_CircleID(int pageIndex, int pageSize, topic_all topic)
        {
            return GetConditionPagingQuery(p => p.topic_circle_id == topic.topic_circle_id, p => p.topic_id != null, pageIndex, pageSize).ToList();
        }

        public List<topic_all> PagingQuery_Topic(int pageIndex, int pageSize, topic_all topic)
        {
            return GetConditionPagingQuery(p => p.topic == topic.topic, p => p.topic_id != null, pageIndex, pageSize).ToList();
        }

        public List<topic_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return GetConditionPagingQuery(p => p.topic.Contains(key_word), p => p.topic_id != null, pageIndex, pageSize).ToList();
        }

        public List<topic_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return GetConditionPagingQuery_DESC(p => p.topic.Contains(key_word), p => p.topic_id, pageIndex, pageSize).ToList();
        }


    }
}
