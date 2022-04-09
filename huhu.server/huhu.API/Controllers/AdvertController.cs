using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 广告
    /// </summary>
    public class AdvertController : ApiController
    {
        public IAdvertService AdvertBLL { get; set; }


        [HttpPost]
        [Route("content_api/v1/advert/create")]
        public HttpResponseMessage Create([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null)
            {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "title", "brief", "content", "picture", "advert_type", "rank", "start_time", "end_time" };
            foreach (var item in ef_item)
            {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "")
                {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            string picture = obj_list["picture"].Value<string>();
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            string domainName = URLUtil.CaptureURL(picture, "domainName");
            if (!URLUtil.CheckURL(picture))
            {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            if (domainNameServer != domainName)
            {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            advert_all advert = new advert_all
            {
                advert_id = Snowflake.Instance.NextId().ToString(),
                title = obj_list["title"].Value<string>(),
                brief = obj_list["brief"].Value<string>(),
                content = obj_list["content"].Value<string>(),
                picture = URLUtil.Parsing_URL(picture),
                advert_type = obj_list["advert_type"].Value<int>(),
                rank = obj_list["rank"].Value<int>(),
                start_time = obj_list["start_time"].Value<string>(),
                end_time = obj_list["end_time"].Value<string>(),
                audit_status = 0,
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };
            AdvertBLL.Add(advert);
            AdvertBLL.SaveChanges();
            string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            advert.picture = MainFileServer + advert.picture;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), advert));
        }


        [HttpPost]
        [Route("content_api/v1/advert/delete")]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "advert_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            advert_all advert = new advert_all {
                advert_id = obj_list["advert_id"].Value<string>()
            };
            if (AdvertBLL.Query_Advert_ID(advert).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ADVERTISING_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.ADVERTISING_DOES_NOT_EXIST)));
            }
            AdvertBLL.Delete(advert);
            AdvertBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpPost]
        [Route("content_api/v1/advert/query")]
        public HttpResponseMessage Query([FromBody] JObject json) {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "advert_id" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            advert_all advert = new advert_all {
                advert_id = obj_list["advert_id"].Value<string>()
            };
            var data = AdvertBLL.Query_Advert_ID(advert).FirstOrDefault();
            if (data == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.CONTENT_IS_NULL, Descripion.GetDescription(ResultCode.CONTENT_IS_NULL)));
            }
            string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            data.picture = MainFileServer + data.picture;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), data));
        }


        [HttpPost]
        [Route("content_api/v1/advert/query_adverts")]
        public HttpResponseMessage Query_Adverts([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null)
            {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            int rank = obj_list["rank"].Value<int>(), layout = obj_list["layout"].Value<int>();
            int row;
            switch (layout)
            {
                case 1: row = 2; break;
                case 2: row = 3; break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            advert_all advert = new advert_all();
            advert.rank = rank;

            List<advert_all> advert_all_list = AdvertBLL.ConditionPagingQuery(0, row, advert);
            int Total = AdvertBLL.ConditionSumQuery(advert);
            List<object> list = IntegratedData(advert_all_list);
            //是否有下一页
            bool More = true;
            if (advert_all_list.Count < Total)
            {
                More = false;
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), Total, "0", list, More));
        }


        [HttpPost]
        [Route("content_api/v1/advert/update")]
        public HttpResponseMessage Update([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "advert_id", "title", "brief", "content", "picture", "advert_type", "rank", "start_time", "end_time" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            string picture = obj_list["picture"].Value<string>();
            string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            string domainName = URLUtil.CaptureURL(picture, "domainName");
            if (!URLUtil.CheckURL(picture)) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
            }
            if (domainNameServer != domainName) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
            }
            advert_all advert = new advert_all {
                advert_id = obj_list["advert_id"].Value<string>(),
                title = obj_list["title"].Value<string>(),
                brief = obj_list["brief"].Value<string>(),
                content = obj_list["content"].Value<string>(),
                picture = URLUtil.Parsing_URL(picture),
                advert_type = obj_list["advert_type"].Value<int>(),
                rank = obj_list["rank"].Value<int>(),
                start_time = obj_list["start_time"].Value<string>(),
                end_time = obj_list["end_time"].Value<string>(),
                audit_status = 0,
                mtime = TimeUtil.GetCurrentTimestamp().ToString(),
                ctime = ""
            };
            if (AdvertBLL.Query_Advert_ID(advert).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ADVERTISING_DOES_NOT_EXIST, Descripion.GetDescription(ResultCode.ADVERTISING_DOES_NOT_EXIST)));
            }
            AdvertBLL.Update_Condition(advert, ef_item);
            AdvertBLL.SaveChanges();
            string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            advert.picture = MainFileServer + advert.picture;
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), advert));
        }


        /// <summary>
        /// 整合数据
        /// </summary>
        /// <param name="advert_list"></param>
        /// <returns></returns>
        private List<object> IntegratedData(List<advert_all> advert_list)
        {
            string MainFileServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
            List<object> all_feed = new List<object>();
            if (advert_list != null)
            {
                foreach (advert_all item in advert_list)
                {
                    advert_all advert = ObjectUtil.ConvertObject<advert_all>(item);
                    advert.picture = MainFileServer + advert.picture;
                    advert.content = "";
                    all_feed.Add(advert);
                }
            }
            return all_feed;
        }

    }
}
