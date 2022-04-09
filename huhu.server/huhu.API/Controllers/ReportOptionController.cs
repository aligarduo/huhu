using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Commom.Snowflake;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 举报问题选项
    /// </summary>
    public class ReportOptionController : ApiController
    {
        public IReportOptionService ReportOptionBLL { get; set; }


        [HttpPost]
        [Route("report_api/v1/option/add")]
        public HttpResponseMessage Add([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("option_name") == null || obj_list["option_name"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }

            Snowflake.Instance.SnowflakesInit(0, 0);
            report_option option = new report_option {
                option_id = Snowflake.Instance.NextId().ToString(),
                option_name = obj_list["option_name"].Value<string>()
            };

            if (ReportOptionBLL.Query_report_Name(option).FirstOrDefault() != null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPTION_ALREADY_EXISTS, Descripion.GetDescription(ResultCode.OPTION_ALREADY_EXISTS)));
            }

            ReportOptionBLL.Add(option);
            ReportOptionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), option));
        }

        [HttpPost]
        [Route("report_api/v1/option/delete")]
        public HttpResponseMessage Delete([FromBody] JObject json)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(json));
            if (obj_list == null || obj_list.Property("option_id") == null || obj_list["option_id"].Value<string>() == "") {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            report_option option = new report_option {
                option_id = obj_list["option_id"].Value<string>()
            };
            if (ReportOptionBLL.Query_report_ID(option).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPTION_ALREADY_NOT_EXIST, Descripion.GetDescription(ResultCode.OPTION_ALREADY_NOT_EXIST)));
            }
            ReportOptionBLL.Delete(option);
            ReportOptionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }

        [HttpGet]
        [Route("report_api/v1/option/list")]
        public HttpResponseMessage List()
        {
            ResultMsg result = new ResultMsg();
            List<report_option> option = ReportOptionBLL.Query_ALL();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), option));
        }

        [HttpPost]
        [Route("report_api/v1/option/update")]
        public HttpResponseMessage Update([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "option_id", "option_name" };
            foreach (var item in ef_item) {
                if (obj_list.Property(item) == null || obj_list[item].Value<string>() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }

            report_option option = new report_option {
                option_id = obj_list["option_id"].Value<string>(),
                option_name = obj_list["option_name"].Value<string>(),
            };

            if (ReportOptionBLL.Query_report_ID(option).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPTION_ALREADY_NOT_EXIST, Descripion.GetDescription(ResultCode.OPTION_ALREADY_NOT_EXIST)));
            }
            ReportOptionBLL.Update_Condition(option, ef_item);
            ReportOptionBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), option));
        }


    }
}