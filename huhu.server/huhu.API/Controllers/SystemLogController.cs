using huhu.Commom;
using huhu.Commom.Enums;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public class SystemLogController : ApiController
    {

        [HttpPost]
        [Route("sys_api/v1/log/info")]
        public HttpResponseMessage Recommend_All_Feed([FromBody] JObject json)
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
            //取出参数
            int cursor = obj_list["cursor"].Value<int>(), limit = obj_list["limit"].Value<int>();
            //
            object data;
            using (huhuLogEntities db = new huhuLogEntities()) {
                data = db.Set<log>().Where(p => p.log_id != "").OrderBy(p => p.log_id != "").Skip(cursor).Take(limit).AsQueryable().ToList();
            }
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), data));
        }


    }
}
