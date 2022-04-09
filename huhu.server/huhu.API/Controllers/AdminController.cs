using huhu.Commom;
using huhu.Commom.Enums;
using huhu.IBLL;
using huhu.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace huhu.API.Controllers
{
    /// <summary>
    /// 管理员
    /// </summary>
    public class AdminController : ApiController
    {
        public IAdminService AdminBLL { get; set; }


        [HttpPost]
        [Route("passport/admin/create")]
        public HttpResponseMessage Create([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            //校验参数及判空
            string[] ef_item = new string[] { "phone", "password", "power" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            //校验手机格式
            if (!RegularUtil.IsPhone(obj_list["phone"].Value<string>())) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PHONE_FORMAT_IS_WRONG, Descripion.GetDescription(ResultCode.PHONE_FORMAT_IS_WRONG)));
            }

            return JsonUtil.ToJson(Create_Info(obj_list));
        }

        private object Create_Info(JObject obj_list)
        {
            ResultMsg result = new ResultMsg();
            lock (this) {
                string phone = obj_list["phone"].Value<string>();
                string password = obj_list["password"].Value<string>();
                int power = obj_list["power"].Value<int>();

                admin_all admin = new admin_all {
                    admin_id = Commom.GradualID.UseridUtil.IDByGUId(),
                    name = "乎乎开发者",
                    phone = phone,
                    password = Commom.Encrypt.MD5Util.GetMD5_8(password),
                    power = power,
                    audit_status = 0,
                    update_time = TimeUtil.GetCurrentTimestamp().ToString(),
                    register_time = TimeUtil.GetCurrentTimestamp().ToString()
                };

                if (AdminBLL.Query_Phone(admin).FirstOrDefault() != null) {
                    return result.SetResultMsg((int)ResultCode.MOBILE_NAME_ALREADY_EXISTS, Descripion.GetDescription(ResultCode.MOBILE_NAME_ALREADY_EXISTS));
                }
                AdminBLL.Add(admin);
                AdminBLL.SaveChanges();
            }
            return result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS));
        }


        [HttpPost]
        [Route("passport/admin/state")]
        public HttpResponseMessage ChangeState([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            List<string> enable_changes = new List<string>();
            string[] ef_item = new string[] { "admin_id", "audit_status" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
                enable_changes.Add(item.Name);
            }
            switch (obj_list["audit_status"].Value<int>()) {
                case 0: break;
                case 1: break;
                default: return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.OPERATION_TYPE_ERROR, Descripion.GetDescription(ResultCode.OPERATION_TYPE_ERROR)));
            }
            //初始化实例
            admin_all admin = new admin_all();
            //动态给实体赋值
            Dictionary<string, object> dic = new Dictionary<string, object>();
            Type t = admin.GetType();
            foreach (PropertyInfo pi in t.GetProperties()) {
                if (pi.PropertyType.Name == "String")
                    dic.Add(pi.Name, "");
                if (pi.PropertyType.Name == "Int")
                    dic.Add(pi.Name, 0);
            }
            admin_all admin_ = JsonConvert.DeserializeObject<admin_all>(JsonConvert.SerializeObject(dic));
            admin_.admin_id = obj_list["admin_id"].Value<string>();
            admin_.audit_status = obj_list["audit_status"].Value<int>();
            if (AdminBLL.Query_ID(admin_).FirstOrDefault() == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ADMIN_NOT_EXIST, Descripion.GetDescription(ResultCode.ADMIN_NOT_EXIST)));
            }
            AdminBLL.Update_Condition(admin_, enable_changes.ToArray());
            AdminBLL.SaveChanges();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS)));
        }


        [HttpGet]
        [Route("passport/admin/query")]
        public HttpResponseMessage Query([FromBody] JObject obj)
        {
            ResultMsg result = new ResultMsg();
            JObject obj_list = JsonConvert.DeserializeObject<JObject>(JsonConvert.SerializeObject(obj));
            if (obj_list == null) {
                return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
            }
            string[] ef_item = new string[] { "admin_id" };
            foreach (var item in obj_list.Properties()) {
                if (!((IList)ef_item).Contains(item.Name) || item.Value.ToString() == "") {
                    return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.PARAMETER_ERROR, Descripion.GetDescription(ResultCode.PARAMETER_ERROR)));
                }
            }
            admin_all admin = new admin_all {
                admin_id = obj_list["admin_id"].Value<string>()
            };
            object data = AdminBLL.Query_ID(admin).FirstOrDefault();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.ADMIN_NOT_EXIST, Descripion.GetDescription(ResultCode.ADMIN_NOT_EXIST), data));
        }


        [HttpGet]
        [Route("passport/admin/list")]
        public HttpResponseMessage List()
        {
            ResultMsg result = new ResultMsg();
            return JsonUtil.ToJson(result.SetResultMsg((int)ResultCode.SUCCESS, Descripion.GetDescription(ResultCode.SUCCESS), AdminBLL.Query_ALL()));
        }




    }
}
