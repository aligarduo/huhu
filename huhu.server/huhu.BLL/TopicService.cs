using huhu.IBLL;
using huhu.Model;
using System.Collections.Generic;

namespace huhu.BLL
{
    public class TopicService : BaseService<topic_all>, ITopicService
    {
        public List<topic_all> Query_All()
        {
            return db.TopicDal.Query_All();
        }

        public List<topic_all> Query_Topic_ID(topic_all topic)
        {
            return db.TopicDal.Query_Topic_ID(topic);
        }

        public List<topic_all> Query_Topic_IDUserID(topic_all topic)
        {
            return db.TopicDal.Query_Topic_IDUserID(topic);
        }

        public bool Update_Condition(topic_all topic, string[] condition)
        {
            return db.TopicDal.Update_Condition(topic, condition);
        }

        public int TotalVolume()
        {
            return db.TopicDal.TotalVolume();
        }

        public List<topic_all> PagingQuery_CircleID(int pageIndex, int pageSize, topic_all topic)
        {
            return db.TopicDal.PagingQuery_CircleID(pageIndex, pageSize, topic);
        }

        public List<topic_all> PagingQuery_Topic(int pageIndex, int pageSize, topic_all topic)
        {
            return db.TopicDal.PagingQuery_Topic(pageIndex, pageSize, topic);
        }

        public List<topic_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return db.TopicDal.Search_Composite_Ranking(pageIndex, pageSize, key_word);
        }

        public List<topic_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word)
        {
            return db.TopicDal.Search_Newest_Ranking(pageIndex, pageSize, key_word);
        }
    }
}
