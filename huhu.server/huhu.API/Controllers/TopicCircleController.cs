using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 话题圈子
    /// </summary>
    public class TopicCircleController : ApiController
    {
        public ITopicCircleService TopicCircleBLL { get; set; }


        [HttpPost]
        [Route("topic_api/v1/club/create")]
        public HttpResponseMessage Create([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "circle_name", "icon" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            string icon = obj_list["icon"].Value<string>(),
             domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString(),
             domainName = URLUtil.CaptureURL(icon, "domainName");
            if (!URLUtil.CheckURL(icon)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            if (domainNameServer != domainName) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            topic_circle circle = new topic_circle {
                circle_id = Snowflake.Instance.NextId().ToString(),
                circle_name = obj_list["circle_name"].Value<string>(),
                icon = URLUtil.Parsing_URL(icon),
                ctime = TimeUtil.GetCurrentTimestamp().ToString(),
                mtime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            if (TopicCircleBLL.Query_Circle_Name(circle).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CIRCLE_NAME_ALREADY_EXISTS, Descripion.GetDescription(ResultCode.CIRCLE_NAME_ALREADY_EXISTS)));
            }

            TopicCircleBLL.Add(circle);
            TopicCircleBLL.SaveChanges();
            circle.icon = icon;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), circle));
        }


        [HttpPost]
        [Route("topic_api/v1/club/delete")]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("circle_id") == null || obj_list["circle_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            topic_circle circle = new topic_circle {
                circle_id = obj_list["circle_id"].Value<string>(),
            };
            if (TopicCircleBLL.Query_Circle_ID(circle).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST)));
            }
            TopicCircleBLL.Delete(circle);
            TopicCircleBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpGet]
        [Route("topic_api/v1/club/list")]
        public HttpResponseMessage List()
        {
            ResultMsg result = new ResultMsg();
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            List<topic_circle> Circle = TopicCircleBLL.Query_All();
            foreach (var item in Circle) {
                item.icon = domainNameServer + item.icon;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Circle));
        }


        [HttpPost]
        [Route("topic_api/v1/club/update")]
        public HttpResponseMessage Update([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "circle_id", "circle_name", "icon" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            string icon = obj_list["icon"].Value<string>();
            if (icon != "") {
                string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                string domainName = URLUtil.CaptureURL(icon, "domainName");
                if (!URLUtil.CheckURL(icon)) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                }
                if (domainNameServer != domainName) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                }
            }

            topic_circle circle = new topic_circle {
                circle_id = obj_list["circle_id"].Value<string>(),
                circle_name = obj_list["circle_name"].Value<string>(),
                icon = icon != "" ? URLUtil.Parsing_URL(icon) : "",
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = ""
            };

            var _C = TopicCircleBLL.Query_Circle_ID(circle).FirstOrDefault();
            if (_C == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.TOPIC_CIRCLE_DOES_NOT_EXIST)));
            }

            TopicCircleBLL.Update_Condition(circle, new string[] { "circle_name", "icon" });
            TopicCircleBLL.SaveChanges();
            circle.icon = icon;
            circle.ctime = _C.ctime;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), circle));
        }


    }
}