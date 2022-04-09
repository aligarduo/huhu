using huhu.API.Filters;
using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.Commom.Token;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 话题
    /// </summary>
    public class TopicController : ApiController
    {
        public ITopicService TopicBLL { get; set; }
        public ITopicCircleService TopicCircleBLL { get; set; }


        [HttpPost]
        [Route("topic_api/v1/topic/publish")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Publish([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数
            string[] ef_item = new string[] { "topic_circle_id", "content", "pic_list", "url" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //校验图片集合
            if (obj_list["pic_list"].GetType().Name != "JArray") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验图片集
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            List<string> pic_List = new List<string>();
            foreach (var item in obj_list["pic_list"]) {
                if (!URLUtil.CheckURL(item.ToString())) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                }
                if (URLUtil.CaptureURL(item.ToString(), "domainName") != domainNameServer) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                }
                pic_List.Add(URLUtil.Parsing_URL(item.ToString()));
            }
            //校验链接
            string url = obj_list["url"].Value<string>();
            if (url != "" && !URLUtil.CheckURL(url)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            //提取话题关键字
            List<string> content_list = new List<string>();
            string content = obj_list["content"].Value<string>();
            Regex reg = new Regex(@"#\w+?#", RegexOptions.IgnoreCase);
            MatchCollection matchs = reg.Matches(content);
            foreach (Match item in matchs) {
                if (item.Success) {
                    content_list.Add(item.Value);
                }
            }
            //填充数据
            string topic_circle_id = obj_list["topic_circle_id"].Value<string>();
            Snowflake.Instance.SnowflakesInit(0, 0);
            topic_all topic = new topic_all {
                topic_id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                topic_circle_id = topic_circle_id == "" ? "0" : topic_circle_id,
                content = content,
                pic_list = string.Join(",", pic_List.ToArray()),
                url = url,
                topic = string.Join(",", content_list.ToArray()),
                status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            //校验所选圈子是否存在
            topic_circle circle = new topic_circle { circle_id = topic.topic_circle_id };
            if (circle.circle_id != "" && TopicCircleBLL.Query_Circle_ID(circle).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST)));
            }
            //保存
            TopicBLL.Add(topic);
            TopicBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Modify_Properties(topic)));
        }


        [HttpPost]
        [Route("topic_api/v1/topic/delete")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("topic_id") == null || obj_list["topic_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            topic_all topic = new topic_all {
                topic_id = obj_list["topic_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
            };
            if (TopicBLL.Query_Topic_IDUserID(topic).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_DOES_NOT_EXIST)));
            }

            TopicBLL.Delete(topic);
            TopicBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("topic_api/v1/topic/list")]
        public HttpResponseMessage List([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] {
                "cursor",//索引
                "limit",//数据量
                "sort_type",//筛选类型 根据圈子 or 根据话题
                "sort_key"//圈子ID or 话题关键字
            };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            return JsonUtil.ToJson(SortType(obj_list));
        }


        [HttpPost]
        [Route("topic_api/v1/topic/update")]
        [SignSecurityFilter]
        [TokenSecurityFilter]
        public HttpResponseMessage Update([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数
            string[] ef_item = new string[] { "topic_id", "topic_circle_id", "content", "pic_list", "url" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //校验图片集合
            if (obj_list["pic_list"].GetType().Name != "JArray") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验图片集
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            List<string> pic_List = new List<string>();
            foreach (var item in obj_list["pic_list"]) {
                if (!URLUtil.CheckURL(item.ToString())) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                }
                if (URLUtil.CaptureURL(item.ToString(), "domainName") != domainNameServer) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                }
                pic_List.Add(URLUtil.Parsing_URL(item.ToString()));
            }
            //校验链接
            string url = obj_list["url"].Value<string>();
            if (url != "" && !URLUtil.CheckURL(url)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            //提取话题关键字
            List<string> content_list = new List<string>();
            string content = obj_list["content"].Value<string>();
            Regex reg = new Regex(@"#\w+?#", RegexOptions.IgnoreCase);
            MatchCollection matchs = reg.Matches(content);
            foreach (Match item in matchs) {
                if (item.Success) {
                    content_list.Add(item.Value);
                }
            }
            //填充数据
            topic_all topic = new topic_all {
                topic_id = obj_list["topic_id"].Value<string>(),
                user_id = token.get_and_parse(ActionContext).setParams,
                topic_circle_id = obj_list["topic_circle_id"].Value<string>(),
                content = content,
                pic_list = string.Join(",", pic_List.ToArray()),
                url = url,
                topic = string.Join(",", content_list.ToArray()),
                status = 0,
                ctime = "",
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            if (TopicBLL.Query_Topic_IDUserID(topic).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_DOES_NOT_EXIST)));
            }
            //校验所选圈子是否存在
            topic_circle circle = new topic_circle { circle_id = topic.topic_circle_id };
            if (circle.circle_id != "" && TopicCircleBLL.Query_Circle_ID(circle).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST)));
            }

            string[] e_item = new string[] { "topic_circle_id", "content", "topic", "pic_list", "url" };
            TopicBLL.Update_Condition(topic, e_item);
            TopicBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Modify_Properties(topic)));
        }



        /// <summary>
        /// 修改返回体属性
        /// </summary>
        /// <param name="topic"></param>
        /// <returns></returns>
        private object Modify_Properties(topic_all topic)
        {
            List<string> pic_list = new List<string>();
            if (topic.pic_list != "") pic_list = new List<string>(topic.pic_list.Split(','));
            List<string> topic_list = new List<string>();
            if (topic.topic != "") topic_list = new List<string>(topic.topic.Split(','));
            return new {
                topic_id = topic.topic_id,
                user_id = topic.user_id,
                topic_circle_id = topic.topic_circle_id,
                content = topic.content,
                pic_list = pic_list,
                url = topic.url,
                topic = topic_list,
                status = topic.status,
                ctime = topic.ctime,
                mtime = topic.mtime
            };
        }

        /// <summary>
        /// 查询拓展
        /// </summary>
        /// <param name="obj_list"></param>
        /// <returns></returns>
        private object SortType(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            int sort_type = obj_list["sort_type"].Value<int>(),
                cursor = obj_list["cursor"].Value<int>(),
                limit = obj_list["limit"].Value<int>();
            string sort_key = obj_list["sort_key"].Value<string>();
            List<topic_all> dataVolume;
            topic_all topic = new topic_all();
            switch (sort_type) {
                case 1: {
                        topic.topic_circle_id = sort_key;
                        dataVolume = TopicBLL.PagingQuery_CircleID(cursor, limit, topic);
                    }; break;
                case 2: {
                        topic.topic = sort_key;
                        dataVolume = TopicBLL.PagingQuery_Topic(cursor, limit, topic);
                    }; break;
                default: return result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR));
            }
            //总条数
            int Total = TopicBLL.TotalVolume();
            //是否有下一页
            bool More = true;
            if (dataVolume.Count < limit) {
                More = false;
            }
            return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + dataVolume.Count).ToString(), dataVolume, More);
        }

    }
}