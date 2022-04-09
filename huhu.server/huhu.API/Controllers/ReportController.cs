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
    /// 举报
    /// </summary>
    public class ReportController : ApiController
    {
        public IReportService ReportBLL { get; set; }
        public IReportOptionService ReportOptionBLL { get; set; }
        public IArticleService ArticleBLL { get; set; }


        [HttpPost]
        [Route("report_api/v1/report/publish")]
        public HttpResponseMessage Publish([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] {
                "issue",
                "article_id",
                "prove_pic",
                "describe"
            };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null) {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            //校验图片集合
            if (obj_list["prove_pic"].GetType().Name != "JArray") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            var prove_pic = obj_list["prove_pic"].ToArray();
            List<string> pic_List = new List<string>();
            if (prove_pic.Length != 0) {
                //校验图片集
                string domainNameServer = ConfigurationManager.AppSettings["MainFileServer"].ToString();
                foreach (var item in obj_list["pic_list"]) {
                    if (!URLUtil.CheckURL(item.ToString())) {
                        return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.INVALID_LINK, Descripion.GetDescription(ResultCode.INVALID_LINK)));
                    }
                    if (URLUtil.CaptureURL(item.ToString(), "domainName") != domainNameServer) {
                        return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.LINK_NOT_SUPPORTED, Descripion.GetDescription(ResultCode.LINK_NOT_SUPPORTED)));
                    }
                    pic_List.Add(URLUtil.Parsing_URL(item.ToString()));
                }
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            report_all report = new report_all {
                report_id = Snowflake.Instance.NextId().ToString(),
                issue = obj_list["issue"].Value<string>(),
                article_id = obj_list["article_id"].Value<string>(),
                prove_pic = prove_pic.Length == 0 ? "" : string.Join(",", pic_List.ToArray()),
                describe = obj_list["describe"].Value<string>(),
                ctime = TimeUtil.GetCurrentTimestamp().ToString()
            };

            report_option option = new report_option {
                option_id = report.issue
            };
            if (report.issue == "" || ReportOptionBLL.Query_report_ID(option).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPTION_ALREADY_NOT_EXIST, Descripion.GetDescription(ResultCode.OPTION_ALREADY_NOT_EXIST)));
            }
            article_all article = new article_all {
                article_id = report.article_id
            };
            if (report.article_id == "" || ArticleBLL.Query_ID(article).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ARTICLE_NOT_EXIST_OR_DELETED, Descripion.GetDescription(ResultCode.ARTICLE_NOT_EXIST_OR_DELETED)));
            }

            ReportBLL.Add(report);
            ReportBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

    }
}