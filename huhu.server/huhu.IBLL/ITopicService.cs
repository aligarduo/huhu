using huhu.Model;
using System.Collections.Generic;

namespace huhu.IBLL
{
    public interface ITopicService : IBaseService<topic_all>
    {
        List<topic_all> Query_All();
        List<topic_all> Query_Topic_ID(topic_all topic);
        List<topic_all> Query_Topic_IDUserID(topic_all topic);
        bool Update_Condition(topic_all topic, string[] condition);
        int TotalVolume();
        List<topic_all> PagingQuery_CircleID(int pageIndex, int pageSize, topic_all topic);
        List<topic_all> PagingQuery_Topic(int pageIndex, int pageSize, topic_all topic);
        List<topic_all> Search_Composite_Ranking(int pageIndex, int pageSize, string key_word);
        List<topic_all> Search_Newest_Ranking(int pageIndex, int pageSize, string key_word);
    }
}
