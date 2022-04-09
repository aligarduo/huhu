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
using System.Web;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 反馈建议
    /// </summary>
    public class FeedBackController : ApiController
    {
        public IFeedBackService FeedBackBLL { get; set; }


        [HttpPost]
        [Route("content_api/v1/short_msg/publish")]
        [TokenSecurityFilter]
        public HttpResponseMessage Publish([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "content", "pic_list" };
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
            //赋值
            string pic = string.Join(",", pic_List.ToArray());
            Snowflake.Instance.SnowflakesInit(0, 0);
            feedback_all feedback = new feedback_all {
                id = Snowflake.Instance.NextId().ToString(),
                user_id = token.get_and_parse(ActionContext).setParams,
                pic_list = pic == "" ? null : pic,
                content = obj_list["content"].Value<string>(),
                audit_status = 0,
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            //保存
            FeedBackBLL.Add(feedback);
            FeedBackBLL.SaveChanges();
            //返回
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Modify_Properties(feedback)));
        }


        [HttpPost]
        [Route("content_api/v1/short_msg/list")]
        public HttpResponseMessage List([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "cursor", "limit" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();

            int Total = FeedBackBLL.TotalVolume();
            List<feedback_all> _Feedback = FeedBackBLL.PagingQuery(cursor, limit);

            List<object> _Feedback2 = new List<object>();
            foreach (var item in _Feedback) {
                _Feedback2.Add(Modify_Properties(item));
            }

            bool More = true;
            if (_Feedback2.Count < limit) {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, (cursor + _Feedback2.Count).ToString(), _Feedback2, More));
        }



        /// <summary>
        /// 修改属性
        /// </summary>
        /// <param name="feedback"></param>
        /// <returns></returns>
        private object Modify_Properties(feedback_all feedback)
        {
            List<string> pic_list = new List<string>();
            if (feedback.pic_list != null) {
                pic_list = new List<string>(feedback.pic_list.Split(','));
            }
            return new {
                feedback.id,
                feedback.user_id,
                pic_list,
                feedback.content,
                feedback.audit_status,
                feedback.ctime
            };
        }

    }
}